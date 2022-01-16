using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    [DataContract]
    public class BoGestionClauseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nom de la rubrique
        /// </summary>
        [DataMember]
        public string Nom1 { get; set; }

        /// <summary>
        /// Nom de la sous-rubrique
        /// </summary>
        [DataMember]
        public string Nom2 { get; set; }

        /// <summary>
        /// Code pour complétion avec rubrique et sous-rubrique
        /// </summary>
        [DataMember]
        public int Nom3 { get; set; }

        /// <summary>
        /// Numéro de version de la clause
        /// </summary>
        [DataMember]
        public int NumeroVersion { get; set; }
        
        /// <summary>
        /// Libellé de la clause
        /// </summary>
        [DataMember]
        public string Libelle { get; set; }

        /// <summary>
        /// Libellé raccouri de la clause
        /// </summary>
        [DataMember]
        public string LibelleRaccourci { get; set; }

        /// <summary>
        /// Lien vers la désignation
        /// </summary>
        [DataMember]
        public int IdDesignation { get; set; }

        /// <summary>
        /// Lien vers l'emplacement du fichier
        /// </summary>
        [DataMember]
        public int IdEmplacement { get; set; }

        /// <summary>
        /// Lien vers les mots clés
        /// </summary>
        [DataMember]
        public int IdMotCle { get; set; }

        /// <summary>
        /// Date de début de validité
        /// </summary>
        [DataMember]
        public int DateValiditeDebut { get; set; }

        /// <summary>
        /// Date de fin de validité
        /// </summary>
        [DataMember]
        public int DateValiditeFin { get; set; }

        /// <summary>
        /// Nom du document
        /// </summary>
        [DataMember]
        public string NomDocument { get; set; }

        /// <summary>
        /// Type du document
        /// </summary>
        [DataMember]
        public string TypeDocument { get; set; }

        /// <summary>
        /// Service
        /// </summary>
        [DataMember]
        public string Service { get; set; }

        /// <summary>
        /// Acte de gestion
        /// </summary>
        [DataMember]
        public string ActeGestion { get; set; }

        /// <summary>
        /// L'utilisateur qui a créé la clause
        /// </summary>
        [DataMember]
        public string CreationUser { get; set; }

        /// <summary>
        /// Date de la création de la clause
        /// </summary>
        [DataMember]
        public int CreationDate { get; set; }

        /// <summary>
        /// Heure de la création de la clause
        /// </summary>
        [DataMember]
        public int CreationHeure { get; set; }

        /// <summary>
        /// Le dernier utilisateur a avoir mis à jour la clause
        /// </summary>
        [DataMember]
        public string MAJUser { get; set; }

        /// <summary>
        /// Date de la dernnière mise à jour de la clause
        /// </summary>
        [DataMember]
        public int MAJDate { get; set; }

        /// <summary>
        /// Heure de la dernière mise à jour de la clause
        /// </summary>
        [DataMember]
        public int MAJHeure { get; set; }

        /// <summary>
        /// BoGestionClauseDesignationDto
        /// </summary>
        [DataMember]
        public BoGestionClauseDesignationDto BoGestionClauseDesignationDto { get; set; }

        /// <summary>
        /// BoGestionClauseEmplacementDto
        /// </summary>
        [DataMember]
        public BoGestionClauseEmplacementDto BoGestionClauseEmplacementDto { get; set; }

        /// <summary>
        /// List<BoGestionClauseMotCleDto>
        /// </summary>
        [DataMember]
        public List<BoGestionClauseMotCleDto> ListBoGestionClauseMotCleDto { get; set; }
    }
}
