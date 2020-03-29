using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Models
{
   
    public class GeneralClass
    {
        public StudentTb studenttb { get; set; }
        public ClassTb classtb { get; set; }
        public Attendence attendencetb { get; set; }
        public TeacherTb teachertb { get; set; }
        public TeacherAttendenceTb teacherAttendenceTb { get; set; }
        public TeacherGeneralClass teachergenralclass {get;set;}
        
        


    }
}