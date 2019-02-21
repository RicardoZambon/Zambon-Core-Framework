using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.SubViews;
using Zambon.Core.Security.BusinessObjects;
using Zambon.Core.Security.Identity;
using Zambon.Core.WebModule.ActionFilters;
using Zambon.Core.WebModule.Helper;

namespace Zambon.Core.WebModule.Controllers
{
    public class CoreUserController<U> : CoreController<U> where U : Users, new()
    {
        private readonly CoreUserManager<U> _coreUserManager;

        public CoreUserController(CoreDbContext ctx, CoreUserManager<U> coreUserManager, ApplicationService app) : base(ctx, app)
        {
            _coreUserManager = coreUserManager;
        }


        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        public ActionResult<DetailModal> AddRoles(ViewInfo viewInfo, U entity, int[] LookupSelection)
        {
            return AddSubListViewItems(viewInfo, entity, entity.Roles, LookupSelection, r => r.RoleId, (x) => x.UserId = entity.ID, (x, id) => x.RoleId = id);
        }

        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        public ActionResult<DetailModal> RemoveRole(ViewInfo viewInfo, U entity, int deleteId)
        {
            return RemoveSubListViewItems(viewInfo, entity, entity.Roles, deleteId);
        }


        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        public ActionResult<DetailModal> AddManagers(ViewInfo viewInfo, U entity, int[] LookupSelection)
        {
            return AddSubListViewItems(viewInfo, entity, entity.Managers, LookupSelection.Where(x => x != entity.ID).ToArray(), r => r.ManagerID, (x) => x.SubordinateID = entity.ID, (x, id) => x.ManagerID = id);
        }

        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        public ActionResult<DetailModal> RemoveManager(ViewInfo viewInfo, U entity, int deleteId)
        {
            return RemoveSubListViewItems(viewInfo, entity, entity.Managers, deleteId);
        }


        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        public ActionResult<DetailModal> AddSubordinates(ViewInfo viewInfo, U entity, int[] LookupSelection)
        {
            return AddSubListViewItems(viewInfo, entity, entity.Subordinates, LookupSelection.Where(x => x != entity.ID).ToArray(), r => r.SubordinateID, (x) => x.ManagerID = entity.ID, (x, id) => x.SubordinateID = id);
        }

        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        public ActionResult<DetailModal> RemoveSubordinate(ViewInfo viewInfo, U entity, int deleteId)
        {
            return RemoveSubListViewItems(viewInfo, entity, entity.Subordinates, deleteId);
        }


        protected override async void ActionSave(BaseView view, U entity)
        {
            if (entity.AuthenticationType == Module.Helper.Enums.AuthenticationType.UsernamePassword)
            {
                if (entity.ID <= 0)
                    await _coreUserManager.CreateAsync(entity, entity.Password);
                else
                {
                    if (!string.IsNullOrWhiteSpace(entity.Password))
                    {
                        var token = _coreUserManager.GeneratePasswordResetTokenAsync(entity);
                        await _coreUserManager.ResetPasswordAsync(entity, token.Result, entity.Password);
                    }
                    else
                        await _coreUserManager.UpdateAsync(entity);
                }
            }
            else
                base.ActionSave(view, entity);
        }

    }
}