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
    public class KPICategoriesController : ApiController
    {
        private Digital_COEEntities db = new Digital_COEEntities();

        // GET: api/KPICategories
        public IQueryable<KPICategory> GetKPICategories()
        {
            var test = db.KPICategories.Where(cat => cat.IsDeleted == false);
            return test;
        }

        // GET: api/KPICategories/5
        [ResponseType(typeof(KPICategory))]
        public async Task<IHttpActionResult> GetKPICategory(int id)
        {
            KPICategory kPICategory = await db.KPICategories.FindAsync(id);
            if (kPICategory == null)
            {
                return NotFound();
            }

            return Ok(kPICategory);
        }

        // PUT: api/KPICategories/5
        
        [HttpPut]
        public async Task<IHttpActionResult> PutKPICategory(int id,[FromBody] KPICategory kPICategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kPICategory.ID)
            {
                return BadRequest();
            }
            if (KPICategoryExistsName(kPICategory.KPICategoryName))
            {
                return BadRequest();

            }

            db.Entry(kPICategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KPICategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/KPICategories
        [ResponseType(typeof(KPICategory))]
        [HttpPost]
        public async Task<IHttpActionResult> PostKPICategory(KPICategory kPICategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (KPICategoryExistsName(kPICategory.KPICategoryName))
            {
                return BadRequest();

            }
            db.KPICategories.Add(kPICategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = kPICategory.ID }, kPICategory);
        }

        // DELETE: api/KPICategories/5
        //[ResponseType(typeof(KPICategory))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteKPICategory(int id)
        {
            //get the kpi category in the DB.
            KPICategory kPICategoryInDB = await db.KPICategories.FindAsync(id);
            if (kPICategoryInDB == null)
            {
                return NotFound();
            }

            if (KPICategoryExistsID(id))
            {
                return BadRequest();

            }

            kPICategoryInDB.IsDeleted = true;
            //db.KPICategories.Remove(kPICategory);
            await db.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KPICategoryExists(int id)
        {
            return db.KPICategories.Count(e => e.ID == id) > 0;
        }

        private bool KPICategoryExistsName(string Name)
        {
            return db.KPICategories.Count(e => e.KPICategoryName == Name) > 0;
        }

        private bool KPICategoryExistsID(int id)
        {

            return db.KPIs.Count(e => e.FK_KPICategory == id) > 0;
        }


    }
}