using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class GarantieDetailDto
    {
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }
        [Column(Name = "TYPE")]
        public String Type { get; set; }
        [Column(Name = "CODEOFFRE")]
        public String CodeOffre { get; set; }
        [Column(Name = "VERSION")]
        public Int32 Version { get; set; }
        [Column(Name = "CODEFORMULE")]
        public Int32 CodeFormule { get; set; }
        [Column(Name = "CODEOPTION")]
        public Int32 CodeOption { get; set; }
        [Column(Name = "GUIDOPTION")]
        public Int64 GuidOption { get; set; }
        [Column(Name = "GARANTIE")]
        public String Garantie { get; set; }
        [Column(Name = "SEQUENCEUNIQUE")]
        public Int64 SequenceUnique { get; set; }
        [Column(Name = "NIVEAU")]
        public Int32 Niveau { get; set; }
        [Column(Name = "SEQUENCEMAITRE")]
        public Int64 SequenceMaitre { get; set; }
        [Column(Name = "SEQUENCENIV1")]
        public Int64 SequenceNiv1 { get; set; }
        [Column(Name = "TRI")]
        public String Tri { get; set; }
        [Column(Name = "NUMPRESENTATION")]
        public Double NumPresentation { get; set; }
        [Column(Name = "GARANTIEAJOUTEE")]
        public String GarantieAjoutee { get; set; }
        [Column(Name = "CARACTEREGARANTIE")]
        public String CaractereGarantie { get; set; }
        [Column(Name = "NATUREPARAMETRE")]
        public String NatureParametre { get; set; }
        [Column(Name = "NATURE")]
        public String Nature { get; set; }
        [Column(Name = "LIENAPPLICATION")]
        public Int64 LienApplication { get; set; }
        [Column(Name = "DEFGARANTIE")]
        public String DefGarantie { get; set; }
        [Column(Name = "LIENINFOSSPEC")]
        public Int64 LienInfosSpec { get; set; }
        [Column(Name = "DEBUTGARANTIE")]
        public Int64 DebutGarantie { get; set; }
        [Column(Name = "HEUREDEB")]
        public Int32 HeureDeb { get; set; }
        [Column(Name = "FINGARANTIE")]
        public Int64 FinGarantie { get; set; }
        [Column(Name = "HEUREFIN")]
        public Int32 HeureFin { get; set; }
        [Column(Name = "DUREEVALEUR")]
        public Int32 DureeValeur { get; set; }
        [Column(Name = "DUREEUNITE")]
        public String DureeUnite { get; set; }
        [Column(Name = "TYPEAPPLICATION")]
        public String TypeApplication { get; set; }
        [Column(Name = "TYPEEMISSION")]
        public String TypeEmission { get; set; }
        [Column(Name = "MONTANTREFERENCE")]
        public String MontantReference { get; set; }
        [Column(Name = "CATNAT")]
        public String CATNAT { get; set; }
        [Column(Name = "INDEXEE")]
        public String Indexee { get; set; }
        [Column(Name = "CODETAXE")]
        public String CodeTaxe { get; set; }
        [Column(Name = "REPARTITIONTAXE")]
        public Single RepartitionTaxe { get; set; }
        [Column(Name = "AVENANTCREATION")]
        public Int32 AvenantCreation { get; set; }
        [Column(Name = "CREATEUSER")]
        public String CreateUser { get; set; }
        [Column(Name = "CREATEDATE")]
        public Int64 CreateDate { get; set; }
        [Column(Name = "AVENANTMODIFICATION")]
        public Int32 AvenantModification { get; set; }
        [Column(Name = "ASSIETTEVALEURORIG")]
        public Double AssietteValeurOrig { get; set; }
        [Column(Name = "ASSIETTEVALEURACTU")]
        public Double AssietteValeurActu { get; set; }
        [Column(Name = "ASSIETTEVALEURTRAVAIL")]
        public Double AssietteValeurTravail { get; set; }
        [Column(Name = "ASSIETTEUNITE")]
        public String AssietteUnite { get; set; }
        [Column(Name = "ASSIETTEBASE")]
        public String AssietteBase { get; set; }
        [Column(Name = "ASSIETTEMODIFIABLE")]
        public String AssietteModifiable { get; set; }
        [Column(Name = "ASSIETTEOBLIGATOIRE")]
        public String AssietteObligatoire { get; set; }
        [Column(Name = "INVENTAIRESPECIFIQUE")]
        public String InventaireSpecifique { get; set; }
        [Column(Name = "INVENTAIRE")]
        public Int64 Inventaire { get; set; }
        [Column(Name = "DATESTDDEB")]
        public Int64 DateStdDeb { get; set; }
        [Column(Name = "HEURESTDDEB")]
        public Int32 HeureStdDeb { get; set; }
        [Column(Name = "DATESTDFIN")]
        public Int64 DateStdFin { get; set; }
        [Column(Name = "HEURESTDFIN")]
        public Int32 HeureStdFin { get; set; }
    }
}
