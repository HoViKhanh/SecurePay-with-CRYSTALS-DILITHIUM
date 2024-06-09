﻿using MongoDB.Bson;

namespace SecurePay.Models
{
    public class BuyerInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        

    }
}