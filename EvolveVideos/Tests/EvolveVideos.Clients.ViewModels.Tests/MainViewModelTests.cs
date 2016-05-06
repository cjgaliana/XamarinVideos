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


        private int Add(int x, int y)
        {
            return x + y;
        }
    }
}