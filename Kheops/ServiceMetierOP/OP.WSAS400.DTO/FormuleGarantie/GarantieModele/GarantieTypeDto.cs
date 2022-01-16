using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GarantieModele
{
    public class GarantieTypeDto
    {
        [DataMember]
        public long NumeroSeq { get; set; }
        [DataMember]
        public string CodeModele { get; set; }
        [DataMember]
        public string NomModele { get; set; }
        [DataMember]
        public int Niveau { get; set; }
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public int Ordre { get; set; }
        [DataMember]
        public string Tri { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public long NumeroSeqM { get; set; }
        [DataMember]
        public long NumeroSeq1 { get; set; }
        [DataMember]
        public string Caractere { get; set; }
        [DataMember]
        public string CaractereLib { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string NatureLib { get; set; }
        [DataMember]
        public bool IsIndexee { get; set; }
        [DataMember]
        public bool SoumisCATNAT { get; set; }
        [DataMember]
        public string CodeTaxe { get; set; }
        [DataMember]
        public int GroupeAlternative { get; set; }
        [DataMember]
        public string Conditionnement { get; set; }
        [DataMember]
        public string TypePrime { get; set; }
        [DataMember]
        public string TypeControleDate { get; set; }
        [DataMember]
        public bool IsMontantRef { get; set; }
        [DataMember]
        public bool IsNatureModifiable { get; set; }
        [DataMember]
        public bool IsMasquerCP { get; set; }

        [DataMember]
        public bool IsModifiable { get; set; }

        public List<GarantieTypeLCIDto> ListLCI { get; set; }
        public List<GarantieTypeDto> ListSousGarantie { get; set; }

        public List<LienGarantieDto> ListLien { get; set; }

        public GarantieTypeDto()
        {
            this.ListSousGarantie = new List<GarantieTypeDto>();
            this.ListLCI = new List<GarantieTypeLCIDto>();
            this.ListLien = new List<LienGarantieDto>();
        }

        public GarantieTypeDto(List<GarantieTypePlatDto> garantieTypes, bool getLCI, bool getLien, bool isModifiable = false)
        {
            GarantieTypePlatDto garantieType = garantieTypes.FirstOrDefault();
            NumeroSeq = garantieType.C2seq;
            CodeModele = garantieType.C2mga;
            NomModele = garantieType.C2obe;
            Niveau = garantieType.C2niv;
            CodeGarantie = garantieType.C2gar;
            Ordre = garantieType.C2ord;
            Tri = garantieType.C2tri;
            Description = garantieType.Gades;
            NumeroSeqM = garantieType.C2sem;
            NumeroSeq1 = garantieType.C2se1;
            Caractere = garantieType.C2car;
            Nature = garantieType.C2nat;
            IsIndexee = garantieType.C2inaBool;
            SoumisCATNAT = garantieType.C2cnaBool;
            CodeTaxe = garantieType.C2tax;
            GroupeAlternative = garantieType.C2alt;
            Conditionnement = garantieType.C2scr;
            TypePrime = garantieType.C2prp;
            TypeControleDate = garantieType.C2tcd;
            IsMontantRef = garantieType.C2mrfBool;
            IsNatureModifiable = garantieType.C2ntmBool;
            IsMasquerCP = garantieType.C2masBool;
            IsModifiable = isModifiable;

            ListLCI = new List<GarantieTypeLCIDto>();
            if (getLCI)
            {
                foreach (var lci in garantieTypes)
                {
                    ListLCI.Add(new GarantieTypeLCIDto()
                    {
                        Type = lci.C4typ,
                        Base = lci.C4bas,
                        Unite = lci.C4unt,
                        Valeur = lci.C4val,
                        Modi = lci.C4maj,
                        Obl = lci.C4oblBool,
                        Alim = lci.C4ala,
                    });
                }
            }

            ListLien = new List<LienGarantieDto>();

            if (getLien)
            {
                foreach (var lien in garantieTypes)
                {
                    ListLien.Add(new LienGarantieDto()
                    {
                        Type = lien.C5typ,
                        GarantieA = lien.C5seq,
                        GarantieB = lien.C5sem,
                        NomGarantieLiee = lien.Garlieenom,
                        ModeleGarantieLiee = lien.Garlieemodele,
                        NiveauGarantieLiee = lien.Garlieeniv,
                    });
                }
            }

            this.ListSousGarantie = new List<GarantieTypeDto>();
        }
    }

    public class GarantieTypeLCIDto
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Base { get; set; }
        [DataMember]
        public string Unite { get; set; }
        [DataMember]
        public decimal Valeur { get; set; }
        [DataMember]
        public string Modi { get; set; }
        [DataMember]
        public bool Obl { get; set; }
        [DataMember]
        public string Alim { get; set; }
    }

    public class LienGarantieDto
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public long GarantieA { get; set; }
        [DataMember]
        public long GarantieB { get; set; }
        [DataMember]
        public string NomGarantieLiee { get; set; }
        [DataMember]
        public string ModeleGarantieLiee { get; set; }
        [DataMember]
        public string NiveauGarantieLiee { get; set; }
    }
}
