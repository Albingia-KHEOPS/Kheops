using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceDetailDto
    {
        [DataMember]
        [Column(Name = "CODENATURECONTRAT")]
        public String CodeNatureContrat { get; set; }

        [DataMember]
        [Column(Name = "LIBELLENATURECONTRAT")]
        public String LibelleNatureContrat { get; set; }

        [DataMember]
        [Column(Name = "PART")]
        public Double Part { get; set; }

        [DataMember]
        [Column(Name = "AVENANT")]
        public Single Avenant { get; set; }

        [DataMember]
        [Column(Name = "DEVISE")]
        public string Devise { get; set; }

        [DataMember]
        [Column(Name = "CODEOPERATION")]
        public Int16 CodeOperation { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEOPERATION")]
        public String LibelleOperation { get; set; }

        [DataMember]
        [Column(Name = "CAPITAL")]
        public Double Capital { get; set; }

        [DataMember]
        [Column(Name = "CODEPERIODICITE")]
        public String CodePeriodicite { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEPERIODICITE")]
        public String LibellePeriodicite { get; set; }

        [DataMember]
        [Column(Name = "DEBUTPERIODEANNEE")]
        public Int16 DebutPeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "DEBUTPERIODEMOIS")]
        public Int16 DebutPeriodeMois { get; set; }

        [DataMember]
        [Column(Name = "DEBUTPERIODEJOUR")]
        public Int16 DebutPeriodeJour { get; set; }

        [DataMember]
        [Column(Name = "FINPERIODEANNEE")]
        public Int16 FinPeriodeAnnee { get; set; }

        [DataMember]
        [Column(Name = "FINPERIODEMOIS")]
        public Int16 FinPeriodeMois { get; set; }

        [DataMember]
        [Column(Name = "FINPERIODEJOUR")]
        public Int16 FinPeriodeJour { get; set; }

        [DataMember]
        [Column(Name = "CODEEMISSION")]
        public String CodeEmission { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEEMISSION")]
        public String LibelleEmission { get; set; }

        [Column(Name = "DATEEMISSIONANNEE")]
        public Int16 DateEmissionAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATEEMISSIONMOIS")]
        public Int16 DateEmissionMois { get; set; }

        [DataMember]
        [Column(Name = "DATEEMISSIONJOUR")]
        public Int16 DateEmissionJour { get; set; }

        [DataMember]
        [Column(Name = "DATEECHEANCEANNEE")]
        public Int16 DateEcheanceAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATEECHEANCEMOIS")]
        public Int16 DateEcheanceMois { get; set; }

        [DataMember]
        [Column(Name = "DATEECHEANCEJOUR")]
        public Int16 DateEcheanceJour { get; set; }

        [DataMember]
        [Column(Name = "CODESITUATION")]
        public String CodeSituation { get; set; }

        [DataMember]
        [Column(Name = "LIBELLESITUATION")]
        public String LibelleSituation { get; set; }

        [DataMember]
        [Column(Name = "DATESITUATIONANNEE")]
        public Int16 DateSituationAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATESITUATIONMOIS")]
        public Int16 DateSituationMois { get; set; }

        [DataMember]
        [Column(Name = "DATESITUATIONJOUR")]
        public Int16 DateSituationJour { get; set; }

        [DataMember]
        [Column(Name = "CODERELANCE")]
        public String CodeRelance { get; set; }

        [DataMember]
        [Column(Name = "LIBELLERELANCE")]
        public String LibelleRelance { get; set; }

        [DataMember]
        [Column(Name = "DATERELANCEANNEE")]
        public Int16 DateRelanceAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATERELANCEMOIS")]
        public Int16 DateRelanceMois { get; set; }

        [DataMember]
        [Column(Name = "DATERELANCEJOUR")]
        public Int16 DateRelanceJour { get; set; }

        [DataMember]
        [Column(Name = "INDICE")]
        public Double Indice { get; set; }

        [DataMember]
        [Column(Name = "CODEACCORD")]
        public String CodeAccord { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEACCORD")]
        public String LibelleAccord { get; set; }

        [DataMember]
        [Column(Name = "CODEMOUVEMENT")]
        public String CodeMouvement { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEMOUVEMENT")]
        public String LibelleMouvement { get; set; }

        [DataMember]
        [Column(Name = "COMPTABILISE")]
        public String Comptabilise { get; set; }

        [DataMember]
        [Column(Name = "DATECOMPTANNEE")]
        public Int16 DateComptabiliseAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATECOMPTMOIS")]
        public Int16 DateComptabiliseMois { get; set; }

        [DataMember]
        [Column(Name = "CODEENCAISSEMENT")]
        public String CodeEncaissement { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEENCAISSEMENT")]
        public String LibelleEncaissement { get; set; }

        [DataMember]
        [Column(Name = "PNICTICONE")]
        public Int32 PNICTIcone { get; set; }
        [DataMember]
        [Column(Name = "PKICTICONE")]
        public Int32 PKICTIcone { get; set; }

        [DataMember]
        [Column(Name = "CODECOURTIER")]
        public Int32 CodeCourtier { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIER")]
        public String NomCourtier { get; set; }
        [DataMember]
        [Column(Name = "DEPCOURTIER")]
        public String DepCourtier { get; set; }
        [DataMember]
        [Column(Name = "CPOCOURTIER")]
        public String CpCourtier { get; set; }
        [DataMember]
        [Column(Name = "VILLCOURTIER")]
        public String VillCourtier { get; set; }

        [DataMember]
        [Column(Name = "USERCREATION")]
        public String UserCreation { get; set; }
        [DataMember]
        [Column(Name = "DATECREATIONANNEE")]
        public Int16 DateCreationAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATECREATIONMOIS")]
        public Int16 DateCreationMois { get; set; }
        [DataMember]
        [Column(Name = "DATECREATIONJOUR")]
        public Int16 DateCreationJour { get; set; }

        [DataMember]
        [Column(Name = "USERUPDATE")]
        public String UserUpdate { get; set; }
        [DataMember]
        [Column(Name = "DATEUPDATEANNEE")]
        public Int16 DateUserUpdateAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEUPDATEMOIS")]
        public Int16 DateUserUpdateMois { get; set; }
        [DataMember]
        [Column(Name = "DATEUPDATEJOUR")]
        public Int16 DateUserUpdateJour { get; set; }

        [DataMember]
        [Column(Name = "TTCAREGLER")]
        public Double TTCARgler { get; set; }

        [DataMember]
        [Column(Name = "REGLE")]
        public double Regle { get; set; }

        [DataMember]
        [Column(Name = "FINEFFETPOLICEANNEE")]
        public Int16 DateFinEffetPoliceAnnee { get; set; }

        [DataMember]
        [Column(Name = "FINEFFETPOLICEMOIS")]
        public Int16 DateFinEffetPoliceMois { get; set; }

        [DataMember]
        [Column(Name = "FINEFFETPOLICEJOUR")]
        public Int16 DateFinEffetPoliceJour { get; set; }

        [DataMember]
        [Column(Name = "DUREEEFFETPOLICE")]
        public Int16 DureeEffetPolice { get; set; }

        [DataMember]
        [Column(Name = "UNITEDUREEEFFETPOLICE")]
        public String UniteDureeEffetPolice { get; set; }

        [DataMember]
        [Column(Name = "NOMBRECOCOUTRIER")]
        public Int32 NombreCocourtier { get; set; }
        



    }
}
