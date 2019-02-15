using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zambon.Core.Security
{
    public static class Enums
    {

        public enum MenuTypes
        {
            View = 0,
            Report = 1,
            ExternalURL = 2,
            Separator = 3
        }

        public enum UserTypes
        {
            InternalUser = 0,
            ADUser = 1
        }

    }
}
