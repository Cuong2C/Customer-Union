
using Customer_Union.IntegrationTest.Fixture;
using Customer_Union.Models;
using System.Net.Http.Json;
using Customer_Union;
using Customer_Union.Domain.Entities;
using System.Net.WebSockets;

namespace Customer_Union.IntegrationTest;

[Collection("IntegrationTests")]
public class IntegrationTest 
{
    private readonly TestDatabaseFixture _fixture;
    private readonly HttpClient _httpClient;

    public IntegrationTest(TestDatabaseFixture fixture, CustomWebApplicationFactory factory)
    {
        _fixture = fixture;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async void LBIntegrationTest()
    {
        // Arrange
        string clientCode01 = "CLIENT01", clientCode02 = "CLIENT02", clientCode03 = "CLIENT03", clientCode04 = "CLIENT04";
        var clientSecretRequest1 = new CreateClientSecretRequest() { ClientCode = clientCode01 };
        var clientSecretRequest2 = new CreateClientSecretRequest() { ClientCode = clientCode02 };
        var clientSecretRequest3 = new CreateClientSecretRequest() { ClientCode = clientCode03 };
        var clientSecretRequest4 = new CreateClientSecretRequest() { ClientCode = clientCode04 };
        var clientSourceNew = new ClientSource() 
        { 
            ClientCode = "CLientCodeNew",
            ClientName = "CLientCodeNew",
            Description = "Add in test",
            IsActive = true
        };
        var clientSecretRequest5 = new CreateClientSecretRequest() { ClientCode = clientSourceNew.ClientCode };
        // Act
        var deleteclientSourceNew = await _httpClient.DeleteAsync($"/api/v1/longbeach/client-sources/{clientSourceNew.ClientCode}");
        var addClientSourceResponse = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/client-sources", clientSourceNew);

        var createClientSecretResponse1 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/client-secrets", clientSecretRequest1);
        var createClientSecretResponse2 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/client-secrets", clientSecretRequest2);
        var createClientSecretResponse3 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/client-secrets", clientSecretRequest3);
        var createClientSecretResponse4 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/client-secrets", clientSecretRequest4);
        var createClientSecretResponseNew = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/client-secrets", clientSecretRequest5);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, createClientSecretResponse1.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, createClientSecretResponse2.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, createClientSecretResponse3.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, createClientSecretResponse4.StatusCode);
        Assert.True(System.Net.HttpStatusCode.OK.Equals(deleteclientSourceNew.StatusCode) || System.Net.HttpStatusCode.NotFound.Equals(deleteclientSourceNew.StatusCode));
        Assert.Equal(System.Net.HttpStatusCode.OK, addClientSourceResponse.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, createClientSecretResponseNew.StatusCode);

        // Arrange
        var clientSecretResponse1 = await createClientSecretResponse1.Content.ReadFromJsonAsync<CreateClientSecretResponse>();
        var clientSecretResponse2 = await createClientSecretResponse2.Content.ReadFromJsonAsync<CreateClientSecretResponse>();
        var clientSecretResponseNew = await createClientSecretResponseNew.Content.ReadFromJsonAsync<CreateClientSecretResponse>();

        var genrateTokenRequest1 = new GenrateTokenRequest
        {
            ClientCode = clientSecretResponse1!.ClientCode,
            ClientSecret = clientSecretResponse1!.ClientSecret
        };

        var genrateTokenRequest2 = new GenrateTokenRequest
        {
            ClientCode = clientSecretResponse2!.ClientCode,
            ClientSecret = clientSecretResponse2!.ClientSecret
        };

        clientSourceNew.IsActive = false;
        var genrateTokenRequestclientSourceNew = new GenrateTokenRequest
        {
            ClientCode = clientSecretResponseNew!.ClientCode,
            ClientSecret = clientSecretResponseNew!.ClientSecret
        };

        var genrateTokenRequestFailed1 = new GenrateTokenRequest
        {
            ClientCode = "FailedCode",
            ClientSecret = "FailedSecret"
        };

        var genrateTokenRequestFailed2 = new GenrateTokenRequest
        {
            ClientCode = clientSecretResponse2!.ClientCode,
            ClientSecret = "FailedSecretffgsdf"
        };

        var genrateTokenRequestFailed3 = new GenrateTokenRequest
        {
            ClientCode = clientSecretResponse1.ClientCode,
            ClientSecret = clientSecretResponse2.ClientSecret
        };

        var genrateTokenRequestFailed4 = new GenrateTokenRequest
        {
            ClientCode = "FailedCode",
            ClientSecret = clientSecretResponse1.ClientSecret
        };

        // Act
        var updateClientSourceResponse = await _httpClient.PutAsJsonAsync("/api/v1/longbeach/client-sources", clientSourceNew);
        var genrateTokenResponse1 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequest1);
        var genrateTokenResponse2 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequest2);
        var genrateTokenResponseClientSourceNew = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequestclientSourceNew);
        var genrateTokenResponseFailed1 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequestFailed1);
        var genrateTokenResponseFailed2 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequestFailed2);
        var genrateTokenResponseFailed3 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequestFailed3);
        var genrateTokenResponseFailed4 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequestFailed4);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, genrateTokenResponse1.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, genrateTokenResponse2.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, genrateTokenResponseClientSourceNew.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, genrateTokenResponseFailed1.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, genrateTokenResponseFailed2.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, genrateTokenResponseFailed3.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, genrateTokenResponseFailed4.StatusCode);

        // Arrange
        var tokenClient01 = await genrateTokenResponse1.Content.ReadFromJsonAsync<GenrateTokenResponse>();
        var tokenClient02 = await genrateTokenResponse2.Content.ReadFromJsonAsync<GenrateTokenResponse>();
        var tokenClientSourceNew = await genrateTokenResponseClientSourceNew.Content.ReadFromJsonAsync<GenrateTokenResponse>();

        // Act
        var requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/client-sources/{clientSourceNew.ClientCode}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode01);
        requestGetClientSource.Headers.Add("token", tokenClient01!.Token);
        var getClientSourceNewResponse = await _httpClient.SendAsync(requestGetClientSource);

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/client-sources/{clientCode01}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode01);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        var getClientSource01 = await _httpClient.SendAsync(requestGetClientSource);

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/client-sources");
        requestGetClientSource.Headers.Add("Client-Source", clientCode01);
        requestGetClientSource.Headers.Add("token", tokenClient01!.Token);
        var getAllClientSourcesResponse = await _httpClient.SendAsync(requestGetClientSource);


        var clientSourceNewResponse = await getClientSourceNewResponse.Content.ReadFromJsonAsync<ClientSource>();
        var clientSourceList = await getAllClientSourcesResponse.Content.ReadFromJsonAsync<IEnumerable<ClientSource>>();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, getClientSourceNewResponse.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, getClientSource01.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, getAllClientSourcesResponse.StatusCode);
        Assert.NotNull(clientSourceNewResponse);
        Assert.Equal(clientSourceNew.ClientCode, clientSourceNewResponse!.ClientCode);
        Assert.NotNull(clientSourceList);
        Assert.True(clientSourceList.Any());
        Assert.Contains(clientSourceList, x => x.ClientCode == clientCode01);
        Assert.DoesNotContain(clientSourceList, x => x.ClientCode == clientSourceNew.ClientCode);

        // Arrange
        var customerRequest1 = new CustomerRequest
        {
            Name = "Test Customer",
            TaxCode = "123456789999",
            Address = "123 Test St",
            Phone = "1234567890",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "Hanoi",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = "PEARL123",
        };

        var customerRequest2 = new CustomerRequest
        {
            Name = "Test Customer2",
            TaxCode = "",
            Address = "123 Test St",
            Phone = "1234567899",
            Phone2 = "0987654321",
            Phone3 = "1122334455",
            Email = null,
            Nationality = "VN",
            Province = "TPHCM",
            District = null,
            Gender = 2,
            DateOfBirth = DateTime.Now,
            BankAccount = "",
            BankName = "Test Bank",
            CustomerType = "Regular",
            PearlCustomerCode = null,
        };

        // Act
        requestGetClientSource = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/longbeach/customers");
        requestGetClientSource.Headers.Add("Client-Source", clientCode01);
        requestGetClientSource.Headers.Add("token", tokenClient01!.Token);
        requestGetClientSource.Content = JsonContent.Create(customerRequest1);
        var hashCodeCus1 = await _httpClient.SendAsync(requestGetClientSource);
        var hashCodeCus1Content = await hashCodeCus1.Content.ReadFromJsonAsync<HashCodeResponse>();

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/longbeach/customers");
        requestGetClientSource.Headers.Add("Client-Source", clientSourceNew.ClientCode);
        requestGetClientSource.Headers.Add("token", tokenClientSourceNew!.Token);
        requestGetClientSource.Content = JsonContent.Create(customerRequest2);
        var hashCodeNew = await _httpClient.SendAsync(requestGetClientSource);

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/longbeach/customers");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        requestGetClientSource.Content = JsonContent.Create(customerRequest2);
        var hashCodeCus2 = await _httpClient.SendAsync(requestGetClientSource);
        var hashCodeCus2Content = await hashCodeCus2.Content.ReadFromJsonAsync<HashCodeResponse>();

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/customers/customer-phone/{customerRequest2.Phone}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        var getCustomer2ByPhoneResponse = await _httpClient.SendAsync(requestGetClientSource);
        var cus2ByPhoneInDB = await getCustomer2ByPhoneResponse.Content.ReadFromJsonAsync<IEnumerable<CustomerResponse>>();
        var cus2InEnumrable = cus2ByPhoneInDB!.First();

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/customers/customer-taxcode/{customerRequest1.TaxCode}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode01);
        requestGetClientSource.Headers.Add("token", tokenClient01!.Token);
        var getCustomer1ByTaxCodeResponse = await _httpClient.SendAsync(requestGetClientSource);
        var cus1ByTaxInDb = await getCustomer1ByTaxCodeResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/customers/customer-pearl/{customerRequest1.PearlCustomerCode}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode01);
        requestGetClientSource.Headers.Add("token", tokenClient01!.Token);
        var getCustomer1PearlCodeResponse = await _httpClient.SendAsync(requestGetClientSource);
        var cus1ByPearlInDb = await getCustomer1PearlCodeResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, hashCodeCus1.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, hashCodeNew.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, hashCodeCus2.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, getCustomer2ByPhoneResponse.StatusCode);
        Assert.True(cus2ByPhoneInDB!.Any());
        Assert.Equal(customerRequest2.Phone, cus2InEnumrable.Phone);

        Assert.Equal(System.Net.HttpStatusCode.OK, getCustomer1ByTaxCodeResponse.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, getCustomer1PearlCodeResponse.StatusCode);
        Assert.Equal(hashCodeCus1Content!.HashCode, cus1ByTaxInDb!.HashCode);
        Assert.Equal(hashCodeCus1Content!.HashCode, cus1ByPearlInDb!.HashCode);
        Assert.Equal(cus1ByTaxInDb.Id, cus1ByTaxInDb!.Id);

        // Arrange
        var customerUpdateRequest = new CustomerRequest
        {
            Name = cus1ByTaxInDb.Name,
            TaxCode = cus1ByTaxInDb.TaxCode,
            Address = "HCM city",
            Phone = cus1ByTaxInDb.Phone,
            Phone2 = cus1ByTaxInDb.Phone2,
            Phone3 = cus1ByPearlInDb.Phone3,
            Email = cus1ByTaxInDb.Email,
            Nationality = cus1ByTaxInDb.Nationality,
            Province = cus1ByTaxInDb.Province,
            District = cus1ByTaxInDb.District,
            Gender = cus1ByTaxInDb.Gender,
            DateOfBirth = cus1ByTaxInDb.DateOfBirth,
            BankAccount = cus1ByTaxInDb.BankAccount,
            BankName = cus1ByTaxInDb.BankName,
            CustomerType = cus1ByTaxInDb.CustomerType,
            PearlCustomerCode = "",
            HashCode = cus1ByTaxInDb.HashCode
        };

        var revokeTkenRequest1 = new RevokeTokenRequest
        {
            ClientCode = clientCode01,
            ClientSecret = clientSecretResponse1!.ClientSecret
        };

        var revokeTkenRequest2 = new RevokeTokenRequest
        {
            ClientCode = clientCode02,
            ClientSecret = clientSecretResponse2!.ClientSecret
        };

        // Act
        requestGetClientSource = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/longbeach/customers/{cus2InEnumrable.Id}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        requestGetClientSource.Content = JsonContent.Create(customerUpdateRequest);
        var failedUpdateCus2 = await _httpClient.SendAsync(requestGetClientSource); // bad request because hashcode is not match

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/longbeach/customers/{cus1ByTaxInDb.Id}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        requestGetClientSource.Content = JsonContent.Create(customerUpdateRequest);
        var succesAfterUpdateCus1 = await _httpClient.SendAsync(requestGetClientSource);
        var HashCode1AfterUpdate = await succesAfterUpdateCus1.Content.ReadFromJsonAsync<HashCodeResponse>(); // ok

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/customers/{cus1ByTaxInDb.Id}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        var succesGetAfterUpdateCus1 = await _httpClient.SendAsync(requestGetClientSource);
        var cus1AfterUpdate = await succesGetAfterUpdateCus1.Content.ReadFromJsonAsync<CustomerResponse>(); // ok

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/longbeach/auth/tokens/revoke");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        requestGetClientSource.Content = JsonContent.Create(revokeTkenRequest1);
        var revokeTokenResponse1 = await _httpClient.SendAsync(requestGetClientSource); // bad request because Client-Source not match

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/longbeach/auth/tokens/revoke");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        requestGetClientSource.Content = JsonContent.Create(revokeTkenRequest2);
        var revokeTokenResponse2 = await _httpClient.SendAsync(requestGetClientSource); 

        var createNewClientSecretResponse2 = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/client-secrets", clientSecretRequest2); 
        var newClientSecretResponse2 = await createNewClientSecretResponse2.Content.ReadFromJsonAsync<CreateClientSecretResponse>();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, failedUpdateCus2.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, succesAfterUpdateCus1.StatusCode);
        Assert.NotNull(HashCode1AfterUpdate);
        Assert.NotEqual(HashCode1AfterUpdate.HashCode, cus1ByTaxInDb.HashCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, succesGetAfterUpdateCus1.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, revokeTokenResponse1.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, revokeTokenResponse2.StatusCode);
        Assert.NotNull(newClientSecretResponse2);

        // Arrange
        var genrateTokenRequest2AfterCreateNewClientSecret = new GenrateTokenRequest
        {
            ClientCode = newClientSecretResponse2!.ClientCode,
            ClientSecret = newClientSecretResponse2!.ClientSecret
        };

        // Act
        var newGenrateTokenResponseFromOldClientSecret = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequest2);

        var newGenrateTokenResponseFromNewClientSecret = await _httpClient.PostAsJsonAsync("/api/v1/longbeach/auth/tokens", genrateTokenRequest2AfterCreateNewClientSecret);
        var newTokenClient02 = await newGenrateTokenResponseFromNewClientSecret.Content.ReadFromJsonAsync<GenrateTokenResponse>();

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/longbeach/customers/{cus1ByTaxInDb.Id}");
        requestGetClientSource.Headers.Add("Client-Source", clientCode02);
        requestGetClientSource.Headers.Add("token", tokenClient02!.Token);
        var deleteCus1ResponseByRevokedToken = await _httpClient.SendAsync(requestGetClientSource);

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/longbeach/customers/{cus1ByTaxInDb.Id}");
        requestGetClientSource.Headers.Add("Client-Source", newTokenClient02!.ClientCode);
        requestGetClientSource.Headers.Add("token", newTokenClient02!.Token);
        var deleteCus1ResponseByValidToken = await _httpClient.SendAsync(requestGetClientSource);

        requestGetClientSource = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/longbeach/customers/{cus1ByTaxInDb.Id}");
        requestGetClientSource.Headers.Add("Client-Source", newTokenClient02!.ClientCode);
        requestGetClientSource.Headers.Add("token", newTokenClient02!.Token);
        var getCustomerAfterDelete = await _httpClient.SendAsync(requestGetClientSource);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, newGenrateTokenResponseFromOldClientSecret.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, newGenrateTokenResponseFromNewClientSecret.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, deleteCus1ResponseByRevokedToken.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, deleteCus1ResponseByValidToken.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getCustomerAfterDelete.StatusCode);

    }
}