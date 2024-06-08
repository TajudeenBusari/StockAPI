using Newtonsoft.Json;
using Stock.API.Dtos;
using Stock.API.Interfaces;
using Stock.API.Mappers;

namespace Stock.API.Service;

public class FMPService: IFMPService
{
    private HttpClient _httpClient;
    private IConfiguration _configuration;
    public FMPService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task<Models.Stock> FindStockBySymbolAsync(string symbol)
    {
        try
        {
            var result =await _httpClient
                .GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                var stock = tasks[0];
                if (stock != null)
                {
                    return StockMappers.MapFromFMPStock(stock);
                    //stock.MapFromFMPStock()
                }

                return null;
            }

            return null;

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }
}

/*https://site.financialmodelingprep.com/developer/docs
//To authorize your requests, add 
?apikey=Bndd2IodQCDUAMUudxveReVFBSGps2Go at the end of every request*/