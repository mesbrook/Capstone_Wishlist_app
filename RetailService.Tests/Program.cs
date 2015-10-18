using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace RetailService.Tests {
    class Program {
        static void Main(string[] args) {
            var serviceFactory = new WebChannelFactory<IRetailer>("RetailService");
            var retailService = serviceFactory.CreateChannel();

            var findResults = retailService.FindItems(ItemCategory.Toys, "minecraft lego set");

            foreach (var item in findResults) {
                Console.WriteLine(item.Title);
            }
        }
    }
}
