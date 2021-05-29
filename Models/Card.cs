using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualCard.Models
{
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        public string Number { get; set; }
        public Client Client { get; set; }
        public string   ClientEmail { get; set; }
        public DateTime DateOrder { get; set; }
    }
}