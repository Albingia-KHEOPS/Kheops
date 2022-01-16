using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPCOUR
    public partial class YpoCour  {
             //YHPCOUR
             //YPOCOUR

            ///<summary>Public empty contructor</summary>
            public YpoCour() {}
            ///<summary>Public empty contructor</summary>
            public YpoCour(YpoCour copyFrom) 
            {
                  this.Pfipb= copyFrom.Pfipb;
                  this.Pfalx= copyFrom.Pfalx;
                  this.Pfavn= copyFrom.Pfavn;
                  this.Pfhin= copyFrom.Pfhin;
                  this.Pfcti= copyFrom.Pfcti;
                  this.Pfict= copyFrom.Pfict;
                  this.Pfsaa= copyFrom.Pfsaa;
                  this.Pfsam= copyFrom.Pfsam;
                  this.Pfsaj= copyFrom.Pfsaj;
                  this.Pfsah= copyFrom.Pfsah;
                  this.Pfsit= copyFrom.Pfsit;
                  this.Pfsta= copyFrom.Pfsta;
                  this.Pfstm= copyFrom.Pfstm;
                  this.Pfstj= copyFrom.Pfstj;
                  this.Pfges= copyFrom.Pfges;
                  this.Pfsou= copyFrom.Pfsou;
                  this.Pfcom= copyFrom.Pfcom;
                  this.Pfoct= copyFrom.Pfoct;
                  this.Pfxcm= copyFrom.Pfxcm;
                  this.Pfxcn= copyFrom.Pfxcn;
                  this.Pftyp= copyFrom.Pftyp;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Pfipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pfalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Pfavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Pfhin { get; set; } 
            
            ///<summary> Courtier Titulaire => O </summary>
            public string Pfcti { get; set; } 
            
            ///<summary> Identifiant courtier </summary>
            public int Pfict { get; set; } 
            
            ///<summary> Année de saisie </summary>
            public int Pfsaa { get; set; } 
            
            ///<summary> Mois de saisie </summary>
            public int Pfsam { get; set; } 
            
            ///<summary> Jour de saisie </summary>
            public int Pfsaj { get; set; } 
            
            ///<summary> Heure de saisie </summary>
            public int Pfsah { get; set; } 
            
            ///<summary> Code situation </summary>
            public string Pfsit { get; set; } 
            
            ///<summary> Année de situation </summary>
            public int Pfsta { get; set; } 
            
            ///<summary> Mois de situation </summary>
            public int Pfstm { get; set; } 
            
            ///<summary> Jour de situation </summary>
            public int Pfstj { get; set; } 
            
            ///<summary> Gestionnaire </summary>
            public string Pfges { get; set; } 
            
            ///<summary> Souscripteur </summary>
            public string Pfsou { get; set; } 
            
            ///<summary> % de commissionnement </summary>
            public Decimal Pfcom { get; set; } 
            
            ///<summary> Référence police chez courtier </summary>
            public string Pfoct { get; set; } 
            
            ///<summary> Taux de commission hors catnat SPAL </summary>
            public Decimal Pfxcm { get; set; } 
            
            ///<summary> Taux de commission Catnat </summary>
            public Decimal Pfxcn { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Pftyp { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoCour  x=this,  y=obj as YpoCour;
            if( y == default(YpoCour) ) return false;
            return (
                    x.Pfipb==y.Pfipb
                    && x.Pfalx==y.Pfalx
                    && x.Pfcti==y.Pfcti
                    && x.Pfict==y.Pfict
                    && x.Pfsaa==y.Pfsaa
                    && x.Pfsam==y.Pfsam
                    && x.Pfsaj==y.Pfsaj
                    && x.Pfsah==y.Pfsah
                    && x.Pfsit==y.Pfsit
                    && x.Pfsta==y.Pfsta
                    && x.Pfstm==y.Pfstm
                    && x.Pfstj==y.Pfstj
                    && x.Pfges==y.Pfges
                    && x.Pfsou==y.Pfsou
                    && x.Pfcom==y.Pfcom
                    && x.Pfoct==y.Pfoct
                    && x.Pfxcm==y.Pfxcm
                    && x.Pfxcn==y.Pfxcn
                    && x.Pftyp==y.Pftyp  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((
                       17 + (this.Pfipb?? "").GetHashCode()
                      * 23 ) + (this.Pfalx.GetHashCode() ) 
                      * 23 ) + (this.Pfcti?? "").GetHashCode()
                      * 23 ) + (this.Pfict.GetHashCode() ) 
                      * 23 ) + (this.Pfsaa.GetHashCode() ) 
                      * 23 ) + (this.Pfsam.GetHashCode() ) 
                      * 23 ) + (this.Pfsaj.GetHashCode() ) 
                      * 23 ) + (this.Pfsah.GetHashCode() ) 
                      * 23 ) + (this.Pfsit?? "").GetHashCode()
                      * 23 ) + (this.Pfsta.GetHashCode() ) 
                      * 23 ) + (this.Pfstm.GetHashCode() ) 
                      * 23 ) + (this.Pfstj.GetHashCode() ) 
                      * 23 ) + (this.Pfges?? "").GetHashCode()
                      * 23 ) + (this.Pfsou?? "").GetHashCode()
                      * 23 ) + (this.Pfcom.GetHashCode() ) 
                      * 23 ) + (this.Pfoct?? "").GetHashCode()
                      * 23 ) + (this.Pfxcm.GetHashCode() ) 
                      * 23 ) + (this.Pfxcn.GetHashCode() ) 
                      * 23 ) + (this.Pftyp?? "").GetHashCode()                   );
           }
        }
    }
}
