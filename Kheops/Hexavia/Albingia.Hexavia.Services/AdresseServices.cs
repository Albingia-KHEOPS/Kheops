using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albingia.Hexavia.CoreDomain;
using Albingia.Hexavia.DataAccess;

namespace Albingia.Hexavia.Services
{
    public class AdresseServices
    {
        private AdresseRepository adresseRepository;

        public AdresseServices(AdresseRepository adresseRepository)
        {
            this.adresseRepository = adresseRepository;
        }

        public ResultatRechercheAdresse VerifieAdresse(Adresse adresse)
        {
            ResultatRechercheAdresse result = ResultatRechercheAdresse.ERREUR;
            int nb = adresseRepository.RechercheAdresseCount(new List<Adresse> { adresse });
            if (nb == 1)
            {
                result = ResultatRechercheAdresse.OK;
            }
            else if (nb > 1)
            {
                result = ResultatRechercheAdresse.MULTIPLE;
            }
            return result;
        }

        public AdressesWrapper RechercheAdresse(Adresse adresseRecherchee, int startRow, int endRow)
        {
            AdressesWrapper result = new AdressesWrapper { Adresses = new List<Adresse>() };
            IEnumerable<Adresse> adressesDeVille = VillesQuiConviennent(adresseRecherchee, result);

            //si le nombre de ville atteint est le nombre maximal
            //on stope le traitement car on peut avoir des resultats errones
            if (adressesDeVille.Count() == AdresseRepository.MAX_ADRESSE)
            {
                result.Overflow = true;
            }

            else if (adressesDeVille.Count() != 0)
            {

                IEnumerable<Adresse> adressesDeVilleFusionees =
                    adresseRecherchee.FusionneAdressesSaufVilleEtCodePostal(adressesDeVille);

                //ECM 03/04/2012 recherche de l'adresse exacte 
                result.Adresses = adresseRepository.RechercheAdresse(adressesDeVilleFusionees, startRow, endRow, true).ToList();
                result.Count = adresseRepository.RechercheAdresseCount(adressesDeVilleFusionees, true);

                //ECM 03/04/2012 si aucun résultat, recherche des mots clefs (voie) sur le CP et Ville
                if (result.Count == 0)
                {
                    result.Adresses = adresseRepository.RechercheAdresse(adressesDeVilleFusionees, startRow, endRow).ToList();
                    result.Count = adresseRepository.RechercheAdresseCount(adressesDeVilleFusionees);
                }
                //si pas de resultat alors qu'on a des villes qui conviennent
                //on affiche toutes les rues des villes qui conviennent
                if (result.Count == 0)
                {
                    result.Adresses = adresseRepository.RechercheAdresse(adressesDeVille, startRow, endRow).ToList();
                    result.Count = adresseRepository.RechercheAdresseCount(adressesDeVille);
                    result.AucuneVoieNeConvient = true;
                }
            }
            return result;
        }

        private IEnumerable<Adresse> VillesQuiConviennent(Adresse adresseRecherchee, AdressesWrapper adressesWrapper)
        {
            IEnumerable<Adresse> result;
            if (adresseRecherchee.IsCedex.Value)
            {
                result = VillesQuiConviennentAlgoCedex(adresseRecherchee, adressesWrapper);
            }
            else
            {
                result = VillesQuiConvienneAlgoNonCedex(adresseRecherchee, adressesWrapper);
            }
            return result;
        }

        private IEnumerable<Adresse> VillesQuiConvienneAlgoNonCedex(Adresse adresseRecherchee, AdressesWrapper adressesWrapper)
        {
            IEnumerable<Adresse> result;
            adressesWrapper.HasCedex = false;
            result = VilleParCodePostalEtVille(adresseRecherchee);
            if (result.Count() == 0)
            {
                result = VilleParDepartementEtVille(adresseRecherchee);
            }
            return result;
        }

        private IEnumerable<Adresse> VillesQuiConviennentAlgoCedex(Adresse adresseRecherchee, AdressesWrapper adressesWrapper)
        {
            IEnumerable<Adresse> result = new List<Adresse>();
            adressesWrapper.HasCedex = true;

            //Recherche des villes à partir du departement et de la distribution
            //s il existe des mots clefs dans distribution
            if (adresseRecherchee.FiltreDistribution().Count != 0)
            {
                result = VilleParDepartementEtDistribution(adresseRecherchee);
            }

            //s il n'y a pas de mots clefs dans distribution
            //ou s'il la recherche par distribution n'a pas de resultat
            //on recherche par ville et departement
            if (result.Count() == 0)
            {
                result = VilleParDepartementEtVille(adresseRecherchee);
            }
            return result;
        }

        private IEnumerable<Adresse> VilleParDepartementEtDistribution(Adresse adresse)
        {
            return adresseRepository.RechercheVille(adresse.Departement, adresse.FiltreDistribution());
        }

        private IEnumerable<Adresse> VilleParDepartementEtVille(Adresse adresse)
        {
            //ZBO - 12/03/2012 : Appliquer le filtre sur la ville
            //return adresseRepository.RechercheVille(adresse.Departement, new List<String> { adresse.VilleSansCedex });
            return adresseRepository.RechercheVille(adresse.Departement, adresse.FiltreVille(adresse.VilleSansCedex));

        }

        private IEnumerable<Adresse> VilleParCodePostalEtVille(Adresse adresse)
        {
            //ZBO - 12/03/2012 : Appliquer le filtre sur la ville
            //return adresseRepository.RechercheVille(adresse.CodePostal, new List<String> { adresse.VilleSansCedex });
            return adresseRepository.RechercheVille(adresse.CodePostal, adresse.FiltreVille(adresse.VilleSansCedex));
        }
    }
}
