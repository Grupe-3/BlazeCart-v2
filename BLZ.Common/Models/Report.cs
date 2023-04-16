using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLZ.Common.Models
{
    [Index(nameof(ItemId))]
    public class Report
    {
        public enum ReportReason
        {
            InaccuratePrice, InaccurateName, InaccurateCategory, NotAvailable
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }

        [JsonIgnore]
        public virtual Item Item { get; set; }

        public ReportReason Reason { get; set; }

        public bool IsSpam { get; set; }
        public bool IsSolved { get; set; }
        public DateTime Created { get; set; }

        public Report()
        {
            UserId = "";
        }
    }
}
