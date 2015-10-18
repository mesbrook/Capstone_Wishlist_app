using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Capstone_Wishlist_app.Services.Amazon;

namespace Capstone_Wishlist_app.Services {
    public class AmazonRetailer : IRetailer {
        private string _associateTag;
        private string _accessKey;
        private AWSECommerceServicePortType _amazonClient;

        public AmazonRetailer(string associateTag, string accessKey, string endpointConfigName) {
            _associateTag = associateTag;
            _accessKey = accessKey;
            _amazonClient = new AWSECommerceServicePortTypeClient(endpointConfigName);
        }

        public async Task<Item[]> FindItemsAsync(ItemCategory category, string keywords) {
            var itemSearch = new ItemSearch {
                AssociateTag = _associateTag,
                AWSAccessKeyId = _accessKey,
                Request = new ItemSearchRequest[] { 
                    new ItemSearchRequest {
                        SearchIndex = Enum.GetName(typeof(ItemCategory), category),
                        Keywords = keywords,
                        ItemPage = "1",
                        ResponseGroup = new string[] { "Images", "ItemAttributes" }
                    }
                }
            };
            var searchResult = await _amazonClient.ItemSearchAsync(new ItemSearchRequest1 { ItemSearch = itemSearch });

            var items = searchResult.ItemSearchResponse.Items[0].Item;
            return items.Select(item => new Item {
                Id = item.ASIN,
                ListPrice = decimal.Parse(item.ItemAttributes.ListPrice.Amount),
                Title = item.ItemAttributes.Title,
                ListingUrl = item.DetailPageURL,
                ImageUrl = item.SmallImage == null ? "" : item.SmallImage.URL
            }).ToArray();
        }


        public async Task<Item[]> LookupItemsAsync(string[] itemIds) {
            if (itemIds.Length == 0) {
                return new Item[] { };
            }

            var itemLookup = new ItemLookup {
                AssociateTag = _associateTag,
                AWSAccessKeyId = _accessKey,
                Request = new ItemLookupRequest[] {
                    new ItemLookupRequest {
                        ItemId = itemIds.Length > 10 ? itemIds.Take(10).ToArray() : itemIds,
                        IdType = ItemLookupRequestIdType.ASIN,
                        IdTypeSpecified = true,
                        ResponseGroup = new string[]{ "Images", "ItemAttributes" }
                    }
                }
            };

            var lookupResult = await _amazonClient.ItemLookupAsync(new ItemLookupRequest1 { ItemLookup = itemLookup });
            var items = lookupResult.ItemLookupResponse.Items[0].Item;
            return items.Select(item => new Item {
                Id = item.ASIN,
                ListPrice = decimal.Parse(item.ItemAttributes.ListPrice.Amount),
                Title = item.ItemAttributes.Title,
                ListingUrl = item.DetailPageURL,
                ImageUrl = item.SmallImage == null ? "" : item.SmallImage.URL
            }).ToArray();
        }
    }
}