//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eExtractor.Test.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pdf1099K
    {
        public int ID { get; set; }
        public int TaxYear { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string TaxID { get; set; }
        public string FederalID { get; set; }
        public string AccountNum { get; set; }
        public byte[] PDFContent { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
    }
}
