using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            
           
            DBEnt db = new DBEnt();
            var mcqs = db.Mcqs.Count().ToString();
            var users = db.AspNetUsers.Count().ToString();
            var messages = db.Messages.Count().ToString();
            var opensources = db.Submissions.Count().ToString();
            List<string> contentCount = new List<string>();
            contentCount.Add(mcqs);
            contentCount.Add(users);
            contentCount.Add(messages);
            contentCount.Add(opensources);
            ViewData["contentCount"] = contentCount;
            return View();
        }

        public ActionResult pendingMcq()
        {
            DBEnt db = new DBEnt();
            var mcqs = db.Mcqs.Where(x => x.Status != "approve").ToList<Mcq>();
            List<McqViewModel> mcq = new List<McqViewModel>();
            foreach(Mcq i in mcqs)
            {
                McqViewModel obj = new McqViewModel()
                {
                    Id = i.Id,
                    Question = i.Question,
                    OptionA = i.OptionA,
                    OptionB = i.OptionB,
                    OptionC = i.OptionC,
                    OptionD = i.OptionD,
                    CorrectOption = i.CorrectOption,
                    Exam = getExamType(i.ExamId),
                    ChapterId = Convert.ToInt32(i.ChapterId),
                    SubjectId = Convert.ToInt32(i.SubjectId),
                    Upvotes = Convert.ToInt32(i.UpVotes),
                    Downvotes = Convert.ToInt32(i.DownVotes),
                    Status = i.Status,
                    Subject = mcqSubject(i.Id),
                    Chapter = mcqChapter(i.Id)

                };
                mcq.Add(obj);
            }
            return View(mcq);

        }

       



        public ActionResult subjects()
        {
            List<Subject> subjects = (new DBEnt()).Subjects.ToList<Subject>();
            List<Subject> sub = new List<Subject>();
            foreach(Subject i in subjects)
            {
                Subject obj = new Subject()
                {
                    Id=i.Id,
                    Name=i.Name
                };
                
                sub.Add(obj);
            }

            List<Subject> sorted = sub.OrderBy(x => x.Name).ToList();

            return Json(sorted, JsonRequestBehavior.AllowGet);

        }

        public ActionResult exams()
        {
            List<Exam> exams = (new DBEnt()).Exams.ToList<Exam>();
            List<Exam> exm = new List<Exam>();
            foreach(Exam i in exams)
            {
                Exam obj = new Exam()
                {
                    Id = i.Id,
                    Name = i.Name

                };
                exm.Add(obj);
            }
            exm = exm.OrderBy(x => x.Name).ToList();
            return Json(exm, JsonRequestBehavior.AllowGet);
        }


        public ActionResult chapters(int id)
        {
            List<Chapter> chapters = (new DBEnt()).Chapters.Where(x => x.SubjectId == id).ToList<Chapter>();
            List<Chapter> chaps = new List<Chapter>();
            foreach(Chapter i in chapters)
            {
                Chapter obj = new Chapter()
                {
                    Id=i.Id,
                    Name=i.Name,
                    ChapterNo=i.ChapterNo
                    

                };
                chaps.Add(obj);
            }
            chaps = chaps.OrderBy(x => x.Name).ToList();
            return Json(chaps, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddMcq()
        { 


            return View(new McqViewModel());
        }

       [HttpPost]
        public ActionResult AddMcq(McqViewModel collection)
        {
            Mcq mcq = new Mcq()
            {
                ExamId = collection.ExamId,
                SubjectId = collection.SubjectId,
                ChapterId = collection.ChapterId,
                Question = collection.Question,
                OptionA = collection.OptionA,
                OptionB = collection.OptionB,
                OptionC = collection.OptionC,
                OptionD = collection.OptionD,
                CorrectOption = collection.CorrectOption,
                UpVotes = 5,
                DownVotes = 0,
                Status = "approve",
                EntryDate = DateTime.Now,
               
            };
            DBEnt db = new DBEnt();
            db.Submissions.Add(new Submission() { UserID = User.Identity.GetUserId(), McqId = mcq.Id });
            db.Mcqs.Add(mcq);
            db.SaveChanges();
            Alerts.addMcq = true;
            return RedirectToAction("allMcq", "Admin");
        }





        public ActionResult editMcq(long id)
        {
            DBEnt db = new DBEnt();
            var mcq = db.Mcqs.Where(x => x.Id == id).FirstOrDefault();
            ViewData["mcq"] = mcq;
            return View();
        }

        [HttpPost]
        public ActionResult editMcq(long id,McqViewModel collection)
        {
            DBEnt db = new DBEnt();
            var mcq = db.Mcqs.Where(x => x.Id == id).FirstOrDefault();
            mcq.Question = collection.Question;
            mcq.OptionA = collection.OptionA;
            mcq.OptionB = collection.OptionB;
            mcq.OptionC = collection.OptionC;
            mcq.OptionD = collection.OptionD;
            mcq.CorrectOption = collection.CorrectOption;
            db.SaveChanges();
            Alerts.mcqupdated = true;
            return RedirectToAction("allMcq", "Admin");
        }






        public ActionResult addExam()
        {
            DBEnt db = new DBEnt();
            var exams = db.Exams.ToList<Exam>();
            ViewData["exams"] = exams;
            return View(new ExamViewModel());
        }


        public ActionResult editExam(long? id)
        {
            DBEnt db = new DBEnt();
            var exam = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            ViewData["exam"] = exam;
            return View();
        }


        [HttpPost]

        public ActionResult editExam(long? id,ExamViewModel collection)
        {
            DBEnt db = new DBEnt();
            var exam = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            if (collection.Image!=null)
            {
                if (!examExist(collection.Name))
                {

                    string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                    string ext = Path.GetExtension(collection.Image.FileName);
                    filename = filename + DateTime.Now.Millisecond.ToString();
                    filename = filename + ext;
                    string filetodb = "/Image/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                    collection.Image.SaveAs(filename);
                    collection.Cover = filetodb;
                    exam.Name = collection.Name;
                    exam.Cover = collection.Cover;
                    exam.TotalQuestions = collection.TotalQuestions;
                    exam.MarkPerMcq = collection.MarkPerMcq;
                    exam.NegativeMark = collection.NegativeMark;
                    db.SaveChanges();
                    Alerts.examupdated = true;
                    return RedirectToAction("addExam", "Admin");

                }
                else
                {
                    if (collection.Image != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                        string ext = Path.GetExtension(collection.Image.FileName);
                        filename = filename + DateTime.Now.Millisecond.ToString();
                        filename = filename + ext;
                        string filetodb = "/Image/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                        collection.Image.SaveAs(filename);
                        collection.Cover = filetodb;
                        exam.Cover = collection.Cover;
                        exam.TotalQuestions = collection.TotalQuestions;
                        exam.MarkPerMcq = collection.MarkPerMcq;
                        exam.NegativeMark = collection.NegativeMark;
                        db.SaveChanges();
                        Alerts.examupdated = true;
                        return RedirectToAction("addExam", "Admin");

                    }
                    Alerts.alreadyExist = true;
                    return RedirectToAction("addExam", "Admin");
                }

            }
            else
            {
                if (!examExist(collection.Name))
                {

                    exam.Name = collection.Name;
                    exam.TotalQuestions = collection.TotalQuestions;
                    exam.MarkPerMcq = collection.MarkPerMcq;
                    exam.NegativeMark = collection.NegativeMark;
                    db.SaveChanges();
                    Alerts.examupdated = true;
                    return RedirectToAction("addExam", "Admin");

                }
                else
                {
                    Alerts.alreadyExist = true;
                    return RedirectToAction("addExam", "Admin");
                }
            }
            return View();
        }



        [HttpPost]
        public ActionResult addExam(ExamViewModel collection)
        {
            if (!examExist(collection.Name))
            {
                DBEnt db = new DBEnt();

                string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                string ext = Path.GetExtension(collection.Image.FileName);
                filename = filename + DateTime.Now.Millisecond.ToString();
                filename = filename + ext;
                string filetodb = "/Image/" + filename;
                filename = Path.Combine(Server.MapPath("~/Image/"), filename);

                collection.Image.SaveAs(filename);
                collection.Cover = filetodb;

                db.Exams.Add(new Exam() { Name = collection.Name, Cover = collection.Cover,TotalQuestions=collection.TotalQuestions,MarkPerMcq=collection.MarkPerMcq,NegativeMark=collection.NegativeMark });
                db.SaveChanges();
                Alerts.addExam = true;
                return RedirectToAction("addExam", "Admin");
                
            }
            else
            {
                Alerts.alreadyExist = true;
                return RedirectToAction("addExam","Admin");
            }
        }

        public ActionResult deleteExam(int? id)
        {
            DBEnt db = new DBEnt();
            var mcqs = db.Mcqs.Where(x => x.ExamId == id).ToList<Mcq>();
            foreach(var i in mcqs)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
                var sub = db.Submissions.Where(x => x.McqId == i.Id).FirstOrDefault();
                db.Entry(sub).State = System.Data.Entity.EntityState.Deleted;

            }
            db.SaveChanges();
            var exam = db.Exams.Where(x => x.Id == id).First();
            db.Entry(exam).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Alerts.deleteExam = true;
            return RedirectToAction("AddExam", "Admin");
        }



        public ActionResult addSubject()
        {
            DBEnt db = new DBEnt();
            List<Subject> subjects = db.Subjects.ToList<Subject>();
            ViewData["subjects"] = subjects;
           return View(new SubjectViewModel());
        }




        public ActionResult editSubject(long? id)
        {
            DBEnt db = new DBEnt();
            var subject = db.Subjects.Where(x => x.Id == id).FirstOrDefault();
            ViewData["subject"] = subject;
            return View();
        }






        [HttpPost]
        public ActionResult editSubject(long? id, SubjectViewModel collection)
        {
            DBEnt db = new DBEnt();
            var subject = db.Subjects.Where(x => x.Id == id).FirstOrDefault();
            if (collection.Image != null)
            {
                if (!subjectExist(collection.Name))
                {
                    string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                    string ext = Path.GetExtension(collection.Image.FileName);
                    filename = filename + DateTime.Now.Millisecond.ToString();
                    filename = filename + ext;
                    string filetodb = "/Image/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                    collection.Image.SaveAs(filename);
                    collection.Cover = filetodb;
                    subject.Name = collection.Name;
                    subject.Cover = collection.Cover;
                    db.SaveChanges();
                    Alerts.subjectupdated = true;
                    return RedirectToAction("addSubject", "Admin");

                }
                else
                {
                    if (collection.Image != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                        string ext = Path.GetExtension(collection.Image.FileName);
                        filename = filename + DateTime.Now.Millisecond.ToString();
                        filename = filename + ext;
                        string filetodb = "/Image/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                        collection.Image.SaveAs(filename);
                        collection.Cover = filetodb;
                        subject.Cover = collection.Cover;
                        db.SaveChanges();
                        Alerts.subjectupdated = true;
                        return RedirectToAction("addSubject", "Admin");

                    }
                    Alerts.alreadyExist = true;
                    return RedirectToAction("addSubject", "Admin");
                }

            }
            else
            {
                if (!subjectExist(collection.Name))
                {

                    subject.Name = collection.Name;
                    db.SaveChanges();
                    Alerts.subjectupdated = true;
                    return RedirectToAction("addSubject", "Admin");

                }
                else
                {
                    Alerts.alreadyExist = true;
                    return RedirectToAction("addSubject", "Admin");
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult addSubject(SubjectViewModel collection)
        {
            if (!subjectExist(collection.Name))
            {
                DBEnt db = new DBEnt();

                string filename = Path.GetFileNameWithoutExtension(collection.Image.FileName);
                string ext = Path.GetExtension(collection.Image.FileName);
                filename = filename + DateTime.Now.Millisecond.ToString();
                filename = filename + ext;
                string filetodb = "/Image/" + filename;
                filename = Path.Combine(Server.MapPath("~/Image/"), filename);

                collection.Image.SaveAs(filename);
                collection.Cover = filetodb;

                db.Subjects.Add(new Subject() { Name = collection.Name,Cover=collection.Cover });
                db.SaveChanges();
                Alerts.addSubject = true;
                return RedirectToAction("addSubject","Admin");
            }
            else
            {
                Alerts.alreadyExist = true;
                return RedirectToAction("addSubject", "Admin");
            }
        }




        public ActionResult addChapter()
        {
            DBEnt db = new DBEnt();
            var chapters = db.Chapters.ToList<Chapter>();
            ViewData["chapters"] = chapters;
            return View(new ChapterViewModel());
        }

        [HttpPost]
        public ActionResult addChapter(ChapterViewModel collection)
        {



            
            DBEnt db = new DBEnt();
            Chapter chapter = new Chapter()
            {
                SubjectId=collection.SubjectId,
                Name=collection.Name,
                ChapterNo=collection.ChapterNo
            };
            db.Chapters.Add(chapter);
            db.SaveChanges();
            Alerts.addChapter = true;
            return RedirectToAction("addChapter","Admin");
            
        }


        public ActionResult editChapter(long? id)
        {
            DBEnt db = new DBEnt();
            var chapter = db.Chapters.Where(x => x.Id == id).FirstOrDefault();
            ViewData["chapter"] = chapter;
            return View();
        }

        [HttpPost]
        public ActionResult editChapter(long? id,ChapterViewModel collection)
        {

            DBEnt db = new DBEnt();
            var chap = db.Chapters.Where(x => x.Id == id).FirstOrDefault();
            chap.Name = collection.Name;
            chap.ChapterNo = collection.ChapterNo;
            db.SaveChanges();
            Alerts.chapterupdated = true;
            return RedirectToAction("addChapter", "Admin");

        }







        public ActionResult allMcq()
        {
            List<Mcq> mcqs = (new DBEnt()).Mcqs.Where(x=>x.Status=="approve").ToList<Mcq>();
            List<McqViewModel> mcqview = new List<McqViewModel>();
            foreach (Mcq i in mcqs)
            {
                McqViewModel obj = new McqViewModel()
                {
                    Id = i.Id,
                    Question = i.Question,
                    OptionA = i.OptionA,
                    OptionB = i.OptionB,
                    OptionC = i.OptionC,
                    OptionD = i.OptionD,
                    CorrectOption = i.CorrectOption,
                    Exam = getExamType(i.ExamId),
                    ChapterId = Convert.ToInt32(i.ChapterId),
                    SubjectId = Convert.ToInt32(i.SubjectId),
                    Upvotes = Convert.ToInt32(i.UpVotes),
                    Downvotes = Convert.ToInt32(i.DownVotes),
                    Status = i.Status,
                    Subject = mcqSubject(i.Id),
                    Chapter = mcqChapter(i.Id) 
                   
                };
                mcqview.Add(obj);

            }

            
            return View(mcqview);
        }

        public ActionResult allUsers()
        {
            DBEnt db = new DBEnt();
            List<AspNetUser> users = db.AspNetUsers.ToList<AspNetUser>();
            List<UserViewModel> usr = new List<UserViewModel>();
            foreach(AspNetUser i in users)
            {
                UserViewModel obj = new UserViewModel()
                {
                    Id=i.Id,
                    UserName = i.UserName,
                    Email = i.Email,
                };
                usr.Add(obj);
            }

            return View(usr);
        }

        public ActionResult deleteUser(string id)
        {
            DBEnt db = new DBEnt();
            
            var user = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            List<AspNetRole> roles=user.AspNetRoles.ToList();
            try
            {
                if (roles.FirstOrDefault().Name == "Admin")
                {
                    return Content("Admin cannot be deleted!");
                }
                if (roles.FirstOrDefault().Name != "Admin")
                {
                    db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    Alerts.deleteUser = true;
                    return RedirectToAction("allUsers", "Admin");
                }
            }
            catch
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Alerts.deleteUser = true;
                return RedirectToAction("allUsers", "Admin");
            }

            return Content("Error........");
            
        }


        


        public ActionResult deleteMcq(int id)
        {
            DBEnt db = new DBEnt();

            List<Submission> submissions = db.Submissions.Where(x => x.McqId == id).ToList<Submission>();
            foreach(Submission i in submissions)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            Mcq mcq = db.Mcqs.Where(x => x.Id == id).First();
            db.Entry(mcq).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Alerts.deleteMcq = true;
            return RedirectToAction("allMcq","Admin");
        }


        public ActionResult deleteChapter(int? id)
        {
            DBEnt db = new DBEnt();         
            List<Mcq> mcq = db.Mcqs.Where(x => x.ChapterId == id).ToList<Mcq>();
            foreach(Mcq i in mcq)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();
            Chapter chapter = db.Chapters.Where(x => x.Id == id).First();
            db.Entry(chapter).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Alerts.deleteChapter = true;
            return RedirectToAction("addChapter", "Admin");
        }



        public ActionResult deleteSubject(int? id)
        {
            DBEnt db = new DBEnt();
            Subject subject = db.Subjects.Where(x => x.Id == id).First();
          
            List<Mcq> mcq = db.Mcqs.Where(x => x.SubjectId == subject.Id).ToList<Mcq>();
            foreach(Mcq i in mcq)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
               
            }
            
            List<Chapter> chapters = db.Chapters.Where(x => x.SubjectId == subject.Id).ToList<Chapter>();
            foreach(Chapter i in chapters)
            {
                db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
            }
            db.Entry(subject).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Alerts.deleteSubject = true;
            return RedirectToAction("addSubject","Admin");
            
        }

        public ActionResult viewMessagaes()
        {
            DBEnt db = new DBEnt();
            List<Message> messages = db.Messages.ToList<Message>();
            List<MessageViewModel> msg = new List<MessageViewModel>();
            foreach(Message i in messages)
            {
                MessageViewModel obj = new MessageViewModel()
                {
                    Id = i.Id,
                    Subject = i.Subject,
                    Message = i.Message1,
                    Time = Convert.ToDateTime(i.Date)
                };

                msg.Add(obj);

            }


            return View(msg);
        }


        public ActionResult deleteMessage(int? id)
        {
            DBEnt db = new DBEnt();
            var message = db.Messages.Where(x => x.Id == id).First();
            db.Entry(message).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Alerts.deleteMessage = true;
            return RedirectToAction("viewMessagaes","Admin");
        }

        public ActionResult approveMcq(int? id)
        {
            DBEnt db = new DBEnt();
            var mcq = db.Mcqs.Where(x => x.Id == id).First();
            mcq.Status = "approve";
            db.SaveChanges();
            Alerts.approve = true;
            return RedirectToAction("pendingMcq", "Admin");

        }

            public ActionResult disapproveMcq(int? id)
        {
            DBEnt db = new DBEnt();
            var mcq = db.Mcqs.Where(x => x.Id == id).First();
            mcq.Status = "pending";
            db.SaveChanges();
            Alerts.disapprove = true;
            return RedirectToAction("allMcq", "Admin");

        }




        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            return Content("hererererer");
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }


        

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public string getExamType(long? id)
        {
            DBEnt db = new DBEnt();
            var obj = db.Exams.Where(x => x.Id == id).First();
            return obj.Name;
        }

        /* public string getRole(int? id)
         {=
             try
             {
                 DBEnt db = new DBEnt();
                 var user = db.UserRoles.Where(x => x.Id == id).First();
                 return user.Name;
             }
             catch
             {
                 return "Invalid";
             }

         }*/

        public bool examExist(string exam)
        {
            DBEnt db = new DBEnt();
            var num = db.Exams.Where(x => x.Name == exam).Count();
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool subjectExist(string subject)
        {
            DBEnt db = new DBEnt();
            var num = db.Subjects.Where(x => x.Name == subject).Count();
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string mcqSubject(long id)
        {
            DBEnt db = new DBEnt();
            var mcq = db.Mcqs.Where(x => x.Id == id).First();
            var subject = db.Subjects.Where(x => x.Id == mcq.SubjectId).First();
            return subject.Name;

        }

        public string mcqChapter(long id)
        {
            DBEnt db = new DBEnt();
            var mcq = db.Mcqs.Where(x => x.Id == id).First();
            var chap = db.Chapters.Where(x => x.Id == mcq.ChapterId).First();
            return chap.ChapterNo.ToString();

        }





       



    }
}
