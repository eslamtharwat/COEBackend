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
    public class CostCentersController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/CostCenters
        public IQueryable<CostCenter> GetCostCenters()
        {
            return db.CostCenters.Where(costCenter => costCenter.IsDeleted == false);
        }

        // GET: api/CostCenters/5
        [ResponseType(typeof(CostCenter))]
        public async Task<IHttpActionResult> GetCostCenter(int id)
        {
            CostCenter costCenter = await db.CostCenters.FindAsync(id);
            if (costCenter == null)
            {
                return NotFound();
            }

            return Ok(costCenter);
        }

        // PUT: api/CostCenters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCostCenter(int id, CostCenter costCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != costCenter.ID)
            {
                return BadRequest();
            }


            db.Entry(costCenter).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCenterExists(id))
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

        // POST: api/CostCenters
        [ResponseType(typeof(CostCenter))]
        public async Task<IHttpActionResult> PostCostCenter(CostCenter costCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CostCenterExistsName(costCenter.CostCenterName))
            {

                return BadRequest();

            }

            db.CostCenters.Add(costCenter);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = costCenter.ID }, costCenter);
        }

        // DELETE: api/CostCenters/5
        [ResponseType(typeof(CostCenter))]
        public async Task<IHttpActionResult> DeleteCostCenter(int id)
        {
            CostCenter costCenter = await db.CostCenters.FindAsync(id);
            if (costCenter == null)
            {
                return NotFound();
            }
            if (CostCenterExistsID(id))
            {
                return BadRequest();

            }

            costCenter.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(costCenter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CostCenterExists(int id)
        {
            return db.CostCenters.Count(e => e.ID == id) > 0;
        }

        private bool CostCenterExistsName(string Name)
        {
            return db.CostCenters.Count(e => e.CostCenterName == Name) > 0;
        }

        private bool CostCenterExistsID(int id)
        {
            return db.CostCenter_ServiceLine.Count(e => e.FK_CostCenter == id) > 0;
        }
    }
}