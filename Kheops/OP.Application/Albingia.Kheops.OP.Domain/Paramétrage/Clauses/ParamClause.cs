using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Transverse;

namespace Albingia.Kheops.OP.Domain.Clauses
{
    public class ParamClause
    {
        public long Id { get; set; }
        public NomClause Nom { get; set; }

        public string Libelle { get; set; }
        public string LibelleRaccourci { get; set; }

        public string Designation { get; set; }
        /// <summary>
        /// KDVID
        /// </summary>
        public long IdEmplacement { get; set; }

        /// <summary>
        ///Lien Mot Clé  KMOTCLE
        /// </summary>
        public long IdMotCle { get; set; }
        /// <summary>
        /// Date validité Début
        /// </summary>
        public DateTime? DateValiditeDebut { get; set; }
        /// <summary>
        /// Date Validité Fin
        /// </summary>
        public DateTime? DateValiditeFin { get; set; }

        /// <summary>
        ///Nom du document
        /// </summary>
        public string NomDoc { get; set; }


        /// <summary>
        /// Type de Document(CG, CS, CP,...)
        /// </summary>
        public TypeDocument TypeDeDocument { get; set; }
        public string Service { get; set; }
        public string ActeDeGestion { get; set; }

        /// <summary>
        /// Code regroupement clauses
        /// </summary>
        public string CodeRegroupement { get; set; }

        //Ordonnancement impression
        //Code annexe
        //Ordonnancement Impression Annexe

        public UpdateMetadata Creation { get; set; }

        public UpdateMetadata MiseAJour { get; set; }
    }
}
