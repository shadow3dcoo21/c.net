using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CharacterController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters()
        {
            string url = "https://hp-api.onrender.com/api/characters";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var characters = JsonSerializer.Deserialize<List<Character>>(jsonResponse, options);
                    return Ok(characters);
                }
                return StatusCode((int)response.StatusCode, "Error fetching characters.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while fetching the characters.");
            }
        }
    }
}
