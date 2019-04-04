using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zambon.Core.Database;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Interfaces;
using Zambon.Core.Module;
using Zambon.Core.Security.BusinessObjects;

namespace Zambon.DemoApplication.Database
{
    [Table("CachedData", Schema = "Cache")]
    public class CachedData : IEntity
    {
        [StringLength(449), Key]
        public string Id { get; set; }

        public byte[] Value { get; set; }

        public DateTimeOffset ExpiresAtTime { get; set; }

        public long? SlidingExpirationInSeconds { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }


        public void ConfigureEntity(EntityTypeBuilder entity)
        {
            entity.Property("Value").IsRequired();
        }

    }

    public class DbSeedData : IDbInitializer
    {
        public void Seed(ModelBuilder modelBuilder)
        {
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Users>();

            var admin = new Users() { ID = 1, AuthenticationType = Enums.AuthenticationType.UserPassword, Username = "admin", LogonAllowed = true };
            admin.Password = passwordHasher.HashPassword(admin, "admin");
            modelBuilder.Entity<Users>().HasData(admin);

            modelBuilder.Entity<Roles>().HasData(new Roles() { ID = 1, Name = "Administrators", IsAdministrative = true });

            modelBuilder.Entity<RolesUsers>().HasData(new RolesUsers() { ID = 1, UserId = 1, RoleId = 1 });


            modelBuilder.Entity(typeof(CachedData));
        }
    }
}