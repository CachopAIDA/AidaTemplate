using Core.Database;
using NUnit.Framework;

namespace AidaTemplate.Tests.Infrastructure {
    [TestFixture]
    public class SqlTest {
        protected SqlServerConnectionProvider TestDatabase { get; private set; }

        [OneTimeSetUp]
        public void RunBeforeAllTests() {
            var connectionString = TestConfiguration.GetConnectionString("SqlServer");
            TestDatabase = new SqlServerConnectionProvider(
                new DatabaseConnectorFromConnectionString<System.Data.SqlClient.SqlConnection>(connectionString));
        }
    }
}