using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBanDT.Models
{
    public class ProductInCartViewModel
    {
        public int ID { get; set; }
        public string TENSP { get; set; }
        public int IDHANG { get; set; }
        public double GIA { get; set; }
        public string TINHTRANG { get; set; }

        public string HINHANH { get; set; }


    }
}