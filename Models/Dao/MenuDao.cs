using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class MenuDao
    {
        OnlineShopDbContext db = null;
        public MenuDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Menu> ListByGroupId(int groupid)
        {
            return db.Menus.Where(x=>x.TypeID == groupid && x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
