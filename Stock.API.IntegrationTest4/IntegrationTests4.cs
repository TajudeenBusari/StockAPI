using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Stock.API.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Stock.API.IntegrationTest4
{
    public class IntegrationTests4 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        //private HttpClient _client;
        private readonly string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImludmVzdG9yVEpAZ21haWwuY29tIiwiZ2l2ZW5fbmFtZSI6ImludmVzdG9yVEoiLCJuYmYiOjE3MTgyNzI5MTYsImV4cCI6MTcxODg3NzcxNiwiaWF0IjoxNzE4MjcyOTE2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNTAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNTAifQ.CVoKgM_LV0apudrGJEltdW8nvv8HS5T_7oLfg28rLTfMLOD3HmhrqgNt5Uyyswh_xGaZQbVArzM4XCCgzsk-2Q";

        public IntegrationTests4(WebApplicationFactory<Program> factory)
        {
            
            _factory = factory;
            
        }

        //1. get stocks with valid token with success
        [Fact]
        public async Task GetStocks_WithValidToken_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            //var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImludmVzdG9yVEpAZ21haWwuY29tIiwiZ2l2ZW5fbmFtZSI6ImludmVzdG9yVEoiLCJuYmYiOjE3MTgyNzI5MTYsImV4cCI6MTcxODg3NzcxNiwiaWF0IjoxNzE4MjcyOTE2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNTAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNTAifQ.CVoKgM_LV0apudrGJEltdW8nvv8HS5T_7oLfg28rLTfMLOD3HmhrqgNt5Uyyswh_xGaZQbVArzM4XCCgzsk-2Q"; // Replace with a valid JWT token
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/api/stock");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            
            
            Assert.Contains("Microsoft", responseContent);
            Assert.Contains("MSFT", responseContent);
            Assert.Contains("Tesla", responseContent);
            Assert.Contains("Mastercard Incorporated", responseContent);

        }

        //2. get stocks without token with unauthorized
        [Fact]
        public async Task GetStock_WithoutToken_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/stock");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        //3. get a stock with valid token with success
        [Fact]
        public async Task GetAStock_WithToken_ReturnsSuccess()
        {
            //Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var stockDtos = IntegrationTest4Helper.GetStockDtos();
            var stockId = stockDtos[0].Id;
            
            
            //Act
            var response = await client.GetAsync($"/api/stock/{stockId}");
            //Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<StockDto>(responseContent);
            Assert.Equal(stockId, resource?.Id);
        }

        //4. post and delete a stock with valid token with success
        [Fact]
        public async Task PostAndDeleteStock_WithToken_ReturnsSuccess()
        {
            //Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var stockDto = IntegrationTest4Helper.TestAddStock();
            
            //Act
           var response = await client.PostAsJsonAsync("/api/stock", stockDto);
            
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<StockDto>(responseContent);
            var createdId = (resource?.Id.Should() != null);
            createdId.Should().BeTrue();
            
            //Test to delete the created stock
            response = await client.DeleteAsync($"/api/stock/{resource?.Id}");
            response.EnsureSuccessStatusCode();
            var stringContent= await response.Content.ReadAsStringAsync();
            Assert.Contains("TJ Logis", stringContent);
            Assert.Contains("companyName", stringContent);
            var deserializeObject = JsonConvert.DeserializeObject<StockDto>(stringContent);
            if (deserializeObject != null)
            {
                Assert.Equal("TJ Logis",deserializeObject.CompanyName );
                Assert.Equal(1000000000, deserializeObject.MarketCap);
                Assert.Equal(100, deserializeObject.Purchase);
                Assert.Equal(100, deserializeObject.LastDiv);
                Assert.Equal("Logistics", deserializeObject.Industry);
                Assert.Equal([], deserializeObject.Comments);
                Assert.Equal("TJL", deserializeObject.Symbol);
            };

        }
        
        
        //5. update a stock with valid token with success
        [Fact]
        public async Task UpdateAStockAnExisting_WithToken_ReturnsSuccess()
        {
            //Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var stockDtoToBeUpdated = IntegrationTest4Helper.TestStockDto();
            var stockToBeUpdatedId = stockDtoToBeUpdated.Id;
            //var existingStockId = 1;
            
            var updated = new 
            {
                
                CompanyName = "MicrosoftU",
                Symbol = "MSFTU",
                Purchase = 100,
                LastDiv = 100,
                Industry = "Logistics",
                MarketCap = 100000000
                
            };
            var content = new StringContent(JsonSerializer.Serialize(updated),
                Encoding.UTF8, "application/json");
            
            //Act
            var response = await client.PutAsync($"/api/stock/{stockToBeUpdatedId}", content);

            //Assert
            response.EnsureSuccessStatusCode();
            var resContent = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<StockDto>(resContent);
            Assert.Equal(stockToBeUpdatedId, res?.Id);
            Assert.Equal("MicrosoftU", res?.CompanyName);
            Assert.Equal("MSFTU", res?.Symbol);
            Assert.Equal(100000000, res?.MarketCap);
        }
        
    }
    
}

/*
 * delete a stock with valid token with success
        [Fact]
        public async Task DeleteAnExistingStock_WithToken_ReturnsSuccess()
        {
            //Arrange
            //var client = _factory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //var stockDtoToBeDeleted = IntegrationTest4Helper.TestStockDto();
            //var stockToBeDeleteId = stockDtoToBeDeleted.Id;
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            /*var deleted = new StockDto
            {
                Id = 8,
                CompanyName = "TJ Logis",
                Symbol = "TJL",
                Purchase = 100,
                LastDiv = 100,
                Industry = "Logistics",
                MarketCap = 100000000

            };* /
            
            
            //var deletedId = deleted.Id;
            var deletedId = 14;
           
            //PostAStock_WithToken_ReturnsSuccess().ToString()
            
           
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/stock/{deletedId}");
            


            //Act
            //var response = await client.DeleteAsync($"/api/stock/{deletedId}");
            var response = await client.SendAsync(requestMessage);
            
            //Assert
            //Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            response.EnsureSuccessStatusCode();
        }
 *
 * 
 */