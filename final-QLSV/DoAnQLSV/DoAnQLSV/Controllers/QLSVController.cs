using DoAnQLSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace DoAnQLSV.Controllers
{
    public class QLSVController : Controller
    {

        dbQLSinhVienDataContext data = new dbQLSinhVienDataContext();
        // GET: QLSV
        public ActionResult Index()
        {

            //TinTuc
            BAIVIET bv = data.BAIVIETs.FirstOrDefault(n => n.Id == 3);
            var tintuc = from t in data.BAIVIETs where t.Id == 3 select t;
            ViewBag.TieuDe = bv.TieuDe;
            ViewBag.TomTat = bv.TomTat;
            ViewBag.IdBv = bv.IdBV;


            //ThanhTuu
            BAIVIET bv2 = data.BAIVIETs.FirstOrDefault(n => n.Id == 4);
            var tintuc2 = from t in data.BAIVIETs where t.Id == 4 select t;
            ViewBag.TieuDe2 = bv2.TieuDe;
            ViewBag.TomTat2 = bv2.TomTat;
            ViewBag.IdBv2 = bv2.IdBV;



            //Doingulienket
            BAIVIET bv3 = data.BAIVIETs.FirstOrDefault(n => n.Id == 5);
            var tintuc3 = from t in data.BAIVIETs where t.Id == 5 select t;
            ViewBag.TieuDe3 = bv3.TieuDe;
            ViewBag.TomTat3 = bv3.TomTat;
            ViewBag.IdBv3 = bv3.IdBV;




            //Hotroviechoc
            BAIVIET bv4 = data.BAIVIETs.FirstOrDefault(n => n.Id == 6);
            var tintuc4 = (from t in data.BAIVIETs where t.Id == 6 select t).Take(7);

            ViewBag.TieuDe4 = bv4.TieuDe;
            ViewBag.TomTat4 = bv4.TomTat;
            ViewBag.IdB4v = bv4.IdBV;

            ViewBag.tieude4 = tintuc4.Select(n => n.TieuDe);
            ViewBag.Id4 = tintuc4.Select(n => n.IdBV);

            return View(data.BAIVIETs.ToList().OrderBy(n => n.NgayViet));
        }





        [HttpGet]
        public ActionResult Hotroviechoc(int id, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Doingugiangvien(int id, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult DoiNguLienKet(int id, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }



        [HttpGet]
        public ActionResult ThanhTuu(int id, int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult TinTuc(int id, int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Chuongtrinhhoc(int id, int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.BAIVIETs.Where(b => b.Id.Equals(id)).ToList().OrderBy(n => n.NgayViet).ToPagedList(pageNumber, pageSize));
        }


      
        public ActionResult ChitietBaiviet(int id)
        {
            BAIVIET bv = data.BAIVIETs.SingleOrDefault(n => n.IdBV == id);
            if (bv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bv);
        }








        public ActionResult Diachi()
        {
           return View();
        }


    }
}