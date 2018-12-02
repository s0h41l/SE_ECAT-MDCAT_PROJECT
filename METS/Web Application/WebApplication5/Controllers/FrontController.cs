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
            db.SaveChanges();
            return Json(mcq, JsonRequestBehavior.AllowGet);



            return null;

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
