using DataAccess.DTOs.Product;

namespace Client.Service;

public class ProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<GETProduct>> GetItems()
    {
        var response = await httpClient.GetAsync("api/Product");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<GETProduct>>();
    }

    public async Task<GETProduct> GetItem(int id)
    {
        var response = await httpClient.GetAsync($"api/Product/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GETProduct>();
    }

    public async Task CreateProduct(POST_PUT_Product product)
    {
        var response = await httpClient.PostAsJsonAsync("api/Product", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateProduct(int id, POST_PUT_Product product)
    {
        var response = await httpClient.PutAsJsonAsync($"api/Product/{id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProduct(int id)
    {
        var response = await httpClient.DeleteAsync($"api/Product/{id}");
        response.EnsureSuccessStatusCode();
    }
}
