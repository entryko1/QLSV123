﻿using System;
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
            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;
            return View();
        }


        public ActionResult Xemdiem()
        {

            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            var diem = from s in data.DIEMs where s.MaSV == tk.TenDN select s;

            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;

            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;

            return View(diem);
        }

        public ActionResult DoingugiangvienAdmin(int id, int? page)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChuongtrinhhocAdmin(int id, int? page)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DiachiAdmin()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;
            return View();
        }

        public ActionResult Thongtincanhan()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;
            var sv = from s in data.SINHVIENs where s.MaSV == tk.TenDN select s;
            return View(sv.Single());
        }



        public ActionResult ChitietBaivietAdmin(int id)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            ViewBag.Tendangnhap = tk.TenDN;
            ViewBag.Matkhau = tk.MatKhau;
            BAIVIET bv = data.BAIVIETs.SingleOrDefault(n => n.IdBV == id);
            if (bv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bv);
        }



        [HttpGet]
        //cap nhat thong tin nguoi dung
        public ActionResult CapNhatThongTinSV(string MaSV)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
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
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            SINHVIEN sinhvien = data.SINHVIENs.SingleOrDefault(n => n.MaSV == sv.MaSV);
            sinhvien.GioiTinh = sv.GioiTinh;
            sinhvien.NgaySinh = sv.NgaySinh;
            sinhvien.QueQuan = sv.QueQuan;
            sinhvien.SDT = sv.SDT;
            sinhvien.NoiThuongTru = sv.NoiThuongTru;
            sinhvien.CMND = sv.CMND;



            if (ModelState.IsValid)
            {

                UpdateModel(sv);
                data.SubmitChanges();
                return RedirectToAction("Thongtincanhan");
            }
            return View(sv);


        }



        [HttpPost]
        public ActionResult TimkiemSV(FormCollection f/*, int? page*/)
        {
            //int pageNumber = (page ?? 1);
            //int pageSize = 3;
            String sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<DIEM> lstKQTK = data.DIEMs.Where(n => n.MaSV.Contains(sTuKhoa)).ToList();
            foreach (Char c in sTuKhoa)
            {
                if (!Char.IsDigit(c))
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.SINHVIEN.TenSV.Contains(sTuKhoa));
                    SINHVIEN sv = data.SINHVIENs.SingleOrDefault(s => s.TenSV.Contains(sTuKhoa));
                    ViewBag.TenSV = sv.TenSV;
                    ViewBag.Mssv = sv.MaSV;
                    ViewBag.Gioitinh = sv.GioiTinh;
                    ViewBag.Ngaysinh = sv.NgaySinh;
                    ViewBag.Noisinh = sv.QueQuan;
                    ViewBag.Lop = sv.LOP.TenLop;
                    ViewBag.Nganh = sv.ChuyenNganh;
                    ViewBag.Khoa = sv.KHOA.TenKhoa;
                    ViewBag.Hedaotao = sv.LOP.HEDAOTAO.TenHDT;
                    ViewBag.Khoahoc = sv.KHOAHOC.TenKhoaHoc;

                    return View(filter/*.ToPagedList(pageNumber, pageSize)*/);
                }
                else
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.MaSV.Contains(sTuKhoa));
                    SINHVIEN sv = data.SINHVIENs.SingleOrDefault(s => s.MaSV.Contains(sTuKhoa));
                    ViewBag.TenSV = sv.TenSV;
                    ViewBag.Mssv = sv.MaSV;
                    ViewBag.Gioitinh = sv.GioiTinh;
                    ViewBag.Ngaysinh = sv.NgaySinh;
                    ViewBag.Noisinh = sv.QueQuan;
                    ViewBag.Lop = sv.LOP.TenLop;
                    ViewBag.Nganh = sv.ChuyenNganh;
                    ViewBag.Khoa = sv.KHOA.TenKhoa;

                    ViewBag.Khoahoc = sv.KHOAHOC.TenKhoaHoc;
                    return View(filter/*.ToPagedList(pageNumber, pageSize)*/);
                }
            }


            return View(lstKQTK/*.ToPagedList(pageNumber, pageSize)*/);
        }
        [HttpGet]
        public ActionResult TimkiemSV(string sTuKhoa/*, int? page*/)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<DIEM> lstKQTK = data.DIEMs.Where(n => n.MaSV.Contains(sTuKhoa)).ToList();
            //int pageNumber = (page ?? 1);
            //int pageSize = 3;
            
            ViewBag.TuKhoa = sTuKhoa;

            foreach (Char c in sTuKhoa)
            {
                if (!Char.IsDigit(c))
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.SINHVIEN.TenSV.Contains(sTuKhoa));

                    SINHVIEN sv = data.SINHVIENs.SingleOrDefault(s => s.TenSV.Contains(sTuKhoa));
                    ViewBag.TenSV = sv.TenSV;
                    ViewBag.Mssv = sv.MaSV;
                    ViewBag.Gioitinh = sv.GioiTinh;
                    ViewBag.Ngaysinh = sv.NgaySinh;
                    ViewBag.Noisinh = sv.QueQuan;
                    ViewBag.Lop = sv.LOP.TenLop;
                    ViewBag.Nganh = sv.ChuyenNganh;
                    ViewBag.Khoa = sv.KHOA.TenKhoa;
                    ViewBag.Hedaotao = sv.LOP.HEDAOTAO.TenHDT;
                    ViewBag.Khoahoc = sv.KHOAHOC.TenKhoaHoc;
                    return View(filter/*.ToPagedList(pageNumber, pageSize)*/);
                }
                else
                {
                    var filter = from d in data.DIEMs select d;
                    filter = filter.Where(a => a.MaSV.Contains(sTuKhoa));

                    SINHVIEN sv = data.SINHVIENs.SingleOrDefault(s => s.MaSV.Contains(sTuKhoa));
                    ViewBag.TenSV = sv.TenSV;
                    ViewBag.Mssv = sv.MaSV;
                    ViewBag.Gioitinh = sv.GioiTinh;
                    ViewBag.Ngaysinh = sv.NgaySinh;
                    ViewBag.Noisinh = sv.QueQuan;
                    ViewBag.Lop = sv.LOP.TenLop;
                    ViewBag.Nganh = sv.ChuyenNganh;
                    ViewBag.Khoa = sv.KHOA.TenKhoa;
                    ViewBag.Hedaotao = sv.LOP.HEDAOTAO.TenHDT;
                    ViewBag.Khoahoc = sv.KHOAHOC.TenKhoaHoc;
                    return View(filter/*.ToPagedList(pageNumber, pageSize)*/);
                }
            }


            return View(lstKQTK/*.ToPagedList(pageNumber, pageSize)*/);

        }
        //thay đổi mật khẩu cho sinh viên
        [HttpGet]

        public ActionResult EditPassStudent(string tendangnhap)
        {
            string user = tendangnhap;
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.SINHVIEN.TenSV;
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Tendangnhap = tk.TenDN;
                ViewBag.Matkhau = tk.MatKhau;
                
                var pw = data.TAIKHOANs.First(p => p.TenDN.Contains(tk.TenDN));
                if (pw == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPassStudent(string tendangnhap, FormCollection collection)
        {
            string usersv = tendangnhap;
            TAIKHOAN tk = (TAIKHOAN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Tendangnhap = tk.TenDN;
                ViewBag.Matkhau = tk.MatKhau;
               

                var pw = data.TAIKHOANs.First(p => p.TenDN.Contains(usersv));
                string oldpw = collection["OldPassword"];
                string pass = collection["Password"];
                string repass = collection["RePassword"];


                if (oldpw != pw.MatKhau)
                {
                    ViewBag.Erorr1 = "Mật khẩu không đúng!!!";
                    return View("EditPassStudent");
                }
                else
                {
                    if (repass != pass)
                    {
                        ViewBag.Error = "Mật khẩu nhập lại không chính xác";
                        return View("EditPassStudent");

                    }
                    else
                    {
                        pw.TenDN = usersv;
                        pw.MatKhau = pass;


                        UpdateModel(pw);
                        data.SubmitChanges();
                        return RedirectToAction("Login");
                    }
                }
            }


        }
    }
}





    