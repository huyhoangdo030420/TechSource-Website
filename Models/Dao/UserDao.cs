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

namespace Models.Dao
{
    
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<User> ListAll()
        {
            return db.Users.Where(x => x.Status == true).OrderBy(x => x.Name).ToList();
        }
        public bool Insert(User entity)
        {

            var result = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if(result == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
               
    

        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }

        }
        public IEnumerable<User> ListAllPading(string Seachstring,int Page,int Pagesize)
        {
            IQueryable<User> query = db.Users;
            if (!string.IsNullOrEmpty(Seachstring))
            {
                query = query.Where(x=>x.UserName.Contains(Seachstring) || x.Name.Contains(Seachstring));
            }
            return query.OrderByDescending(x => x.CreatedDate).ToPagedList(Page, Pagesize);
        }
        public User GetByID(string userName)
        {
            return db.Users.SingleOrDefault(x=>x.UserName==userName);
        }
        public User ViewDetail(int ID)
        {
            return db.Users.SingleOrDefault(x => x.ID == ID);
        }
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.UserName = entity.UserName;
                user.PassWord = entity.PassWord;
                user.Name = entity.Name;
                user.CreatedDate = DateTime.Now;
                user.GroupID = entity.GroupID;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

           public bool ChangeStatus(long id)
        {
           var Dao =  db.Users.Find(id);
            Dao.Status = !Dao.Status;
            db.SaveChanges();
            return Dao.Status;
        }
                
            
               
        
        
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where (b.ID == user.GroupID)
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }
            public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return -1;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP || result.GroupID == CommonConstants.MEMBER_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -2;
                        }
                        else
                        {
                            if (result.PassWord == passWord)
                                return 0;
                            else
                                return 1;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -2;
                    }
                    else
                    {
                        if (result.PassWord == passWord)
                            return 0;
                        else
                            return -2;
                    }
                }
            }
        }
        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}
