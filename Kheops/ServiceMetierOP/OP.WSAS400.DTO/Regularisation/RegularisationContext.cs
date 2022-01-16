using ALBINGIA.Framework.Common.Business;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Contrats;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]       
    public class RegularisationContext : IKeyLocker
    {
        private bool hasMultiRC;

        [DataMember]
        public bool IsSetReguleAlreadyCalled { get; set; }

        /// <summary>
        /// Gets or sets a string array containing key values in the context
        /// </summary>
        [DataMember]
        public string[] KeyValues { get; set; }

        [DataMember]
        public IdContratDto IdContrat { get; set; }

        [DataMember]
        public string RegimeTaxe { get; set; }

        /// <summary>
        /// Gets or set the identifier of current Regularisation
        /// </summary>
        [DataMember]
        public long RgId { get; set; }

        [DataMember]
        public long RsqId { get; set; }

        [DataMember]
        public long GrId { get; set; }

        [DataMember]
        public int CodeFormule { get; set; }

        [DataMember]
        public long RgGrId { get; set; }

        [DataMember]
        public string GrLabel { get; set; }

        [DataMember]
        public long LotId { get; set; }

        [DataMember]
        public bool IsMultiRC { get; set; }

        [DataMember]
        public bool HasMultiRC {
            get => IsMultiRC || this.hasMultiRC;
            set => this.hasMultiRC = value;
        }

        [DataMember]
        public RegularisationMode Mode { get; set; }

        [DataMember]
        public RegularisationScope Scope { get; set; }

        [DataMember]
        public RegularisationStep Step { get; set; }

        [DataMember]
        public char RgHisto { get; set; }

        [DataMember]
        public string Type { get; set; }

        [IgnoreDataMember]
        public bool IsMultiRisques
        {
            get { return NbRisques > 1; }
        }

        [DataMember]
        public bool IsReadOnlyMode { get; set; }

        [IgnoreDataMember]
        public AccessMode AccessMode
        {
            get { return IsReadOnlyMode ? AccessMode.CONSULT : RgId == 0 ? AccessMode.CREATE : AccessMode.UPDATE; }
        }

        [DataMember]
        public string TypeAvt { get; set; }

        [DataMember]
        public bool IsMultiGaranties { get; set; }

        [DataMember]
        public bool IsRisquesHomogenes { get; set; }

        [DataMember]
        public int NbRisques { get; set; }

        [DataMember]
        public int NbGaranties { get; set; }

        [DataMember]
        public bool IsSimplifiedRegule { get; set; }

        [DataMember]
        public RegularisationSimplifieeDto SimpleRegule { get; set; }

        [DataMember]
        public bool IsSaveAndQuit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether at leat one item (risque/garantie) has been validated
        /// </summary>
        [DataMember]
        public bool ValidationDone { get; set; }

        [DataMember]
        public bool ComputeDone { get; set; }

        [DataMember]
        public AlbErrorDto Error { get; set; }

        [DataMember]
        public string Souscripteur { get; set; }

        [DataMember]
        public string Gestionnaire { get; set; }

        [DataMember]
        public AvenantRegularisationDto ModeleAvtRegul { get; set; }

        [DataMember]
        public string DateDebut { get; set; }

        [DataMember]
        public string DateFin { get; set; }

        [DataMember]
        public int Exercice { get; set; }

        [DataMember]
        public string User { get; set; }

        [DataMember]
        public string CodeICT { get; set; }

        [DataMember]
        public string CodeICC { get; set; }

        [DataMember]
        public string TauxCom { get; set; }

        [DataMember]
        public string TauxComCATNAT { get; set; }

        [DataMember]
        public string CodeEnc { get; set; }

        [DataMember]
        public List<RegularisationStateDto> States { get; set; }

        [DataMember]
        public List<RegulMatriceDto> Matrix { get; set; }

        [DataMember]
        public string ReportChargesNouveau { get; set; }

        public RegularisationContext Refresh()
        {
            if (States?.Any() == true && Matrix?.Any() == true)
            {
                if (RsqId > 0)
                {
                    ValidationDone = Matrix?.FirstOrDefault(item => item.RisqueId == RsqId && item.RisqueStatus == "V") != null;
                }
                else
                {
                    ValidationDone = States.Any(st => (st.Type == "GUR" || st.Type == "GUG") && st.NbValidated > 0);
                }
            }
            else
            {
                ValidationDone = false;
            }

            if (Matrix?.Any() == true)
            {
                NbRisques = Matrix.Select(m => m.RisqueId).Distinct().Count();
                NbGaranties = Matrix.Select(m => m.GarId).Distinct().Count();
                RsqId = NbRisques == 1 ? Matrix.Select(m => m.RisqueId).First() : RsqId;
                GrId = NbGaranties == 1 ? Matrix.Select(m => m.GarId).First() : GrId;
                GrLabel = NbGaranties == 1 ? Matrix.Select(m => m.GarLabel).First() : GrLabel;
                if (!ValidationDone && Mode != RegularisationMode.Standard && NbRisques == 1)
                {
                    Scope = RegularisationScope.Contrat;
                }
            }
            else
            {
                NbRisques = 0;
                NbGaranties = 0;
                RsqId = 0;
                GrId = 0;
                GrLabel = string.Empty;
            }

            return this;
        }
        public string LastValidNumAvt { get; set; }

        public long LastValidRgId { get; set; }

        public string LastNumAvt { get; set; }

        public bool CanSimplifyStepFlow { get; set; }
    }
}
