using System;
using ALBINGIA.Framework.Common.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OP.Services;
using OPServiceContract;

namespace Test.OP.Services
{
    [TestClass]
    public class LablatTests
    {
        /// <summary>
        /// http://lab.albingia.local:88/WebApi/api/Search?source=KHEOPS&lastName=ALBINGIA&idPartnerAs400=1&typeAs400=CT&username=NEOS4&userEmail=mohamed.ncir%40albingia.fr
        /// Le type de réponse  : 0: Entité non suspecte
        /// </summary>
        [TestMethod]
        public void VerificationPartenanireNotsuspiciousTest()
        {
            // Arrange
            var client = new Lablat();
            // Act
            var result = client.VerificationPartenaire(1, "1", "ALBINGIA", "NEOS4", contactAs400Id: 560);
            // Assert
            Assert.IsTrue(result == LablatResult.Notsuspicious, string.Format("Expected for '{0}': Notsuspicious; Actual: {1}", LablatResult.Notsuspicious, result));
        }
        /// <summary>
        /// http://lab.albingia.local:88/WebApi/api/Search?source=KHEOPS&lastName=GOYARD%20JEAN%20(SA%20DISTILLERIE)&idPartnerAs400=2222&typeAs400=AS&username=NEOS4&userEmail=mohamed.ncir%40albingia.fr
        /// Le type de réponse  : 1: Entité suspecte
        /// </summary>
        [TestMethod]
        public void VerificationPartenanireSuspiciousTest()
        {
            // Arrange
            var client = new Lablat();
            // Act
            var result = client.VerificationPartenaire(7, "2222", "GOYARD JEAN (SA DISTILLERIE)", "NEOS4");
            // Assert
            Assert.IsTrue(result == LablatResult.Suspicious, string.Format("Expected for '{0}': Suspicious; Actual: {1}", LablatResult.Suspicious, result));
        }
        /// <summary>
        /// http://lab.albingia.local:88/WebApi/api/Search?source=KHEOPS&firstName=Ana%20del%20Socorro&lastName=ABARCA%20UCLES&countryName=Honduras&idPartnerAs400=700&idContactAs400=260&typeAs400=CI&username=htaki&userEmail=htaki%40albingia.fr
        /// Le type de réponse  : -1: Entité Black Listée
        /// </summary>
        [TestMethod]
        public void VerificationPartenanireBlacklistedTest()
        {
            // Arrange
            var client = new Lablat();
            // Act
            var result = client.VerificationPartenaire(3, "700", "ABARCA UCLES", "NEOS4",countryName: "Honduras", partnerFirstName: "Ana del Socorro", contactAs400Id: 260);
            // Assert
            Assert.IsTrue(result == LablatResult.Blacklisted, string.Format("Expected for '{0}': Blacklisted; Actual: {1}", LablatResult.Blacklisted, result));
        }
        /// <summary>
        /// http://lab.albingia.local:88/WebApi/api/Search?source=KHEOPS&lastName=&idPartnerAs400=4444&typeAs400=CI&username=htaki&userEmail=htaki%40albingia.fr
        /// Le champ Nom partenaire est vide.
        /// Le type de réponse  : 400 bad request : Entité Black Listée
        /// </summary>
        [TestMethod]
        public void VerificationPartenanireErrorTest()
        {
            // Arrange
            var client = new Lablat();
            // Act
            var result = client.VerificationPartenaire(3, "4444", "", "NEOS4");
            // Assert
            Assert.IsTrue(result == LablatResult.Error, string.Format("Expected for '{0}': Error; Actual: {1}", LablatResult.Error, result));
        }


    }
}
