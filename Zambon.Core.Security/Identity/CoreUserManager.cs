using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Zambon.Core.Module;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Security.Identity
{
    public class CoreUserManager<TUser> : UserManager<TUser> where TUser : class, IUsers
    {

        #region Variables

        private IdentityError _AdUserChangePasswordError;

        #endregion

        #region Properties

        private readonly IOptions<AppSettings> _options;

        public static PasswordOptions passwordOptions
        {
            get
            {
                return new PasswordOptions()
                {
                    RequireDigit = false,
                    RequiredLength = 5,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            }
        }

        #endregion

        #region Constructors

        public CoreUserManager(IOptions<AppSettings> options, IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _options = options;
            _AdUserChangePasswordError = new IdentityError() { Code = "0", Description = "Informed user is Active Directory user." };
        }

        #endregion

        #region Overrides

        public override Task<IdentityResult> AddPasswordAsync(TUser user, string password)
        {
            return AddPasswordAsync(user, password, true);
        }
        public async Task<IdentityResult> AddPasswordAsync(TUser user, string password, bool useTransaction = true)
        {
            if (user.AuthenticationType == Module.Helper.Enums.AuthenticationType.UserPassword)
            {
                var passwordStore = GetPasswordStore();
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var hash = await passwordStore.GetPasswordHashAsync(user, CancellationToken);
                if (hash != null)
                {
                    Logger.LogWarning(1, "User {userId} already has a password.", await GetUserIdAsync(user));
                    return IdentityResult.Failed(ErrorDescriber.UserAlreadyHasPassword());
                }
                var result = await UpdatePasswordHash(passwordStore, user, password);
                if (!result.Succeeded)
                    return result;
                return await UpdateUserAsync(user, useTransaction);
            }
            return IdentityResult.Failed(_AdUserChangePasswordError);
        }

        public override Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            return ChangePasswordAsync(user, currentPassword, newPassword, true);
        }
        public async Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword, bool useTransaction = true)
        {
            if (user.AuthenticationType == Module.Helper.Enums.AuthenticationType.UserPassword)
            {
                var passwordStore = GetPasswordStore();
                if (user == null)
                    throw new ArgumentNullException(nameof(user));


                if (await VerifyPasswordAsync(passwordStore, user, currentPassword) != PasswordVerificationResult.Failed)
                {
                    var result = await UpdatePasswordHash(passwordStore, user, newPassword);
                    if (!result.Succeeded)
                        return result;
                    return await UpdateUserAsync(user, useTransaction);
                }
                Logger.LogWarning(2, "Change password failed for user {userId}.", await GetUserIdAsync(user));
                return IdentityResult.Failed(ErrorDescriber.PasswordMismatch());
            }
            return IdentityResult.Failed(_AdUserChangePasswordError);
        }

        public override Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            if (user.LogonAllowed)
                if (user.AuthenticationType == Module.Helper.Enums.AuthenticationType.UserPassword)
                    return base.CheckPasswordAsync(user, password);
                else if (user.AuthenticationType == Module.Helper.Enums.AuthenticationType.LDAP)
                {
                    if (string.IsNullOrWhiteSpace(_options.Value.DefaultDomainName))
                        throw new ApplicationException("Missing domain configuration in appsettings.json file.");

                    using (var context = new PrincipalContext(ContextType.Domain, _options.Value.DefaultDomainName, "service_acct_user", ContextOptions.SimpleBind))
                        return Task.FromResult(context.ValidateCredentials(user.Username, password));
                }
            return Task.FromResult(false);
        }

        public override Task<IdentityResult> CreateAsync(TUser user)
        {
            return CreateAsync(user, true);
        }
        public async Task<IdentityResult> CreateAsync(TUser user, bool useTransaction = true)
        {
            var result = await ValidateUserAsync(user);
            if (!result.Succeeded)
                return result;

            if (Options.Lockout.AllowedForNewUsers && SupportsUserLockout)
                await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken);

            await UpdateNormalizedEmailAsync(user);

            return await (Store as CoreUserStore<TUser>).CreateAsync(user, CancellationToken, useTransaction);
        }

        public override Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            return CreateAsync(user, password, true);
        }
        public async Task<IdentityResult> CreateAsync(TUser user, string password, bool useTransaction = true)
        {
            var passwordStore = GetPasswordStore();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var result = await UpdatePasswordHash(passwordStore, user, password);
            if (!result.Succeeded)
                return result;

            return await CreateAsync(user, useTransaction);
        }

        public override Task<IdentityResult> DeleteAsync(TUser user)
        {
            return DeleteAsync(user, true);
        }
        public async Task<IdentityResult> DeleteAsync(TUser user, bool commitChanges = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await (Store as CoreUserStore<TUser>).DeleteAsync(user, CancellationToken, commitChanges);
        }

        public override Task<IdentityResult> RemovePasswordAsync(TUser user)
        {
            return RemovePasswordAsync(user, true);
        }
        public async Task<IdentityResult> RemovePasswordAsync(TUser user, bool useTransaction = true)
        {
            if (user.AuthenticationType == Module.Helper.Enums.AuthenticationType.UserPassword)
            {
                var passwordStore = GetPasswordStore();
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                await UpdatePasswordHash(passwordStore, user, null, validatePassword: false);
                return await UpdateUserAsync(user, useTransaction);
            }
            return IdentityResult.Failed(_AdUserChangePasswordError);
        }

        public override Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            return ResetPasswordAsync(user, token, newPassword, true);
        }
        public async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword, bool useTransaction = true)
        {
            if (user.AuthenticationType == Module.Helper.Enums.AuthenticationType.UserPassword)
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                if (!await VerifyUserTokenAsync(user, Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token))
                    return IdentityResult.Failed(ErrorDescriber.InvalidToken());

                var result = await UpdatePasswordHash(user, newPassword, validatePassword: true);
                if (!result.Succeeded)
                    return result;

                return await UpdateUserAsync(user, useTransaction);
            }
            return IdentityResult.Failed(_AdUserChangePasswordError);
        }

        public override Task<IdentityResult> UpdateAsync(TUser user)
        {
            return UpdateAsync(user, true);
        }
        public async Task<IdentityResult> UpdateAsync(TUser user, bool useTransaction = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await UpdateUserAsync(user, useTransaction);
        }

        protected override Task<IdentityResult> UpdateUserAsync(TUser user)
        {
            return UpdateUserAsync(user, true);
        }
        protected async Task<IdentityResult> UpdateUserAsync(TUser user, bool useTransaction = true)
        {
            var result = await ValidateUserAsync(user);
            if (!result.Succeeded)
                return result;

            await UpdateNormalizedEmailAsync(user);
            return await (Store as CoreUserStore<TUser>).UpdateAsync(user, CancellationToken, useTransaction);
        }

        #endregion

        #region Methods

        private IUserLockoutStore<TUser> GetUserLockoutStore()
        {
            var cast = Store as IUserLockoutStore<TUser>;
            if (cast == null)
                throw new NotSupportedException("");
            return cast;
        }

        private IUserPasswordStore<TUser> GetPasswordStore()
        {
            var cast = Store as IUserPasswordStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("");
            }
            return cast;
        }

        private async Task<IdentityResult> UpdatePasswordHash(IUserPasswordStore<TUser> passwordStore, TUser user, string newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                var validate = await ValidatePasswordAsync(user, newPassword);
                if (!validate.Succeeded)
                    return validate;
            }
            var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;
            await passwordStore.SetPasswordHashAsync(user, hash, CancellationToken);
            return IdentityResult.Success;
        }

        #endregion

    }
}