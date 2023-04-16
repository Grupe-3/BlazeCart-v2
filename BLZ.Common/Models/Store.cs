using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLZ.Common.Models
{
    [Index(nameof(Id), nameof(Merchant), IsUnique = true)]
    public class Store
    {
        [Column("StoreId")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public Merchant Merchant { get; set; }
    }
}
