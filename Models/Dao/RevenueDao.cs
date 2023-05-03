using Model.Dao;
using Models.EF;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Models.Dao
{
    public class RevenueDao
    {
        OnlineShopDbContext db = null;
        public RevenueDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<RevenueViewModel> GetRevenues(DateTime? fromDate, DateTime? toDate)
        {
            var dao = new OrderDao();
            var products = dao.ListExcel(fromDate, toDate).Where(x=>x.Status == true);
            Dictionary<DateTime, decimal> DictionaryRevenues = new Dictionary<DateTime, decimal>();
            foreach (var product in products)
            {
                if (DictionaryRevenues.ContainsKey(product.CreateDate.Value.Date))
                {
                    DictionaryRevenues[product.CreateDate.Value.Date] += product.TotalOrder;
                }
                else
                {
                    DictionaryRevenues.Add(product.CreateDate.Value.Date, product.TotalOrder);
                }
            }
            List<RevenueViewModel> revenues = new List<RevenueViewModel>();
            foreach(var DictionaryRevenue in DictionaryRevenues)
            {
                revenues.Add(new RevenueViewModel() { Date = DictionaryRevenue.Key, TotalRevenue = DictionaryRevenue.Value });

                    
            }
            return revenues.OrderBy(x=>x.Date).ToList();
        }
        
    }
}
