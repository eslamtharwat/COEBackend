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
using System.Web.Http.Cors;
using System.Web.Http.Description;
using COEDigitalDashboard.Models;

namespace COEDigitalDashboard.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LeadsController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/Leads
        public IEnumerable<Lead> GetLeads()
        {
            var t = db.Configuration;
            //return new List<Lead>() { new Lead() };
            return db.Leads.Where(lead => lead.IsDeleted == false);
        }


        // GET: api/Leads/5
        [ResponseType(typeof(Lead))]
        public async Task<IHttpActionResult> GetLead(int id)
        {
            Lead lead = await db.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            return Ok(lead);
        }

        // PUT: api/Leads/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLead(int id, Lead lead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lead.ID)
            {
                return BadRequest();
            }

            if (LeadExistsName(lead.LeadName))
            {
                return BadRequest();
            }
            db.Entry(lead).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeadExists(id))
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

        // POST: api/Leads
        [ResponseType(typeof(Lead))]
        public async Task<IHttpActionResult> PostLead(Lead lead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (LeadExistsName(lead.LeadName))
            {
                return BadRequest();
            }
            db.Leads.Add(lead);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = lead.ID }, lead);
        }


        // DELETE: api/Leads/5
        [ResponseType(typeof(Lead))]
        public async Task<IHttpActionResult> DeleteLead(int id)
        {
            Lead lead = await db.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }
            if (LeadExistsID(id))
            {

                return BadRequest();
            }

            lead.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(lead);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeadExists(int id)
        {
            return db.Leads.Count(e => e.ID == id) > 0;
        }
        private bool LeadExistsName(string name)
        {
            return db.Leads.Count(e => e.LeadName == name) > 0;
        }

        private bool LeadExistsID(int id)
        {
            return db.ServiceLines.Count(e => e.Fk_Lead == id) > 0;
        }
    }
}