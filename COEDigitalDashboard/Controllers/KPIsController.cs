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
using System.Web.Http.Results;
using System.Web.Mvc;
using COEDigitalDashboard.Models;

namespace COEDigitalDashboard.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KPIsController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/KPIs
        public JsonResult GetKPIs()
        {
            var x = from n in db.KPIs
                    join c in db.KPICategories
                    on n.FK_KPICategory equals c.ID
                    join g in db.Units on n.FK_Unit equals g.ID
                    where n.IsDeleted == false
                    select new { n.ID, n.IsDeleted,n.FK_KPICategory,n.FK_Unit,n.Formula,n.KPIDescription,n.KPIName,n.ModifiedBy,n.ModifiedDate,n.Negative,n.Unit,n.KPICategory };//id=f.ID,f.FiscalYearName,g.KPICategory 
            var x2 = x.ToList();

            return new JsonResult { Data = x2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            // return db.KPIs.Where(kpi => kpi.IsDeleted == false).Include(k => k.KPICategory).Include(s2=> s2.Unit);
        }

        // GET: api/KPIs/5
        [ResponseType(typeof(KPI))]
        public JsonResult GetKPI(int id)
        {
            var x = from n in db.KPIs
                    join c in db.KPICategories
                    on n.FK_KPICategory equals c.ID
                    join g in db.Units on n.FK_Unit equals g.ID
                    where n.IsDeleted == false where n.ID==id
                    select new { n.ID, n.IsDeleted, n.FK_KPICategory, n.FK_Unit, n.Formula, n.KPIDescription, n.KPIName, n.ModifiedBy, n.ModifiedDate, n.Negative, n.Unit, n.KPICategory };//id=f.ID,f.FiscalYearName,g.KPICategory 
            var x2 = x.ToList();

            return new JsonResult { Data = x2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        // PUT: api/KPIs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutKPI(int id, KPI kPI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kPI.ID)
            {
                return BadRequest();
            }



            

            try
            {
                kPI.FK_KPICategory = kPI.KPICategory.ID;
                kPI.KPICategory = null;

                kPI.FK_Unit = kPI.Unit.ID;
                kPI.Unit = null;


                db.Entry(kPI).State = EntityState.Modified;
                await db.SaveChangesAsync();

                kPI.KPICategory = db.KPICategories.SingleOrDefault(c => c.ID == kPI.FK_KPICategory);
                kPI.Unit = db.Units.SingleOrDefault(c => c.ID == kPI.FK_Unit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KPIExists(id))
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

        // POST: api/KPIs
        [ResponseType(typeof(KPI))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> PostKPI([FromBody]KPI kPI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
               
                kPI.FK_KPICategory = kPI.KPICategory.ID;
                kPI.KPICategory = null;

                kPI.FK_Unit = kPI.Unit.ID;
                kPI.Unit = null;


                db.KPIs.Add(kPI);
                await db.SaveChangesAsync();

                kPI.KPICategory = db.KPICategories.SingleOrDefault(c => c.ID == kPI.FK_KPICategory);
                kPI.Unit = db.Units.SingleOrDefault(c => c.ID == kPI.FK_Unit);

                return CreatedAtRoute("DefaultApi", new { id = kPI.ID }, kPI);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
            

            
        }

        // DELETE: api/KPIs/5
        [ResponseType(typeof(KPI))]
        public async Task<IHttpActionResult> DeleteKPI(int id)
        {
            KPI kPI = await db.KPIs.FindAsync(id);
            if (kPI == null)
            {
                return NotFound();
            }
            if (KPIExistsID(id))
            {
                return BadRequest();

            }
            kPI.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(kPI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KPIExists(int id)
        {
            return db.KPIs.Count(e => e.ID == id) > 0;
        }

        private bool KPIExistsID(int id)
        {
            return db.CostCenter_ServiceLine_KPIs.Count(e => e.FK_KPI == id) > 0;
        }
    }
}