﻿using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDto,
            Url = SD.ShoppingCartAPIbase + "/api/cart/AddCart",
            AccessToken = token
        });

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ShoppingCartAPIbase + "/api/cart/GetCart/" + userId,
            AccessToken = token
        });      

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = SD.ShoppingCartAPIbase + "/api/cart/RemoveCart/" + cartId,
            AccessToken = token
        });

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDto,
            Url = SD.ShoppingCartAPIbase + "/api/cart/UpdateCart",
            AccessToken = token
        });

        public async Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = cartDto,
            Url = SD.ShoppingCartAPIbase + "/api/cart/ApplyCoupon",
            AccessToken = token
        });

        public async Task<T> RemoveCoupon<T>(string userId, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = userId,
            Url = SD.ShoppingCartAPIbase + "/api/cart/RemoveCoupon",
            AccessToken = token
        });

        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null) => await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = cartHeader,
            Url = SD.ShoppingCartAPIbase + "/api/cart/Checkout",
            AccessToken = token
        });
    }
}
