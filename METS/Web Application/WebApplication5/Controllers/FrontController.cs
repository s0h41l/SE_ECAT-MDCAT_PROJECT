using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;


namespace WebApplication5.Controllers
{
    //Controller to make subscriber side management
    public class FrontController : Controller
    {
        // GET: Front
        public ActionResult Index()
        {
            DBEnt db= new DBEnt();
            List<Exam> exams = db.Exams.ToList();
            List<Subject> subjects = db.Subjects.ToList();
            ViewData["subjects"] = subjects;
            ViewData["exams"] = exams;

            return View();
        }

        //Open Source Submission

        public ActionResult addOpenSpurceMcq(McqViewModel collection)
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
                Status = "pending",
                EntryDate = DateTime.Now,

            };

            string userId = User.Identity.GetUserId().ToString();
            DBEnt db = new DBEnt();
            db.Mcqs.Add(mcq);
            db.Submissions.Add(new Submission() { UserID=User.Identity.GetUserId(),McqId=mcq.Id });
            db.SaveChanges();
            Alerts.openSubmit = true;
            return RedirectToAction("Index","Front");

        }
        
        [HttpPost]
        public ActionResult sendMessage(MessageViewModel collection)
        {
            DBEnt db = new DBEnt();
            Message msg = new Message() {
                Subject = collection.Subject,
                Message1 = collection.Message,
                Date = DateTime.Now,
                Status = "unread",
                UserID=User.Identity.GetUserId()
            };
            Alerts.messageSent = true;
            db.Messages.Add(msg);
            db.SaveChanges();

            return RedirectToAction("Index", "Front");
        }

        public ActionResult takeProperExam(long? id)
        {
            DBEnt db = new DBEnt();
            var exam = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            return Json(exam, JsonRequestBehavior.AllowGet);
        }


        public ActionResult takeSubjectExam(long? id)
        {
            DBEnt db = new DBEnt();
            var subject = db.Subjects.Where(x => x.Id == id).FirstOrDefault();
            return Json(subject, JsonRequestBehavior.AllowGet);
        }




























        // GET: Front/Details/5
        public ActionResult Details(int id)
        {
            return Content("here");
            return View();
        }

        // GET: Front/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Front/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
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

        // GET: Front/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Front/Edit/5
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

        // GET: Front/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Front/Delete/5
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
