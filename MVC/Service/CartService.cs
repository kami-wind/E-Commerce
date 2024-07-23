using BusinessObjects.Entites;
using DataAccess.DTOs.CartItem;
using System.Net.Http;

namespace MVC.Service;

public class CartService
{
    private readonly HttpClient httpClient;

    public IHttpContextAccessor HttpContextAccessor { get; }

    public CartService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        this.httpClient = httpClient;
        HttpContextAccessor = httpContextAccessor;
    }

    private void AddAuthorizationHeader()
    {
        //var token = HttpContextAccessor.HttpContext.User.FindFirst("jwtToken")?.Value;
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

    public async Task<CartItemDTO> AddToCartAsync(AddCartItemDTO cartItem)
    {
        var response = await httpClient.PostAsJsonAsync("odata/Carts/add", cartItem);
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default;
            }
            return await response.Content.ReadFromJsonAsync<CartItemDTO>();
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Http status: {response.StatusCode} Message - {message}");
        }

    }

    public async Task<CartItemDTO> RemoveCartItemAsync(AddCartItemDTO cartItem)
    {
        var response = await httpClient.PostAsJsonAsync("odata/Carts/add", cartItem);
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default;
            }
            return await response.Content.ReadFromJsonAsync<CartItemDTO>();
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Http status: {response.StatusCode} Message - {message}");
        }

    }

    public async Task RemoveCartItemAsync(int cartItemId)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PostAsync($"odata/Carts/remove/{cartItemId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task ClearAllCartItems()
    {
        AddAuthorizationHeader();
        var response = await httpClient.PostAsync("odata/Carts/clearAll", null);
        response.EnsureSuccessStatusCode();
    }
}
