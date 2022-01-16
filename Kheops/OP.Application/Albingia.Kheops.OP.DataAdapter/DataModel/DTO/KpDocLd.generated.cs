using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPDOCLDW
    public partial class KpDocLD  {
             //KPDOCLDW
             //KPDOCLDW

            ///<summary>Public empty contructor</summary>
            public KpDocLD() {}
            ///<summary>Public empty contructor</summary>
            public KpDocLD(KpDocLD copyFrom) 
            {
                  this.Kemid= copyFrom.Kemid;
                  this.Kemkelid= copyFrom.Kemkelid;
                  this.Kemord= copyFrom.Kemord;
                  this.Kemtypd= copyFrom.Kemtypd;
                  this.Kemtypl= copyFrom.Kemtypl;
                  this.Kemtyenv= copyFrom.Kemtyenv;
                  this.Kemtamp= copyFrom.Kemtamp;
                  this.Kemtyds= copyFrom.Kemtyds;
                  this.Kemtyi= copyFrom.Kemtyi;
                  this.Kemids= copyFrom.Kemids;
                  this.Kemdstp= copyFrom.Kemdstp;
                  this.Keminl= copyFrom.Keminl;
                  this.Kemnbex= copyFrom.Kemnbex;
                  this.Kemdoca= copyFrom.Kemdoca;
                  this.Kemtydif= copyFrom.Kemtydif;
                  this.Kemlmai= copyFrom.Kemlmai;
                  this.Kemaemo= copyFrom.Kemaemo;
                  this.Kemaem= copyFrom.Kemaem;
                  this.Kemkesid= copyFrom.Kemkesid;
                  this.Kemnta= copyFrom.Kemnta;
                  this.Kemstu= copyFrom.Kemstu;
                  this.Kemsit= copyFrom.Kemsit;
                  this.Kemstd= copyFrom.Kemstd;
                  this.Kemsth= copyFrom.Kemsth;
                  this.Kemenvu= copyFrom.Kemenvu;
                  this.Kemenvd= copyFrom.Kemenvd;
                  this.Kemenvh= copyFrom.Kemenvh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kemid { get; set; } 
            
            ///<summary> Lien KPDOCL </summary>
            public Int64 Kemkelid { get; set; } 
            
            ///<summary> N° ordre </summary>
            public int Kemord { get; set; } 
            
            ///<summary> Type Document Généré/Externe </summary>
            public string Kemtypd { get; set; } 
            
            ///<summary> Lien KPDOC ou KPDOCEXT </summary>
            public Int64 Kemtypl { get; set; } 
            
            ///<summary> Type envoi (AR,Recommandé ...) </summary>
            public string Kemtyenv { get; set; } 
            
            ///<summary> Tampon: Original Copie Duplicata </summary>
            public string Kemtamp { get; set; } 
            
            ///<summary> Type de destinataire </summary>
            public string Kemtyds { get; set; } 
            
            ///<summary> Type Intervenant </summary>
            public string Kemtyi { get; set; } 
            
            ///<summary> Identifiant Destinataire </summary>
            public int Kemids { get; set; } 
            
            ///<summary> Destinataire principal O/N </summary>
            public string Kemdstp { get; set; } 
            
            ///<summary> Code interlocuteur </summary>
            public int Keminl { get; set; } 
            
            ///<summary> Nombre exemplaires </summary>
            public int Kemnbex { get; set; } 
            
            ///<summary> Accompagnant lien Document   KDOC </summary>
            public Int64 Kemdoca { get; set; } 
            
            ///<summary> Type diffusion (Cour,Mail...) </summary>
            public string Kemtydif { get; set; } 
            
            ///<summary> Mail Lot </summary>
            public int Kemlmai { get; set; } 
            
            ///<summary> Mail Objet </summary>
            public string Kemaemo { get; set; } 
            
            ///<summary> Mail Adresse messagerie si dif </summary>
            public string Kemaem { get; set; } 
            
            ///<summary> Mail Lien KPDOCTX Texte  Corps mail </summary>
            public Int64 Kemkesid { get; set; } 
            
            ///<summary> Nature de la génération (O/P/S) </summary>
            public string Kemnta { get; set; } 
            
            ///<summary> Situation User </summary>
            public string Kemstu { get; set; } 
            
            ///<summary> Situation Code </summary>
            public string Kemsit { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kemstd { get; set; } 
            
            ///<summary> Situation Heure </summary>
            public int Kemsth { get; set; } 
            
            ///<summary> Envoi User </summary>
            public string Kemenvu { get; set; } 
            
            ///<summary> Envoi Date </summary>
            public int Kemenvd { get; set; } 
            
            ///<summary> Envoi Heure </summary>
            public int Kemenvh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpDocLD  x=this,  y=obj as KpDocLD;
            if( y == default(KpDocLD) ) return false;
            return (
                    x.Kemid==y.Kemid
                    && x.Kemkelid==y.Kemkelid
                    && x.Kemord==y.Kemord
                    && x.Kemtypd==y.Kemtypd
                    && x.Kemtypl==y.Kemtypl
                    && x.Kemtyenv==y.Kemtyenv
                    && x.Kemtamp==y.Kemtamp
                    && x.Kemtyds==y.Kemtyds
                    && x.Kemtyi==y.Kemtyi
                    && x.Kemids==y.Kemids
                    && x.Kemdstp==y.Kemdstp
                    && x.Keminl==y.Keminl
                    && x.Kemnbex==y.Kemnbex
                    && x.Kemdoca==y.Kemdoca
                    && x.Kemtydif==y.Kemtydif
                    && x.Kemlmai==y.Kemlmai
                    && x.Kemaemo==y.Kemaemo
                    && x.Kemaem==y.Kemaem
                    && x.Kemkesid==y.Kemkesid
                    && x.Kemnta==y.Kemnta
                    && x.Kemstu==y.Kemstu
                    && x.Kemsit==y.Kemsit
                    && x.Kemstd==y.Kemstd
                    && x.Kemsth==y.Kemsth
                    && x.Kemenvu==y.Kemenvu
                    && x.Kemenvd==y.Kemenvd
                    && x.Kemenvh==y.Kemenvh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((
                       17 + (this.Kemid.GetHashCode() ) 
                      * 23 ) + (this.Kemkelid.GetHashCode() ) 
                      * 23 ) + (this.Kemord.GetHashCode() ) 
                      * 23 ) + (this.Kemtypd?? "").GetHashCode()
                      * 23 ) + (this.Kemtypl.GetHashCode() ) 
                      * 23 ) + (this.Kemtyenv?? "").GetHashCode()
                      * 23 ) + (this.Kemtamp?? "").GetHashCode()
                      * 23 ) + (this.Kemtyds?? "").GetHashCode()
                      * 23 ) + (this.Kemtyi?? "").GetHashCode()
                      * 23 ) + (this.Kemids.GetHashCode() ) 
                      * 23 ) + (this.Kemdstp?? "").GetHashCode()
                      * 23 ) + (this.Keminl.GetHashCode() ) 
                      * 23 ) + (this.Kemnbex.GetHashCode() ) 
                      * 23 ) + (this.Kemdoca.GetHashCode() ) 
                      * 23 ) + (this.Kemtydif?? "").GetHashCode()
                      * 23 ) + (this.Kemlmai.GetHashCode() ) 
                      * 23 ) + (this.Kemaemo?? "").GetHashCode()
                      * 23 ) + (this.Kemaem?? "").GetHashCode()
                      * 23 ) + (this.Kemkesid.GetHashCode() ) 
                      * 23 ) + (this.Kemnta?? "").GetHashCode()
                      * 23 ) + (this.Kemstu?? "").GetHashCode()
                      * 23 ) + (this.Kemsit?? "").GetHashCode()
                      * 23 ) + (this.Kemstd.GetHashCode() ) 
                      * 23 ) + (this.Kemsth.GetHashCode() ) 
                      * 23 ) + (this.Kemenvu?? "").GetHashCode()
                      * 23 ) + (this.Kemenvd.GetHashCode() ) 
                      * 23 ) + (this.Kemenvh.GetHashCode() )                    );
           }
        }
    }
}
