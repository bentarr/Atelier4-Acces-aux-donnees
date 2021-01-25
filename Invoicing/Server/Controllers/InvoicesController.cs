using Invoicing.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoicing.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IBusinessData _data;

        public InvoicesController(ILogger<InvoicesController> logger, IBusinessData data)
        {
            _logger = logger;
            _data = data;
        }

        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            return _data.AllInvoices;
        }

        [HttpGet("{reference}")]
        public ActionResult<Invoice> Get(string reference)
        {
            var invoice = _data.AllInvoices.Where(inv => inv.Reference == reference).FirstOrDefault();

            if(invoice != null)
            {
                return invoice;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public void Post()
        {
            

            _data.EnvoyerCA();

            Response.Redirect("/");
        }
    }
}
