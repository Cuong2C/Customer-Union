namespace Customer_Union.IntegrationTest.Fixture
{
    public class TestDatabaseFixture
    {
        public TestDatabaseFixture()
        {
            // run sync due to constructor is not support async
            Task.Run(() => TestDatabaseInitializer.InitializeDatabaseAsync()).GetAwaiter().GetResult();
        }
    }
}
