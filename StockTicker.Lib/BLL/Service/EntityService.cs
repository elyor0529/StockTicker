﻿using System;
using System.Linq;
using System.Linq.Expressions;
using StockTicker.Lib.BLL.Repository;
using StockTicker.Lib.DAL.EF;

namespace StockTicker.Lib.BLL.Service
{
    public abstract class EntityService<T> : IEntityService<T> where T : class, IEntity<long>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<T> _repository;

        protected EntityService(IUnitOfWork unitOfWork, IEntityRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual void Create(T entity)
        { 
            _repository.Add(entity);
            _unitOfWork.Commit();
        }

        public T GetById(long id)
        {
            return _repository.GetSingle(id);
        }

        public virtual void Update(T entity)
        { 
            _repository.Edit(entity);
            _unitOfWork.Commit();
        }

        public virtual void Delete(T entity, bool directly)
        { 
            _repository.Delete(entity, directly);
            _unitOfWork.Commit();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            return _repository.GetAllIncluding(includeProperties);
        }

    }
}
