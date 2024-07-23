using DataAccess.DTOs.Cart;
using DataAccess.DTOs.CartItem;
using DataAccess.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MVC.Service;

public class ProductService
{
    private readonly HttpClient httpClient;

    public IHttpContextAccessor HttpContextAccessor { get; }

    public ProductService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        this.httpClient = httpClient;
        HttpContextAccessor = httpContextAccessor;
    }

    private void AddAuthorizationHeader()
    {
        var token = HttpContextAccessor.HttpContext.Request.Cookies["jwtToken"];
        if (token != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine("Token: " + token);
        }
        else
        {
            //Console.WriteLine("Token not found in claims.");
        }
    }

    public async Task<IEnumerable<GETProduct>> GetItems()
    {
        var response = await httpClient.GetAsync("odata/Products");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<GETProduct>>();
        //var response = await httpClient.GetFromJsonAsync<IEnumerable<GETProduct>>("odata/Products");
        //return response;
    }

    public async Task<GETProduct> GetItem(int id)
    {
        var response = await httpClient.GetAsync($"odata/Products/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GETProduct>();
    }

    public async Task CreateProduct(POST_PUT_Product product)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PostAsJsonAsync("odata/Products", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateProduct(int id, POST_PUT_Product product)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PutAsJsonAsync($"odata/Products/{id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProduct(int id)
    {
        AddAuthorizationHeader();
        var response = await httpClient.DeleteAsync($"odata/Products/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AddToCart(AddCartItemDTO addCartItemDTO)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PostAsJsonAsync("odata/Carts/add", addCartItemDTO);
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Unauthorized access - token may be expired.");
        }

        response.EnsureSuccessStatusCode();
    }

    public async Task<CartDTO> GetCart()
    {
        AddAuthorizationHeader();
        var response = await httpClient.GetAsync("odata/Carts");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CartDTO>();
    }

    public async Task ClearCart(int cartId)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PostAsJsonAsync("odata/Carts/clear", cartId);
        response.EnsureSuccessStatusCode();
    }

}
