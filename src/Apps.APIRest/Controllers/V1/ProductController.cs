using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business.Interfaces;
using Apps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class ProductController : MainController
    {
        private readonly IProductService _appsToSellService;

        public ProductController(INotes notes, IUser userApp, IProductService appsToSellService) : base(notes, userApp) 
        {
            _appsToSellService = appsToSellService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return CustomResponse((await _appsToSellService.FindAll()).Select(t => new AppsToSellModels(t)));
        }
    }
}
