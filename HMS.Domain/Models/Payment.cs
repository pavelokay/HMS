using System;
using System.Collections.Generic;
using System.Text;

namespace HMS.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
