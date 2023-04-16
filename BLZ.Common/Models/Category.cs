using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BLZ.Common.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MerchantCatId { get; set; }
        public Merchant Merchant { get; set; }

        public Uri? Uri { get; set; }
        public string NameLT { get; set; }

        /* Parent categories, identified by their cat id */
        [ForeignKey(nameof(Category))]
        public string? ParentCatId { get; set; }

        /* Virtual fields that can be accessed through help of entity framework */
        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; }

        [JsonIgnore]
        public virtual Category? ParentCat { get; set; } /* Parent category */

        [JsonIgnore]
        public virtual ICollection<Category> SubCategories { get; set; } /* Subcategories */

        public Category(string nameLT, string merchantCatId, Merchant merchant)
        {
            NameLT = nameLT;
            MerchantCatId = merchantCatId;
            Merchant = merchant;
            /* If there are none, EF will leave as null, so - create new here */
            Items = new List<Item>();
            SubCategories = new List<Category>();
            ParentCat = null;
        }

        public override string ToString()
        {
            var id = MerchantCatId is null ? "ID: null" : "ID: '" + MerchantCatId + "'";
            var str = NameLT is null ? "null" : NameLT;
            var count = Items is null ? "null" : Items.Count.ToString();
            return id + ", " + str + "' [" + count + "] ";
        }
    }
}
