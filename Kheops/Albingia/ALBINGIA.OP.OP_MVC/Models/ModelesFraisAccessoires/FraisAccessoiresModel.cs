using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.FraisAccessoires;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFraisAccessoires
{
    public class FraisAccessoiresModel
    {
        /// <summary>
        /// Branche
        /// </summary>
        public string Branche { get; set; }

        public string BrancheLabel { get; set; }

        /// <summary>
        /// Sous branche
        /// </summary>
        public string SousBranche { get; set; }

        public string SousBrancheLabel { get; set; }

        /// <summary>
        /// Catégorie
        /// </summary>
        public string Categorie { get; set; }

        /// <summary>
        /// Année
        /// </summary>
        public int? Annee { get; set; }

        /// <summary>
        /// Montant
        /// </summary>
        public int? Montant { get; set; }

        /// <summary>
        /// Frais accessoires en dessous du montant
        /// </summary>
        public int? FRaiSACCMIN { get; set; }

        /// <summary>
        ///Frais accessoires au dessu du montant
        /// </summary>
        public int? FRaiSACCMAX { get; set; }

        public string Id {
            get {
                return string.Format("{1}{0}{2}{0}{3}{0}{4}", ALBINGIA.OP.OP_MVC.MvcApplication.SPLIT_CONST_HTML,
                    Branche, SousBranche, Categorie, Annee);
            }
        }

        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }


        public List<AlbSelectListItem> Branches { get; set; }
        public List<AlbSelectListItem> SousBranches { get; set; }
        public List<AlbSelectListItem> Categories { get; set; }


        public static explicit operator FraisAccessoiresModel(FraisAccessoiresEngDto dto)
        {
            return new FraisAccessoiresModel {
                Branche = dto.Branche,
                SousBranche = dto.SousBranche,
                Categorie = dto.Categorie,
                Annee = dto.Annee,
                Montant = dto.Montant,
                FRaiSACCMIN = dto.FRaiSACCMIN,
                FRaiSACCMAX = dto.FRaiSACCMAX
            };
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            FraisAccessoiresModel m = (FraisAccessoiresModel)obj;
            return (Branche == m.Branche) && (SousBranche == m.SousBranche) && (Categorie == m.Categorie) && (Annee == m.Annee);
        }

        public override int GetHashCode()
        {
            return ((((Branche.GetHashCode() * 23 + SousBranche.GetHashCode()) * 23 + Categorie.GetHashCode()) * 23) + Annee.GetHashCode()) + 17;
        }

        public FraisAccessoiresEngDto ToDto()
        {
            return new FraisAccessoiresEngDto {
                Branche = this.Branche,
                SousBranche = this.SousBranche,
                Categorie = this.Categorie,
                Annee = this.Annee != null ? (int)this.Annee : 0,
                Montant = this.Montant != null ? (int)this.Montant : 0,
                FRaiSACCMIN = this.FRaiSACCMIN != null ? (int)this.FRaiSACCMIN : 0,
                FRaiSACCMAX = this.FRaiSACCMAX != null ? (int)this.FRaiSACCMAX : 0,
            };
        }
    }
}