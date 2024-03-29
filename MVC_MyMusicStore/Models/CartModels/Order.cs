﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_MyMusicStore.Models.CartModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        //
        public string Username { get; set; }
        //
        [Required(ErrorMessage = " First name is required")]
        public string FirstName { get; set; }
        //
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        //
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        //
        [Required(ErrorMessage = "Please enter your city name")]
        public string City { get; set; }
        //
        [Required(ErrorMessage = "Please enter your state")]
        public string State { get; set; }
        //
        [Required(ErrorMessage = "Pin code is required")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        //
        [Required (ErrorMessage = "Enter your country")]
        public string Country { get; set; }
        //
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        //
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //
        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        //
        public System.DateTime OrderDate { get; set; }
        //
        public List<OrderDetail> OrderDetails { get; set; }
        public string? Status { get; set; }
    }
}
