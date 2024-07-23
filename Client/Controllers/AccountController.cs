using DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace Client.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;

    public AccountController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7091/api/");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDTO model)
    {
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("Account/register", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
        }
        else
        {
            // Handle error response
            return View();
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    //[HttpPost]
    //public async Task<IActionResult> Login(LoginDTO model)
    //{
    //    var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
    //    var response = await _httpClient.PostAsync("Account/login", content);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        // Handle successful login
    //        return RedirectToAction("Index", "Home");
    //    }
    //    else
    //    {
    //        // Handle login failure
    //        return View();
    //    }
    //    var formData = new FormUrlEncodedContent(new Dictionary<string, string>
    //{
    //    { "username", model.Username },
    //    { "password", model.Password }
    //     Add other form fields as needed
    //});

    //    var response = await _httpClient.PostAsync("Account/login", formData);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        Handle successful login
    //        return RedirectToAction("Index", "Home");
    //    }
    //    else
    //    {
    //        Handle login failure
    //        return View();
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        var formData = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "username", model.Username },
        { "password", model.Password }
        // Add other form fields as needed
    });

        // Set Content-Type header explicitly
        formData.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.PostAsync("Account/login", formData);

        if (response.IsSuccessStatusCode)
        {
            // Handle successful login
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Handle login failure
            return View();
        }
    }

    public async Task<IActionResult> Logout()
    {
        // Perform logout logic here
        return RedirectToAction("Index", "Home");
    }
}
