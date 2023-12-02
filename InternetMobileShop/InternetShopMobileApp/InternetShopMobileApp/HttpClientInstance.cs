using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp
{
    public static class HttpClientInstance
    {
        public static readonly HttpClient Client = new HttpClient(new HttpClientHandler { UseCookies = true });
    }

}
