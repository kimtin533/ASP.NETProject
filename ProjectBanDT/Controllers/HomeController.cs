using ProjectBanDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ProjectBanDT.Controllers
{
    public class HomeController : Controller
    {
        private QLDTEntities1 db = new QLDTEntities1();

        public ActionResult Index(string search, int? page)
        {
            int pageSize = 10; // Set the number of items per page
            int pageNumber = (page ?? 1);

            ViewBag.Search = search;

            if (!string.IsNullOrEmpty(search))
            {
                var sanPhamTimKiem = db.SANPHAMs
                    .Where(sp => sp.TENSP.Contains(search))
                    .OrderBy(sp => sp.ID)
                    .ToPagedList(pageNumber, pageSize);

                ViewBag.totalPageCount = sanPhamTimKiem.PageCount;
                ViewBag.PageNumber = pageNumber;

                return View(sanPhamTimKiem);
            }
            else
            {
                var tatCaSanPham = db.SANPHAMs
                    .OrderBy(sp => sp.ID)
                    .ToPagedList(pageNumber, pageSize);

                ViewBag.totalPageCount = tatCaSanPham.PageCount;
                ViewBag.PageNumber = pageNumber;

                return View(tatCaSanPham);
            }


        }

        public ActionResult Login()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult Login(string TENDANGNHAP, string MATKHAU)                          
        {
            
            var taiKhoan = db.TAIKHOANs.SingleOrDefault(m => m.USERNAME == TENDANGNHAP && m.PASSWORD == MATKHAU);
            if (taiKhoan != null)
            {
                
                if (taiKhoan.TYPE == 0)
                {
                    Session["IsUser"] = taiKhoan;
                    return RedirectToAction("Index");
                    

                }
                if(taiKhoan.TYPE == 1)

                {
                    Session["IsUserAdmin"] = taiKhoan;
                    return RedirectToAction("Index");
                    
                }

            }


            return RedirectToAction("Login");
        }

        public ActionResult SignUp()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(TAIKHOAN taiKhoan)
        {
            db.TAIKHOANs.Add(taiKhoan);
            db.SaveChanges();
            Session["user"] = taiKhoan;
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int? ID)
        {


            
                var product = db.SANPHAMs.SingleOrDefault(p => p.ID == ID);
                if (product != null)
                {

                    return View(product);
                }
                else
                {

                    return Redirect("Index");
                }

            
                return View();
        }


        //public ActionResult Cart()
        //{
        //    if (Session["IsUser"] != null)
        //    {
        //        TAIKHOAN user = (TAIKHOAN)Session["IsUser"];
        //        string username = user.USERNAME;

        //        // Thực hiện truy vấn bằng Entity Framework
        //        var cartItems = (from giohang in db.GIOHANGs
        //                         join sanpham in db.SANPHAMs on giohang.IDSANPHAM equals sanpham.ID
        //                         where giohang.IDTAIKHOAN == username
        //                         select new CartItemViewModel
        //                         {
        //                             TENSP = sanpham.TENSP,
        //                             GIA = sanpham.GIA,
        //                             HINHANH = sanpham.HINHANH,
        //                             SOLUONG = giohang.SOLUONG
        //                         }).ToList();

        //        // Đổ dữ liệu vào view và trả về view "Cart" với danh sách ViewModel
        //        return View(cartItems);
        //    }
        //    else
        //    {
        //        // Nếu session "IsUser" không tồn tại hoặc có giá trị là null, bạn có thể xử lý logic khác, hoặc chuyển hướng người dùng về trang đăng nhập, ví dụ:
        //        return RedirectToAction("Login");
        //    }

        //}


        public ActionResult Cart()
        {
            if (Session["IsUser"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["IsUser"];
                string username = user.USERNAME;


                var cartItems = (from giohang in db.GIOHANGs
                                 join sanpham in db.SANPHAMs on giohang.IDSANPHAM equals sanpham.ID
                                 where giohang.IDTAIKHOAN == username
                                 select new CartItemViewModel
                                 {
                                     TENSP = sanpham.TENSP,
                                     GIA = sanpham.GIA,
                                     HINHANH = sanpham.HINHANH,
                                     SOLUONG = giohang.SOLUONG,
                                     ID = sanpham.ID
                                 }).ToList();


                double tongGia = cartItems.Sum(item => item.GIA * item.SOLUONG);


                ViewBag.TongGia = tongGia;

                return View(cartItems);
            }
            else
            {
              return RedirectToAction("Login");
            }
        }

        public ActionResult RemoveFromCart(int productID)
        {
            if (Session["IsUser"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["IsUser"];
                string username = user.USERNAME;


                var productInCart = db.GIOHANGs.FirstOrDefault(g => g.IDSANPHAM == productID && g.IDTAIKHOAN == username);

                if (productInCart != null)
                {

                    db.GIOHANGs.Remove(productInCart);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Cart");
        }






        [HttpPost]
        public ActionResult AddToCart(int IDSP, int SL)
        {

            if (Session["IsUser"] == null)
            {
                return RedirectToAction("Login");
            }
            TAIKHOAN user = (TAIKHOAN)Session["IsUser"];

            var userID = user.USERNAME;

            var gioHang = new GIOHANG
            {
                IDTAIKHOAN = userID,
                IDSANPHAM = IDSP,
                SOLUONG = SL
            };



            db.GIOHANGs.Add(gioHang);
            db.SaveChanges();

            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult CreateInvoice()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa. Nếu chưa đăng nhập, bạn có thể chuyển hướng hoặc xử lý một cách phù hợp.

            if (Session["IsUser"] == null)
            {
                return RedirectToAction("Login"); // Chuyển hướng đến trang đăng nhập hoặc xử lý khác.
            }

            // Lấy thông tin người dùng từ Session
            TAIKHOAN user = (TAIKHOAN)Session["IsUser"];
            string userID = user.USERNAME;

            // Lấy giỏ hàng của người dùng từ cơ sở dữ liệu
            var gioHang = db.GIOHANGs.Where(g => g.IDTAIKHOAN == userID).ToList();

            // Tạo một hóa đơn mới
            var hoadon = new HOADON
            {
                IDTAIKHOAN = userID,
                NGAYMUA = DateTime.Now, // Sử dụng thời gian hiện tại làm ngày mua
                TINHTRANG = "Chờ xác nhận" // Có thể cần điều chỉnh tùy theo yêu cầu của bạn.
            };
            db.HOADONs.Add(hoadon);
            db.SaveChanges();
            // Thêm hóa đơn vào cơ sở dữ liệu

            int hoadonID = hoadon.ID;

            // Sao chép các sản phẩm từ giỏ hàng vào chi tiết hóa đơn
            foreach (var item in gioHang)
            {
                var tthoadon = new TTHOADON
                {
                    IDHOADON = hoadonID,
                    IDSANPHAM = item.IDSANPHAM,
                    SOLUONG = item.SOLUONG
                };

                db.TTHOADONs.Add(tthoadon);
            }


            // Lưu các thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Xóa sản phẩm từ giỏ hàng
            db.GIOHANGs.RemoveRange(gioHang);
            db.SaveChanges();

            // Điều hướng người dùng đến trang xác nhận đặt hàng hoặc trang hóa đơn đã tạo
            return RedirectToAction("Done"); // Điều hướng đến trang xác nhận đặt hàng hoặc trang hóa đơn đã tạo.
        }





        public ActionResult Done()
        {
            return View();
        }





        public ActionResult Search(string search)
        {

            if (!string.IsNullOrEmpty(search))
            {
                var result = db.SANPHAMs.Where(tt => tt.TENSP.Contains(search)).ToList();
                return View(result);
            }
    
            return View();
        }

        

        public ActionResult Logout()
        {
            Session["IsUser"] = null;
            Session["IsUserAdmin"] = null;
            return RedirectToAction("Index");
        }

    }
}