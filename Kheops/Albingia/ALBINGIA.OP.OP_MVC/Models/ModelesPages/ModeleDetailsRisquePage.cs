using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleDetailsRisquePage : MetaModelsBase
    {
        #region Avenant

        public DateTime? DateEffetAvenant { get; set; }
        public bool IsTraceAvnExist { get; set; }
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
  
        public DateTime? DateEffetAvenantModificationLocaleCopy { get; set; }
        public bool IsModeAvenant { get; set; }
        public int AvnCreationRsq { get; set; } //Numéro d'avenant dans lequel a été créé le risque courant
        public bool IsIndexe { get; set; }
        public bool IsAlertePeriode { get; set; }
        public bool IsAvenantModificationLocale { get; set; }

        #endregion

        public string txtParamRedirect { get; set; }
        public string Code { get; set; }
        public string Branche { get; set; }
        public string MonoObjet { get; set; }
        public int ChronoDesi { get; set; }
        public string RisqueObj { get; set; }       //Savoir s'il y a un objet renseigné ou pas.
        public string CibleMonoObjet { get; set; }  //Connaitre la cible du mono objet
        public string CodeApeMonoObjet { get; set; }
        public string CodeTreMonoObjet { get; set; }
        public string CodeClasseMonoObjet { get; set; }
        public string TerritorialiteMonoObjet { get; set; }
        public string Nomenclature1MonoObjet { get; set; }
        //public string AddObjet { get; set; }        //Savoir s'il on ajoute un objet lors du submit de la page  
        public string OpenObjet { get; set; }       //Identifiant de l'objet à ouvrir
        //public string OpenISRequest { get; set; }   //Savoir si on ouvre les IS risque
        public string RedirectRisque { get; set; } //Savoir la redirection à partir du risque (IS, add obj ou open obj)
        public Int64 CountRsq { get; set; }           //Connaitre le nombre de risque de l'offre       
        public ModeleContactAdresse ContactAdresse { get; set; }

        public ModeleInformationsGeneralesRisque InformationsGenerales { get; set; }
        public DateTime? DateSortieGarantieCopy { get; set; }
        public ModeleObjetsRisque ObjetsRisqueMetaData { get; set; }

        [Display(Name = "Valeur")]
        public Int64? ValeurObjets { get; set; }

        [Display(Name = "Unite")]
        public string UniteObjets { get; set; }

        [Display(Name = "Type")]
        public string TypeObjets { get; set; }
        public List<AlbSelectListItem> TypesLstObjets { get; set; }

        [Display(Name = "HT/TTC")]
        public string ValeurHTObjets { get; set; }

        [Display(Name = "Cout/m²")]
        public Int64? CoutM2Objets { get; set; }

        public string EnsembleType { get; set; }

        [Display(Name = "Reporter la valeur des objets sur le risque")]
        public bool ActiverReport { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [Display(Name = "Valeur")]
        //public string Valeur { get; set; }
        public Int64? Valeur { get; set; }

        [Display(Name = "Unité")]
        public String Unite { get; set; }
        /// <summary>
        /// Gets or sets the unites.
        /// </summary>
        /// <value>
        /// The unites.
        /// </value>
        public List<AlbSelectListItem> Unites { get; set; }

        [Display(Name = "Type")]
        public String Type { get; set; }
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public List<AlbSelectListItem> Types { get; set; }

        [Display(Name = "HT/TTC")]
        public string ValeurHT { get; set; }

        [Display(Name = "Cout/m²")]
        public Int64? CoutM2 { get; set; }
        public List<AlbSelectListItem> ValeursHT { get; set; }


        public string ReportValeur { get; set; }
        public string ReportObligatoire { get; set; }

        public string DateDebStr { get; set; }
        public string HeureDebStr { get; set; }
        public string MinuteDebStr { get; set; }
        public string DateFinStr { get; set; }
        public string HeureFinStr { get; set; }
        public string MinuteFinStr { get; set; }

        public string DateDebMinObj { get; set; }
        public string HeureDebMinObj { get; set; }
        public string MinuteDebMinObj { get; set; }
        public string DateDebMaxObj { get; set; }
        public string HeureDebMaxObj { get; set; }
        public string MinuteDebMaxObj { get; set; }

        public string DateFinMaxObj { get; set; }
        public string HeureFinMaxObj { get; set; }
        public string MinuteFinMaxObj { get; set; }
        public string DateFinMinObj { get; set; }
        public string HeureFinMinObj { get; set; }
        public string MinuteFinMinObj { get; set; }

        public string DateDebMinObjAvn { get; set; }
        public string HeureDebMinObjAvn { get; set; }
        public string MinuteDebMinObjAvn { get; set; }
        public string DateFinMaxObjAvn { get; set; }
        public string HeureFinMaxObjAvn { get; set; }
        public string MinuteFinMaxObjAvn { get; set; }

        public bool HasFormules { get; set; }

        public string TypeRisque { get; set; }
        public List<AlbSelectListItem> TypesRisque { get; set; }

        public string TypeMateriel { get; set; }
        public List<AlbSelectListItem> TypesMateriel { get; set; }

        public string NatureLieux { get; set; }
        public List<AlbSelectListItem> NaturesLieux { get; set; }

        public DateTime? DateEntreeDescr { get; set; }
        public DateTime? DateSortieDescr { get; set; }

        public string DateDebHisto { get; set; }
        public string HeureDebHisto { get; set; }

        public bool IsRisqueSortiAvn {
            get {
                if (!IsModeAvenant || !DateEffetAvenant.HasValue || !DateSortieGarantieCopy.HasValue) {
                    return false;
                }

                return (DateEffetAvenantModificationLocaleCopy ?? DateEffetAvenant) > DateSortieGarantieCopy;
            }
        }

        public bool HasAllObjetsSortisAvn {
            get {
                if (!IsModeAvenant || !DateEffetAvenant.HasValue
                    || (!ObjetsRisqueMetaData?.Objets?.Any() ?? true)
                    || ObjetsRisqueMetaData.Objets.Any(o => !o.DateSortie.HasValue)) {
                    return false;
                }
                return (DateEffetAvenantModificationLocaleCopy ?? DateEffetAvenant) > ObjetsRisqueMetaData.Objets.Min(o => o.DateSortie);
            }
        }

        public ModeleDetailsRisquePage()
        {
            ContactAdresse = new ModeleContactAdresse();
        }

        internal void InitDatesCopy() {
            DateEffetAvenantModificationLocaleCopy = DateEffetAvenantModificationLocale;
            DateSortieGarantieCopy = InformationsGenerales?.DateSortieGarantie;
        }
    }
}