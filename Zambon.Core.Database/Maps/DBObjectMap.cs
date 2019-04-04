//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Zambon.Core.Database.Domain.Entities;

//namespace Zambon.Core.Database.Maps
//{
//    public class DBObjectMap<TEntity> : BaseDBObjectMap<TEntity> where TEntity : DBObject
//    {
//        public override void Configure(EntityTypeBuilder<TEntity> builder)
//        {
//            base.Configure(builder);

//            builder.HasQueryFilter(p => !p.IsDeleted);
//            builder.Property(p => p.IsDeleted).HasDefaultValueSql("0");
//        }
//    }
//}