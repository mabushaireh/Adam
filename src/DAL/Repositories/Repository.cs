// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.i2be.com
// 

// ======================================

namespace i2fam.DAL.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using i2fam.DAL.Repositories.Interfaces;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DbContext context)
        {
            this._context = context;
            this._entities = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            this._entities.Add(entity);
        }

        public virtual Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return this._entities.AddAsync(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            this._entities.AddRange(entities);
        }


        public virtual void Update(TEntity entity)
        {
            this._entities.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            this._entities.UpdateRange(entities);
        }



        public virtual void Remove(TEntity entity)
        {
            this._entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            this._entities.RemoveRange(entities);
        }


        public virtual int Count()
        {
            return this._entities.Count();
        }


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this._entities.Where(predicate);
        }

        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this._entities.SingleOrDefault(predicate);
        }

        public virtual TEntity Get(int id)
        {
            return this._entities.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this._entities.ToList();
        }
    }
}
