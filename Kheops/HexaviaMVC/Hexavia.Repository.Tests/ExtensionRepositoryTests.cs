using Hexavia.Repository.Interfaces;
using NUnit.Framework;

namespace Hexavia.Repository.Tests
{
    [TestFixture]
    public class ExtensionRepositoryTests
    {
        private IExtensionRepository extensionRepository;

        [SetUp]
        public void SetUp()
        {
            extensionRepository = new ExtensionRepository(new DataAccessManager(new EasyCom()));
        }

        [Test]
        public void GetList_should_Return_List_Of_Extensions()
        {
            var listExtensions = extensionRepository.GetList();
            Assert.That(listExtensions.Count, Is.EqualTo(9));
        }
    }
}
