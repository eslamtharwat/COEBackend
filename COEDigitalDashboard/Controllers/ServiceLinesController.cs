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
    public class ServiceLinesController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/ServiceLines
        public IQueryable<ServiceLine> GetServiceLines()
        {
            return db.ServiceLines.Where(serviceLine => serviceLine.IsDeleted == false).Include(serviceLine => serviceLine.Lead);
        }

        // GET: api/ServiceLines/5
        [ResponseType(typeof(ServiceLine))]
        public async Task<IHttpActionResult> GetServiceLine(int id)
        {
            ServiceLine serviceLine = await db.ServiceLines.FindAsync(id);
            if (serviceLine == null)
            {
                return NotFound();
            }

            return Ok(serviceLine);
        }

        // PUT: api/ServiceLines/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServiceLine(int id, ServiceLine serviceLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != serviceLine.ID)
            {
                return BadRequest();
            }

            serviceLine.Fk_Lead = serviceLine.Lead.ID;
            serviceLine.Lead = null;

            db.Entry(serviceLine).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                serviceLine.Lead = db.Leads.SingleOrDefault(l => l.ID == serviceLine.Fk_Lead);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLineExists(id))
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

        // POST: api/ServiceLines
        [ResponseType(typeof(ServiceLine))]
        public async Task<IHttpActionResult> PostServiceLine(ServiceLine serviceLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ServiceLineExistsName(serviceLine.ServiceLineName))
            {
                return BadRequest();

            }

            serviceLine.Fk_Lead = serviceLine.Lead.ID;
            serviceLine.Lead = null;
            db.ServiceLines.Add(serviceLine);
            await db.SaveChangesAsync();
            serviceLine.Lead = db.Leads.SingleOrDefault(l => l.ID == serviceLine.Fk_Lead);
            return CreatedAtRoute("DefaultApi", new { id = serviceLine.ID }, serviceLine);
        }

        // DELETE: api/ServiceLines/5
        [ResponseType(typeof(ServiceLine))]
        public async Task<IHttpActionResult> DeleteServiceLine(int id)
        {
            ServiceLine serviceLine = await db.ServiceLines.FindAsync(id);
            if (serviceLine == null)
            {
                return NotFound();
            }
            if (ServiceLineExistsID(id))
            {

                return BadRequest();

            }
            serviceLine.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(serviceLine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceLineExists(int id)
        {
            return db.ServiceLines.Count(e => e.ID == id) > 0;
        }

        private bool ServiceLineExistsName(string Name)
        {
            return db.ServiceLines.Count(e => e.ServiceLineName == Name ) > 0;
        }

        private bool ServiceLineExistsID(int id)
        {
            return db.CostCenter_ServiceLine.Count(e => e.FK_ServiceLine == id) > 0;
        }
    }
}