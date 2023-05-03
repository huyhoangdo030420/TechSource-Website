using Models.Dao;
using Models.EF;
using OnlineShop.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProducerController : BaseController
    {
        // GET: Producer
        [HttpGet]
        public ActionResult Index(string Seachstring, int Page = 1, int Pagesize = 10)
        {
            var Dao = new ProducerDao();
            var model = Dao.ListAllPading(Seachstring, Page, Pagesize);
            ViewBag.Seachstring = Seachstring;
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            var Dao = new ProducerDao();
            var product = Dao.GetById(id);

            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Producer model)
        {
            new ProducerDao().Update(model);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Producer model)
        {
            new ProducerDao().Create(model);
            return RedirectToAction("Index");
        }
        [HttpPost]

        public JsonResult ChangeStatus(long id)
        {
            var Dao = new ProducerDao();
            var result = Dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpDelete]

        public ActionResult Delete(int ID)
        {
            var Dao = new ProducerDao();
            Dao.Delete(ID);
            SetAlert("Xoá tài khoản thành công", "success");
            return RedirectToAction("Index");


        }
    }
}