//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Zambon.Core.Database.Domain.Entities;

//namespace Zambon.Core.Database.Maps
//{
//    public class BaseDBObjectMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseDBObject
//    {
//        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
//        {
//            builder.Property(p => p.ID).UseSqlServerIdentityColumn();
//            builder.Property(p => p.ID).ValueGeneratedOnAdd();
//            builder.Property(p => p.ID).Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
//        }
//    }
//}