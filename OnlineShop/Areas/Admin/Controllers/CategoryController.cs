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
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            
            ViewBag.SearchString = searchString;

            return View(model);
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var Dao = new CategoryDao();
            var result = Dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public ActionResult Edit(long id)
        {
            var Dao = new CategoryDao();
            var product = Dao.GetById(id);
            SetViewPage(id);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Category model)
        {
            new CategoryDao().Update(model);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            SetViewPage();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                var currentCulture = Session[CommonConstants.CurrentCulture];
                model.Language = currentCulture.ToString();
                var id = new CategoryDao().Insert(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.InsertCategoryFailed);
                }
            }
            return View(model);
        }
        public ActionResult Delete(int ID)
        {
            var Dao = new CategoryDao();
            Dao.Delete(ID);
            SetAlert("Xoá tài khoản thành công", "success");
            return RedirectToAction("Index");


        }
        public void SetViewPage(long? selectID = null)
        {
            var Dao = new CategoryDao();
            Category Null = new Category()
            {
                ID = 0,
                Name = "Không"
            };

            var ListCategory = Dao.ListAll();
            ListCategory.Insert(0, Null);
            ViewBag.ParentID = new SelectList(ListCategory, "ID", "Name", selectID);

        }

    }
}