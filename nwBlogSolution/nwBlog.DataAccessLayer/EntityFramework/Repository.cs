using nwBlog.Common;
using nwBlog.Core.DataAccess;
using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {

            _objectSet = db.Set<T>();
        }



        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntityBase<Guid>)
            {
                MyEntityBase<Guid> o = obj as MyEntityBase<Guid>;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }

            else if (obj is MyEntityBase<int>)
            {
                MyEntityBase<int> o = obj as MyEntityBase<int>;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }


            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase<Guid>)
            {
                MyEntityBase<Guid> o = obj as MyEntityBase<Guid>;
                DateTime now = DateTime.Now;

                o.ModifiedOn = now;
                o.ModifiedUsername = "system";
            }

            else if (obj is MyEntityBase<int>)
            {
                MyEntityBase<int> o = obj as MyEntityBase<int>;
                DateTime now = DateTime.Now;
         
                o.ModifiedOn = now;
                o.ModifiedUsername = "system";
            }

            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
