//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeacherFeeTb
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public long Salary { get; set; }
        public int TeacherId { get; set; }
        public long Pending { get; set; }
        public string Status { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual TeacherTb TeacherTb { get; set; }
    }
}