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
using System.Web.Mvc;
using COEDigitalDashboard.Models;

namespace COEDigitalDashboard.Controllers
{
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class CostCenterKPIsTargetsController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/CostCenterKPIsTargets
        public JsonResult GetCostCenterKPIsTargets()
        {
            // return db.CostCenterKPIsTargets.Include(s => s.CostCenter_ServiceLine).Include(ss=>ss.FiscalYear).Include(d=>d.KPI);

            // return db.CostCenterKPIsTargets;

            var x = from n in db.CostCenterKPIsTargets
                    join c in db.CostCenter_ServiceLine
                    on n.FK_CostCenterServiceLine equals c.ID
                    join f in db.FiscalYears on n.FK_FiscalYear equals f.ID
                    where n.IsDeleted == false
                    select new { n.ID, n.IsDeleted, n.FK_CostCenterServiceLine, n.FK_FiscalYear, n.FK_KPI, n.KPIReportingFTEs, n.ModifiedBy, n.ModifiedDate, n.Target, n.CostCenter_ServiceLine, n.FiscalYear, n.KPI };//id=f.ID,f.FiscalYearName,g.KPICategory 
            var x2 = x.ToList();

            return new JsonResult { Data = x2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        // GET: api/CostCenterKPIsTargets/5
        [ResponseType(typeof(CostCenterKPIsTarget))]
        public  JsonResult GetCostCenterKPIsTarget(int id)
        {
            
            var x = from n in db.CostCenterKPIsTargets
                    join c in db.CostCenter_ServiceLine
                    on n.FK_CostCenterServiceLine equals c.ID
                    join f in db.FiscalYears on n.FK_FiscalYear equals f.ID
                    join g in db.KPIs on n.FK_KPI equals g.ID
                    where n.IsDeleted == false where n.ID==id
                    select new { n.ID, n.IsDeleted, n.FK_CostCenterServiceLine, n.FK_FiscalYear, n.FK_KPI, n.KPIReportingFTEs, n.ModifiedBy, n.ModifiedDate, n.Target, n.CostCenter_ServiceLine, n.FiscalYear, n.KPI };//id=f.ID,f.FiscalYearName,g.KPICategory 
            var x2 = x.ToList();

            return new JsonResult { Data = x2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


           
        }

        // PUT: api/CostCenterKPIsTargets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCostCenterKPIsTarget(int id, CostCenterKPIsTarget costCenterKPIsTarget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(costCenterKPIsTarget).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCenterKPIsTargetExists(id))
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

        // POST: api/CostCenterKPIsTargets
        [ResponseType(typeof(CostCenterKPIsTarget))]
        public async Task<IHttpActionResult> PostCostCenterKPIsTarget(CostCenterKPIsTarget costCenterKPIsTarget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CostCenterKPIsTargets.Add(costCenterKPIsTarget);
            await db.SaveChangesAsync();

            costCenterKPIsTarget.FiscalYear = db.FiscalYears.SingleOrDefault(f => f.ID == costCenterKPIsTarget.FK_FiscalYear);
            costCenterKPIsTarget.KPI = db.KPIs.SingleOrDefault(k => k.ID == costCenterKPIsTarget.FK_KPI);
            costCenterKPIsTarget.CostCenter_ServiceLine = db.CostCenter_ServiceLine.SingleOrDefault(c => c.ID == costCenterKPIsTarget.FK_CostCenterServiceLine);

            return CreatedAtRoute("DefaultApi", new { id = costCenterKPIsTarget.ID }, costCenterKPIsTarget);
        }

        // DELETE: api/CostCenterKPIsTargets/5
        [ResponseType(typeof(CostCenterKPIsTarget))]
        public async Task<IHttpActionResult> DeleteCostCenterKPIsTarget(int id)
        {
            CostCenterKPIsTarget costCenterKPIsTarget = await db.CostCenterKPIsTargets.FindAsync(id);
            if (costCenterKPIsTarget == null)
            {
                return NotFound();
            }

            costCenterKPIsTarget.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(costCenterKPIsTarget);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CostCenterKPIsTargetExists(int id)
        {
            return db.CostCenterKPIsTargets.Count(e => e.ID == id) > 0;
        }
    }
}