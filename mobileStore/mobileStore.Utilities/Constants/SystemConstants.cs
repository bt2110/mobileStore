using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileStore.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "mobileStoreDB";
        public const string CartSession = "CartSession";


        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfNewProducts = 4;
        }
    }
}
