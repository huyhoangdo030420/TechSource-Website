using Common;
using Model.Dao;
using Models.Dao;
using Models.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OnlineShop.Hubs;
using CommonConstants = OnlineShop.Common.CommonConstants;
using Microsoft.AspNet.SignalR;
using System.Web.UI.WebControls.WebParts;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        
        public ActionResult Index()
        {
            var Cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if(Cart != null)
            {
                list = (List<CartItem>)Cart;
            }
            return View(list);
        }
        public JsonResult DeleteAll()
        {
          
            Session[CommonConstants.CartSession] = null;
            return Json(new { status = true });
        }
        public JsonResult Delete(long id)
        {

            var CartSession = (List<CartItem>)Session[CommonConstants.CartSession];
            CartSession.RemoveAll(x => x.Product.ID == id);
            Session[CommonConstants.CartSession] = CartSession;
            return Json(new { status = true });
        }
        public JsonResult Update(string cartModel)
        {
            var ListCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var CartSession = (List<CartItem>)Session[CommonConstants.CartSession];
            foreach(var item in CartSession)
            {
                var CartItem = ListCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if(CartItem != null)
                {
                     item.Quantity = CartItem.Quantity ;
                }
                
            }
            Session[CommonConstants.CartSession] = CartSession;
            return Json(new { status = true });
        }
        public ActionResult AddItem(long productId, int quantity )
        {
            var Cart = Session[CommonConstants.CartSession];
            
            if(Cart != null)
            {
                var list = (List<CartItem>)Cart; ;
                
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }

                else
                {
                    var item = new CartItem();
                    item.Quantity = quantity;
                    item.Product = new ProductDao().ViewDetail(productId);
                    list.Add(item);
                }
                Session[CommonConstants.CartSession] = list;
            }
            else
            {
                 var item = new CartItem();
                item.Quantity = quantity;
                item.Product = new ProductDao().ViewDetail(productId);
                var list = new List<CartItem>();
                list.Add(item);
                Session[CommonConstants.CartSession] = list;
            }
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CommonConstants.CartSession];
            var user = Session[CommonConstants.userSession];
            var list = new List<CartItem>();
            if (user != null)
            {
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                }
            }
            else
            {
                return RedirectToAction("Login","User");
            }
            
            
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;
            order.Status = null;
            order.ShowOnHome = false;
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CommonConstants.CartSession];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    if(item.Product.IncludedVAT == true)
                    {
                        orderDetail.Price = item.Product.PromotionPrice.Value;
                    }
                    else
                    {
                        orderDetail.Price = item.Product.Price;
                    }
                    
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                    total += (item.Product.Price * item.Quantity);

                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
            }
            catch (Exception ex)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<DemoHub>();
           
            hubContext.Clients.All.GetAll(new OrderDao().ListAll());
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
           
            return View();
        }
    }
}
