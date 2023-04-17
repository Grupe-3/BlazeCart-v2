#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using BLZ.Common.Models;

namespace BLZ.Client.Models
{
    [Serializable]
    public partial class Item : ObservableObject
    {
        public int Id { get; set; }
        public enum UnitOfMeasure { UNKNOWN, VNT, KG, L, M, PAK }
        public string MerchantProductId { get; set; }
        public Merchant Merchant { get; set; }

        /* 1:m mapping of categories to items */
        public string CategoryId { get; set; }
        public string RemappedCategoryName { get; set; }
        public string NameLT { get; set; }
        public string? NameEN { get; set; }
        public string? Description { get; set; }
        public UnitOfMeasure? MeasureUnit { get; set; }

        /* Amount of item sold measured by `MeasureUnit` */
        public float? Amount { get; set; }

        /* Price in cents for sold unit */
        public int Price { get; set; }
        public int? DiscountPrice { get; set; }
        /* Price in cents with loyanty card discounts */
        public int? LoyaltyPrice { get; set; }

        // Price of one unit of measurement
        // For example: a store might sell half a kilo of tomatoes for is 1.00 eu
        // Then Price would be 1.00, whilst PricePerUnitOfMeasure would be 2.00
        // ditto for `DiscountPricePerUnitOfMeasure` and `LoyaltyPricePerUnitOfMeasure`
        public int? PricePerUnitOfMeasure { get; set; }
        public int? DiscountPricePerUnitOfMeasure { get; set; }
        public int? LoyaltyPricePerUnitOfMeasure { get; set; }

        // URIs pointing to an image of that product
        public Uri? Image { get; set; }

    }
    
}
