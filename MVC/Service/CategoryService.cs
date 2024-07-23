using DataAccess.DTOs.Category;
using DataAccess.DTOs.Product;

namespace MVC.Service;

public class CategoryService
{
    private readonly HttpClient httpClient;

    public IHttpContextAccessor HttpContextAccessor { get; }

    public CategoryService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

    public async Task<IEnumerable<GETCategory>> GetItems()
    {
        
        var response = await httpClient.GetAsync("odata/Categories");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<GETCategory>>();
    }

    public async Task<GETCategory> GetItem(int id)
    {
        var response = await httpClient.GetAsync($"odata/Categories/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GETCategory>();
    }

    public async Task CreateCategory(POST_PUT_Category category)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PostAsJsonAsync("odata/Categories", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategory(int id, POST_PUT_Category category)
    {
        AddAuthorizationHeader();
        var response = await httpClient.PutAsJsonAsync($"odata/Categories/{id}", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategory(int id)
    {
        AddAuthorizationHeader();
        var response = await httpClient.DeleteAsync($"odata/Categories/{id}");
        response.EnsureSuccessStatusCode();
    }
}
