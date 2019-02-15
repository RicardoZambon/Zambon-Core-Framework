using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Zambon.Core.Database;
using Zambon.Core.Database.Cache.ChangeTracker;
using Zambon.Core.Database.Entity;
using Zambon.Core.Database.Helper;
using Zambon.Core.Database.Operations;
using Zambon.Core.Module.Helper;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views;
using Zambon.Core.Module.Xml.Views.DetailViews;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.SubViews;
using Zambon.Core.WebModule.ActionFilters;
using Zambon.Core.WebModule.Helper;

namespace Zambon.Core.WebModule.Controllers
{
    public abstract class CoreController<T> : Controller where T : BaseDBObject, new()
    {

        protected readonly CoreContext _ctx;

        protected readonly ApplicationService _app;

        public CoreController(CoreContext ctx, ApplicationService app)
        {
            _ctx = ctx;
            _app = app;
        }


        #region Delete

        protected virtual T ActionDelete(int objectId)
        {
            return _ctx.Delete<T>(objectId);
        }

        [HttpPost, LoadTrackedEntities]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<JsonResult> Delete(ViewInfo viewInfo, int objectId)
        {
            var view = _app.GetView(viewInfo.ViewId);
            if (view != null)
                try
                {
                    var entity = ActionDelete(objectId);
                    return GetJsonResult(GetMessageText("{0}_Deleted", view, entity), "alert-success", submitViewId: new[] { view.ViewId });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound(GetMessageText("{0}_NotFound", view));
                }
                catch (Exception ex)
                {
                    return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
                }
            return NotFound();
        }

        #endregion

        #region Refresh

        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, KeepCurrentTabOpen]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<DetailModal> Refresh(ViewInfo viewInfo, T entity)
        {
            try
            {
                return GetPartialView(viewInfo, GetCurrentObject(entity));
            }
            catch (Exception ex)
            {
                return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
            }
        }

        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<bool> UpdateData(T entity)
        {
            try
            {
                _ctx.ApplyChanges(entity, true);
                return true;
            }
            catch (Exception ex)
            {
                return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
            }
        }

        #endregion

        #region New

        [HttpPost]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<bool> Cancel(string objectId)
        {
            try
            {
                _ctx.RemoveTrackedEntity(typeof(T), Convert.ToInt32(objectId));
                return true;
            }
            catch (Exception ex)
            {
                return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
            }
        }


        protected virtual T ActionNew(T currentObject, Dictionary<string, object> parameters)
        {
            return currentObject;
        }

        [HttpPost, ClearTrackedEntities, LoadTrackedEntities, ParameterAsDictionary(Parameters = "*", Ignore = "ParentViewId,ModalViewId,ParentObjectId")]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<DetailModal> New(ViewInfo viewInfo, int ParentObjectId = 0, Dictionary<string, object> parameters = null)
        {
            try
            {
                var entry = _ctx.Attach(_ctx.CreateProxy<T>());

                if (!string.IsNullOrEmpty(viewInfo.ParentViewId))
                {
                    var view = _app.GetView(viewInfo.ParentViewId);
                    if (view is DetailView)
                    {
                        var dbParentModel = _ctx.Model.FindEntityType(view.GetEntityType());
                        var navigation = dbParentModel.GetNavigations().FirstOrDefault(x => x.ForeignKey.DeclaringEntityType.Name == entry.Metadata.ClrType.GetCorrectType().FullName).FindInverse();
                        if (navigation != null)
                        {
                            var parentKeys = new Dictionary<string, object> { { navigation.ForeignKey.Properties[0].Name, ParentObjectId } };
                            entry.CurrentValues.SetValues(parentKeys);
                        }
                    }
                }

                entry.State = EntityState.Added;

                return GetPartialView(viewInfo, GetCurrentObject(ActionNew(entry.Entity, parameters))).Result;
            }
            catch (Exception ex)
            {
                return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
            }
        }

        #endregion

        #region Edit

