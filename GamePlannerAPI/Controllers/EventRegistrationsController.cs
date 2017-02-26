using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GamePlannerAPI.Data;
using GamePlannerModel;

namespace GamePlannerAPI.Controllers
{
    public class EventRegistrationsController : ApiController
    {
        private GamePlannerContext db = new GamePlannerContext();

        // GET: api/EventRegistrations
        public IQueryable<EventRegistration> GetEventRegistrations()
        {
            return db.EventRegistrations;
        }

        // GET: api/EventRegistrations/5
        [ResponseType(typeof(EventRegistration))]
        public IHttpActionResult GetEventRegistration(int id)
        {
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return NotFound();
            }

            return Ok(eventRegistration);
        }

        // PUT: api/EventRegistrations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEventRegistration(int id, EventRegistration eventRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventRegistration.ID)
            {
                return BadRequest();
            }

            db.Entry(eventRegistration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventRegistrationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EventRegistrations
        [ResponseType(typeof(EventRegistration))]
        public IHttpActionResult PostEventRegistration(EventRegistration eventRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventRegistrations.Add(eventRegistration);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = eventRegistration.ID }, eventRegistration);
        }

        // DELETE: api/EventRegistrations/5
        [ResponseType(typeof(EventRegistration))]
        public IHttpActionResult DeleteEventRegistration(int id)
        {
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return NotFound();
            }

            db.EventRegistrations.Remove(eventRegistration);
            db.SaveChanges();

            return Ok(eventRegistration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventRegistrationExists(int id)
        {
            return db.EventRegistrations.Count(e => e.ID == id) > 0;
        }
    }
}