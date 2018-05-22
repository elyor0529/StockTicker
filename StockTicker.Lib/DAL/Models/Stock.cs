using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockTicker.Lib.DAL.EF;

namespace StockTicker.Lib.DAL.Models
{

    public class Stock : BaseEntity
    {

        [Required]
        [Index("IX_Symbol", 1, IsUnique = true)]
        [StringLength(50)]
        public string Symbol { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public User User { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public override string ToString()
        {
            return String.Format("{0}-{1}({2:C})", Symbol, Name, Price);
        }
    }
}
