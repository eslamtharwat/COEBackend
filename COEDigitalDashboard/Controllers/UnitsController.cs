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
    public class UnitsController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/Units
        public IEnumerable<Unit> GetUnits()
        {
            List <Unit> nit = db.Units.Where(unit => unit.IsDeleted == false).ToList();

            return nit;
        }

        // GET: api/Units/5
        [ResponseType(typeof(Unit))]
        public async Task<IHttpActionResult> GetUnit(int id)
        {
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            return Ok(unit);
        }

        // PUT: api/Units/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUnit(int id, Unit unit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != unit.ID)
            {
                return BadRequest();
            }

            if (UnitExistsName(unit.UnitName))
            {
                return BadRequest();
            }

            db.Entry(unit).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(id))
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

        // POST: api/Units
        [ResponseType(typeof(Unit))]
        public async Task<IHttpActionResult> PostUnit(Unit unit)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UnitExistsName(unit.UnitName))
            {
                return BadRequest();
            }

            db.Units.Add(unit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = unit.ID }, unit);
        }

        // DELETE: api/Units/5
        [ResponseType(typeof(Unit))]
        public async Task<IHttpActionResult> DeleteUnit(int id)
        {
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            if (UnitExistsID(id))
            {
                return BadRequest();


            }

            unit.IsDeleted = true;
            await db.SaveChangesAsync();

            return Ok(unit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UnitExists(int id)
        {
            return db.Units.Count(e => e.ID == id) > 0;
        }

        private bool UnitExistsName(string Name)
        {
            return db.Units.Count(e => e.UnitName == Name) > 0;
        }

        private bool UnitExistsID(int id)
        {
            return db.KPIs.Count(e => e.FK_Unit == id) > 0;
        }

    }
}