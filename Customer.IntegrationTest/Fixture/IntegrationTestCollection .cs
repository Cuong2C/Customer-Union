
using Customer_Union.IntegrationTest.Fixture;

namespace Customer_Union.IntegrationTest;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory>, ICollectionFixture<TestDatabaseFixture>
{
}
