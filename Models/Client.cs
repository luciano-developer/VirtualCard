using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualCard.Models
{
    public class Client
    {
        [Key]
        public string Email { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