        [HttpPost, ClearTrackedEntities, LoadTrackedEntities]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<DetailModal> Edit(ViewInfo viewInfo, int objectId)
        {
            if (_app.GetView(viewInfo.ViewId) is BaseView view)
                try
                {
                    if (_ctx.Find<T>(objectId) is T entity)
                        return GetPartialView(viewInfo, GetCurrentObject(entity)).Result;
                    return NotFound(GetMessageText("{0}_NotFound", view));
                }
                catch (Exception ex)
                {
                    return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
                }
            return NotFound();
        }

        #endregion

        #region SubListViews

        protected ActionResult<DetailModal> AddSubListViewItems<C>(ViewInfo viewInfo, T entity, ICollection<C> collection, int[] addIds, Func<C, int> childExistsCompare, Action<C> setChildToEntity, Action<C, int> setChildToItem) where C : class
        {
            if (_app.GetView(viewInfo.ViewId) is BaseView view)
                try
                {
                    var ids = addIds.Distinct().ToArray();
                    for (var i = 0; i < ids.Length; i++)
                    {
                        var id = ids[i];
                        if (!collection.Select(childExistsCompare).Any(x => x == id))
                        {
                            var newSubItem = _ctx.CreateProxy<C>();

                            setChildToItem(newSubItem, id);
                            setChildToEntity(newSubItem);

                            if (newSubItem is ITrackableEntity trackableSubItem)
                                _ctx.ApplyChanges(trackableSubItem);
                            else
                                _ctx.SaveChanges(newSubItem);
                        }
                    }
                    return GetPartialView(viewInfo, GetCurrentObject(entity));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound(GetMessageText("{0}_NotFound", view));
                }
                catch (Exception ex)
                {
                    return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
                }
            return NotFound();
        }
        
        protected ActionResult<DetailModal> RemoveSubListViewItems<C>(ViewInfo viewInfo, T entity, ICollection<C> collection, int deleteId) where C : BaseDBObject
        {
            if (_app.GetView(viewInfo.ViewId) is BaseView view)
                try
                {
                    if (collection.Any(x => x.ID == deleteId))
                    {
                        var r = _ctx.Delete<C>(deleteId, false);
                        if (r.ID <= 0) collection.Remove(r);
                    }
                    return GetPartialView(viewInfo, GetCurrentObject(entity));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound(GetMessageText("{0}_NotFound", view));
                }
                catch (Exception ex)
                {
                    return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
                }
            return NotFound();
        }

        #endregion

        #region Save

        protected virtual void ActionSave(BaseView parentView, T entity)
        {
            if (parentView is ListView)
                _ctx.SaveChanges(entity);
            else
                _ctx.ApplyChanges(entity);
        }

        [HttpPost, LoadTrackedEntities, MergeActionValuesFromDB, ValidateActionOnSaving, KeepCurrentTabOpen]
        [ProducesResponseType(200), ProducesResponseType(404)]
        public ActionResult<DetailModal> Save(ViewInfo viewInfo, T entity)
        {
            if (ModelState.IsValid && _app.GetView(viewInfo.ViewId) is BaseView view)
                try
                {
                    var parentView = _app.GetView(viewInfo.ParentViewId);

                    ActionSave(parentView, entity);
                    TempData["CurrentTabId"] = string.Empty;

                    if (parentView is DetailView)
                        return Ok(GetJsonResult(closeModalId: new [] { viewInfo.ModalId }, submitViewId: new [] { viewInfo.ParentViewId }, submitViewAction: Url.Action("Refresh", parentView.ControllerName)));
                    else
                        return Ok(GetJsonResult(GetMessageText("{0}_Saved", view, entity), "alert-success", closeModalId: new [] { viewInfo.ModalId }, submitViewId: new[] { viewInfo.ParentViewId }));
                }
                catch (Exception ex)
                {
                    return NotFound(string.Format(_app.GetStaticText("Error_Message"), ex.Message));
                }
            return GetPartialView(viewInfo, GetCurrentObject(entity));
        }

        #endregion


        #region Object methods

        protected virtual string GetObjectView(T currentObject)
        {
            return string.Empty;
        }

        protected virtual void OnGetCurrentObject(T currentObject)
        {

        }


