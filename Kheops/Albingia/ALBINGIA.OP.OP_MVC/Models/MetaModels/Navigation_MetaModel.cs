namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    public class Navigation_MetaModel : MetaModelsBase
    {
        #region
        public const int ECRAN_HISTORIQUE = 0;
        public const int ECRAN_INFOGENERALE = 1;
        public const int ECRAN_RISQUEETGARANTIE = 2;
        public const int ECRAN_ENGAGEMENTS = 3;
        public const int ECRAN_REGULE = 3;
        public const int ECRAN_COTISATIONS = 4;
        public const int ECRAN_INFOFIN = 5;

        public const int ECRAN_SAISIE = 6;
        public const int ECRAN_CONFIRMATION = 7;

        public const int ECRAN_AFFAIRENOUVELLE = 8;
        public const int ECRAN_RSQOBJAFFNOUV = 9;
        public const int ECRAN_FORMVOLAFFNOUV = 10;
        public const int ECRAN_OPTTARAFFNOUV = 11;

        public int GetEcranInfoGene { get { return ECRAN_INFOGENERALE; } }
        public int GetEcranRisqueGarantie { get { return ECRAN_RISQUEETGARANTIE; } }
        public int GetEcranEngagements { get { return ECRAN_ENGAGEMENTS; } }
        public int GetEcranRegules { get { return ECRAN_REGULE; } }
        public int GetEcranCotisations { get { return ECRAN_COTISATIONS; } }
        public int GetEcranInfoFin { get { return ECRAN_INFOFIN; } }

        public int GetEcranSaisie { get { return ECRAN_SAISIE; } }
        public int GetEcranConfirmation { get { return ECRAN_CONFIRMATION; } }

        public int GetEcranAffaireNouvelle { get { return ECRAN_AFFAIRENOUVELLE; } }
        public int GetEcranRsqObjAffNouv { get { return ECRAN_RSQOBJAFFNOUV; } }
        public int GetEcranFormVolAffNouv { get { return ECRAN_FORMVOLAFFNOUV; } }
        public int GetEcranOptTarAffNouv { get { return ECRAN_OPTTARAFFNOUV; } }

        #endregion

        public string IdOffre { get; set; }
        public int? Version { get; set; }
        public int Etape { get; set; }
        public bool Brouillon { get; set; }


    }
}