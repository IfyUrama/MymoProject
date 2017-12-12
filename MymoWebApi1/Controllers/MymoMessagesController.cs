using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MymoWebApi1.Models;

namespace MymoWebApi1.Controllers
{
    public class MymoMessagesController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/MymoMessages
        public IQueryable<MymoMessage> GetMymoMessage()
        {
            return db.MymoMessage;
        }

        

    // GET: api/MymoMessages/5
    [ResponseType(typeof(MymoMessage))]
        public async Task<IHttpActionResult> GetMymoMessage(int id)
        {
            MymoMessage mymoMessage = await db.MymoMessage.FindAsync(id);
            if (mymoMessage == null)
            {
                return NotFound();
            }

            return Ok(mymoMessage);
        }

        // PUT: api/MymoMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMymoMessage(int id, MymoMessage mymoMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mymoMessage.MessageID)
            {
                return BadRequest();
            }

            db.Entry(mymoMessage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MymoMessageExists(id))
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

        // POST: api/MymoMessages
        [ResponseType(typeof(MymoMessage))]
        public async Task<IHttpActionResult> PostMymoMessage(MymoMessage mymoMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MymoMessage.Add(mymoMessage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mymoMessage.MessageID }, mymoMessage);
        }

        // DELETE: api/MymoMessages/5
        [ResponseType(typeof(MymoMessage))]
        public async Task<IHttpActionResult> DeleteMymoMessage(int id)
        {
            MymoMessage mymoMessage = await db.MymoMessage.FindAsync(id);
            if (mymoMessage == null)
            {
                return NotFound();
            }

            db.MymoMessage.Remove(mymoMessage);
            await db.SaveChangesAsync();

            return Ok(mymoMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MymoMessageExists(int id)
        {
            return db.MymoMessage.Count(e => e.MessageID == id) > 0;
        }
    }
}