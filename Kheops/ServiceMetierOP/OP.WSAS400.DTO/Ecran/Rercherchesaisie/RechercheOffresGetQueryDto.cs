using ALBINGIA.Framework.Common.Constants;
using System;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.Rercherchesaisie
{
    [DataContract]
    public class RechercheOffresGetQueryDto : _RechercheOffres_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the offres.
        /// </summary>
        /// <value>
        /// The offres.
        /// </value>
        [DataMember]
        public Offres.OffreDto Offre { get; set; }

        /// <summary>
        /// Gets or sets the type date recherche.
        /// </summary>
        /// <value>
        /// The type date recherche.
        /// </value>
        [DataMember]
        public AlbConstantesMetiers.TypeDateRecherche TypeDateRecherche { get; set; }

        /// <summary>
        /// Gets or sets the date debut.
        /// </summary>
        /// <value>
        /// The date debut.
        /// </value>
        [DataMember]
        public DateTime? DateDebut { get; set; }

        /// <summary>
        /// Gets or sets the date fin.
        /// </summary>
        /// <value>
        /// The date fin.
        /// </value>
        [DataMember]
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Gets or sets the texte libre.
        /// </summary>
        /// <value>
        /// The texte libre.
        /// </value>
        [DataMember]
        public string TexteLibre { get; set; }

        /// <summary>
        /// Gets or sets the texte contenu dans adresse risque.
        /// </summary>
        /// <value>
        /// The texte contenu dans adresse risque.
        /// </value>
        [DataMember]
        public string TexteContenuDansAdresseRisque { get; set; }

        /// <summary>
        /// Gets or sets the Start Line.
        /// </summary>
        /// <value>
        /// The Start Line.
        /// </value>
        [DataMember]
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the End Line.
        /// </summary>
        /// <value>
        /// The Sorting End Line.
        /// </value>
        [DataMember]
        public int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the sorting name.
        /// </summary>
        /// <value>
        /// The sorting name.
        /// </value>
        [DataMember]
        public string SortingName { get; set; }

        /// <summary>
        /// Gets or sets the sorting order.
        /// </summary>
        /// <value>
        /// The sorting order.
        /// </value>
        [DataMember]
        public string SortingOrder { get; set; }

        /// <summary>
        /// Gets or sets the sorting order.
        /// </summary>
        /// <value>
        /// The sorting order.
        /// </value>
        [DataMember]
        public int LineCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RechercheOffresGetQueryDto"/> class.
        /// </summary>
        public RechercheOffresGetQueryDto()
        {}


        /// <summary>
        /// Initializes a new instance of the <see cref="RechercheOffresGetQueryDto"/> class.
        /// </summary>
        /// <param name="argOffre">The arg offre.</param>
        /// <param name="argTypeDateRecherche">The arg type date recherche.</param>
        /// <param name="argDateDebut">The arg date debut.</param>
        /// <param name="argDateFin">The arg date fin.</param>
        /// <param name="argTexteLibre">The arg texte libre.</param>
        /// <param name="argTexteContenuDansAdresseRisque">The arg texte contenu dans adresse risque.</param>
        /// <param name="argSortingName">The arg Sorting Name</param>
        /// <param name="argSortingOrder">The arg Sorting Order</param>
        public RechercheOffresGetQueryDto(Offres.OffreDto argOffre, AlbConstantesMetiers.TypeDateRecherche argTypeDateRecherche, Nullable<DateTime> argDateDebut, Nullable<DateTime> argDateFin, string argTexteLibre, string argTexteContenuDansAdresseRisque, string argSortingName, string argSortingOrder)
        {
            this.Offre = argOffre;
            this.TypeDateRecherche = argTypeDateRecherche;
            this.DateDebut = argDateDebut;
            this.DateFin = argDateFin;
            this.TexteLibre = argTexteLibre;
            this.TexteContenuDansAdresseRisque = argTexteContenuDansAdresseRisque;
            this.SortingName = argSortingName;
            this.SortingOrder = argSortingOrder;
        }
    }
}
