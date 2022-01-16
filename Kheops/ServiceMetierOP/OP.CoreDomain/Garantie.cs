using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Garantie
    {
        public string GuidId { get; set; }
        public string Type { get; set; }
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string GuidOption { get; set; }
        public string CodeGarantie { get; set; }
        public int SequenceUnique { get; set; }
        public int Niveau { get; set; }
        public int SequenceMaitre { get; set; }
        public int SequenceNiv1 { get; set; }
        public string Tri { get; set; }
        public int NumPresentation { get; set; }
        public string GarantieAjoutee { get; set; }
        public string CaractereGarantie { get; set; }
        public string NatureParametre { get; set; }
        public string Nature { get; set; }
        public string LienApplication { get; set; }
        public string DefinitionGarantie { get; set; }
        public string LienInfosSpec { get; set; }
        public DateTime? DebutGarantie { get; set; }
        public DateTime? FinGarantie { get; set; }
        public int DureeValeur { get; set; }
        public string DureeUnitee { get; set; }
        public string TypeApplication { get; set; }
        public string TypeEmission { get; set; }
        public string InMontantReference { get; set; }
        public string InCATNAT { get; set; }
        public string Indexee { get; set; }
        public string CodeTaxe { get; set; }
        public string RepartionTaxe { get; set; }
        public int AvenantCreation { get; set; }
        public string User { get; set; }
        public DateTime? CreateDate { get; set; }
        public int AvenantModification { get; set; }
        public string AssietteValeurOrigine { get; set; }
        public string AssietteValeurActualisee { get; set; }
        public string AssietteValeurTravail { get; set; }
        public string AssietteUnite { get; set; }
        public string AssietteBase { get; set; }
        public string AssietteModifiable { get; set; }
        public string AssietteObligatoire { get; set; }
        public string InventaireSpecPossible { get; set; }
        public string Inventaire { get; set; }
        public DateTime? DateDebStandard { get; set; }
        public DateTime? DateFinStandard { get; set; }
    }
}
