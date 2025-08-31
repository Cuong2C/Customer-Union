using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longbeach.IntegrationTest.Fixture
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
