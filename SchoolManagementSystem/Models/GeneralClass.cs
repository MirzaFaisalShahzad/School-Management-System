using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Models
{
   
    public class GeneralClass
    {
        public string sMonth { get; set; }
        public long sSalary { get; set; }
        public int sStudentId { get; set; }
        public long sPending { get; set; }
        public string sStatus { get; set; }
        public System.DateTime sDate { get; set; }
        public StudentTb studenttb { get; set; }
        public ClassTb classtb { get; set; }
        public Attendence attendencetb { get; set; }
        public TeacherTb teachertb { get; set; }
        public TeacherAttendenceTb teacherAttendenceTb { get; set; }
        public TeacherGeneralClass teachergenralclass {get;set;}
        public StudentFeeTb studentfeetb { get; set; }
        
        


    }
}