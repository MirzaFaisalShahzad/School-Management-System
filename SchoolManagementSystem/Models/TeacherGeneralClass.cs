using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
   
    public class TeacherGeneralClass
    {
        public enum months
        {
            Jan,
            Feb,
            March,
            April,
            May,
            June,
            Julay,
            August,
            September,
            October,
            November,
            December
        };
        public enum SalaryStatus
        {
            paid,
            Unpaid
        }
       // public months month { get; set; }
        public string fMonth { get; set; }
        public long fSalary { get; set; }
        public int fTeacherId { get; set; }
        public long fPending { get; set; }
        public string fStatus { get; set; }
        public SalaryStatus fStatusenum { get; set; }
        public System.DateTime fDate { get; set; }

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
        public TeacherFeeTb teacherFeetb { get; set; }
       
    }
}