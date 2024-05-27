using ProjectBanDT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectBanDT.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        QLDTEntities1 db = new QLDTEntities1();
        public ActionResult Index()
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            return View(db.TAIKHOANs.ToList());
        }

        public ActionResult Add()
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Add(TAIKHOAN taiKhoan)
        {

            db.TAIKHOANs.Add(taiKhoan);
            db.SaveChanges();
            Session["user"] = taiKhoan;
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string ID)
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }

            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN taiKhoan = db.TAIKHOANs.Find(ID);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", taiKhoan);
        }

        [HttpPost]
        public ActionResult Edit(TAIKHOAN taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        public ActionResult Delete(string ID)
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TAIKHOAN taiKhoan = db.TAIKHOANs.Find(ID);

            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            // Xóa sản phẩm từ cơ sở dữ liệu
            db.TAIKHOANs.Remove(taiKhoan);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}