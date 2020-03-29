using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        SchoolDBEntities db = new SchoolDBEntities();
        // GET: Student
        public ActionResult Index()
        {
            StudentCount();

            return View();            
            
        }
        public ActionResult FirstPage()
        {
            StudentCount();
            List<StudentTb> st = db.StudentTbs.ToList();
            var s = st.Select(x => new StudentTb
            {
                s_id = x.s_id,
                F_Name = x.F_Name,
                L_Name = x.L_Name,
                Father_Name = x.Father_Name,
                DOB = x.DOB,
                Phone = x.Phone,
                ClassName = x.ClassTb.Class_Name
            }).ToList();
            return View(s);
        }
        [HttpGet]
        public ActionResult AddStudent()
        {

            ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddStudent(StudentTb s)
        {

            
            SqlParameter p1 = new SqlParameter("@fname",s.F_Name);
            SqlParameter p2 = new SqlParameter("@lname", s.L_Name);
            SqlParameter p3 = new SqlParameter("@father_name", s.Father_Name);
            SqlParameter p4 = new SqlParameter("@dob",s.DOB);
            SqlParameter p5 = new SqlParameter("@phone", s.Phone);
            SqlParameter p6 = new SqlParameter("@c_id",s.c_id);
           // ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            db.Database.ExecuteSqlCommand("sp_insertStu @fname,@lname,@father_name,@dob,@phone,@c_id",p1,p2,p3,p4,p5,p6);
           // ViewBag.m = "data submit";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateStudent(int id)
        {
            ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            SqlParameter p1 = new SqlParameter("@id",id);
            var d = db.Database.SqlQuery<StudentTb>("sp_SelectById @id", p1).SingleOrDefault();
            return View(d);
        }
        [HttpPost]
        public ActionResult UpdateStudent(int id ,StudentTb s)
        {
            SqlParameter p1 = new SqlParameter("@fname", s.F_Name);
            SqlParameter p2 = new SqlParameter("@lname", s.L_Name);
            SqlParameter p3 = new SqlParameter("@father_name", s.Father_Name);
            SqlParameter p4 = new SqlParameter("@dob", s.DOB);
            SqlParameter p5 = new SqlParameter("@phone", s.Phone);
            SqlParameter p6 = new SqlParameter("@c_id", s.c_id);
            SqlParameter p7 = new SqlParameter("@id", id);
            // ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            db.Database.ExecuteSqlCommand("sp_UpdateStu @fname,@lname,@father_name,@dob,@phone,@c_id,@id", p1, p2, p3, p4, p5, p6,p7);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DelStudent(int id)
        {
            SqlParameter p1 = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand("sp_delStuById @id",p1);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult TakeAttendence()
        {
            ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
           
            return View();
        }
       [HttpPost]
        public ActionResult TakeAttendence(FormCollection c)
        {
            ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            ViewData["a_id"] = new SelectList(db.AttendenceValues, "a_id", "Status");
           
            
            int cid=Int32.Parse(Request.Form["c_id"].ToString());
            DateTime c_date = DateTime.ParseExact(Request.Form["choosedate"].ToString(), "MM/dd/yyyy", null);

            List<StudentTb> stu = db.StudentTbs.ToList();
            List<ClassTb> cls = db.ClassTbs.ToList();
            List<Attendence> att = db.Attendences.ToList();



            var res = from s in stu
                      where s.c_id == cid
                      join cl in cls on s.c_id equals cl.c_id into table1
                      from cl in table1.DefaultIfEmpty()
                      join a in att on s.s_id equals a.studentId into table2
                      from a in table2.DefaultIfEmpty()
                      
                      select new GeneralClass { studenttb = s, classtb = cl , attendencetb = a };
            List<GeneralClass> g = res.ToList();
            if (g != null)
            {
                for (int f = 0; f < g.Count(); f++)
                {
                    if (g[f].attendencetb == null)
                    {
                        g[f].studenttb.s_Date = c_date;
                        g[f].studenttb.s_remarks = "";
                        g[f].studenttb.s_Status = false;
                    }
                    else
                    {
                        g[f].studenttb.s_Date = g[f].attendencetb.Date;
                        g[f].studenttb.s_remarks = g[f].attendencetb.Remarks;
                        if (g[f].attendencetb.Status == 1)
                            g[f].studenttb.s_Status = true;
                        else if (g[f].attendencetb.Status == 2)
                            g[f].studenttb.s_Status = false;
                        else
                            g[f].studenttb.s_Status = false;

                    }


                }
            }

            return View(g);
           // return View(res);
        }

        [HttpPost]
        public ActionResult SubmitAttndence(List<GeneralClass> model )
        {
            int p;
              foreach (var i in model)
             {
                var userRows = db.Attendences.Where(x => x.StudentTb.s_id.Equals(i.studenttb.s_id))
                    .Where(x=>x.Date.Equals(i.studenttb.s_Date)).SingleOrDefault();
               
                if (userRows == null)
                {
                    Attendence userRow = new Attendence();
                    userRow.Date = i.studenttb.s_Date;
                    if (i.studenttb.s_Status == true)
                    {
                         p = 1;
                    }
                    else
                        p = 2;
                    userRow.Status = p;
                    userRow.Remarks = i.studenttb.s_remarks;
                    userRow.studentId = i.studenttb.s_id;
                    db.Attendences.Add(userRow);
                    db.SaveChanges();

                }
                else
                {
                    
                    int tg;
                    if (i.studenttb.s_Status == true)
                    {
                        tg = 1;
                    }
                    else
                        tg = 2;

                    if (i.studenttb.s_remarks == null)
                    {
                        i.studenttb.s_remarks = DBNull.Value.ToString();
                    }
                    SqlParameter p1 = new SqlParameter("@date", i.studenttb.s_Date);
                    SqlParameter p2 = new SqlParameter("@status",tg );
                    SqlParameter p3 = new SqlParameter("@studentid", i.studenttb.s_id);
                    SqlParameter p4 = new SqlParameter("@remarks", i.studenttb.s_remarks);
                   
                    db.Database.ExecuteSqlCommand("sp_updateAttendence @date,@status,@studentid,@remarks", p1, p2, p3, p4);

                }
                
            }
          return View();
        }

        [HttpGet]
        public ActionResult DisplayStudentAttendance()
        {
            ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            return View();
        }
        [HttpPost]
        public ActionResult DisplayStudentAttendance(FormCollection c)
        {
            ViewData["c_id"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            int cls=Int32.Parse(Request.Form["c_id"].ToString());
            DateTime da = DateTime.ParseExact(Request.Form["selectDate"].ToString(), "MM/dd/yyyy", null);

            List<StudentTb> stu = db.StudentTbs.ToList();
            List<Attendence> att = db.Attendences.ToList();
            var data = from s in stu
                       where s.c_id == cls
                       join a in att.Where(x=>x.Date==da) on s.s_id equals a.studentId into table1
                       from a in table1.DefaultIfEmpty()
                       select new GeneralClass { studenttb = s, attendencetb = a };
            List<GeneralClass> g = data.ToList();
            for(int t=0;t<g.Count();t++)
            {
                if(g[t].attendencetb==null)
                {
                    g[t].studenttb.s_Date = da;
                    g[t].studenttb.temStatus = "Not Take Yet";
                    g[t].studenttb.s_remarks = "";
                }
                else
                {
                    g[t].studenttb.s_Date = g[t].attendencetb.Date;

                   // g[t].attendencetb.AttendenceValue.Status = g[t].attendencetb.Status;
                    g[t].studenttb.s_remarks = g[t].attendencetb.Remarks;
                }
            };
            return View(g);
        }

        public void StudentCount()
        {
            string c = "Data Source=DESKTOP-B8EGR2A;Initial Catalog=SchoolDB;User ID=sa;Password=faisal786;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection con = new SqlConnection(c);
            SqlCommand cmd = new SqlCommand("sp_TotalStudentCounter", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Total_student", SqlDbType.Int);
            cmd.Parameters["@Total_student"].Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            ViewData["TotalStudent"] = Int32.Parse(cmd.Parameters["@Total_student"].Value.ToString());
            con.Close();

        }
    }
}