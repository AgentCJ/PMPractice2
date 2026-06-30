using System.Net.Http.Json;
using MasterFloorAPP.Models;

namespace MasterFloorAPP.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5150";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<PartnerListItem>?> GetPartnersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/partners");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<PartnerListItem>>();
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return null;
        }
    }


    public async Task<bool> CreatePartnerAsync(Partner partner)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/api/partners", partner);
        if (response.IsSuccessStatusCode)
            return true;
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Ошибка создания ({(int)response.StatusCode}): {error}");
    }



    public async Task<bool> UpdatePartnerAsync(int id, Partner partner)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/api/partners/{id}", partner);
        if (response.IsSuccessStatusCode)
            return true;
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Ошибка обновления ({(int)response.StatusCode}): {error}");
    }



    public async Task<Partner?> GetPartnerDetailAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/partners/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Partner>();
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения деталей: {ex.Message}");
            return null;
        }
    }

    public async Task<List<PartnerType>?> GetPartnerTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/api/partners/types");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<PartnerType>>();
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Ошибка получения типов ({(int)response.StatusCode}): {error}");
    }

    public async Task<List<SaleHistoryItem>?> GetPartnerSalesAsync(int partnerId)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/api/partners/{partnerId}/sales");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<SaleHistoryItem>>();
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Ошибка получения истории ({(int)response.StatusCode}): {error}");
    }

}