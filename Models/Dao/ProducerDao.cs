using BotDetect;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class ProducerDao
    {
        OnlineShopDbContext db = null;

        public ProducerDao()
        {
            db = new OnlineShopDbContext();
        }
        public Producer ViewDetail(long? ID)
        {
            return db.Producers.Find(ID);

        }
        public List<Producer> ListAll()
        {
            return db.Producers.Where(x => x.Status == true).OrderBy(x => x.Name).ToList();
        }
        public IEnumerable<Producer> ListAllPading(string Seachstring, int Page, int Pagesize)
        {

            IQueryable<Producer> query = db.Producers;
            if (!string.IsNullOrEmpty(Seachstring))
            {
                query = query.Where(x => x.Name.Contains(Seachstring) || x.Name.Contains(Seachstring));
            }
            return query.OrderByDescending(x => x.ID).ToPagedList(Page, Pagesize);
        }
        public Producer GetById(long ID)
        {
            return db.Producers.Find(ID);

        }
        public bool ChangeStatus(long id)
        {
            var Dao = db.Producers.Find(id);
            Dao.Status = !Dao.Status;
            db.SaveChanges();
            return Dao.Status;
        }
        public void Update(Producer model)
        {
            var content = db.Producers.Find(model.ID);
            content.Name = model.Name;




            content.Discription = model.Discription;

            content.Image = model.Image;


            content.Status = model.Status;
            db.SaveChanges();
        }
        public void Create(Producer product)
        {
            //Xử lý alias


            db.Producers.Add(product);
            db.SaveChanges();

        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Producers.Find(id);
                db.Producers.Remove(user);
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
