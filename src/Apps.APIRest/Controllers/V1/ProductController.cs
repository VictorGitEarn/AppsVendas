using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business.Interfaces;
using Apps.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    public class ProductController : MainController
    {
        private readonly IProductService _productService;

        public ProductController(INotes notes, IUser userApp, IProductService productService) : base(notes, userApp) 
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return CustomResponse((await _productService.FindAll()).Select(t => new ProductModels(t)));
        }
    }
}
