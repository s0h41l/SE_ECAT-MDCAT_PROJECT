using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class ExamController : Controller
    {
        public ActionResult fullExam(long? id)
        {
            DBEnt db = new DBEnt();
            var exam = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            ViewData["fullexam"] = exam;
            return View();

        }

        public ActionResult subjectExam(long? id)
        {
            DBEnt db = new DBEnt();
            var subject = db.Subjects.Where(x => x.Id == id).FirstOrDefault();
            return Json(subject, JsonRequestBehavior.AllowGet);
        }


        public ActionResult startExam(long id)
        {
            DBEnt db = new DBEnt();
            List<Mcq> mcqs = db.Mcqs.Where(x=>x.ExamId==id).Where(x=>x.Status=="approve").ToList<Mcq>();
            var exam = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            List<Mcq> selected_mcqs = new List<Mcq>();
            Random ran = new Random();
            while (selected_mcqs.Count<exam.TotalQuestions && selected_mcqs.Count !=mcqs.Count)
            {
                int index = ran.Next(0, mcqs.Count);
                if (!selected_mcqs.Contains(mcqs[index]))
                {
                    selected_mcqs.Add(mcqs[index]);

                }
                
            }

            
            List<McqViewModel> m = new List<McqViewModel>();
            foreach (Mcq i in selected_mcqs)
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
                    Subject = mcqSubject(i.Id),
                    Chapter = mcqChapter(i.Id)
                };
                m.Add(obj);
            }

            ViewData["mcqs"] = m;

            return View();
        }



     

        // GET: Exam/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: Exam/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exam/Create
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

        // GET: Exam/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Exam/Edit/5
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

        // GET: Exam/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Exam/Delete/5
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

        public ActionResult mcqApi()
        {
            DBEnt db = new DBEnt();
            var mcqs = db.Mcqs.ToList<Mcq>();
            List<McqViewModel> m = new List<McqViewModel>();
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
                    Exam=getExamType(i.ExamId),
                    Subject= mcqSubject(i.Id),
                    Chapter=mcqChapter(i.Id)
                };
                m.Add(obj);
            }

            return Json(m,JsonRequestBehavior.AllowGet);
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


        public string getExamType(long? id)
        {
            DBEnt db = new DBEnt();
            var obj = db.Exams.Where(x => x.Id == id).First();
            return obj.Name;
        }


    }
}
