using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    public class CabinetCourtagePlatDto
    {    
        [Column(Name = "CODECAB")]
        public int Code { get; set; }     
        [Column(Name = "NOMCAB")]
        public string NomCabinet { get; set; }
        [Column(Name = "NOM2")]
        public string NomSecondaire { get; set; }    
        [Column(Name = "TYPECAB")]
        public string Type { get; set; }    
        [Column(Name = "FINVANNEE")]
        public Int32 FinValiditeAnee { get; set; }    
        [Column(Name = "FINVMOIS")]
        public Int32 FinValiditeMois { get; set; }      
        [Column(Name = "FINVJOUR")]
        public Int32 FinValiditeJour { get; set; }      
        [Column(Name = "CODEDELEGATION")]
        public string CodeDelegation { get; set; }      
        [Column(Name = "NOMDELEGATION")]
        public string NomDelegation { get; set; }       
        [Column(Name = "NUMEROCHRONO")]
        public Int32 NumeroChrono { get; set; }
        public string MatriculeHexavia { get; set; }      
        [Column(Name = "BATIMENT")]
        public string Batiment { get; set; }
        [Column(Name = "NUMEROVOIE")]
        public Int16 NumeroVoie { get; set; }
        [Column(Name = "EXTENSIONVOIE")]
        public string ExtensionVoie { get; set; }
        [Column(Name = "NOMVOIE")]
        public string NomVoie { get; set; }
        [Column(Name = "BOITEPOSTALE")]
        public string BoitePostale { get; set; }        
        public string CodeInsee { get; set; }        
        [Column(Name = "DEPARTEMENT")]
        public string Departement { get; set; }        
        [Column(Name = "NOMVILLE")]
        public string NomVille { get; set; }        
        [Column(Name = "CODEPOSTAL")]
        public Int16 CodePostal { get; set; }
        [Column(Name = "NOMCEDEX")]
        public string NomCedex { get; set; }    
        [Column(Name = "CODEPOSTALCEDEX")]
        public Int16 CodePostalCedex { get; set; }        
        [Column(Name = "TYPECEDEX")]
        public string TypeCedex { get; set; }
        [Column(Name = "CODEPAYS")]
        public string CodePays { get; set; }        
        [Column(Name = "NOMPAYS")]
        public string NomPays { get; set; }        
        [Column(Name = "DEMARCHECOM")]
        public string DemarcheCom { get; set; }        
        [Column(Name = "IDINTER")]
        public int IdInterlocuteur { get; set; }
        [Column(Name = "NOMINTER")]
        public string NomInterlocuteur { get; set; }        
        [Column(Name = "ESTVALIDEINTER")]
        public string EstValideInterlocuteur { get; set; }        
        [Column(Name = "CODEPOSTALYC")]
        public string CodePostalYc { get; set; }      
              
        [Column(Name = "TELEPHONEBUREAU")]
        public string TelephoneBureau { get; set; }
        [Column(Name = "EMAILBUREAU")]
        public string EmailBureau { get; set; }
        [Column(Name = "CODEENCAISSEMENT")]
        public string CodeEncaissement { get; set; }
        [Column(Name = "LIBENCAISSEMENT")]
        public string LibEncaissement { get; set; }
        [Column(Name = "FONCTIONINTERLOCUTEUR")]
        public string FonctionInterlocuteur { get; set; }
        [Column(Name = "TELEPHONEINTERLOCUTEUR")]
        public string TelephoneInterlocuteur { get; set; }
        [Column(Name = "EMAILINTERLOCUTEUR")]
        public string EmailInterlocuteur { get; set; }
        [Column(Name = "INSPECTEUR")]
        public string Inspecteur { get; set; }
    }
}
