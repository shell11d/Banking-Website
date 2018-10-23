using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Entities
{
    public class Card
    {

        public string CardNumber;

        public DateTime DateOfIssue;

        public DateTime DateOfExpiry;

        public string NameOnCard;

        public int Cvv;


        public CardStatus Status;

    }
}
