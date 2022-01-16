using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class Utilisateur_ListeNom {
        ///<summary>Public empty contructor</summary>
        public Utilisateur_ListeNom() {}
        ///<summary>Public empty contructor</summary>
        public Utilisateur_ListeNom(Utilisateur_ListeNom copyFrom) 
        {
 
            this.Nom= copyFrom.Nom;
 
            this.Prenom= copyFrom.Prenom;
 
            this.B1nombureau= copyFrom.B1nombureau;
 
            this.B1ad1= copyFrom.B1ad1;
 
            this.Utiut= copyFrom.Utiut;
        
        }        



        ///<summary> Nom utilisateur </summary>
        public string Nom { get; set; } 
        ///<summary> Prénom utilisateur </summary>
        public string Prenom { get; set; } 
        ///<summary> Nom bureau </summary>
        public string B1nombureau { get; set; } 
        ///<summary> Adresse bureau </summary>
        public string B1ad1 { get; set; } 
        ///<summary> Identifiant Utilisateur </summary>
        public string Utiut { get; set; } 
        }
}
