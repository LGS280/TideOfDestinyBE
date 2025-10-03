using Microsoft.Extensions.Configuration;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class DownloadGameService : IDownloadGameService
    {
        private readonly IFileRepo _repo;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl; // Cloudflare R2 public gateway base URL

        public DownloadGameService(IFileRepo repo, IHttpClientFactory httpClientFactory, HttpClient httpClient, IConfiguration configuration)
        {
            _repo = repo;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _baseUrl = configuration["R2Storage:AccountUrl"]
                       ?? throw new ArgumentNullException("CloudflareR2:PublicBaseUrl is missing in configuration");
        }

        public async Task<GameFile?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Stream?> DownloadFromR2Async(GameFile file)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(file.DownloadUrl, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
