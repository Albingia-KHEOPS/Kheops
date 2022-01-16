using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Offres.Risque.Inventaire
{
    [DataContract]
    public class InventaireGridRowDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "DESIGNATION")]
        public string Designation { get; set; }
        [DataMember]
        [Column(Name = "LIENADRESSE")]
        public int LienADH { get; set; }
        [DataMember]
        [Column(Name = "LIEU")]
        public string Lieu { get; set; }
        [DataMember]
        [Column(Name = "CODEPOSTAL")]
        public Int32 CodePostal { get; set; }
        [DataMember]
        [Column(Name = "VILLE")]
        public string Ville { get; set; }
        [Column(Name = "DATEDEB")]
        public int DateDebDB { get; set; }
        [Column(Name = "HEUREDEB")]
        public int HeureDebDB { get; set; }
        [Column(Name = "DATEFIN")]
        public int DateFinDB { get; set; }
        [Column(Name = "HEUREFIN")]
        public int HeureFinDB { get; set; }
        [DataMember]
        [Column(Name = "NBEVEN")]
        public int NbEvenement { get; set; }
        [DataMember]
        [Column(Name = "NBPERS")]
        public int NbPers { get; set; }
        [DataMember]
        [Column(Name = "MONTANT")]
        public double Montant { get; set; }
        [DataMember]
        [Column(Name = "NOM")]
        public string Nom { get; set; }
        [DataMember]
        [Column(Name = "PRENOM")]
        public string Prenom { get; set; }
        [DataMember]
        [Column(Name = "FONCTION")]
        public string Fonction { get; set; }
        [Column(Name = "DATENAIS")]
        public int DateNaissanceDB { get; set; }
        [DataMember]
        [Column(Name = "CAPDECES")]
        public Int64 CapitalDeces { get; set; }
        [DataMember]
        [Column(Name = "CAPIP")]
        public Int64 CapitalIP { get; set; }
        [Column(Name = "ACCIDENT")]
        public string AccidentSeulDB { get; set; }
        [Column(Name = "AVTPROD")]
        public string AvantProdDB { get; set; }
        [DataMember]
        [Column(Name = "NUMSERIE")]
        public string NumSerie { get; set; }
        [DataMember]
        public Int64 InventaireType { get; set; }
        [DataMember]
        public DateTime? DateDeb { get; set; }
        [DataMember]
        public TimeSpan? HeureDeb { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public TimeSpan? HeureFin { get; set; }
        [DataMember]
        public DateTime? DateNaissance { get; set; }
        [DataMember]
        public bool AccidentSeul { get; set; }
        [DataMember]
        public bool AvantProd { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "NATLIEU")]
        public string NatureLieu { get; set; }
        [DataMember]
        [Column(Name = "DESCNATLIEU")]
        public string DescNatLieu { get; set; }
        [DataMember]
        [Column(Name = "CODEMAT")]
        public string CodeMateriel { get; set; }
        [DataMember]
        [Column(Name = "DESCMAT")]
        public string DescMat { get; set; }
        [DataMember]
        [Column(Name = "CODEEXTENSION")]
        public string CodeExtension { get; set; }
        [DataMember]
        [Column(Name = "DESCEXTENSION")]
        public string DescExtension { get; set; }
        [DataMember]
        [Column(Name = "FRANCHISE")]
        public string Franchise { get; set; }
        [DataMember]
        public string AnneeNaissance { get; set; }
        [DataMember]
        public List<ParametreDto> NaturesLieu { get; set; }
        [DataMember]
        public List<ParametreDto> CodesMat { get; set; }
        [DataMember]
        public List<ParametreDto> CodesExtension { get; set; }
        [DataMember]
        [Column(Name = "CODEQUALITE")]
        public string CodeQualite { get; set; }
        [DataMember]
        [Column(Name = "DESCQUALITE")]
        public string DescQualite { get; set; }
        [DataMember]
        public List<ParametreDto> CodesQualite { get; set; }
        [DataMember]
        [Column(Name = "CODERENONCE")]
        public string CodeRenonce { get; set; }
        [DataMember]
        [Column(Name = "DESCRENONCE")]
        public string DescRenonce { get; set; }
        [DataMember]
        public List<ParametreDto> CodesRenonce { get; set; }
        [DataMember]
        [Column(Name = "CODERSQLOC")]
        public string CodeRsqLoc { get; set; }
        [DataMember]
        [Column(Name = "DESCRSQLOC")]
        public string DescRsqLoc { get; set; }
        [DataMember]
        public List<ParametreDto> CodesRsqLoc { get; set; }
        [DataMember]
        [Column(Name = "MNT2")]
        public double Mnt2 { get; set; }
        [DataMember]
        [Column(Name = "CONTENU")]
        public double Contenu { get; set; }
        [DataMember]
        [Column(Name = "MATBUR")]
        public double MatBur { get; set; }
         [DataMember]
        [Column(Name = "NATUREMARCHANDISE")]
        public long NatureMarchandise { get; set; }
        [DataMember]
        [Column(Name = "DESCNATUREMARCHANDISE")]
        public string DescNatureMarchandise { get; set; }

        [DataMember]
        [Column(Name = "MODELE")]
        public string Modele { get; set; }
        [DataMember]
        [Column(Name = "MARQUE")]
        public string Marque { get; set; }
        [DataMember]
        [Column(Name = "IMMATRICULATION")]
        public string Immatriculation { get; set; }
       

        [DataMember]
        [Column(Name = "DEPART")]
        public long Depart { get; set; }
        [DataMember]
        [Column(Name = "DESCDEPART")]
        public string DescDepart { get; set; }


        [DataMember]
        [Column(Name = "DESTINATION")]
        public long Destination { get; set; }
        [DataMember]
        [Column(Name = "DESCDESTINATION")]
        public string DescDestination { get; set; }

        
        [DataMember]
        [Column(Name = "MODALITE")]
        public long Modalite { get; set; }
        [DataMember]
        [Column(Name = "DESCMODALITE")]
        public string DescModalite { get; set; }


        [DataMember]
        [Column(Name = "PAYS")]
        public string Pays { get; set; }
        [DataMember]
        [Column(Name = "DESCPAYS")]
        public string DescPays { get; set; }
        [DataMember]
        public List<ParametreDto> ListPays { get; set; }

        [DataMember]
        public Adresses.AdressePlatDto Adresse { get; set; }
    }
}
