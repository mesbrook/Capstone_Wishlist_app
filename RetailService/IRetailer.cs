using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RetailService {
    [ServiceContract]
    public interface IRetailer {

        [OperationContract]
        [WebGet(
            BodyStyle=WebMessageBodyStyle.Wrapped,
            ResponseFormat=WebMessageFormat.Json,
            UriTemplate="/items?category={category}&keywords={keywords}")]
        Item[] FindItems(ItemCategory category, string keywords);
    }

    [DataContract]
    public class Item {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public decimal ListPrice { get; set; }

        [DataMember]
        public bool IsAvailable { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        [DataMember]
        public string ListingUrl { get; set; }
    }

    [DataContract]
    public enum ItemCategory {
        Toys,
        Books,
        Games
    }
}
