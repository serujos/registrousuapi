using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using registrousuapi.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace registrousuapi.Services
{
    public class PaisService
    {
        private readonly HttpClient _httpClient;

        public PaisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaisInfo?> GetInfoPaisAsync(string codigo)
        {
            var url = $"https://restcountries.com/v3.1/alpha/{codigo}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var datos = JsonSerializer.Deserialize<List<JsonElement>>(json, opciones);

                var pais = datos?.FirstOrDefault();
                if (pais == null || pais.Value.ValueKind == JsonValueKind.Undefined || pais.Value.ValueKind == JsonValueKind.Null)
                    return null;

                var paisElement = pais.Value;

                string nombreOficial = null;
                string region = null;
                string banderaUrl = null;

                if (paisElement.TryGetProperty("name", out JsonElement nameElement) &&
                    nameElement.TryGetProperty("official", out JsonElement officialElement))
                {
                    nombreOficial = officialElement.GetString();
                }

                if (paisElement.TryGetProperty("region", out JsonElement regionElement))
                {
                    region = regionElement.GetString();
                }

                if (paisElement.TryGetProperty("flags", out JsonElement flagsElement) &&
                    flagsElement.TryGetProperty("png", out JsonElement pngElement))
                {
                    banderaUrl = pngElement.GetString();
                }

                return new PaisInfo
                {
                    NombreOficial = nombreOficial,
                    Region = region,
                    BanderaUrl = banderaUrl
                };
            }
            catch
            {
                return null;
            }
        }
    }
}

