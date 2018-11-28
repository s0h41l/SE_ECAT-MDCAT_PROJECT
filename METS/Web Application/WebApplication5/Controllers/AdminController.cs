﻿using System;
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
                    Id=i.Id,
                    Question=i.Question,
                    OptionA=i.OptionA,
                    OptionB=i.OptionB,
                    OptionC=i.OptionC,
                    OptionD=i.OptionD,
                    CorrectOption=i.CorrectOption,
                    ExamId=Convert.ToInt16(i.ExamId),
                    ChapterId=Convert.ToInt16(i.ChapterId),
                    Status=i.Status,
                    Upvotes=Convert.ToInt16(i.UpVotes),
                    Downvotes=Convert.ToInt16(i.DownVotes),
                    EntryDate=Convert.ToDateTime(i.EntryDate)

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

            return Json(sub,JsonRequestBehavior.AllowGet);

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
            return Json(exm,JsonRequestBehavior.AllowGet);
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

            return Json(chaps,JsonRequestBehavior.AllowGet);
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
            db.Mcqs.Add(mcq);
            db.SaveChanges();
            return Json(mcq,JsonRequestBehavior.AllowGet);
        }


        public ActionResult addExam()
        {
            DBEnt db = new DBEnt();
            var exams = db.Exams.ToList<Exam>();
            ViewData["exams"] = exams;

            return View(new ExamViewModel());
        }


        [HttpPost]
        public ActionResult addExam(ExamViewModel collection)
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

            db.Exams.Add(new Exam() { Name = collection.Name,Cover=collection.Cover });
            db.SaveChanges();
            return Json(new Exam() { Name = collection.Name }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteExam(int? id)
        {
            DBEnt db = new DBEnt();
            var exam = db.Exams.Where(x => x.Id == id).First();
            db.Entry(exam).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("AddExam", "Admin");
        }



        public ActionResult addSubject()
        {
            DBEnt db = new DBEnt();
            List<Subject> subjects = db.Subjects.ToList<Subject>();
            ViewData["subjects"] = subjects;
           return View(new SubjectViewModel());
        }


        [HttpPost]
        public ActionResult addSubject(SubjectViewModel collection)
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

            db.Subjects.Add(new Subject() { Name = collection.Name });
            db.SaveChanges();
            return Json(new Subject() { Name = collection.Name }, JsonRequestBehavior.AllowGet);
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
            return Json(chapter, JsonRequestBehavior.AllowGet);
            
        }



        public ActionResult allMcq()
        {
            List<Mcq> mcqs = (new DBEnt()).Mcqs.ToList<Mcq>();
            List<McqViewModel> mcqview = new List<McqViewModel>();
            foreach (Mcq i in mcqs)
            {
                McqViewModel obj = new McqViewModel()
                {
                    Id=i.Id,
                    Question=i.Question,
                    OptionA=i.OptionA,
                    OptionB=i.OptionB,
                    OptionC=i.OptionC,
                    OptionD=i.OptionD,
                    CorrectOption=i.CorrectOption,
                    Exam=getExamType(i.ExamId),
                    ChapterId=Convert.ToInt32(i.ChapterId),
                    SubjectId=Convert.ToInt32(i.SubjectId),
                    Upvotes=Convert.ToInt32(i.UpVotes),
                    Downvotes=Convert.ToInt32(i.DownVotes),
                    Status=i.Status
                   
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
                    return RedirectToAction("allUsers", "Admin");
                }
            }
            catch
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("allUsers", "Admin");
            }

            return Content("WTH");
            
        }


        


        public ActionResult deleteMcq(int id)
        {
            DBEnt db = new DBEnt();
            Mcq mcq = db.Mcqs.Where(x => x.Id == id).First();
            db.Entry(mcq).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Content(mcq.Question.ToString());
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
            return RedirectToAction("viewMessagaes","Admin");
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
        {
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
    }
}