        protected ActionResult<DetailModal> GetPartialView(ViewInfo viewInfo, object currentObject)
        {
            _app.ClearDetailViewCurrentObject(viewInfo.ViewId);
            _app.SetDetailViewCurrentObject(viewInfo.ViewId, currentObject);

            var view = _app.GetDetailView(viewInfo.ViewId);

            var viewFolder = view.ViewFolder;
            if (!string.IsNullOrEmpty(viewFolder))
                viewFolder += "/";

            var viewCshtml = string.Empty;
            if (currentObject is T)
                viewCshtml = GetObjectView((T)currentObject);
            if (string.IsNullOrWhiteSpace(viewCshtml))
                viewCshtml = view.DefaultView;

            _app.SetDetailViewCurrentView(viewInfo.ViewId, $"~/Views/{viewFolder}{ControllerContext.ActionDescriptor.ControllerName}/{viewCshtml}.cshtml");

            TempData["CurrentModalID"] = viewInfo.ModalId;
            TempData["CurrentViewID"] = viewInfo.ViewId;

            if (currentObject is T)
                TempData["CurrentObjectID"] = ((T)currentObject).ID;

            if (string.IsNullOrWhiteSpace(viewInfo.ModalId))
            {
                if (viewInfo.ViewOrigin?.ToLower() == "menu")
                    return PartialView("./Views/Shared/Components/DetailView/Default.cshtml", view);
                else
                    return PartialView("./Views/Shared/Components/DetailView/DetailView_Body.cshtml", view);
            }
            else
            {
                var parentView = _app.GetView(viewInfo.ParentViewId);

                var modalView = parentView.GetSubView(viewInfo.ModalId) as BaseModal;
                if (modalView.ParentViewId != viewInfo.ParentViewId)
                    modalView.ParentViewId = viewInfo.ParentViewId;

                    TempData["CurrentModalTitle"] = !string.IsNullOrWhiteSpace(viewInfo.ModalTitle) ? viewInfo.ModalTitle
                                                        : !string.IsNullOrWhiteSpace(modalView.Title) ? modalView.Title
                                                        : modalView.View.Title;

                return PartialView("./Views/Shared/Components/ModalView/DetailModalView_Body.cshtml", modalView);
            }
        }

        protected T GetCurrentObject(T currentObject)
        {
            var entry = _ctx.Entry(currentObject);
            if (entry.State == EntityState.Detached)
                entry.State = EntityState.Unchanged;

            OnGetCurrentObject(entry.Entity);
            _ctx.ApplyChanges(currentObject, true);
            return currentObject;
        }

        #endregion

        #region Return methods

        protected JsonResult GetJsonResult(string message = "", string messageClass = "", string[] closeModalId = null, string[] submitViewId = null, string submitViewAction = "")
        {
            return new JsonResult(new
            {
                message,
                messageClass,
                submitViewId = (submitViewId?.Length ?? 0 )> 0 ? string.Join(",", submitViewId.Select(x => !x.StartsWith("#") ? "#" + x : x)) : "",
                closeModalId = (closeModalId?.Length ?? 0) > 0 ? string.Join(",", closeModalId.Select(x => (!x.StartsWith("#") ? "#" + x : x) + "_modal")) : "",
                submitViewAction
            });
        }

        protected string GetMessageText(string key, BaseView view = null, T currentObject = null)
        {
            return GetMessageText(key, view?.GetEntityType(), currentObject);
        }
        protected string GetMessageText(string key, Type type = null, T currentObject = null)
        {
            if (type != null && currentObject != null)
            {
                var text = _app.GetStaticText(string.Format(key, type.Name));
                if (currentObject != null)
                {
                    if (key.ToLower().Contains("deleted"))
                        text = "<span class='oi oi-x mr-1 text-danger'></span>" + text;
                    else if (key.ToLower().Contains("saved"))
                        text = "<span class='oi oi-check mr-1 text-primary'></span>" + text;

                    var defaultValue = type.GetProperty(type.GetDefaultProperty() ?? "ID").GetValue(currentObject);
                    return string.Format(text.Replace("{", "<b>{").Replace("}", "}</b>"), defaultValue);
                }
                return text;
            }
            else
                return _app.GetStaticText(key);
        }

        #endregion

    }
}