using Models.Dao;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            var Dao = new ProductDao();
            var product = Dao.GetById(id);
            SetViewPage(product.CategoryID);
            SetViewPageProducer();
            return View(product);
        }
        [HttpPost]
        [ValidateInput (false)]
        public ActionResult Edit(Product model) 
        {
            new ProductDao().Update(model);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            SetViewPage();
            SetViewPageProducer();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product model)
        {
            new ProductDao().Create(model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var Dao = new ProductDao();
            var result = Dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpDelete]
        
        public ActionResult Delete(int ID)
        {
            var Dao = new ProductDao();
            Dao.Delete(ID);
            SetAlert("Xoá tài khoản thành công", "success");
            return RedirectToAction("Index");


        }

        public ActionResult IndexPromotion(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            var model = dao.ListPromotion(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        public ActionResult EditPromotion(long id)
        {
            var Dao = new ProductDao();
            var product = Dao.GetById(id);
            SetViewPage(product.CategoryID);
            SetViewPageProducer();
            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPromotion(Product model)
        {
            new ProductDao().Update(model);
            return RedirectToAction("IndexPromotion");
        }
        public ActionResult CreatePromotion()
        {
            SetViewPage();
            SetViewPageProducer();
            return View();
        }
        [HttpPost]
        public ActionResult CreatePromotion(Product model)
        {
            new ProductDao().CreatePromotion(model);
            return RedirectToAction("IndexPromotion");
        }

        public ActionResult CategoryIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        public void SetViewPage(long? selectID = null)
        {
            var Dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(Dao.ListAll(), "ID", "Name", selectID);

        }
        public void SetViewPageProducer(long? selectID = null)
        {
            var Dao = new ProducerDao();


            var ListCategory = Dao.ListAll();

            ViewBag.Producer = new SelectList(ListCategory, "ID", "Name", selectID);

        }
    }
}