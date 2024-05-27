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
    public class AdminController : Controller
    {
         QLDTEntities1 db = new QLDTEntities1();
        // GET: Admin/Admin

        public ActionResult Index()
        {

            
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            return View();
        }

        public ActionResult Products()
        {


            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            return View(db.SANPHAMs.ToList());
        }

        public ActionResult Add()
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }


            return View(db.HANGSXes.ToList());
        }

        [HttpPost]
        public ActionResult Add(SANPHAM sanpham, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {
                string urlTuongDoi = "/images/";
                string urlTuyetDoi = Server.MapPath(urlTuongDoi);
                HINHANH.SaveAs(urlTuyetDoi + HINHANH.FileName);

                sanpham.HINHANH = HINHANH.FileName;
                db.SANPHAMs.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Products");
            }
            // Nếu ModelState.IsValid là false (có lỗi xác thực), trở lại form với thông báo lỗi
            return View();
        }

        public ActionResult Edit(int? ID)
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sanpham = db.SANPHAMs.Find(ID);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", sanpham);
        }
        

        [HttpPost]
        public ActionResult Edit(SANPHAM sanpham, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem sản phẩm với ID cụ thể có tồn tại trong cơ sở dữ liệu không
                SANPHAM existingProduct = db.SANPHAMs.Find(sanpham.ID);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin sản phẩm với dữ liệu từ biểu mẫu
                existingProduct.TENSP = sanpham.TENSP;
                existingProduct.IDHANG = sanpham.IDHANG;
                existingProduct.GIAGOC = sanpham.GIAGOC;
                existingProduct.GIA = sanpham.GIA;
                existingProduct.TINHTRANG = sanpham.TINHTRANG;

                string urlTuongDoi = "/images/";
                string urlTuyetDoi = Server.MapPath(urlTuongDoi);
                HINHANH.SaveAs(urlTuyetDoi + HINHANH.FileName);

                existingProduct.HINHANH = HINHANH.FileName;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return RedirectToAction("Products"); // Chuyển hướng về trang danh sách sản phẩm sau khi cập nhật thành công
            }

            // Nếu có lỗi, hiển thị biểu mẫu cập nhật lại
            return View("Edit", sanpham);
        }

        public ActionResult Delete(int? ID)
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SANPHAM sanpham = db.SANPHAMs.Find(ID);

            if (sanpham == null)
            {
                return HttpNotFound();
            }

            // Xóa sản phẩm từ cơ sở dữ liệu
            db.SANPHAMs.Remove(sanpham);
            db.SaveChanges();

            return RedirectToAction("Products");
        }


    }
}