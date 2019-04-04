using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zambon.Core.Database.Entity;

namespace Zambon.DemoApplication.BusinessObjects
{
    [DefaultProperty("Name")]
    public class Departments : DBObject
    {

        #region Properties

        [DisplayName("Name1")]
        public string Name { get; set; }

        #endregion

        public override void Configure(EntityTypeBuilder entity)
        {
            base.Configure(entity);
        }

    }
}