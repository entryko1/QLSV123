using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnQLSV.Models;
using PagedList;
using System.IO;

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
            if(tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                return View();
            }
        
        }

        [HttpGet]
        public ActionResult AddPointStudents()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                return View();
            }
         
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPointStudents(DIEM diem)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                data.DIEMs.InsertOnSubmit(diem);
                data.SubmitChanges();
                return RedirectToAction("AddPointStudents");
            }
          
        }


        public ActionResult ViewNews(int? page, string timkiem, string Id)
        {
           
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                var filter = from d in data.BAIVIETs select d;
                ViewBag.Id = new SelectList(data.DANHMUCs.ToList().OrderBy(n => n.Id), "Id", "TenDM");
                ViewBag.SearchId = Id;
                ViewBag.SearchString = timkiem;
                if ((!string.IsNullOrEmpty(timkiem) && Id == "") || (!string.IsNullOrEmpty(timkiem) && Id == null))
                {

                    filter = filter.Where(a => a.TieuDe.Contains(timkiem));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }
                if (string.IsNullOrEmpty(timkiem) && Id != "" && Id != null)
                {

                    filter = filter.Where(a => a.Id.Equals(Id));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }
                if (!string.IsNullOrEmpty(timkiem) && Id != "" && Id != null)
                {
                    filter = filter.Where(a => a.TieuDe.Contains(timkiem) && a.Id.Equals(Id));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }

                if (Id == "" && string.IsNullOrEmpty(timkiem))
                {
                    return View(data.BAIVIETs.ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
                }
                return View(data.BAIVIETs.ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
            }
           
            
        }

     
        public ActionResult ViewPointStudent(int? page, string timkiem)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
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

        #region [Edit]
        [HttpGet]
        public ActionResult EditPoint(int id)
        {
            
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                var diem = data.DIEMs.First(d => d.Stt == id);
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                ViewBag.MaMH = new SelectList(data.MONHOCs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
                if (diem == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(diem);
            }
          
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPoint(int id, FormCollection collection)
        {
           
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                var diem = data.DIEMs.First(d => d.Stt == id);
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
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
         
        }
        #endregion



        [HttpGet]
        public ActionResult DeletePoint(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                var diem = data.DIEMs.First(d => d.Stt == id);

                if (diem == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(diem);
            }
         
        }


        [HttpPost, ActionName("DeletePoint")]
        public ActionResult ComfirmDeletePoint(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
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



        public ActionResult IAdminITU()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                return View();
            }
         
        }


        [HttpGet]
        public ActionResult PostNews()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                ViewBag.Id = new SelectList(data.DANHMUCs.ToList().OrderBy(n => n.Id), "Id", "TenDM");
                return View();
            }
          
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostNews(BAIVIET bv, HttpPostedFileBase fileupload)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                ViewBag.Id = new SelectList(data.DANHMUCs.ToList().OrderBy(n => n.Id), "Id", "TenDM");


                if (fileupload == null)
                {
                    ViewBag.ThongBaoIma = "Vui lòng chọn ảnh !";
                    return View();
                }
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                   
                    var path = Path.Combine(Server.MapPath("~/images/imagesNews"), fileName);


                    fileupload.SaveAs(path);


                    bv.HinhAnh = fileName;

                    data.BAIVIETs.InsertOnSubmit(bv);
                    data.SubmitChanges();


                }
                return RedirectToAction("IAdminITU");
            }
        
            
        }




        [HttpGet]
        public ActionResult EditNews(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;

                var bv = data.BAIVIETs.First(m => m.IdBV == id);
                if (bv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(bv);
            }
          
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNews(int id, FormCollection collection, HttpPostedFileBase fileupload)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }

            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;


                var bv = data.BAIVIETs.First(m => m.IdBV == id);
                BAIVIET b = data.BAIVIETs.First(m => m.IdBV == id);
                if (fileupload == null)
                {
                    ViewBag.ThongBaoIma = "Vui lòng chọn ảnh !";
                    return View();
                }
                
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/images/imagesNews"), fileName);
                fileupload.SaveAs(path);
                string hinhanh = fileName;

                bv.IdBV = b.IdBV;
                bv.Id = b.Id;
                bv.TieuDe = collection["TieuDe"];
                bv.HinhAnh = hinhanh;
                bv.TomTat = collection["TomTat"];
                bv.NgayViet = DateTime.Parse(collection["NgayViet"]);
                bv.ChiTiet = collection["ChiTiet"];
                bv.Nguon = collection["Nguon"];

                UpdateModel(bv);
                data.SubmitChanges();



                return RedirectToAction("ViewNews");
            }
          
        }



        [HttpGet]
        [ValidateInput(false)]
        public ActionResult DeleteNews(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;
                var bv = data.BAIVIETs.First(d => d.IdBV == id);

                if (bv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(bv);
            }
          
        }



        [HttpPost, ActionName("DeleteNews")]
        public ActionResult ComfirmDeleteNews(int id)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;



                var bv = data.BAIVIETs.First(d => d.IdBV == id);

                if (bv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                else
                {
                    data.BAIVIETs.DeleteOnSubmit(bv);
                    data.SubmitChanges();
                    return RedirectToAction("ViewNews");
                }
            }
        

        }



        [HttpGet]
        public ActionResult AddNewStudent()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.MaKhoa), "MaKhoa", "TenKhoa");
                ViewBag.MaKhoaHoc = new SelectList(data.KHOAHOCs.ToList().OrderBy(n => n.MaKhoaHoc), "MaKhoaHoc", "TenKhoaHoc");
                ViewBag.MaLop = new SelectList(data.LOPs.ToList().OrderBy(n => n.MaLop), "MaLop", "TenLop");


                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;



                return View();
            }
          
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewStudent(SINHVIEN sv)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.MaKhoa = new SelectList(data.KHOAs.ToList().OrderBy(n => n.MaKhoa), "MaKhoa", "TenKhoa");
                ViewBag.MaKhoaHoc = new SelectList(data.KHOAHOCs.ToList().OrderBy(n => n.MaKhoaHoc), "MaKhoaHoc", "TenKhoaHoc");
                ViewBag.MaLop = new SelectList(data.LOPs.ToList().OrderBy(n => n.MaLop), "MaLop", "TenLop");

                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;

                data.SINHVIENs.InsertOnSubmit(sv);
                data.SubmitChanges();
                return RedirectToAction("AddNewStudent");
            }
          
        }




        [HttpGet]
        public ActionResult AddAccountStudent()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;


                var count = (from c in data.BAIVIETs select c).Count();
                ViewBag.Count = count;



                return View();
            }
          
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAccountStudent(TAIKHOAN tksv, FormCollection collection)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                string rePass = collection["NLMK"];
                string Pass = collection["MatKhau"];
                if(rePass.Equals(Pass))
                {
                    var count = (from c in data.BAIVIETs select c).Count();
                    ViewBag.Count = count;



                    data.TAIKHOANs.InsertOnSubmit(tksv);
                    data.SubmitChanges();
                    return RedirectToAction("AddAccountStudent");
                }
                else
                {
                    ViewBag.Mk = "Mật khẩu không khớp, xin nhập lại !";
                    return View();
                }

               
            }
          
        }


        [HttpGet]

        public ActionResult EditPassAdStudent(string username)
        {
            string un = username;
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                var pw = data.ADMINs.First(p => p.Username.Contains(username));
                if (pw == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(pw);
            }
          

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPassAdStudent(string username, FormCollection collection)
        {
            string un1 = username;
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                //ADMIN ad = data.ADMINs.SingleOrDefault(n => n.Name == pw.Name);

                //if (ModelState.IsValid)
                //{

                //    ad.Password = pw.Password; 
                //    UpdateModel(pw);
                //    data.SubmitChanges();
                //    return RedirectToAction("LoginAdminStudent");
                //}
                var pw = data.ADMINs.First(p => p.Username.Contains(username));
                string oldpw = collection["OldPassword"];
                string pass = collection["Password"];
                string repass = collection["RePassword"];
                string un = collection["Username"];
                string name = collection["Name"];

                if (oldpw != pw.Password)
                {
                    ViewBag.Erorr1 = "Mật khẩu sai!!!";
                    return View("EditPassAdStudent");
                }
                else
                {
                    if (repass != pass)
                    {
                        ViewBag.Error = "Mật khẩu không khớp";
                        return View("EditPassAdStudent");

                    }
                    else
                    {
                        pw.Username = un;
                        pw.Password = pass;
                        pw.Name = name;
                        pw.Role = pw.Role;
                        UpdateModel(pw);
                        data.SubmitChanges();
                        return RedirectToAction("LoginAdminStudent");
                    }
                }
            }
           
            //return RedirectToAction("LoginAdminStudent");

        }
        [HttpGet]

        public ActionResult EditPassAdITU(string username)
        {
            string un = username;
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                var pw = data.ADMINs.First(p => p.Username.Contains(username));
                if (pw == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(pw);
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPassAdITU(string username, FormCollection collection)
        {
            string un1 = username;
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                
                var pw = data.ADMINs.First(p => p.Username.Contains(username));
                string oldpw = collection["OldPassword"];
                string pass = collection["Password"];
                string repass = collection["RePassword"];
                string un = collection["Username"];
                string name = collection["Name"];

                if (oldpw != pw.Password)
                {
                    ViewBag.Erorr1 = "Mật khẩu không đúng!!!";
                    return View("EditPassAdITU");
                }
                else
                {
                    if (repass != pass)
                    {
                        ViewBag.Error = "Mật khẩu nhập lại không khớp";
                        return View("EditPassAdITU");

                    }
                    else
                    {
                        pw.Username = un;
                        pw.Password = pass;
                        pw.Name = name;
                        pw.Role = pw.Role;
                        UpdateModel(pw);
                        data.SubmitChanges();
                        return RedirectToAction("LoginAdminStudent");
                    }
                }
            }

            //return RedirectToAction("LoginAdminStudent");

        }
        [HttpGet]
        public ActionResult AddAccountAdmin()
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                ViewBag.Password = tk.Password;
                
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAccountAdmin(ADMIN ad, FormCollection collection)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                ViewBag.Password = tk.Password;
                
                string pass = collection["Password"];
                string username = collection["Username"];
                string  name= collection["Name"];
                string role = collection["Role"];
                if (pass == null)
                {
                    ViewBag.Error1 = "Vui lòng nhập mật khẩu";
                    return RedirectToAction("AddAccountAdmin");
                }
                else
                {
                    if (username == null)
                    {
                        ViewBag.Error2 = "Vui lòng nhập tên đăng nhập";
                        return RedirectToAction("AddAccountAdmin");
                    }
                    else
                    {
                        if (name == null)
                        {
                            ViewBag.Error3 = "Vui lòng nhập tên hiển thị cho tài khoản";
                            return RedirectToAction("AddAccountAdmin");
                        }
                        else
                        {
                            ad.Username = username;
                            ad.Password = pass;
                            ad.Name = name;
                            ad.Role = int.Parse(role);
                            data.ADMINs.InsertOnSubmit(ad);
                            data.SubmitChanges();
                            return RedirectToAction("AddAccountAdmin");
                        }
                    }
                }             
            }
        }

        public ActionResult ViewAccountAdmin( string timkiem, int? page)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.UserName = tk.Username;
                ViewBag.Taikhoan = tk.Name;
                ViewBag. Pass= tk.Password;
                ViewBag.Role = tk.Role;

                int pageNumber = (page ?? 1);
                int pageSize = 7;



                var filter = from d in data.ADMINs select d;

                if (!string.IsNullOrEmpty(timkiem))
                {
                    filter = filter.Where(a => a.Username.Contains(timkiem));
                    return View(filter.ToPagedList(pageNumber, pageSize));
                }

                return View(data.ADMINs.Where(b => b.Role == 2).ToList().ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]

        public ActionResult EditTeacher(string username)
        { 
            string un = username;  // cmm rồi đó
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("ViewAccountAdmin");
            }
            else
            {
                //var gv = data.ADMINs.First(p => p.Role == 2);
                ViewBag.Name = tk.Name;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                var pw = data.ADMINs.First(p => p.Username.Contains(username));
                if (pw == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(pw);
            }


        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditTeacher(string username, FormCollection collection)
        {
            string un1 = username;
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("ViewAccountAdmin");
            }
            else
            {
                
                ViewBag.Name = tk.Name;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                
                var pw = data.ADMINs.First(p => p.Username.Contains(username));
                string oldpw = collection["OldPassword"];
                string pass = collection["Password"];
                string repass = collection["RePassword"];
                string un = collection["Username"];
                string name = collection["Name"];

                if (oldpw != pw.Password)
                {
                    ViewBag.Erorr1 = "Mật khẩu sai!!!";
                    return View("EditTeacher");
                }
                else
                {
                    if (repass != pass)
                    {
                        ViewBag.Error = "Mật khẩu không khớp";
                        return View("EditTeacher");

                    }
                    else
                    {
                        pw.Username = un;
                        pw.Password = pass;
                        pw.Name = name;
                        pw.Role = pw.Role;
                        UpdateModel(pw);
                        data.SubmitChanges();
                        return RedirectToAction("ViewAccountAdmin");
                    }
                }
            }

        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult DeleteTeacher(string username)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Name = tk.Name;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;
                var bv = data.ADMINs.First(d => d.Username == username);

                if (bv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return View(bv);
            }

        }



        [HttpPost, ActionName("DeleteTeacher")]
        public ActionResult ComfirmDeleteTeacher(string username)
        {
            ADMIN tk = (ADMIN)Session["TaikhoanSV"];
            if (tk == null || String.IsNullOrEmpty(tk.ToString()))
            {
                return RedirectToAction("LoginAdminStudent");
            }
            else
            {
                ViewBag.Name = tk.Name;
                ViewBag.Taikhoan = tk.Name;
                ViewBag.UserName = tk.Username;



                var bv = data.ADMINs.First(d => d.Username == username);

                if (bv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                else
                {
                    data.ADMINs.DeleteOnSubmit(bv);
                    data.SubmitChanges();
                    return RedirectToAction("ViewAccountAdmin");
                }
            }


        }
    }
}