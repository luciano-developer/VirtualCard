using CreditCardValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualCard.Models
{
    public static class CardFactory
    {
        public static string getNewCardNumber() => CreditCardFactory.RandomCardNumber(CardIssuer.Visa);
    }
}
