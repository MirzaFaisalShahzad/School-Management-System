using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class TeacherGeneralClass
    {
        public int t_idt { get; set; }
        public string T_Fnamet { get; set; }
        public string T_Lnamet { get; set; }
        public System.DateTime T_DOBt { get; set; }
        public long T_Phonet { get; set; }
        public string T_Addresst { get; set; }
        public int t_Cidt { get; set; }
        public string ClassNamet { get; set; }
        public bool tstatust { get; set; }
        public string temstatus { get; set; }
        public string tRemarkst { get; set; }
        
        public DateTime t_Datet { get; set; }
        public virtual ClassTb ClassTbt { get; set; }
       
        public TeacherTb teachertbt { get; set; }
        public TeacherAttendenceTb teacherAttendenceTbt { get; set; }
       
    }
}