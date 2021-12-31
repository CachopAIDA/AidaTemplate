using NUnit.Framework;

namespace AidaTemplate.Tests.Infrastructure {
    public class SqlProxyTest : SqlTest {

        [SetUp]
        public void SetUp() {
            TestDatabase.EstablishConnection();
        }

        [TearDown]
        public void TearDown() {
            TestDatabase.Close();
        }

    }
}