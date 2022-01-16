using EmitMapper;
using OP.WSAS400.DTO.GestionDocumentContrat;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionDocumentContrat
{
    public class ModeleLigneDocument
    {
        public Int64 GuidId { get; set; }
        public int DateEnvoi { get; set; }
        public DateTime? DDateEnvoi { get; set; }
        public string LibelleDocument { get; set; }
        public string NomDocument { get; set; }
        public string TypeDocumentCode { get; set; }
        public string TypeDocumentLib { get; set; }
        public string TypeDiffusion { get; set; }
        public string TypeDestinataire { get; set; }
        public string TypeEdition { get; set; }
        public int CodeDestinataire { get; set; }
        public string NomDestinataire { get; set; }
        public string Lot { get; set; }
        public int NumAvenant { get; set; }
        public int NbPieceJointe { get; set; }
        public string Utilisateur { get; set; }

        #region infobulle
        public string NomFichier { get; set; }
        public string Service { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public string Tampon { get; set; }
        public string TypeOrigine { get; set; }
        public string CheminFichier { get; set; }
        #endregion

        public static explicit operator ModeleLigneDocument(LigneDocumentDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneDocumentDto, ModeleLigneDocument>().Map(data);
        }
    }
}