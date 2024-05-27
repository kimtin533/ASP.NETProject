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
    public class BillController : Controller
    {

        QLDTEntities1 db = new QLDTEntities1();
        // GET: Admin/Bill
        public ActionResult Index()
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            return View(db.HOADONs.ToList());
        }
        
        public ActionResult Comfirm(int? ID)
        {
            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hoaDon = db.HOADONs.Find(ID);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            hoaDon.TINHTRANG = "Đã xác nhận";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Info(int? IDHOADON)
        {

            if (Session["IsUserAdmin"] == null)
            {
                return Redirect("~/Home/Index");
            }
            if (IDHOADON == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy hóa đơn dựa trên ID
            HOADON hoaDon = db.HOADONs.Find(IDHOADON);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách chi tiết hóa đơn cho hóa đơn này
            List<TTHOADON> chiTietHoaDon = db.TTHOADONs.Where(t => t.IDHOADON == IDHOADON).ToList();

            // Lấy danh sách sản phẩm liên quan từ danh sách chi tiết hóa đơn
            var danhSachSanPham = (from sanPham in db.SANPHAMs
                                   join chiTiet in db.TTHOADONs
                                   on sanPham.ID equals chiTiet.IDSANPHAM
                                   where chiTiet.IDHOADON == IDHOADON
                                   select sanPham).ToList();

            ViewBag.HoaDon = hoaDon;
            ViewBag.ChiTietHoaDon = chiTietHoaDon;
            ViewBag.DanhSachSanPham = danhSachSanPham;

            return View("Info");
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

            // Tìm hóa đơn dựa trên ID
            HOADON hoaDon = db.HOADONs.Find(ID);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách chi tiết hóa đơn cho hóa đơn này
            List<TTHOADON> chiTietHoaDon = db.TTHOADONs.Where(t => t.IDHOADON == ID).ToList();

            // Xóa hóa đơn
            db.HOADONs.Remove(hoaDon);

            // Xóa chi tiết hóa đơn
            db.TTHOADONs.RemoveRange(chiTietHoaDon);

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            return RedirectToAction("Index");
        }






    }


}