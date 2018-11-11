using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            DBEntities db = new DBEntities();
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

        public ActionResult json()
        {
            return Json(new { key = "yoyoy", value = "hello" }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult subjects()
        {
            List<Subject> subjects = (new DBEntities()).Subjects.ToList<Subject>();
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
            List<Exam> exams = (new DBEntities()).Exams.ToList<Exam>();
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
            List<Chapter> chapters = (new DBEntities()).Chapters.Where(x => x.SubjectId == id).ToList<Chapter>();
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

            DBEntities db = new DBEntities();
            db.Mcqs.Add(mcq);
            db.SaveChanges();
            return Json(mcq,JsonRequestBehavior.AllowGet);
        }


        public ActionResult addExam()
        {

            return View(new ExamViewModel());
        }


        [HttpPost]
        public ActionResult addExam(ExamViewModel collection)
        {
            DBEntities db = new DBEntities();
            db.Exams.Add(new Exam() { Name = collection.Name });
            db.SaveChanges();
            return Json(new Exam() { Name = collection.Name }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult addSubject()
        {
           return View(new SubjectViewModel());
        }


        [HttpPost]
        public ActionResult addSubject(SubjectViewModel collection)
        {
            DBEntities db = new DBEntities();
            db.Subjects.Add(new Subject() { Name = collection.Name });
            db.SaveChanges();
            return Json(new Subject() { Name = collection.Name }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult addChapter()
        {


            return View(new ChapterViewModel());
        }

        [HttpPost]
        public ActionResult addChapter(ChapterViewModel collection)
        {
            
            DBEntities db = new DBEntities();
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
    }
}
