using Models.Dao;
using Models.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string Seachstring,int Page = 1,int Pagesize = 10)
        {
            var Dao = new UserDao();
            var model = Dao.ListAllPading(Seachstring,Page, Pagesize);
            ViewBag.Seachstring = Seachstring;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int ID)
        {
            var Dao = new UserDao();
            var user = Dao.ViewDetail(ID);
            SetViewPage();
            return View(user);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }

            [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            var Dao = new UserDao();
            var encryptedMD5Pas = Encryptor.MD5Hash(user.PassWord);
            user.PassWord = encryptedMD5Pas;
            var result = Dao.Update(user);
            
            if(result)
            {
                SetAlert("Sửa tài khoản thành công", "success");
                return RedirectToAction("Index","User");
            }
            else
            {
                ModelState.AddModelError("", "Tạo tài khoản không thành công");
            }
            return View("Index");
        }


        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            var Dao = new UserDao();
            user.CreatedDate = DateTime.Now;
            var encryptedMD5Pas = Encryptor.MD5Hash(user.PassWord);
            user.PassWord = encryptedMD5Pas;
            user.GroupID = "MEMBER";
            bool ID = Dao.Insert(user);
            if (ID == true)
            {
                SetAlert("Tạo tài khoản thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else if(ID == false)
            {
                ModelState.AddModelError("", "Tạo tài khoản không thành công");
            }
            return View("Index");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int ID)
        {
            var Dao = new UserDao();
            Dao.Delete(ID);
            SetAlert("Xoá tài khoản thành công", "success");
            return RedirectToAction("Index");


        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var Dao = new UserDao();
            var result = Dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewPage(long? selectID = null)
        {
            var Dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(Dao.ListAll(), "ID", "Name", selectID);

        }
    }
}