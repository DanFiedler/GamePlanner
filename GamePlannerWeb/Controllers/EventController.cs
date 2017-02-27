using GamePlannerWeb.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamePlannerWeb.Controllers
{
    public class EventController : Controller
    {
        private GamePlannerWebClient _api = new GamePlannerWebClient(
            new TokenCredentials("foo")
            );

        // GET: Event
        public ActionResult Index()
        {
            return View(_api.Events.GetEvents());
        }

        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            return View(_api.Events.GetEvent(id));
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(EventModel evt)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    _api.Events.PostEvent(evt);
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                ViewBag.Message = $"Error:{e.Message}";
                return View();
            }

            return View();
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
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

        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5
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
