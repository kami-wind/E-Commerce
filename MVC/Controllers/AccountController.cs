using BusinessObjects.Entites;
using DataAccess.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MVC.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using DataAccess;

namespace MVC.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory, HttpClient httpClient)
    {
        _httpClient = httpClient;
        //_httpClient.BaseAddress = new Uri("https://nghia-ecommerce-api.odour.site/api/");
        this.httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _httpClient.PostAsJsonAsync("odata/Accounts/register", model);

            if (response.IsSuccessStatusCode)
            {
                // Registration successful, redirect to a success page or home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Registration failed, display error message
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _httpClient.PostAsJsonAsync("odata/Accounts/login", model);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // Deserialize the response content to get the token
                var tokenResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseData);
                var token = tokenResponse.Token;

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError(string.Empty, "Token not found in response.");
                    return View(model);
                }

                // Store the JWT token in local storage or cookie
                HttpContext.Response.Cookies.Append("jwtToken", token, new CookieOptions { HttpOnly = true });



                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, tokenResponse.Email),
                        new Claim(ClaimTypes.NameIdentifier, tokenResponse.PersonName)
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                //var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // Set the current user principal
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Login successful, redirect to a success page or home page
                return RedirectToAction("Index", "Product");
            }
            else
            {
                // Login failed, display error message
                //var errorMessage = await response.Content.ReadAsStringAsync();
                //ModelState.AddModelError(string.Empty, errorMessage);

                return RedirectToAction("Index", "Home");
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var token = HttpContext.Request.Cookies["jwtToken"];

        if (!string.IsNullOrEmpty(token))
        {
            // Perform logout on the server side
            var response = await _httpClient.GetAsync("odata/Accounts/logout");

            if (response.IsSuccessStatusCode)
            {
                // Remove the JWT token from the client-side
                HttpContext.Response.Cookies.Delete("jwtToken");
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

                // Logout successful, redirect to the home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Logout failed, display error message
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }
        }

        // If no token found or logout failed, redirect to home page
        return RedirectToAction("Index", "Home");

    }
}