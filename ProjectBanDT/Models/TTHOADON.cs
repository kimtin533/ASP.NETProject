//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectBanDT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TTHOADON
    {
        public int ID { get; set; }
        public int IDHOADON { get; set; }
        public int IDSANPHAM { get; set; }
        public int SOLUONG { get; set; }
    
        public virtual HOADON HOADON { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }
    }
}
