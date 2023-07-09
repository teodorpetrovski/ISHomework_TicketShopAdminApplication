using System.ComponentModel.DataAnnotations;
using System;

namespace TicketShopAdminApplication.Models
{
    public class Ticket
    {
       
        public string MovieName { get; set; }
        
        public string MovieDescription { get; set; }
       
        public double TicketPrice { get; set; }
       
        public double MovieRating { get; set; }

        public string Genre { get; set; }

        public DateTime ValidFor { get; set; }
    }
}
