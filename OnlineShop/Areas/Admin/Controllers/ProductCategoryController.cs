using Models.Dao;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var Dao = new ProductCategoryDao();
            var result = Dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public ActionResult Edit(long id)
        {
            var Dao = new ProductCategoryDao();
            var product = Dao.GetById(id);
            SetViewPage(id);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory model)
        {
            new ProductCategoryDao().Update(model);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            SetViewPage();
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory model)
        {
            new ProductCategoryDao().Create(model);
            return RedirectToAction("Index");
        }
        
        [HttpDelete]

        public ActionResult Delete(int ID)
        {
            var Dao = new ProductDao();
            Dao.Delete(ID);
            SetAlert("Xoá tài khoản thành công", "success");
            return RedirectToAction("Index");


        }
        public void SetViewPage(long? selectID = null)
        {
            var Dao = new ProductCategoryDao();
            ProductCategory Null = new ProductCategory()
            {
                ID = 0,
                Name= "Không"
            };

            var ListCategory = Dao.ListAll();
            ListCategory.Insert(0, Null);
            ViewBag.ParentID = new SelectList(ListCategory, "ID", "Name", selectID);

        }
        
    }
}