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

        [HttpGet("{startId}/{endId}")]
        public async Task<IActionResult> GetCharacters(int startId, int endId)
        {
            var tasks = new List<Task<Character>>();
            string baseUrl = "https://rickandmortyapi.com/api/character/";

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            for (int i = startId; i <= endId; i++)
            {
                string url = baseUrl + i;

                tasks.Add(GetCharacterAsync(url, options));
            }

            var characters = await Task.WhenAll(tasks);

            return Ok(characters.Where(c => c != null));
        }

        private async Task<Character> GetCharacterAsync(string url, JsonSerializerOptions options)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Character>(jsonResponse, options);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
