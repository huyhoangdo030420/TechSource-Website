using Microsoft.AspNet.SignalR;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Hubs
{
    public class DemoHub : Hub
    {
        public void GetAll()
        {
            using (OnlineShopDbContext db = new OnlineShopDbContext())
            {
                Clients.All.getAll(db.Orders.OrderByDescending(x=>x.CreateDate).ToList());
            }
        }
    }
}