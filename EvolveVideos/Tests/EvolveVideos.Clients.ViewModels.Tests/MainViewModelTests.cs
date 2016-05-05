

using NUnit.Framework;

namespace EvolveVideos.Clients.ViewModels.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void PassingTest()
        {
            Assert.AreEqual(4, Add(2, 2));
        }

        [Test]
        public void FailingTest()
        {
            Assert.AreEqual(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}