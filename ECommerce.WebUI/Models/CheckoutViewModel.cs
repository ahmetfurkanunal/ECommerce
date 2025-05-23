﻿using ECommerce.Core.Entities;

namespace ECommerce.WebUI.Models
{
    public class CheckoutViewModel
    {

        public List<CartLine>? CartProducts { get; set; }

        public decimal TotalPrice { get; set; }
        public List<Adress>? Adresses { get; set; }
        public List<Adress> Addresses { get; internal set; }
    }
}
