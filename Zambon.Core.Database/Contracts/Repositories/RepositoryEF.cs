//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;
//using Zambon.Core.Database.Domain.Contracts.Repositories;
//using Zambon.Core.Database.Domain.Interfaces;
//using Zambon.Core.Database.Domain.Models;

//namespace Zambon.Core.Database.Contracts.Repositories
//{
//    public abstract class RepositoryEF<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
//    {
//        protected readonly CoreDbContext _ctx;
//        protected readonly DbSet<TEntity> _db;
//        protected readonly IMapper _mapper;

//        public RepositoryEF(CoreDbContext ctx, IMapper mapper)
//        {
//            _ctx = ctx;
//            _db = _ctx.Set<TEntity>();
//            _mapper = mapper;
//        }


//        public void Add(TEntity entity)
//        {
//            _db.Add(entity);
//            _ctx.SaveChanges();
//        }

//        public TModelView Add<TModelView>(IModel model) where TModelView : class
//        {
//            var entity = _mapper.Map<TEntity>(model);
//            Add(entity);
//            return _mapper.Map<TModelView>(entity);
//        }

//        public async Task DeleteAsync(params object[] keyValues)
//        {
//            _db.Remove(await _db.FindAsync(keyValues));
//            _ctx.SaveChanges();
//        }

//        public async Task<IQueryable<TModelView>> GetAsync<TModelView>()
//        {
//            return await Task<IQueryable<TModelView>>.Factory.StartNew(() => _mapper.ProjectTo<TModelView>(_db));
//        }

//        public async Task<TModelView> GetAsync<TModelView>(params object[] keyValues)
//        {
//            return _mapper.Map<TModelView>(await _db.FindAsync(keyValues));
//        }

//        public void Update(TEntity entity)
//        {
//            _ctx.Update(entity);
//            _ctx.SaveChanges();
//        }

//        public TModelView Update<TModelView>(IModel model) where TModelView : class
//        {
//            var entity = _mapper.Map<TEntity>(model);
//            Update(entity);
//            return _mapper.Map<TModelView>(entity);
//        }
//    }
//}