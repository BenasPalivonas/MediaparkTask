using MediaPark.Services.ApiHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MediaPark.Services.ApiHelper
{
    public class ApiHelper : IApiHelper
    {
        public HttpClient ApiClient { get; set; }

        public void InitializeClient() {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("https://kayaposoft.com/enrico/")
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
