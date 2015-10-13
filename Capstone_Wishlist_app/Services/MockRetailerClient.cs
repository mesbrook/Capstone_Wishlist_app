using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetailService;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Capstone_Wishlist_app.Services {
    public class MockRetailerClient : IRetailerClient {
        private string _itemFileName;

        public MockRetailerClient(string itemFileName) {
            _itemFileName = itemFileName;
        }

        public async Task<Item[]> FindItemsAsync(ItemCategory category, string keywords) {
            using (var reader = new StreamReader(_itemFileName)) {
                string serializedItems = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Item[]>(serializedItems);
            }
        }
    }
}