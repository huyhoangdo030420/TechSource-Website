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
    public class ProductCategoryDao
    {
        OnlineShopDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<ProductCategory> ListAll() 
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x=>x.Name).ToList();
        }
        public List<ProductCategory> ViewPageList()
        {
            return db.ProductCategories.Where(x => x.Status == true&&x.ParentID==null).OrderBy(x => x.Name).ToList();
        }
        public ProductCategory ViewDetail(long? id)
        {
            return db.ProductCategories.Find(id);
        }
        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories.OrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }

            return model.ToPagedList(page, pageSize);
        }
        public bool ChangeStatus(long id)
        {
            var Dao = db.ProductCategories.Find(id);
            Dao.Status = !Dao.Status;
            db.SaveChanges();
            return Dao.Status;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public ProductCategory GetById(long ID)
        {
            return db.ProductCategories.Find(ID);

        }
        public void Update(ProductCategory model)
        {
            var content = db.ProductCategories.Find(model.ID);
            content.Name = model.Name;
            if (string.IsNullOrEmpty(model.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(model.Name);
            }
          
            content.CreatedDate = DateTime.Now;
            if(model.ParentID == 0)
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
        public void Create(ProductCategory product)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(product.MetaTitle))
            {
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
            }
            if(product.ParentID == 0)
            {
                product.ParentID = null;
            }
            
            product.CreatedDate = DateTime.Now;
            
            db.ProductCategories.Add(product);
            db.SaveChanges();

        }
    }
}
