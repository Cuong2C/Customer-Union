
using Longbeach.IntegrationTest.Fixture;

namespace Longbeach.IntegrationTest;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory>, ICollectionFixture<TestDatabaseFixture>
{
}
