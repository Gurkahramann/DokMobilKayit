using DockMobile.ApiClient.Models;
using DockMobile.ApiClient.Models.ApiModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DockMobile.ApiClient
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        public ApiClientService(IOptions<ApiClientOptions> apiClientOptions)
        {
            var options = apiClientOptions.Value;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(options.ApiBaseAddress)
            };
        }
        public async Task<List<DokPartiAna>?> GetDocks()
        {
            return await _httpClient.GetFromJsonAsync<List<DokPartiAna>?>("/api/DokPartiAna");
        }
        public async Task<DokPartiAna?> GetDockByNumber(string dokNumara)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/DokPartiAna/byNumber/{dokNumara}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DokPartiAna>();
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
        }

        public async Task<List<string>?> GetPartiNumaralari(int dokId)
        {
            return await _httpClient.GetFromJsonAsync<List<string>?>($"/api/DokPartiAna/{dokId}/partinumaralari");
        }
        public async Task<DokPartiAna?> SaveDock(DokPartiAna dok)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/DokPartiAna", dok);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<DokPartiAna>();
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> UpdateDock(DokPartiAna dokPartiAna)
        {
            try
            {
                return await _httpClient.PutAsJsonAsync("/api/DokPartiAna", dokPartiAna);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error updating dock info: {ex.Message}", ex);
            }
        }

        public async Task DeleteDock(int id)
        {
            await _httpClient.DeleteAsync($"/api/DokPartiAna/{id}");
        }

        //Dock Parti Detay ile alakalı kısımlar
        public async Task<List<DokPartiDetay>?> GetPartyDetails()
        {
            return await _httpClient.GetFromJsonAsync<List<DokPartiDetay>?>("/api/DokPartiDetay");
        }
        public async Task<DokPartiDetay?> GetPartyById(int id)
        {
            return await _httpClient.GetFromJsonAsync<DokPartiDetay?>($"/api/DokPartiDetay/{id}");
        }
        public async Task<HttpResponseMessage> SaveParty(DokPartiDetay party)
        {
            try
            {
                return await _httpClient.PostAsJsonAsync("/api/DokPartiDetay", party);
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
        }
        public async Task<DokPartiDetay?> GetPartyByNumber(string partiNumara)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/DokPartiDetay/number/{partiNumara}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DokPartiDetay>();
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
        }
        public async Task UpdateParty(DokPartiDetay party)
        {
            await _httpClient.PutAsJsonAsync("/api/DokPartiDetay", party);
        }
        public async Task <HttpResponseMessage> DeleteParty(int id)
        {
             return await _httpClient.DeleteAsync($"/api/DokPartiDetay/{id}");
        }

    }
}
