using Albingia.Kheops.OP.Domain.Extension;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public class AffaireId : Value
    {
        private string _codeAffaire;
        public string CodeAffaire
        {
            get {
                return _codeAffaire;
            }
            set {
                this._codeAffaire = value?.PadLeft(9, ' ');
            }
        }
        public int NumeroAliment { get; set; }
        public AffaireType TypeAffaire { get; set; }
        public int? NumeroAvenant { get; set; }

        public AffaireId()
        {

        }

        public AffaireId(AffaireType type, string code, int aliment, int avenant = 0, bool isHisto = false)
        {
            TypeAffaire = type;
            CodeAffaire = code;
            NumeroAliment = aliment;
            NumeroAvenant = avenant;
            IsHisto = isHisto;
        }

        /// <summary>
        /// Gets or sets a value indicating whether current data are from histo table
        /// </summary>
        public bool IsHisto { get; set; }

        public string TypeTraitement { get; set; }

        public AffaireId MakeCopy(bool? forcedHistoValue = null) {
            return new AffaireId {
                CodeAffaire = CodeAffaire,
                IsHisto = forcedHistoValue.GetValueOrDefault(IsHisto),
                NumeroAliment = NumeroAliment,
                TypeAffaire = TypeAffaire,
                NumeroAvenant = NumeroAvenant,
                TypeTraitement = TypeTraitement
            };
        }
        public string AsAffaireKey()
        {
            var ext = IsHisto ? $"_{NumeroAvenant}" : "";
            return $"{TypeAffaire.AsCode()}_{CodeAffaire}_{NumeroAliment}{ext}";
        }
    }
}