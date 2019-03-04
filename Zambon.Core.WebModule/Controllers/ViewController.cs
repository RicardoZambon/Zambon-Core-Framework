using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Zambon.Core.Database;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Views.ListViews;
using Zambon.Core.Module.Xml.Views.ListViews.Search;
using Zambon.Core.Module.Xml.Views.SubViews;
using Zambon.Core.WebModule.ActionFilters;
using Zambon.Core.WebModule.Helper;

namespace Zambon.Core.WebModule.Controllers
{
    public class ViewController : Controller
    {

        private readonly CoreDbContext _ctx;
        private readonly ApplicationService _app;

        public ViewController(ApplicationService appService, CoreDbContext ctx)
        {
            _app = appService;
            _ctx = ctx;
        }


        #region Menus

        [HttpPost]
        public ActionResult<int> GetBagdeItemsCount(string ListViewId)
        {
            if (!string.IsNullOrWhiteSpace(ListViewId))
                if (_app.GetListView(ListViewId) is ListView listView)
                    return listView.GetItemsCount(_app, _ctx);
            return 0;
        }

        #endregion


        #region DetailViews

        //[HttpPost, GenerateInstanceKey, ClearTrackedEntities]
        //public IActionResult GetDetail(string ViewId)
        //{
        //    try
        //    {
        //        var detailView = _app.GetDetailView(ViewId);

        //        detailView.ActivateInstance(_app, _ctx);
        //        TempData["CurrentViewId"] = detailView.ViewId;

        //        return RedirectToAction("New", detailView.ControllerName, new[] { new ViewInfo() { ViewId = ViewId } });
        //        //return PartialView("~/Views/Shared/Components/DetailView/Default.cshtml", detailView);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
        //    }
        //}

        #endregion

        #region ListViews

        //[HttpPost, GenerateInstanceKey, ClearTrackedEntities]
        //public IActionResult GetList(ViewInfo viewInfo, int page = 1, SearchOptions searchOptions = null)
        //{
        //    try
        //    {
        //        var listView = _app.GetListView(viewInfo.ViewId);

        //        listView.SetCurrentPage(_app, _ctx, page, searchOptions);
        //        TempData["CurrentViewId"] = listView.ViewId;

                
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
        //    }
        //}
        [HttpPost, GenerateInstanceKey, ClearTrackedEntities]
        public IActionResult ListView(ViewInfo viewInfo, int page = 1, SearchOptions searchOptions = null)
        {
            try
            {
                var listView = _app.GetListView(viewInfo.ViewId);

                listView.SetCurrentPage(_app, _ctx, page, searchOptions);
                TempData["CurrentViewId"] = listView.ViewId;

                if (viewInfo.ViewOrigin?.ToLower() == "menu")
                    return PartialView("~/Views/Shared/Components/ListView/Default.cshtml", listView);
                else
                    return PartialView("~/Views/Shared/Components/ListView/ViewForm.cshtml", listView);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        #endregion

        #region LookupViews

        [HttpPost]
        public IActionResult LookupView(ViewInfo viewInfo, SearchOptions searchOptions = null, PostBackOptions postBackOptions = null)
        {
            try
            {
                var view = _app.GetView(viewInfo.ParentViewId);
                var lookupModal = view.GetSubView(viewInfo.ModalId) as LookupModal;

                ((LookupView)lookupModal.View).PopulateView(_app, _ctx, searchOptions);
                ((LookupView)lookupModal.View).SetPostBackOptions(postBackOptions ?? new PostBackOptions() { PostbackActionName = "", PostbackFormId = "" });

                return PartialView("~/Views/Shared/Components/ModalView/LookupModalView_Form.cshtml", lookupModal);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        #endregion

    }
}