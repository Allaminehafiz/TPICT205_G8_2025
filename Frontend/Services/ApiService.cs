using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace Frontend.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthResponse?> LoginAsync(LoginModel model)
    {
        var response = await _http.PostAsJsonAsync("auth/login", model);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
        return null;
    }
}

public class AuthResponse
{
    public string Token { get; set; } = "";
    public string Role { get; set; } = "";
}

public class LoginModel
{
    public string Identifier { get; set; } = "";
    public string Password { get; set; } = "";
}
