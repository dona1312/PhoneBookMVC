using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PhoneBook.Repositories
{
    public class BaseRepository<T> where T:BaseModel,new ()
    {
        public DbContext Context { get; set; }
        public DbSet<T> dbSet { get; set; }
        public UnitOfWork UnitOfWork { get; set; }
        public BaseRepository()
        {
            this.Context = new AppContext();
            this.dbSet = this.Context.Set <T>();
        }
        public BaseRepository(UnitOfWork unit)
        {
            this.UnitOfWork = unit;
            this.Context = unit.Context;
            this.dbSet = this.Context.Set<T>();
        }
        public List<T> GetAll(Expression<Func<T,bool>> filter=null)
        {
            IQueryable<T> result = dbSet;
            if (filter != null)
                return result.Where(filter).ToList();
            else
                return result.ToList();
        }
        public void Add(T item)
        {
            this.dbSet.Add(item);
        }
        public void Edit(T item)
        {
            this.Context.Entry(item).State = EntityState.Modified;
        }
        public void Delete(T item)
        {
            this.dbSet.Remove(GetByID(item.ID));
            this.Context.SaveChanges();
        }
        public void Save(T item)
        {
            if (item.ID != 0)
                Edit(item);
            else
                Add(item);

            this.Context.SaveChanges();
        }
        public T GetByID(int id)
        {
            return this.dbSet.Find(id);
        }
    }
}