using Common;
using Models.EF;
using Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products.Where(x=>x.IncludedVAT == false).OrderByDescending(x=>x.Code);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Code.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Product> ListPromotion(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products.Where(x => x.IncludedVAT == true).OrderByDescending(x => x.Code);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Code.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Product GetById(long ID)
        {
            return db.Products.Find(ID);

        }
        public bool ChangeStatus(long id)
        {
            var Dao = db.Products.Find(id);
            Dao.Status = !Dao.Status;
            db.SaveChanges();
            return Dao.Status;
        }
        public void Update(Product model)
        {
            var content = db.Products.Find(model.ID);
            content.Name = model.Name;
            if (string.IsNullOrEmpty(model.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(model.Name);
            }
            
            
            
            content.Description = model.Description;
            content.CreatedDate = DateTime.Now;
            content.Image = model.Image;
            content.PromotionPrice = model.PromotionPrice;
            content.Price = model.Price;
            content.CategoryID = model.CategoryID;
            content.Detail = model.Detail;
            content.Quantity = model.Quantity;
            content.Warranty = model.Warranty;
            content.MetaKeyWords = model.MetaKeyWords;
            content.MetaDescriptions = model.MetaDescriptions;
            content.TopHot = model.TopHot;
            content.Status = model.Status;
            db.SaveChanges();
        }
        public void Create(Product product)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(product.MetaTitle))
            {
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
            }
            product.CreatedDate = DateTime.Now;
            product.IncludedVAT = false;
            db.Products.Add(product);
            db.SaveChanges();
  
        }
        public void CreatePromotion(Product product)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(product.MetaTitle))
            {
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
            }
            product.CreatedDate = DateTime.Now;
            product.IncludedVAT = true;
            db.Products.Add(product);
            db.SaveChanges();

        }
        public List<Product> ListNewProduct(Producer manufacturers, string price, ProductCategory categorys,int top)
        {
            var model = new List<Product>();
            if (!string.IsNullOrEmpty(price))
            {
                if(price == "duoi-5-trieu")
                {
                    if (manufacturers != null && categorys != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false && x.Price < 5000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys != null)
                    {
                        model = db.Products.Where(x => (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false && x.Price < 5000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (categorys == null && manufacturers != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == false && x.Price < 5000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys == null)
                    {
                        model = db.Products.Where(x => x.IncludedVAT == false && x.Price < 5000000 ).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                }
                else if(price == "tu-5-den-10-trieu")
                {
                    if (manufacturers != null && categorys != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false && x.Price >= 5000000 && x.Price <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys != null)
                    {
                        model = db.Products.Where(x => (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false && x.Price >= 5000000 && x.Price <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (categorys == null && manufacturers != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == false && x.Price >= 5000000 && x.Price <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys == null)
                    {
                        model = db.Products.Where(x => x.IncludedVAT == false && x.Price >= 5000000 && x.Price <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                }
                else if (price == "tren-10-trieu")
                {
                    if (manufacturers != null && categorys != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false && x.Price > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys != null)
                    {
                        model = db.Products.Where(x => (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false && x.Price > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (categorys == null && manufacturers != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == false && x.Price > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys == null)
                    {
                        model = db.Products.Where(x => x.IncludedVAT == false && x.Price > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                }
            }
            else
            {
                if (manufacturers != null && categorys != null)
                {
                    model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }
                else if(manufacturers == null && categorys != null)
                {
                    model = db.Products.Where(x =>  (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == false).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }
                else if (manufacturers != null && categorys == null)
                {
                    model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == false).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }else if(manufacturers == null && categorys == null)
                {
                    model = db.Products.Where(x =>  x.IncludedVAT == false).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }
                
            }
            return model;
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public List<Product> ListFeatureProduct(Producer manufacturers, string price, ProductCategory categorys,int top)
        {
            var model = new List<Product>();
            if (!string.IsNullOrEmpty(price))
            {
                if (price == "duoi-5-trieu")
                {
                    if (manufacturers != null && categorys != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice < 5000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    
                    else if (manufacturers == null && categorys != null)
                    {
                        model = db.Products.Where(x =>  (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice < 5000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (categorys == null && manufacturers != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name  && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice < 5000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys == null)
                    {
                        model = db.Products.Where(x => x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice < 5000000 ).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                }
                else if (price == "tu-5-den-10-trieu")
                {
                    if (manufacturers != null && categorys != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice >= 5000000 && x.PromotionPrice <=10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }

                    else if (manufacturers == null && categorys != null)
                    {
                        model = db.Products.Where(x => (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice >= 5000000 && x.PromotionPrice <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (categorys == null && manufacturers != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice >= 5000000 && x.PromotionPrice <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys == null)
                    {
                        model = db.Products.Where(x => x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice >= 5000000 && x.PromotionPrice <= 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                }
                else if (price == "tren-10-trieu")
                {
                    if (manufacturers != null && categorys != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }

                    else if (manufacturers == null && categorys != null)
                    {
                        model = db.Products.Where(x => (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (categorys == null && manufacturers != null)
                    {
                        model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                    else if (manufacturers == null && categorys == null)
                    {
                        model = db.Products.Where(x => x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now && x.PromotionPrice > 10000000).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                    }
                }
            }
            else
            {
                if (manufacturers != null && categorys != null)
                {
                    model = db.Products.Where(x => x.Producer == manufacturers.Name && (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }

                else if (manufacturers == null && categorys != null)
                {
                    model = db.Products.Where(x => (x.CategoryID == categorys.ID || x.CategoryID == categorys.ParentID) && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }
                else if (manufacturers != null && categorys == null)
                {
                    model = db.Products.Where(x => x.Producer == manufacturers.Name && x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }
                else if (manufacturers == null && categorys == null)
                {
                    model = db.Products.Where(x => x.IncludedVAT == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
                }

            }

            
            return model;
            
        }
        public List<ProductViewModel> ListByCategoryId(long categoryId,ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryId).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.CategoryID == categoryId
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice,
                             IncludedVAT = a.IncludedVAT
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.PromotionPrice,
                             IncludedVAT = x.IncludedVAT
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice,
                             IncludedVAT = a.IncludedVAT
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.PromotionPrice,
                             IncludedVAT = x.IncludedVAT
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
        public List<Product> ListRelatedProduct(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Products.Find(id);
                db.Products.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
