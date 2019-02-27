using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Services
{
    public interface ICurrentUserService
    {

        string CurrentIdentityName { get; }

        IUsers CurrentUser { get; }

        void CheckUserChanged();
        
    }
}