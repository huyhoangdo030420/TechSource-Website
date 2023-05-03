using Common;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x=>x.Status == true).ToList();
        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public IEnumerable<Category> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories.OrderBy(x => x.ID);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }

            return model.ToPagedList(page, pageSize);
        }
        public bool ChangeStatus(long id)
        {
            var Dao = db.Categories.Find(id);
            Dao.Status = !Dao.Status;
            db.SaveChanges();
            return Dao.Status;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Categories.Find(id);
                db.Categories.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public void Update(Category model)
        {
            var content = db.Categories.Find(model.ID);
            content.Name = model.Name;
            if (string.IsNullOrEmpty(model.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(model.Name);
            }

            content.CreatedDate = DateTime.Now;
            if (model.ParentID == 0)
            {
                content.ParentID = null;
            }
            else
            {
                content.ParentID = model.ParentID;
            }

            content.DisplayOrder = model.DisplayOrder;
            content.Status = model.Status;
            db.SaveChanges();
        }
        public Category GetById(long ID)
        {
            return db.Categories.Find(ID);

        }
    }
}
