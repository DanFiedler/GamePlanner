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
        private BGGClient.BoardGameGeekClient _bggClient = new BGGClient.BoardGameGeekClient();

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
        public ActionResult ImportGames(int id, string bggUserName )
        {
            var result = _bggClient.GetGamesForUser(bggUserName);

            if( result.Success )
            {
                ViewBag.Message = $"Successfully found {result.Games.Count} game IDs for user:{bggUserName}. Starting background work to import them.";

                System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancelToken =>
                   {
                       EventModel eventModel = _api.Events.GetEvent(id);
                       var games = _bggClient.GetDetailedGameList(result.Games);

                       if (eventModel.GameOptions == null)
                           eventModel.GameOptions = new List<Game>();

                       foreach(var game in games)
                       {
                           eventModel.GameOptions.Add(game);
                       }
                       _api.Events.PutEvent(id, eventModel);
                   });
            }
            else
            {
                ViewBag.Message = $"Failed to add games for user:{bggUserName}. Error:{result.Error}";
            }

            return View();
        }
    }
}
