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
    public class FiscalYearsController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/FiscalYears
        public IQueryable<FiscalYear> GetFiscalYears()
        {
            return db.FiscalYears;
        }

        // GET: api/FiscalYears/5
        [ResponseType(typeof(FiscalYear))]
        public async Task<IHttpActionResult> GetFiscalYear(int id)
        {
            FiscalYear fiscalYear = await db.FiscalYears.FindAsync(id);
            if (fiscalYear == null)
            {
                return NotFound();
            }

            return Ok(fiscalYear);
        }

        // PUT: api/FiscalYears/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFiscalYear(int id, FiscalYear fiscalYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fiscalYear.ID)
            {
                return BadRequest();
            }

            
            try
            {
                
                db.Entry(fiscalYear).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FiscalYearExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fiscalYear.ID }, fiscalYear);
        }

        // POST: api/FiscalYears
        [ResponseType(typeof(FiscalYear))]
        public async Task<IHttpActionResult> PostFiscalYear(FiscalYear fiscalYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (FiscalYearExistsName(fiscalYear.FiscalYearName))
            {
                return BadRequest();

            }

            db.FiscalYears.Add(fiscalYear);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = fiscalYear.ID }, fiscalYear);
        }

        // DELETE: api/FiscalYears/5
        [ResponseType(typeof(FiscalYear))]
        public async Task<IHttpActionResult> DeleteFiscalYear(int id)
        {
            FiscalYear fiscalYear = await db.FiscalYears.FindAsync(id);
            if (fiscalYear == null)
            {
                return NotFound();
            }

            if (FiscalYearExistsID(id))
            {
                return BadRequest();

            }

            fiscalYear.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(fiscalYear);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FiscalYearExists(int id)
        {
            return db.FiscalYears.Count(e => e.ID == id) > 0;
        }


        private bool FiscalYearExistsName(string Name)
        {
            return db.FiscalYears.Count(e => e.FiscalYearName == Name ) > 0;
        }

        private bool FiscalYearExistsID(int id)
        {
            return db.CostCenter_ServiceLine_KPIs.Count(e => e.FK_FiscalYear == id) > 0;
        }
    }
}