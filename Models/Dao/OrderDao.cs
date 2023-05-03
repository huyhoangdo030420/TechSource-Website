using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using Models.ViewModel;
using PagedList;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineShopDbContext db = null;
        public OrderDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Order> ListAll()
        {
            return db.Orders.OrderByDescending(x=>x.CreateDate).ToList();
            
             
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public void ChangeShowOnHome(long id, bool status)
        {

          var model =   db.Orders.Find(id);
            model.ShowOnHome = !status;
            db.SaveChanges();
            
        }
        public void ChangeStatus(long id, bool status)
        {

            var model = db.Orders.Find(id);
            model.Status = status;
            db.SaveChanges();

        }
        public IEnumerable<SalesViewModel> ListSales(DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {

            var model = (from a in db.Orders
                         select new SalesViewModel
                         {
                             ID = a.ID,
                             CustomerID = a.CustomerID,

                             ShipName = a.ShipName,
                             ShipAddress = a.ShipAddress,
                             ShipEmail = a.ShipEmail,
                             ShipMobile = a.ShipMobile,
                             TotalOrder = db.OrderDetails.Where(b => b.OrderID == a.ID).Sum(b => b.Price),
                             CreateDate = a.CreateDate,

                             Status = a.Status
                         });
            if(fromDate != null && toDate != null)
            {
                model = model.Where(x => EntityFunctions.TruncateTime(x.CreateDate) >= fromDate && EntityFunctions.TruncateTime(x.CreateDate) <= toDate);
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
            
        }

        public List<SalesViewModel> ListExcel(DateTime? fromDate, DateTime? toDate)
        {

            var model = (from a in db.Orders
                         select new SalesViewModel
                         {
                             ID = a.ID,
                             CustomerID = a.CustomerID,

                             ShipName = a.ShipName,
                             ShipAddress = a.ShipAddress,
                             ShipEmail = a.ShipEmail,
                             ShipMobile = a.ShipMobile,
                             TotalOrder = db.OrderDetails.Where(b => b.OrderID == a.ID).Sum(b => b.Price),
                             CreateDate = a.CreateDate,

                             Status = a.Status
                         });
            if (fromDate != null && toDate != null)
            {
                model = model.Where(x => EntityFunctions.TruncateTime(x.CreateDate) >= fromDate && EntityFunctions.TruncateTime(x.CreateDate) <= toDate);
            }
            return model.OrderByDescending(x => x.CreateDate).ToList();

        }

        public IEnumerable<SalesViewModel> Sales(long id)
        {

            var model = (from a in db.Orders
                         join b in db.OrderDetails
                         on a.ID equals b.OrderID
                         join c in db.Products
                         on b.ProductID equals c.ID
                         select new SalesViewModel
                         {
                             ID = a.ID,
                             CustomerID = a.CustomerID,
                             ProductName = c.Name,
                             ProductImage = c.Image,
                             ShipName = a.ShipName,
                             ShipAddress = a.ShipAddress,
                             ShipEmail = a.ShipEmail,
                             ShipMobile = a.ShipMobile,
                             TotalOrder = db.OrderDetails.Where(x => x.OrderID == a.ID).Sum(x => x.Price),  //Tổng đơn hàng
                             Total = b.Price,   // Tổng từng sản phẩm
                             Price = c.Price,   // Giá từng sản phẩm
                             Quantity = b.Quantity,
                             CreateDate = a.CreateDate,

                             Status = a.Status
                         });
            model = model.Where(x => x.ID == id);
            return model;

        }
    }
}