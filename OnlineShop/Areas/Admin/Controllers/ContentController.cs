using Models.Dao;
using Models.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var Dao = new ContentDao();
            var content = Dao.GetById(id);
            SetViewPage(content.CategoryID);
            return View(content);
        }
        [HttpDelete]
        public ActionResult Delete(int ID)
        {
            var Dao = new ContentDao();
            Dao.Delete(ID);
            SetAlert("Xoá tài khoản thành công", "success");
            return RedirectToAction("Index");


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Content model)
        {
            new ContentDao().Update(model);
            
            
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {

            SetViewPage();
            return View();
        }
        public JsonResult ChangeStatus(long id)
        {
            var Dao = new ContentDao();
            var result = Dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.userSession];
                model.CreatedBy = session.UserName;
                var culture = Session[CommonConstants.CurrentCulture];
                model.Language = culture.ToString();
                
                new ContentDao().Create(model);
                return RedirectToAction("Index");
            }
            SetViewPage();
            return View();
        }
        public void SetViewPage(long? selectID = null)
        {
            var Dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(Dao.ListAll(), "ID", "Name", selectID);

        }
    }
}