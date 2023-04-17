using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLZ.Client.Models
{
    public class Cart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> CartItems { get; set; }

        public string Image { get; set; }
        public int? ItemsCount { get; set; }

        public double? TotalPrice { get; set; }
    }
}
