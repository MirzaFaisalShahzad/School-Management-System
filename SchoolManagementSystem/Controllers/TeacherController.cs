using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        SchoolDBEntities db = new SchoolDBEntities();
        public ActionResult Index()
        {
            List<TeacherTb> teacherlist = db.TeacherTbs.ToList();
            var teacherAndClass = teacherlist.Select(x => new TeacherGeneralClass
            {
                t_idt = x.t_id,
                T_Fnamet = x.T_Fname,
                T_Lnamet = x.T_Lname,
                T_DOBt = x.T_DOB,
                T_Addresst = x.T_Address,
                T_Phonet = x.T_Phone,
                ClassNamet = x.ClassTb.Class_Name
            }).ToList();
            return View(teacherAndClass);
        }
        [HttpGet]
        public ActionResult UpdateTeacher(int id)
        {
            ViewData["t_Cid"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            SqlParameter p1 = new SqlParameter("@id", id);
            var res = db.Database.SqlQuery<TeacherTb>("sp_UpdateTeacher @id", p1).SingleOrDefault();
            return View(res);
        }
        [HttpPost]
        public ActionResult UpdateTeacher(int id, TeacherTb t)
        {
            // SqlParameter[] par = new SqlParameter[7];
            db.Database.ExecuteSqlCommand("sp_UpdateTeacherById @fname,@lname,@dob,@address,@phone,@cid,@id",
                new object[] {
                   new SqlParameter("@fname",t.T_Fname),
                   new SqlParameter("@lname",t.T_Lname),
                   new SqlParameter("@dob",t.T_DOB),
                   new SqlParameter("@address",t.T_Address),
                   new SqlParameter("@phone",t.T_Phone),
                   new SqlParameter("@cid",t.t_Cid),
                   new SqlParameter("@id",t.t_id),
                });

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AddTeacher()
        {
            ViewData["t_Cid"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");

            return View();
        }
        [HttpPost]
        public ActionResult AddTeacher(TeacherTb t)
        {
            ViewData["t_Cid"] = new SelectList(db.ClassTbs, "c_id", "Class_Name");
            db.Database.ExecuteSqlCommand("sp_CreateTeacher @fname,@lname,@dob,@address,@phone,@cid,@id",
               new object[] {
                   new SqlParameter("@fname",t.T_Fname),
                   new SqlParameter("@lname",t.T_Lname),
                   new SqlParameter("@dob",t.T_DOB),
                   new SqlParameter("@address",t.T_Address),
                   new SqlParameter("@phone",t.T_Phone),
                   new SqlParameter("@cid",t.t_Cid),
                   new SqlParameter("@id",t.t_id),
               });
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DelTeacher(int id)
        {
            db.Database.ExecuteSqlCommand("sp_DelTeacherById @id",
                new object[] { new SqlParameter("@id", id) });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SubmitTeacherAttndance()
        {
           
           
            return View();
        }
        [HttpPost]
        public ActionResult SubmitTeacherAttndance(List<TeacherGeneralClass> td)
        {
            List<TeacherTb> tech = db.TeacherTbs.ToList();
            List<TeacherAttendenceTb> techAtt = db.TeacherAttendenceTbs.ToList();
            var da = DateTime.ParseExact(Request.Form["SelectedDate"].ToString(), "MM/dd/yyyy",null);
           TempData["g"]= da;
            var data = from t in tech
                       join ta in techAtt.Where(x=>x.Date==da) on t.t_id equals ta.teacherId into table1
                       from ta in table1.DefaultIfEmpty()
                       select new TeacherGeneralClass { teachertbt = t, teacherAttendenceTbt = ta };
            List<TeacherGeneralClass> g = data.ToList();
            if (g != null)
            {
                for (int j=0; j<g.Count();j++)
                {
                    if (g[j].teacherAttendenceTbt != null)
                    {
                        if (g[j].teacherAttendenceTbt.Date == null)
                            g[j].t_Datet = da;
                        else
                            g[j].t_Datet = g[j].teacherAttendenceTbt.Date;
                        if (g[j].teacherAttendenceTbt.Status == 1)
                            g[j].tstatust = true;
                        else if (g[j].teacherAttendenceTbt.Status == 2)
                            g[j].tstatust = false;
                        else
                            g[j].tstatust = false;
                        g[j].tRemarkst = g[j].teacherAttendenceTbt.Remarks;
                    }
                    else
                    g[j].t_Datet = da;
                    
                }
            }
            return View(g);


        }

        [HttpPost]
        public ActionResult SubmitTeacherAttendancetoDb(List<TeacherGeneralClass> t)
        {
            int p;
            DateTime ui;
            var ft = TempData["g"];
            if (DateTime.TryParse((ft ?? "").ToString(), out ui))
                
            foreach (var i in t)
            {

                var row = db.TeacherAttendenceTbs.Where(x => x.TeacherTb.t_id.Equals(i.teachertbt.t_id))
                    .Where(x => x.Date.Equals(i.t_Datet)).SingleOrDefault();
                if (row == null)
                {
                    if (i.tstatust == true)
                    {
                        p = 1;
                    }
                    else
                        p = 2;
                    TeacherAttendenceTb userrow = new TeacherAttendenceTb();
                    userrow.Date = i.t_Datet;
                    userrow.Status = p;
                    userrow.teacherId = i.teachertbt.t_id;
                    userrow.Remarks = i.tRemarkst;
                    db.TeacherAttendenceTbs.Add(userrow);
                    db.SaveChanges();

                }
                else
                {
                    int y;
                    if (i.tstatust == true)
                        y = 1;
                    else
                        y = 2;
                    if (i.tRemarkst == null)
                        i.tRemarkst = DBNull.Value.ToString();
                    db.Database.ExecuteSqlCommand("sp_UpdateTeacherAttendence @data,@status,@teacherId,@remarks",
                        new object[]
                        {new SqlParameter("@data",i.t_Datet),
                        new SqlParameter("@status",y),
                        new SqlParameter("@teacherId",i.teachertbt.t_id),
                        new SqlParameter("@remarks",i.tRemarkst)
                        });

                }
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult DisplayTeacherAttendanceDetails()
        {
           
            
            return View();
        }
        [HttpPost]
        public ActionResult DisplayTeacherAttendanceDetails(int id=0)
        {
            var da = DateTime.ParseExact(Request.Form["SelectedDate"].ToString(), "MM/dd/yyyy", null);
            List<TeacherTb> tea = db.TeacherTbs.ToList();
            List<TeacherAttendenceTb> tat = db.TeacherAttendenceTbs.ToList();
            List<AttendenceValue> avl = db.AttendenceValues.ToList();
            var g = from t in tea
                    join ta in tat.Where(x=>x.Date==da) on t.t_id equals ta.teacherId into table1
                    from ta in table1.DefaultIfEmpty()
                    select new TeacherGeneralClass { teachertbt = t, teacherAttendenceTbt = ta };
            List<TeacherGeneralClass> tg = g.ToList();
            for(int i =0;i<tg.Count();i++)
            {
                if(tg[i].teacherAttendenceTbt==null)
                {
                    tg[i].temstatus = "Not Take Yet";
                    tg[i].t_Datet = da;
                    tg[i].tRemarkst = "";
                    
                }
            }

            return View(tg);
        }



    }
}