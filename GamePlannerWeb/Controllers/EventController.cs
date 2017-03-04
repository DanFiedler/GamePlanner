using GamePlannerWeb.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GamePlannerWeb.Controllers
{
    public class EventController : Controller
    {
        private IGamePlannerWebClient _api = Util.ApiUtil.GetClient();

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

        public ActionResult ImportGames(int id)
        {
            return View( new EventModel() { ID = id });            
        }

        [HttpPost]
        public void ImportGames(int id, string bggUserName )
        {
            EventModel eventModel = _api.Events.GetEvent(id);

            var bggClient = new BGGClient.BoardGameGeekClient();
            var result = bggClient.GetGamesForUser(bggUserName);

            if( result.Success )
            {
                var sb = new StringBuilder();
                sb.Append($"<p>Successfully added {result.Games.Count} games.</p>");
                sb.Append("<ul>");
                foreach (var game in result.Games)
                    sb.Append($"<li>{game.Name}</li>");
                sb.Append("</ul>");

                ViewBag.Message = sb.ToString();

                if (eventModel.GameOptions == null)
                    eventModel.GameOptions = new List<Game>();

                foreach(var g in result.Games)
                    eventModel.GameOptions.Add(g);

                _api.Events.PutEvent(id, eventModel);
            }
            else
            {
                ViewBag.Message = $"Failed to add games for user:{bggUserName}. Error:{result.Error}";
            }
        }
    }
}
