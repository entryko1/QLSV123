using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAnQLSV.Models;
using PagedList;

namespace DoAnQLSV.Controllers
{
    public class LoginUserController : Controller
    {
        dbQLSinhVienDataContext data = new dbQLSinhVienDataContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["Username"];

            var matkhau = collection["Password"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Bạn chưa gõ tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Bạn chưa gõ mật khẩu";
            }
            else
            {
                TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TenDN == tendn && n.MatKhau == matkhau);

                if (tk != null)
                {
                    Session["TaikhoanSV"] = tk;

                    return RedirectToAction("IndexLogin", new { mssv = tk.SINHVIEN.MaSV });
                }
                else
                {
                    ViewBag.Thongbao = "Bạn gõ sai tên đăng nhập hoặc mật khẩu !";
                }
            }

            return View();
        }

        public ActionResult IndexLogin(string mssv)
        {

            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TenDN == mssv);
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            return View();
        }


        public ActionResult Xemdiem()
        {

            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            var diem = from s in data.DIEMs where s.MaSV == tk.TenDN select s;



            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;

            return View(diem);
        }

        public ActionResult DoingugiangvienAdmin()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            return View();
        }
        public ActionResult ChuongtrinhhocAdmin()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            return View();
        }
        public ActionResult DiachiAdmin()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            return View();
        }

        public ActionResult Thongtincanhan()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            var sv = from s in data.SINHVIENs where s.MaSV == tk.TenDN select s;
            return View(sv.Single());
        }
        [HttpGet]
        //cap nhat thong tin nguoi dung
        public ActionResult CapNhatThongTinSV(string MaSV)
        {

            //lấy đối tượng là mã sinh viên
            SINHVIEN sv = data.SINHVIENs.SingleOrDefault(n => n.MaSV == MaSV);
            if (sv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sv);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CapNhatThongTinSV(SINHVIEN sv)
        {
            SINHVIEN sinhvien = data.SINHVIENs.SingleOrDefault(n => n.MaSV == sv.MaSV);
            sinhvien.GioiTinh = sv.GioiTinh;
            sinhvien.NgaySinh = sv.NgaySinh;
            sinhvien.QueQuan = sv.QueQuan;
            sinhvien.SDT = sv.SDT;
            sinhvien.NoiThuongTru = sv.NoiThuongTru;
            sinhvien.CMND = sv.CMND;



            if (ModelState.IsValid)
            {
                //SINHVIEN sinhvien = data.SINHVIENs.SingleOrDefault(n => n.MaSV == sv.MaSV);
                //sinhvien.GioiTinh = sv.GioiTinh;
                //sinhvien.NgaySinh = sv.NgaySinh;
                //sinhvien.QueQuan = sv.QueQuan;
                //sinhvien.SDT = sv.SDT;
                //sinhvien.NoiThuongTru = sv.NoiThuongTru;
                //sinhvien.CMND = sv.CMND;
                UpdateModel(sv);
                data.SubmitChanges();
                return RedirectToAction("Thongtincanhan");
            }
            return View(sv);


        }
        //tìm kiếm sinh viên bên ngoài trang chủ
        [HttpPost]
        public ActionResult TimkiemSV(FormCollection f, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            String sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<DIEM> lstKQTK = data.DIEMs.Where(n => n.MaSV.Contains(sTuKhoa)).ToList();
            foreach (Char c in sTuKhoa)
            {
                if (!Char.IsDigit(c))
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.SINHVIEN.TenSV.Contains(sTuKhoa));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.MaSV.Contains(sTuKhoa));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }
            }


            return View(lstKQTK.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult TimkiemSV(string sTuKhoa, int? page)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<DIEM> lstKQTK = data.DIEMs.Where(n => n.MaSV.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 3;


            ViewBag.TuKhoa = sTuKhoa;

            foreach (Char c in sTuKhoa)
            {
                if (!Char.IsDigit(c))
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.SINHVIEN.TenSV.Contains(sTuKhoa));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.MaSV.Contains(sTuKhoa));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }
            }


            return View(lstKQTK.ToPagedList(pageNumber, pageSize));

        }

    }
}
