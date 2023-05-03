using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model.EF;
using Models.EF;
using PagedList;
using PagedList.Mvc;
public class UserGroupDao
{
    OnlineShopDbContext db = null;
    public UserGroupDao()
    {
        db = new OnlineShopDbContext();
    }
    public List<UserGroup> ListAll()
    {
        return db.UserGroups.OrderBy(x => x.Name).ToList();
    }
}