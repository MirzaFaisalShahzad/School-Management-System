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
    
    public partial class StudentTb
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentTb()
        {
            this.Attendences = new HashSet<Attendence>();
            this.StudentFeeTbs = new HashSet<StudentFeeTb>();
        }
    
        public int s_id { get; set; }
        public string F_Name { get; set; }
        public string L_Name { get; set; }
        public string Father_Name { get; set; }
        public System.DateTime DOB { get; set; }
        public long Phone { get; set; }
        public int c_id { get; set; }

        public bool s_Status { get; set; }
        public string temStatus { get; set; }

        public string ClassName { get; set; }
        public System.DateTime s_Date { get; set; }
        public string s_remarks { get; set; }
        public virtual ClassTb ClassTb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendence> Attendences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentFeeTb> StudentFeeTbs { get; set; }
    }
}
