using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using RetailService;
using System.Net;
using Newtonsoft.Json;

namespace Capstone_Wishlist_app.Services {
    public class RetailerClient : IRetailerClient {
        private string _serviceUrl;

        public RetailerClient(string serviceUrl) {
            _serviceUrl = serviceUrl;
        }

        public async Task<Item[]> FindItemsAsync(ItemCategory category, string keywords) {
            var webClient = new WebClient {
                QueryString = {
                    { "category", Enum.GetName(typeof(ItemCategory), category) },
                    { "keywords", keywords }
                }
            };
            var methodUrl = _serviceUrl + "/items/find";
            var findResult = await webClient.DownloadStringTaskAsync(methodUrl);
            return JsonConvert.DeserializeObject<Item[]>(findResult);
        }
    }
}