using Models.Dao;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var Dao = new UserDao();
                var result = Dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);
                if(result == 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = Dao.GetByID(model.UserName).UserName;
                    userSession.UserID = Dao.GetByID(model.UserName).ID;
                    var listCredential = Dao.GetListCredential(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonConstants.userSession, userSession);
                   return RedirectToAction("Index","Home");
                }
                else if (result == -1)
                {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if(result == 1)
                {
                    ModelState.AddModelError("", "Mật khẩu đăng nhập sai");
                }else if(result == -2)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn bị khoá");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập");
                }
            }
            
            return View("Index");
        }
    }
}