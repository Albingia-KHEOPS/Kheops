using Hexavia.Models;
using Hexavia.Repository.Interfaces;
using Hexavia.Tools.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Hexavia.Repository.Tests
{
    [TestFixture]
    public class AdresseRepositoryTests
    {
        private IAddressRepository addressRepository;
        private int numCronoSample;

        [SetUp]
        public void SetUp()
        {
            numCronoSample = 781471;
            addressRepository = new AdresseRepository(new DataAccessManager(new EasyCom()));
        }

        [Test]
        public void GetAdresseByNumeroChrono_Should_Return_Null_If_Number_Invalid()
        {
            var invalidAdresse = addressRepository.GetAdresseByNumeroChrono(-1);
            Assert.That(invalidAdresse, Is.Null);
        }

        [Test]
        public void GetAdresseByNumeroChrono_Should_Return_Adress_If_Number_Is_Valid()
        {
            var adresse = addressRepository.GetAdresseByNumeroChrono(numCronoSample);
            Assert.That(adresse, Is.Not.Null);
        }

        [Test]
        public void RechercheAdresse_Should_Throw_Exception_When_Not_Adress()
        {
            var adresse = addressRepository.GetAdresseByNumeroChrono(numCronoSample);
            adresse.CodePostal = string.Empty;
            adresse.Ville = null;

            var listAdress = new List<Adresse> { adresse };

            Assert.Throws<AdresseException>(() => addressRepository.RechercheAdresse(listAdress));
        }

        [Test]
        public void RechercheAdresse_Should_Return_Adress()
        {
            var adresse = addressRepository.GetAdresseByNumeroChrono(numCronoSample);
            var listAdress = new List<Adresse> { adresse };
            var adresses = addressRepository.RechercheAdresse(listAdress);

            Assert.That(adresses, Is.Not.Null);
        }
    }
}
