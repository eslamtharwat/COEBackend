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
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class CostCenter_ServiceLine_KPIsController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/CostCenter_ServiceLine_KPIs
        // public IQueryable<CostCenter_ServiceLine_KPIs> GetCostCenter_ServiceLine_KPIs()
        public JsonResult GetCostCenter_ServiceLine_KPIs()
        {
            //var test = db.CostCenter_ServiceLine_KPIs.Where(cc => cc.IsDeleted == false).Include(s1=>s1.FiscalYear).Join();
            //var x = db.CostCenter_ServiceLine_KPIs.SqlQuery(@"SELECT TOP (1000) [ID]
     // ,[FK_CostCenter]
  //    ,[FK_ServiceLine]
  //    ,[CostCenterServiceLineDesc]
  //    ,[TotalFTEs]
  //    ,[ReportingFTEs]
  //    ,[IsDeleted]
  //    ,[ModifiedDate]
  //    ,[ModifiedBy]
  //FROM[Digital_COE].[dbo].[CostCenter_ServiceLine]");
  var x=from n in db.CostCenter_ServiceLine_KPIs
        join c in db.CostCenter_ServiceLine
        on n.FK_CostCenterServiceline equals c.ID
        join f in db.FiscalYears on n.FK_FiscalYear equals f.ID
       join g in db.KPIs on n.FK_KPI equals g.ID
        where n.IsDeleted==false
        select new { n.ID,n.IsDeleted,n.FK_CostCenterServiceline,n.FK_FiscalYear,n.FK_KPI,n.KPIDate,n.KPIReportingFTEs,n.ModifiedBy,n.ModifiedDate,n.Target,n.Value,n.CostCenter_ServiceLine,n.FiscalYear,n.KPI };//id=f.ID,f.FiscalYearName,g.KPICategory 
            var x2 = x.ToList();

            return  new JsonResult { Data = x2, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; 

            //return test;
            //  return db.CostCenter_ServiceLine_KPIs.Include(s => s.CostCenter_ServiceLine).Include(c=> c.KPI).Include(ss=> ss.FiscalYear).Where(cc => cc.IsDeleted == false);
        }

        // GET: api/CostCenter_ServiceLine_KPIs/5
        [ResponseType(typeof(CostCenter_ServiceLine_KPIs))]
        public  JsonResult GetCostCenter_ServiceLine_KPIs(int id)
        {
            var x = from n in db.CostCenter_ServiceLine_KPIs
                    join c in db.CostCenter_ServiceLine
                    on n.FK_CostCenterServiceline equals c.ID
                    join f in db.FiscalYears on n.FK_FiscalYear equals f.ID
                    join g in db.KPIs on n.FK_KPI equals g.ID
                    where n.IsDeleted == false where n.ID==id
                    select new { n.ID, n.IsDeleted, n.FK_CostCenterServiceline, n.FK_FiscalYear, n.FK_KPI, n.KPIDate, n.KPIReportingFTEs, n.ModifiedBy, n.ModifiedDate, n.Target, n.Value, n.CostCenter_ServiceLine, n.FiscalYear, n.KPI };//id=f.ID,f.FiscalYearName,g.KPICategory 
            var x2 = x.ToList();

            return new JsonResult { Data = x2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        // PUT: api/CostCenter_ServiceLine_KPIs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCostCenter_ServiceLine_KPIs(int id, CostCenter_ServiceLine_KPIs costCenter_ServiceLine_KPIs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(costCenter_ServiceLine_KPIs).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCenter_ServiceLine_KPIsExists(id))
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

        // POST: api/CostCenter_ServiceLine_KPIs
        [ResponseType(typeof(CostCenter_ServiceLine_KPIs))]
        public async Task<IHttpActionResult> PostCostCenter_ServiceLine_KPIs(CostCenter_ServiceLine_KPIs costCenter_ServiceLine_KPIs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CostCenter_ServiceLine_KPIs.Add(costCenter_ServiceLine_KPIs);
            await db.SaveChangesAsync();

            costCenter_ServiceLine_KPIs.CostCenter_ServiceLine = db.CostCenter_ServiceLine.SingleOrDefault(c => c.ID == costCenter_ServiceLine_KPIs.FK_CostCenterServiceline);
            costCenter_ServiceLine_KPIs.KPI = db.KPIs.SingleOrDefault(k => k.ID == costCenter_ServiceLine_KPIs.FK_KPI);
            costCenter_ServiceLine_KPIs.FiscalYear = db.FiscalYears.SingleOrDefault(f => f.ID == costCenter_ServiceLine_KPIs.FK_FiscalYear);

            return CreatedAtRoute("DefaultApi", new { id = costCenter_ServiceLine_KPIs.ID }, costCenter_ServiceLine_KPIs);
        }

        // DELETE: api/CostCenter_ServiceLine_KPIs/5
        [ResponseType(typeof(CostCenter_ServiceLine_KPIs))]
        public async Task<IHttpActionResult> DeleteCostCenter_ServiceLine_KPIs(int id)
        {
            CostCenter_ServiceLine_KPIs costCenter_ServiceLine_KPIs = await db.CostCenter_ServiceLine_KPIs.FindAsync(id);
            if (costCenter_ServiceLine_KPIs == null)
            {
                return NotFound();
            }

            costCenter_ServiceLine_KPIs.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(costCenter_ServiceLine_KPIs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CostCenter_ServiceLine_KPIsExists(int id)
        {
            return db.CostCenter_ServiceLine_KPIs.Count(e => e.ID == id) > 0;
        }
    }
}