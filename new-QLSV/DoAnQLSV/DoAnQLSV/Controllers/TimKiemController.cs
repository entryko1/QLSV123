using DoAnQLSV.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnQLSV.Controllers
{
    public class TimKiemController : Controller
    {
        dbQLSinhVienDataContext data = new dbQLSinhVienDataContext();
        // GET: TimKiem
        [HttpPost]
        public ActionResult TiemkiemSV(int? page, FormCollection f)
        {
            string sTukhoa = f["txtTimkiem"].ToString();
            List<DIEM> lstKQ = data.DIEMs.Where(n => n.MaSV.Contains(sTukhoa)).ToList();
            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            var filter = from d in data.DIEMs select d;
            if (lstKQ.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm Thấy tên cần tìm";
                return View(data.DIEMs.OrderBy(n => n.MaSV).ToPagedList(pageNumber, pageSize));
            }
            return View(lstKQ.OrderBy(n => n.MaSV).ToPagedList(pageNumber, pageSize));


        }
    }
}