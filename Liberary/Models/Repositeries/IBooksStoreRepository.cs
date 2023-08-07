using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.Models.Repositeries
{
    public interface IBooksStoreRepository<TEntity>
    {
        IList<TEntity> List();

        TEntity Find(int id);

        void Add(TEntity entity);

        void Update(int id, TEntity entity);

        void Delete(int id);
        public List<TEntity> Search(string term);
    }
}
