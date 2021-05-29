using System.Collections.Generic;

namespace VirtualCard.Models
{
    public class CardByClient
    {
        public List<string> Numbers { get; set; } = new List<string>();
        public string Client { get; set; }
    }
}