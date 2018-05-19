﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAnQLSV.Models;

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
                   
                    return RedirectToAction("IndexLogin",  new {mssv = tk.SINHVIEN.MaSV});
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
    }
}