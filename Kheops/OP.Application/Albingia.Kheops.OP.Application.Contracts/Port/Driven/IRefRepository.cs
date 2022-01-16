using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Domain.Parametrage;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Parametrage.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IReferentialRepository : IRepositoryCache
    {
        Categorie GetCategory(string branche, string sousBranche, string categorie);
        Cible GetCible(string cible);
        CibleCatego GetCibleCatego(string cible, string branche);
        IEnumerable<CibleCatego> GetCibleCategos();
        IEnumerable<Cible> GetCibles();

        Utilisateur GetUtilisateur(string code);
        IEnumerable<Utilisateur> GetUtilisateurs();

        T GetValue<T>(string code) where T : RefValue, new();
        IEnumerable<T> GetValues<T>() where T : RefValue, new();
        IEnumerable<RefValue> GetValues(string concept, string famille);

        BaseDeCalcul GetBase(TypeDeValeur typeDeValeur, string @base);
        Unite GetUnite(TypeDeValeur typeDeValeur, string unite);
    }

    public interface IFiltreRepository
    {
        //IEnumerable<Filtre> GetFiltres();
        Filtre GetFiltre(long id);
    }
    public interface IParamInventaireRepository
    {
        TypeInventaire GetTypeInventaire(long id);
        IDictionary<long, TypeInventaire> GetTypeInventaires();
    }
}