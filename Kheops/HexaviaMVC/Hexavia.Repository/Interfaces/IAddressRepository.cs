using Hexavia.Models;
using System.Collections.Generic;

namespace Hexavia.Repository.Interfaces
{
    public interface IAddressRepository
    {
        /// <summary>
        /// Recupere la liste des adresses
        /// </summary>
        /// <returns></returns>
        IList<Adresse> GetAllAdresses();

        IList<Adresse> GetAllAdressesNotGeolocalisated();

        IList<Adresse> GetAllAdressesNotGeolocalisatedByPage(int pageNumber, int pageSize);
        /// <summary>
        /// Permet d'obtenir une adresse par son numéro chrono.
        /// </summary>
        /// <param name="numeroChrono"></param>
        /// <returns></returns>
        Adresse GetAdresseByNumeroChrono(int numeroChrono);

        int RechercheAdresseCount(IEnumerable<Adresse> adresses, bool adrExacte = false);

        IEnumerable<Adresse> RechercheAdresse(IEnumerable<Adresse> adresses, int startRow = 0, int endRow = 100, bool adrExacte = false);

        IEnumerable<Adresse> RechercheVille(string codePostal, IEnumerable<string> motsVille);
    }
}
