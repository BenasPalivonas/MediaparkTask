using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaPark.Services.ApiHelper
{
    public interface IApiHelper
    {
        public void InitializeClient();
        public HttpClient ApiClient { get; set; }
    }
}
