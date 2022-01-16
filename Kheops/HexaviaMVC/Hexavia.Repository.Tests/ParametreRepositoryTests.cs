using Hexavia.Repository.Interfaces;
using NUnit.Framework;

namespace Hexavia.Repository.Tests
{
    [TestFixture]
    public class ParametreRepositoryTests
    {
        private IParametreRepository parametreRepository;

        [SetUp]
        public void SetUp()
        {
            parametreRepository = new ParametreRepository(new DataAccessManager(new EasyCom()));
        }

        [Test]
        public void Load_should_Return_List_Of_Parameters()
        {
            var parameters = parametreRepository.Load(null,null);
            Assert.That(parameters.Count, Is.GreaterThan(16000));
        }
    }
}
