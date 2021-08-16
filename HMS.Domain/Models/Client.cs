using System;
using System.Collections.Generic;
using System.Text;

namespace HMS.Domain.Models
{
    public class Client : User
    {
        public ICollection<Transaction> Transactions { get; set; }
    }
}
