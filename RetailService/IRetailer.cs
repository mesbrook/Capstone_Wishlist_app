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
            UriTemplate="/items/find?category={category}&keywords={keywords}")]
        Item[] FindItems(ItemCategory category, string keywords);

        [OperationContract]
        [WebGet(
            BodyStyle=WebMessageBodyStyle.Wrapped,
            ResponseFormat=WebMessageFormat.Json,
            UriTemplate="/items/lookup?itemIds={itemIds}")]
        Item[] LookupItems(string itemIds);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/placeOrder")]
        PlacedOrder PlaceOrder(Order order);

        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/order/{id}")]
        PlacedOrder GetOrder(string id);

        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/order/{id}/status")]
        OrderStatus GetOrderStatus(int id);
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

    [DataContract]
    public class Address {
        [DataMember]
        public string LineOne { get; set; }

        [DataMember]
        public string LineTwo { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string PostalCode { get; set; }
    }

    [DataContract]
    public class CardPayment {
        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Address BillingAddress { get; set; }

        [DataMember]
        public string VerficationCode { get; set; }

        [DataMember]
        public int ExpirationMonth { get; set; }

        [DataMember]
        public int ExpirationYear { get; set; }
    }

    [DataContract]
    public class OrderItem {
        [DataMember]
        public string ItemId { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }

    [DataContract]
    public class Order {
        [DataMember]
        public Address ShippingAddress { get; set; }

        [DataMember]
        public CardPayment Payment { get; set; }

        [DataMember]
        public OrderItem[] Items { get; set; }
    }

    [DataContract]
    public enum OrderStatus {
        Placed,
        Shipped,
        Failed
    }

    [DataContract]
    public class OrderLine {
        [DataMember]
        public string ItemId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal Subtotal { get; set; }
    }

    [DataContract]
    public class PlacedOrder {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public OrderStatus Status { get; set; }

        [DataMember]
        public Address ShippingAddress { get; set; }

        [DataMember]
        public decimal Tax { get; set; }

        [DataMember]
        public decimal Total { get; set; }
    }
}
