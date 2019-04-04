using System.Linq;
using System.Threading.Tasks;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Database.Domain.Models;

namespace Zambon.Core.Database.Domain.Contracts.Repositories
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntity, new()
    {
        TModelView Add<TModelView>(IModel model) where TModelView : class;
        void Add(TEntity entity);

        TModelView Update<TModelView>(IModel model) where TModelView : class;
        void Update(TEntity entity);

        Task DeleteAsync(params object[] keyValues);

        Task<IQueryable<TModelView>> GetAsync<TModelView>();
        Task<TModelView> GetAsync<TModelView>(params object[] keyValues);
    }
}