using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnQLSV.Models;
using PagedList;

namespace DoAnQLSV.Controllers
{
    public class AdminController : Controller
    {
        dbQLSinhVienDataContext data = new dbQLSinhVienDataContext();
        // GET: Admin
        [HttpGet]
        public ActionResult LoginAdminStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdminStudent(FormCollection collection)
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
                ADMIN tk = data.ADMINs.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);

                if (tk != null && tk.Role ==2)
                {
                    Session["TaikhoanSV"] = tk;

                    return RedirectToAction("IAdminStudent");
                }
                if (tk != null && tk.Role == 1)
                {
                    Session["TaikhoanSV"] = tk;

                    return RedirectToAction("IAdminITU");
                }
                else
                {
                    ViewBag.Thongbao = "Bạn gõ sai tên đăng nhập hoặc mật khẩu !";
                }
            }

            return View();
        }

        public ActionResult IAdminStudent()
        {
            ADMIN tk = (ADMIN) Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;
            return View();
        }

        [HttpGet]
        public ActionResult AddPointStudents()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;
            ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPointStudents(DIEM diem)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;
            ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            data.DIEMs.InsertOnSubmit(diem);
            data.SubmitChanges();
            return RedirectToAction("AddPointStudents");
        }



        public ActionResult ViewPointStudent(int? page, string timkiem)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            var filter = from d in data.DIEMs select d;
            if (!string.IsNullOrEmpty(timkiem))
            {
                filter = filter.Where(a => a.MaSV.Contains(timkiem));
                return View(filter.ToPagedList(pageNumber, pageSize));
            }

            return View(data.DIEMs.ToList().OrderBy(n => n.Stt).ToPagedList(pageNumber, pageSize));
        }
  
    }
}