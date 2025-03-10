﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using Mango.Web.Services;
using Mango.Web.Models;
using Microsoft.AspNetCore.Authentication;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    
    public async Task<IActionResult> Index()
    {
        List<ProductDto> products = new ();
        var response = await _productService.GetAllProductsAsync<ResponseDto>();

        if(response != null && response.IsSuccess)
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

        return View(products);
    }

    public async Task<IActionResult> Details(int productId)
    {
        ProductDto product = new();
        var response = await _productService.GetProductByIdAsync<ResponseDto>(productId);

        if (response != null && response.IsSuccess)
            product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

        return View(product);
    }

    [HttpPost]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPost(ProductDto productDto)
    {
        CartDto CartDto = new()
        {
            CartHeader = new CartHeaderDto()
            {
                UserId =  "0"//User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailsDto cartDetails = new CartDetailsDto()
        {
            Count = productDto.Count,
            ProductId = productDto.ProductId
        };

        var response = await _productService.GetProductByIdAsync<ResponseDto>(productDto.ProductId);

        if (response != null && response.IsSuccess)
            cartDetails.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

        List<CartDetailsDto> cartDetailsDtos = new();
        cartDetailsDtos.Add(cartDetails);
        CartDto.CartDetails = cartDetailsDtos;

        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var addToCartResp = await _cartService.AddToCartAsync<ResponseDto>(CartDto, accessToken);

        if(addToCartResp != null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(productDto);
    }

    //[Authorize]
    //public IActionResult Login()
    //{
    //    return RedirectToAction(nameof(Index));
    //}
    //
    //public IActionResult Logout()
    //{
    //    return SignOut("Cookies", "iodc");
    //}


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}