using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Database.ChangeTracker.Services
{
    public interface IFormKeyProvider
    {
        Guid RetrieveFormKey();
    }
}