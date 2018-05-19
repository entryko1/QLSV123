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

        #region [Edit]
        [HttpGet]
        public ActionResult EditPoint(int id)
        {
            var diem = data.DIEMs.First(d => d.Stt == id);
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;
            ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            if (diem == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(diem);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPoint(int id, FormCollection collection)
        {
            var diem = data.DIEMs.First(d => d.Stt == id);
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;
            ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
  
            string stt = collection["Stt"];
            int Stt = int.Parse(stt);

            string hk = collection["HocKy"];
            int Hk = int.Parse(hk);

            string dl1 = collection["DiemLan1"];
            string dl2 = collection["DiemLan2"];
            string dtb = collection["DiemTB"];
            int DL1 = int.Parse(dl1);
            int DL2 = int.Parse(dl2);
            float DTB = float.Parse(dl1);

            diem.Stt = Stt;
            diem.MaSV = collection["MaSV"];
            diem.MaMH = collection["MaMH"];
            diem.HocKy = Hk;
            diem.DiemLan1 = DL1;
            diem.DiemLan2 = DL2;
            diem.DiemTB = DTB;

            UpdateModel(diem);
            data.SubmitChanges();


            return RedirectToAction("ViewPointStudent");
        }
        #endregion



        [HttpGet]
        public ActionResult DeletePoint(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;

            var diem = data.DIEMs.First(d => d.Stt == id);

            if (diem == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(diem);
        }


        [HttpPost, ActionName("DeletePoint")]
        public ActionResult ComfirmDeletePoint(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            ViewBag.Taikhoan = tk.Name;

            var diem = data.DIEMs.First(d => d.Stt == id);


            if (diem == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                data.DIEMs.DeleteOnSubmit(diem);
                data.SubmitChanges();
                return RedirectToAction("ViewPointStudent");
            }

           
              
           
           
        }
    }
}