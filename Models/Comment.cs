//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResearchGate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int ID { get; set; }
        public string comment1 { get; set; }
        public Nullable<System.DateTime> dates { get; set; }
        public Nullable<int> ArticalID { get; set; }
        public Nullable<int> AuthorID { get; set; }
    
        public virtual Artical Artical { get; set; }
        public virtual Author Author { get; set; }
    }
}
