using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace OP.WSAS400.DTO.MatriceRisque
{
    public class MatriceRisqueLineDto
    {
        [Column(Name = "TYPEEENREGISTREMENT")]
        public string TypeEnregistrement { get; set; }
        [Column(Name = "CODERSQ")]
        public Int32 CodeRsq { get; set; }
        [Column(Name = "CODEOBJ")]
        public Int64 CodeObj { get; set; }
        [Column(Name = "NUMCHRONO")]
        public Int32 NumChrono { get; set; }
        [Column(Name = "LIBELLE")]
        public string LibelleRsqObj { get; set; }
        public string CodeAlpha { get; set; }
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [Column(Name = "VALEUR")]
        public Int64 Valeur { get; set; }
        [Column(Name = "UNITE")]
        public string Unite { get; set; }
        [Column(Name = "TYPE")]
        public string Type { get; set; }

        public DateTime? DateEntree { get; set; }
        [Column(Name = "ANNEEDEB")]
        public int DateEntreeAnnee { get; set; }
        [Column(Name = "MOISDEB")]
        public int DateEntreeMois { get; set; }
        [Column(Name = "JOURDEB")]
        public int DateEntreeJour { get; set; }

        public DateTime? DateDebAvn { get; set; }
        [Column(Name = "DEBAVNANNEE")]
        public int DebAvnAnnee { get; set; }
        [Column(Name = "DEBAVNMOIS")]
        public int DebAvnMois { get; set; }
        [Column(Name = "DEBAVNJOUR")]
        public int DebAvnJour { get; set; }

        public DateTime? DateSortie { get; set; }
        [Column(Name = "DATEFIN")]
        public Int64 DateFin { get; set; }
        [Column(Name = "ANNEEFIN")]
        public int DateSortieAnnee { get; set; }
        [Column(Name = "MOISFIN")]
        public int DateSortieMois { get; set; }
        [Column(Name = "JOURFIN")]
        public int DateSortieJour { get; set; }
        [Column(Name = "INDEXEE")]
        public string Indexee { get; set; }
        [Column(Name = "INDEXEENTETE")]
        public string IndexeEntete { get; set; }
        public bool isIndexe { get; set; }
        [Column(Name = "INVENTAIRE")]
        public string Inventaire { get; set; }
        [Column(Name = "HASBADDATE")]
        public Int32 hasBadDate { get; set; }

        [Column(Name = "ISAFFECTE")]
        public string IsAffecte { get; set; }
        
        public bool isBadDate { get; set; }
        public bool hasInventaire { get; set; }      
    }
}
