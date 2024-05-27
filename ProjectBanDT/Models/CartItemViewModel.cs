using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBanDT.Models
{
    public class CartItemViewModel
    {
        public string TENSP { get; set; }
        public double GIA { get; set; }
        public int SOLUONG { get; set; }
        public string HINHANH { get; set; }
        public int ID { get; internal set; }
    }
}