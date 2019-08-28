using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class Purchases
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Books")]
        public int BookId { get; set; }
        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public int Term { get; set; }
        public DateTime DateOfPurchase { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfReturn { get; set; }
        public Books Books { get; set; }
        public Users Users { get; set; }
    }
}