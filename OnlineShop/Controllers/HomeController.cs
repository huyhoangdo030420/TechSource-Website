using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Dao;
using Models.EF;
using OnlineShop.Common;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(long? manufacturer, string price, long? category)
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();
            var producerDao = new ProducerDao();
            var categoryDao = new ProductCategoryDao();
            var manufacturers = producerDao.ViewDetail(manufacturer);
            var categorys = categoryDao.ViewDetail(category);
            ViewBag.NewProducts = productDao.ListNewProduct( manufacturers, price, categorys, 4);
            ViewBag.FeatureProducts = productDao.ListFeatureProduct( manufacturers, price, categorys,4);
            
            ViewBag.category = categoryDao.ViewPageList();
            ViewBag.manufacturer = producerDao.ListAll();
            ViewBag.Title = ConfigurationManager.AppSettings["HomeTitle"];
            ViewBag.Keywords = ConfigurationManager.AppSettings["HomeKeywords"];
            ViewBag.Descriptions = ConfigurationManager.AppSettings["HomeDiscription"];
            return View();
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult MainMenu()
        {
            var Dao = new MenuDao();
            var model = Dao.ListByGroupId(1);
            return PartialView(model);
        }

        [ChildActionOnly]
       
        public ActionResult TopMenu()
        {
            var Dao = new MenuDao();
            var model = Dao.ListByGroupId(2);
            return PartialView(model);
        }
        
        public PartialViewResult HeaderCart()
        {
            var Cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (Cart != null)
            {
                list = (List<CartItem>)Cart;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600*24)]
        public ActionResult Footer()
        {
            var Dao = new FooterDao();
            var model = Dao.GetFooter();
            return PartialView(model);
        }
    }
}