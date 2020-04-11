using COEDigitalDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace COEDigitalDashboard.Controllers
{
    [EnableCors(origins:"*", headers: "*", methods: "*")]
    public class ExcelController : Controller
    {
        // GET: Excel
        [Route("Excel/ExportMonthlyKpisIndex")]
        [HttpPost]
        public ActionResult ExportMonthlyKpisIndex(List<CostCenter_ServiceLine_KPIs> data)
        {
            return View();
        }

        public FileResult ExportMonthlyKpis()
        {
            byte[] arr = null;
            return File(arr, " ", " ");
        }

        private void exportDataToExcel()
        {

        }
    }
}