using Hexavia.Repository.Interfaces;
using NUnit.Framework;

namespace Hexavia.Repository.Tests
{
    [TestFixture]
    public class PaysRepositoryTests
    {
        private IPaysRepository paysRepository;
        private IParametreRepository parametreRepository;
        private DataAccessManager dataAccessManager;

        [SetUp]
        public void SetUp()
        {
            dataAccessManager = new DataAccessManager(new EasyCom());
            parametreRepository = new ParametreRepository(dataAccessManager);
            paysRepository = new PaysRepository(dataAccessManager, parametreRepository);
        }

        [Test]
        public void GetPays_should_Return_List_Of_Parameters()
        {
            var listCountry = paysRepository.GetPays();
            Assert.That(listCountry.Count, Is.GreaterThan(0));

            var france = listCountry.Find(pays => pays.Code == "F" && pays.Libelle == "France");
            Assert.That(france, Is.Not.Null);
        }
    }
}
