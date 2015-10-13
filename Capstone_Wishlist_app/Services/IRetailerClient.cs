using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailService;

namespace Capstone_Wishlist_app.Services {
    interface IRetailerClient {
        Task<Item[]> FindItemsAsync(ItemCategory category, string keywords);
    }
}
