using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BLZ.Common.Models
{
    [Index(nameof(MerchantProductId), nameof(Merchant), IsUnique = true), Index(nameof(RemappedCategoryName))]
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public enum UnitOfMeasure { UNKNOWN, VNT, KG, L, M, PAK }

        public string MerchantProductId { get; set; }
        public Merchant Merchant { get; set; }

        /* 1:m mapping of categories to items */
        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        /* Bad place to store this - but whatever */
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

        public Item(string internalID, string nameLT, string categoryId)
        {
            MerchantProductId = internalID;
            CategoryId = categoryId;
            NameLT = nameLT;
        }

        public Item()
        {
            NameLT = "";
            MerchantProductId = "";
        }

        public void FillPerUnitOfMeasureByPrice()
        {
            throw new NotImplementedException();
        }

        public void FillPerUnitOfMeasureByAmmount()
        {
            PricePerUnitOfMeasure = (int)(Price / Amount!);
            DiscountPricePerUnitOfMeasure = (int?)(DiscountPrice / Amount!);
            LoyaltyPricePerUnitOfMeasure = (int?)(LoyaltyPrice / Amount!);
        }

        public static UnitOfMeasure? ParseUnitOfMeasurement(string str)
        {
            UnitOfMeasure? ret = null;
            try
            {
                ret = Enum.Parse<UnitOfMeasure>(str.ToUpper());
            }
            catch (ArgumentException) { ret = null; }

            return ret;
        }

        public override string ToString()
        {
            return NameLT + " [" + MerchantProductId + "]";
        }
    }
}
