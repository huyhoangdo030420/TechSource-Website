using Model.Dao;
using Models.Dao;
using Models.EF;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using System.Drawing;
using BotDetect;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        
        public ActionResult Index(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            var dao = new OrderDao();
            var model = dao.ListSales(fromDate, toDate, page, pageSize);

            
            return View(model);
        }

        public ActionResult Assess(long id)
        {
            var dao = new OrderDao();
            var model = dao.Sales(id);


            return View(model);
        }
        public ActionResult OrderDetail(long id)
        {
            var dao = new OrderDao();
            var model = dao.Sales(id);


            return View(model);
        }
        public ActionResult AssessOrder(bool Status, long id)
        {
            var Dao = new OrderDao();
            Dao.ChangeStatus(id, Status);
            return RedirectToAction("Index");
        }
        public ActionResult Back()
        {
            
            return RedirectToAction("Index");
        }
        public ActionResult ExportToExcel(DateTime? fromDate, DateTime? toDate)
        {
            // Lấy đường dẫn đến file Excel mẫu
            var templateFilePath = Server.MapPath("~/Export/Donhang.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Mở file Excel mẫu
            var fileInfo = new FileInfo(templateFilePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                
                // Lấy sheet đầu tiên trong file Excel mẫu
                var worksheet = package.Workbook.Worksheets[0];

                // Thêm dữ liệu vào sheet
                var dao = new OrderDao();
                var products = dao.ListExcel(fromDate, toDate);
                

                for (int i = 0; i < products.Count; i++)
                {
                    worksheet.Cells[i + 6, 2].Value = products[i].ShipName;
                    worksheet.Cells[i + 6, 3].Value = products[i].ShipMobile;
                    worksheet.Cells[i + 6, 4].Value = products[i].ShipAddress;
                    worksheet.Cells[i + 6, 5].Value = products[i].TotalOrder;
                    worksheet.Cells[i + 6, 6].Value = products[i].ShipAddress;
                    worksheet.Cells[i + 6, 7].Value = (products[i].Status == null ? "Chưa thanh toán" : (products[i].Status.Value ? "Đã thanh toán" : "Đã Huỷ đơn"));
                }

                // Thiết lập kiểu dữ liệu cho cột "Giá"
                worksheet.Column(5).Style.Numberformat.Format = "#,000";

                // Thiết lập định dạng cho bảng dữ liệu
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Lưu lại file Excel mới
                var newFilePath = Server.MapPath("~/Export/Content/Donhang.xlsx");
                package.SaveAs(new FileInfo(newFilePath));

                // Xuất file Excel mới
                var fileContents = System.IO.File.ReadAllBytes(newFilePath);
                var fileName = "Donhang.xlsx";
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        public ActionResult AlertsDropdown()
        {
            
            return PartialView();
        }
        public ActionResult Xuly(string id)
        {
            bool ShowOnHome ;
            var dao = new OrderDao();
            
            string[] idAndShowOnHome = id.Split('-');
            long ID = long.Parse(idAndShowOnHome[0]);
            dao.ChangeShowOnHome(ID, false);
            if (idAndShowOnHome[1] == "1")
            {
                ShowOnHome = true;
                return RedirectToAction("OrderDetail", new { id = ID });
            }
            else
            {
                ShowOnHome = false;
                return RedirectToAction("Assess", new { id = ID });
            }
           
            
            // Trả về ActionResult khác với giá trị ID truyền vào
            
            
        }


    }
}