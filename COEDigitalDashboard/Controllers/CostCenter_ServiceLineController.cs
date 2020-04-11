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
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class CostCenter_ServiceLineController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/CostCenter_ServiceLine
        public IQueryable<CostCenter_ServiceLine> GetCostCenter_ServiceLine()
        {
            return db.CostCenter_ServiceLine.Include(s => s.ServiceLine).Include(c => c.CostCenter).Where(cc => cc.IsDeleted == false);
           // return db.CostCenter_ServiceLine;
           // return db.CostCenter_ServiceLine.Include(c => c.CostCenter);
        }

        // GET: api/CostCenter_ServiceLine/5
        [ResponseType(typeof(CostCenter_ServiceLine))]
        public async Task<IHttpActionResult> GetCostCenter_ServiceLine(int id)
        {
            CostCenter_ServiceLine costCenter_ServiceLine = await db.CostCenter_ServiceLine.FindAsync(id);
            if (costCenter_ServiceLine == null)
            {
                return NotFound();
            }

            return Ok(costCenter_ServiceLine);
        }

        // PUT: api/CostCenter_ServiceLine/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCostCenter_ServiceLine(int id, CostCenter_ServiceLine costCenter_ServiceLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != costCenter_ServiceLine.ID)
            {
                return BadRequest();
            }

            db.Entry(costCenter_ServiceLine).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCenter_ServiceLineExists(id))
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

        // POST: api/CostCenter_ServiceLine
        [ResponseType(typeof(CostCenter_ServiceLine))]
        public async Task<IHttpActionResult> PostCostCenter_ServiceLine(CostCenter_ServiceLine costCenter_ServiceLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CostCenter_ServiceLine.Add(costCenter_ServiceLine);
            await db.SaveChangesAsync();

            costCenter_ServiceLine.ServiceLine = db.ServiceLines.SingleOrDefault(s => s.ID == costCenter_ServiceLine.FK_ServiceLine);
            costCenter_ServiceLine.CostCenter = db.CostCenters.SingleOrDefault(s => s.ID == costCenter_ServiceLine.FK_CostCenter);

            return CreatedAtRoute("DefaultApi", new { id = costCenter_ServiceLine.ID }, costCenter_ServiceLine);
        }

        // DELETE: api/CostCenter_ServiceLine/5
        [ResponseType(typeof(CostCenter_ServiceLine))]
        public async Task<IHttpActionResult> DeleteCostCenter_ServiceLine(int id)
        {
            CostCenter_ServiceLine costCenter_ServiceLine = await db.CostCenter_ServiceLine.FindAsync(id);
            if (costCenter_ServiceLine == null)
            {
                return NotFound();
            }

            costCenter_ServiceLine.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(costCenter_ServiceLine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CostCenter_ServiceLineExists(int id)
        {
            return db.CostCenter_ServiceLine.Count(e => e.ID == id) > 0;
        }
    }
}