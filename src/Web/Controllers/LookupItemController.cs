using AutoMapper;
using i2fam.DAL;
using i2fam.DAL.Core;
using i2fam.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.Web.Controllers
{

    [Route("api/[controller]")]
    public class LookupItemController : Controller
    {
        private readonly ILogger logger;
        private IUnitOfWork _unitOfWork;

        public LookupItemController(IUnitOfWork unitOfWork, ILogger<AuthorizationController> logger)
        {
            this.logger = logger;
            this._unitOfWork = unitOfWork;
        }

        // GET: api/lookupItem/params/localeCategory?
        [HttpGet("params/{localeCategory}")]
        [Produces(typeof(List<LookupItemViewModel>))]
        public async Task<IActionResult> GetLookupItemsByCategory(LocaleCategory localeCategory)
        {
            var allLookupItems = await _unitOfWork.LookupItems.GetLookupItemByCategoryAsync(localeCategory);
            return this.Ok(value: Mapper.Map<IEnumerable<LookupItemViewModel>>(allLookupItems));
        }
    }

}
