using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Threading;

namespace ClientApp.Classes
{
    public class HTTPClientInstance
    {
        private static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost"),
            Timeout = TimeSpan.FromSeconds(60)
            //если сервер не отвечает в течении 60 секунд то сервер генерирует исключение(избежание зависания)
        };
        public static HttpClient Instance => httpClient;
        //доступ к одному и тому же экземпляру HttpClient
    }
}
