using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Validation
{
    [DataContract]
    public class ValidationDto : Validation_Base
    {
        [Column(Name = "NBLGN")]
        public int NbLigne { get; set; }
        [Column(Name ="PBTTR")]
        public string TypeTraitement { get; set; }
        [DataMember]
        public string OffreComplete { get; set; }
        [DataMember]
        public List<ParametreDto> OffreCompletes { get; set; }

        [Column(Name = "MOTIF")]
        [DataMember]
        public string Motif { get; set; }
        [DataMember]
        public List<ParametreDto> Motifs { get; set; }
        [DataMember]
        public string ValidationRequise { get; set; }
        [DataMember]
        public List<ValidationCritereDto> Criteres { get; set; }
        [DataMember]
        public ValidationEditionDto Docs { get; set; }
        #region champs supplémentaires offre

        [Column(Name = "DATESTATISTIQUE")]
        [DataMember]
        public int DateStatistique { get; set; }

        [Column(Name = "CODEOBSERVATION")]
        [DataMember]
        public Int64 CodeObservation { get; set; }

        [Column(Name = "OBSERVATION")]
        [DataMember]
        public string Observation { get; set; }

        [Column(Name = "DELEGATIONOFFRE")]
        [DataMember]
        public string DelegationOffre { get; set; }

        [Column(Name = "SECTEUROFFRE")]
        [DataMember]
        public string SecteurOffre { get; set; }

        [Column(Name = "MONTANTREFERENCE")]
        [DataMember]
        public double MontantReference { get; set; }

        [Column(Name = "VALIDABLE")]
        [DataMember]
        public string Validable { get; set; }

        #endregion

        #region champs supplémentaires contrat

        [Column(Name = "MONTANTREFCALC")]
        [DataMember]
        public double MontantReferenceCalcule { get; set; }

        [Column(Name = "MONTANTREFFORCE")]
        [DataMember]
        public double MontantReferenceForce { get; set; }

        [Column(Name = "MONTANTREFACQUIS")]
        [DataMember]
        public double MontantReferenceAcquis { get; set; }

        [Column(Name = "DELEGATIONAPP")]
        [DataMember]
        public string DelegationApporteur { get; set; }

        [Column(Name = "SECTEURAPP")]
        [DataMember]
        public string SecteurApporteur { get; set; }

        [Column(Name = "DELEGATIONGEST")]
        [DataMember]
        public string DelegationGestionnaire { get; set; }

        [Column(Name = "SECTEURGEST")]
        [DataMember]
        public string SecteurGestionnaire { get; set; }

        [Column(Name = "COURTIERAPP")]
        [DataMember]
        public int CourtierApporteur { get; set; }

        [Column(Name="VALIDAPP")]
        public int ValidApp { get; set; }

        [Column(Name = "COURTIERGEST")]
        [DataMember]
        public int CourtierGestionnaire { get; set; }

        [Column(Name = "VALIDGEST")]
        public int ValidGest { get; set; }
        
        [Column(Name = "ETAT")]
        [DataMember]
        public string Etat { get; set; }

        [Column(Name = "ISDOCEDIT")]
        [DataMember]
        public string IsDocEdit { get; set; }

        [DataMember]
        public bool IsControleOk { get; set; }
        #endregion

        [Column(Name="ETATREGULE")]
        [DataMember]
        public string EtatRegule { get; set; }

        [Column(Name="ISDOCGENER")]
        [DataMember]
        public string IsDocGener { get; set; }

        [Column(Name="DATEACCORD")]
        [DataMember]
        public Int64 DateAccordInt { get; set; }
    }
}
