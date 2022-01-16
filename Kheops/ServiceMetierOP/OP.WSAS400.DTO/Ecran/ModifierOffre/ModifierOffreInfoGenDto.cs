using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Ecran.ModifierOffre
{
    [DataContract]
    public class ModifierOffreInfoGenDto
    {
        #region INFOS HIDDEN

        [DataMember]
        [Column(Name = "CODEOFFRE")]
        public String CodeOffre { get; set; }

        [DataMember]
        [Column(Name = "VERSION")]
        public Int32 Version { get; set; }

        [DataMember]
        [Column(Name = "TYPE")]
        public String Type { get; set; }

        [DataMember]
        [Column(Name = "CODEBRANCHE")]
        public String CodeBranche { get; set; }

        [DataMember]
        [Column(Name = "LBLBRANCHE")]
        public String LblBranche { get; set; }

        #endregion
        #region INFOS BANDEAU

        [DataMember]
        [Column(Name = "CODEASSURE")]
        public Int64 CodeAssure { get; set; }

        [DataMember]
        [Column(Name = "LBLASSURE")]
        public String LblAssure { get; set; }

        #endregion
        #region INFOS GENERALES
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public String Descriptif { get; set; }

        [DataMember]
        [Column(Name = "CODECIBLE")]
        public String CodeCible { get; set; }

        [DataMember]
        [Column(Name = "NOMCIBLE")]
        public String NomCible { get; set; }

        [DataMember]
        [Column(Name = "OBSERVATION")]
        public String Observation { get; set; }

        [DataMember]
        [Column(Name = "DEVISE")]
        public String Devise { get; set; }

        [DataMember]
        [Column(Name = "CODEREGIME")]
        public String CodeRegime { get; set; }

        [DataMember]
        [Column(Name = "LBLREGIME")]
        public String LblRegime { get; set; }

        [DataMember]
        [Column(Name = "SOUMISCATNAT")]
        public String SoumisCatNat { get; set; }

        [DataMember]
        [Column(Name = "CODEPERIODICITE")]
        public String CodePeriodicite { get; set; }

        [DataMember]
        [Column(Name = "LBLPERIODICITE")]
        public String LblPeriodicite { get; set; }

        [DataMember]
        [Column(Name = "ECHEANCEPRINCIPALE")]
        public Int64 EcheancePrincipale { get; set; }

        [DataMember]
        [Column(Name = "DATEDEBUTEFFET")]
        public Int64 DateDebutEffet { get; set; }

        [DataMember]
        [Column(Name = "HEUREDEBUTEFFET")]
        public Int64 HeureDebutEffet { get; set; }

        [DataMember]
        [Column(Name = "DATEFINEFFET")]
        public Int64 DateFinEffet { get; set; }

        [DataMember]
        [Column(Name = "HEUREFINEFFET")]
        public Int64 HeureFinEffet { get; set; }

        [DataMember]
        [Column(Name = "DUREE")]
        public Int64 Duree { get; set; }

        [DataMember]
        [Column(Name = "UNITEDUREE")]
        public String UniteDuree { get; set; }

        [DataMember]
        [Column(Name = "INDICEREFERENCE")]
        public String IndiceReference { get; set; }

        [DataMember]
        [Column(Name = "VALEURINDICE")]
        public Single ValeurIndice { get; set; }

        [DataMember]
        [Column(Name = "CODENATURECONTRAT")]
        public String CodeNatureContrat { get; set; }

        [DataMember]
        [Column(Name = "LBLNATURECONTRAT")]
        public String LblNatureContrat { get; set; }

        [DataMember]
        [Column(Name = "PARTAPERITEUR")]
        public Single PartAperiteur { get; set; }

        [DataMember]
        [Column(Name = "FRAISAPERITEUR")]
        public Single FraisAperiteur { get; set; }

        [DataMember]
        [Column(Name = "INTERCALAIREEXISTE")]
        public String IntercalaireExiste { get; set; }

        [DataMember]
        [Column(Name = "CODESOUSCRIPTEUR")]
        public String CodeSouscripteur { get; set; }

        [DataMember]
        [Column(Name = "LBLSOUSCRIPTEUR")]
        public String LblSouscripteur { get; set; }

        [DataMember]
        [Column(Name = "CODEGESTIONNAIRE")]
        public String CodeGestionnaire { get; set; }

        [DataMember]
        [Column(Name = "LBLGESTIONNAIRE")]
        public String LblGestionnaire { get; set; }

        #endregion
    }
}
