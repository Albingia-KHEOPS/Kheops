using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocumentsPlatDto
    {
        [Column(Name = "CURAVN")]
        [DataMember]
        public Int64 CurAvn { get; set; }
        [Column(Name = "LOTID")]
        [DataMember]
        public Int64 LotId { get; set; }
        [Column(Name = "LOTLIB")]
        [DataMember]
        public string LotLibelle { get; set; }
        [Column(Name = "CODEUTILISATEUR")]
        [DataMember]
        public string CodeUtilisateur { get; set; }
        [Column(Name = "NOMUTILISATEUR")]
        [DataMember]
        public string NomUtilisateur { get; set; }
        [Column(Name = "PRENOMUTILISATEUR")]
        [DataMember]
        public string PrenomUtilisateur { get; set; }
        [Column(Name = "UNITESERVICE")]
        [DataMember]
        public string UniteService { get; set; }
        [Column(Name = "TYPEAFFAIRE")]
        [DataMember]
        public string TypeAffaire { get; set; }
        [Column(Name = "CODEOFFRE")]
        [DataMember]
        public string CodeOffre { get; set; }
        [Column(Name = "VERSION")]
        [DataMember]
        public Int32 Version { get; set; }
        [Column(Name = "LOTDETAILID")]
        [DataMember]
        public Int64 LotDetailId { get; set; }
        [Column(Name = "CODESITUATION")]
        [DataMember]
        public string CodeSituation { get; set; }
        [Column(Name = "DATESITUATION")]
        [DataMember]
        public Int32 DateSituation { get; set; }
        [Column(Name = "HEURESITUATION")]
        [DataMember]
        public Int32 HeureSituation { get; set; }
        [Column(Name = "UTILISATEURSITUATION")]
        [DataMember]
        public string UtilisateurSituation { get; set; }
        [Column(Name = "ACTEGESTION")]
        [DataMember]
        public string ActeGestion { get; set; }
        [Column(Name = "ACTEGESTIONLIB")]
        [DataMember]
        public string ActeGestionLib { get; set; }
        [Column(Name = "NUMINTERNE")]
        [DataMember]
        public Int32 NumInterne { get; set; }
        [Column(Name = "NUMEXTERNE")]
        [DataMember]
        public Int64 NumExterne { get; set; }
        [Column(Name = "NOMDOC")]
        [DataMember]
        public string NomDoc { get; set; }
        [Column(Name = "CHEMINDOC")]
        [DataMember]
        public string CheminDoc { get; set; }
        [Column(Name = "DOCID")]
        [DataMember]
        public Int64 DocId { get; set; }
        [Column(Name = "TYPEDOC")]
        [DataMember]
        public string TypeDoc { get; set; }
        [Column(Name = "EMPTYLINE")]
        [DataMember]
        public Double EmptyLine { get; set; }
        [Column(Name = "CODEDOC")]
        [DataMember]
        public string CodeDoc { get; set; }
        [Column(Name = "SERVDOC")]
        [DataMember]
        public string ServiceDoc { get; set; }
        [Column(Name = "CREATEDOC")]
        [DataMember]
        public Int64 CreateDoc { get; set; }
        [Column(Name = "MODIFDOC")]
        [DataMember]
        public Int64 UpdateDoc { get; set; }
        [Column(Name = "TYPEGENDOC")]
        [DataMember]
        public string TypeGenDoc { get; set; }
        [Column(Name = "TYPOEDITDOC")]
        [DataMember]
        public string TypoEditDoc { get; set; }
        [Column(Name = "TYPEDOCLIB")]
        [DataMember]
        public string TypeDocLib { get; set; }
        [Column(Name = "TYPEDESTINATAIRE")]
        [DataMember]
        public string TypeDestinataire { get; set; }
        [Column(Name = "TYPEINTERVENANT")]
        [DataMember]
        public string TypeIntervenant { get; set; }
        [Column(Name = "CODEDESTINATAIRE")]
        [DataMember]
        public Int32 CodeDestinataire { get; set; }
        [Column(Name = "NOMDESTINATAIRE")]
        [DataMember]
        public string NomDestinataire { get; set; }
        [Column(Name = "CODEINTERLOCUTEUR")]
        [DataMember]
        public Int32 CodeInterlocuteur { get; set; }
        [Column(Name = "NOMINTERLOCUTEUR")]
        [DataMember]
        public string NomInterlocuteur { get; set; }
        [Column(Name = "CODEDIFFUSION")]
        [DataMember]
        public string CodeDiffusion { get; set; }
        [Column(Name = "LIBDIFFUSION")]
        [DataMember]
        public string LibDiffusion { get; set; }
        [Column(Name = "DOCEXT")]
        [DataMember]
        public Int32 DocExterne { get; set; }
    }
}
