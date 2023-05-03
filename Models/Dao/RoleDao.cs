using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class RoleDao
    {
        OnlineShopDbContext db = null;
        public RoleDao()
        {
            db = new OnlineShopDbContext();
        }

    }
}
