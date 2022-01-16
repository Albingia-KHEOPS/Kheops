using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YPOTRAC
    public partial class YpoTrac  {
             //YPOTRAC

            ///<summary>Public empty contructor</summary>
            public YpoTrac() {}
            ///<summary>Public empty contructor</summary>
            public YpoTrac(YpoTrac copyFrom) 
            {
                  this.Pytyp= copyFrom.Pytyp;
                  this.Pyipb= copyFrom.Pyipb;
                  this.Pyalx= copyFrom.Pyalx;
                  this.Pyavn= copyFrom.Pyavn;
                  this.Pyttr= copyFrom.Pyttr;
                  this.Pyvag= copyFrom.Pyvag;
                  this.Pyord= copyFrom.Pyord;
                  this.Pytra= copyFrom.Pytra;
                  this.Pytrm= copyFrom.Pytrm;
                  this.Pytrj= copyFrom.Pytrj;
                  this.Pytrh= copyFrom.Pytrh;
                  this.Pylib= copyFrom.Pylib;
                  this.Pyinf= copyFrom.Pyinf;
                  this.Pysda= copyFrom.Pysda;
                  this.Pysdm= copyFrom.Pysdm;
                  this.Pysdj= copyFrom.Pysdj;
                  this.Pysfa= copyFrom.Pysfa;
                  this.Pysfm= copyFrom.Pysfm;
                  this.Pysfj= copyFrom.Pysfj;
                  this.Pymju= copyFrom.Pymju;
                  this.Pymja= copyFrom.Pymja;
                  this.Pymjm= copyFrom.Pymjm;
                  this.Pymjj= copyFrom.Pymjj;
                  this.Pymjh= copyFrom.Pymjh;
        
            }        
            
            ///<summary> Type  O Offre  P Police </summary>
            public string Pytyp { get; set; } 
            
            ///<summary> N° Offre ou Police </summary>
            public string Pyipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pyalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Pyavn { get; set; } 
            
            ///<summary> Type de traitement (Affnouv/avenant) </summary>
            public string Pyttr { get; set; } 
            
            ///<summary> Gestion G ou Validation V </summary>
            public string Pyvag { get; set; } 
            
            ///<summary> N° ordre </summary>
            public int Pyord { get; set; } 
            
            ///<summary> Traitement : Année </summary>
            public int Pytra { get; set; } 
            
            ///<summary> Traitement : Mois </summary>
            public int Pytrm { get; set; } 
            
            ///<summary> Traitement : Jour </summary>
            public int Pytrj { get; set; } 
            
            ///<summary> Traitement Heure </summary>
            public int Pytrh { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Pylib { get; set; } 
            
            ///<summary> Type Info : I Info / S Suspension </summary>
            public string Pyinf { get; set; } 
            
            ///<summary> Garantie suspendue : Année Début </summary>
            public int Pysda { get; set; } 
            
            ///<summary> Garantie suspendue : Mois  Début </summary>
            public int Pysdm { get; set; } 
            
            ///<summary> Garantie suspendue : Jour  Début </summary>
            public int Pysdj { get; set; } 
            
            ///<summary> Garantie suspendue : Année Fin </summary>
            public int Pysfa { get; set; } 
            
            ///<summary> Garantie suspendue : Mois  Fin </summary>
            public int Pysfm { get; set; } 
            
            ///<summary> Garantie suspendue : Jour  Fin </summary>
            public int Pysfj { get; set; } 
            
            ///<summary> Màj : User </summary>
            public string Pymju { get; set; } 
            
            ///<summary> Màj : Année </summary>
            public int Pymja { get; set; } 
            
            ///<summary> Màj : Mois </summary>
            public int Pymjm { get; set; } 
            
            ///<summary> Màj : Jour </summary>
            public int Pymjj { get; set; } 
            
            ///<summary> Màj : Heure </summary>
            public int Pymjh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoTrac  x=this,  y=obj as YpoTrac;
            if( y == default(YpoTrac) ) return false;
            return (
                    x.Pytyp==y.Pytyp
                    && x.Pyipb==y.Pyipb
                    && x.Pyalx==y.Pyalx
                    && x.Pyavn==y.Pyavn
                    && x.Pyttr==y.Pyttr
                    && x.Pyvag==y.Pyvag
                    && x.Pyord==y.Pyord
                    && x.Pytra==y.Pytra
                    && x.Pytrm==y.Pytrm
                    && x.Pytrj==y.Pytrj
                    && x.Pytrh==y.Pytrh
                    && x.Pylib==y.Pylib
                    && x.Pyinf==y.Pyinf
                    && x.Pysda==y.Pysda
                    && x.Pysdm==y.Pysdm
                    && x.Pysdj==y.Pysdj
                    && x.Pysfa==y.Pysfa
                    && x.Pysfm==y.Pysfm
                    && x.Pysfj==y.Pysfj
                    && x.Pymju==y.Pymju
                    && x.Pymja==y.Pymja
                    && x.Pymjm==y.Pymjm
                    && x.Pymjj==y.Pymjj
                    && x.Pymjh==y.Pymjh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((
                       17 + (this.Pytyp?? "").GetHashCode()
                      * 23 ) + (this.Pyipb?? "").GetHashCode()
                      * 23 ) + (this.Pyalx.GetHashCode() ) 
                      * 23 ) + (this.Pyavn.GetHashCode() ) 
                      * 23 ) + (this.Pyttr?? "").GetHashCode()
                      * 23 ) + (this.Pyvag?? "").GetHashCode()
                      * 23 ) + (this.Pyord.GetHashCode() ) 
                      * 23 ) + (this.Pytra.GetHashCode() ) 
                      * 23 ) + (this.Pytrm.GetHashCode() ) 
                      * 23 ) + (this.Pytrj.GetHashCode() ) 
                      * 23 ) + (this.Pytrh.GetHashCode() ) 
                      * 23 ) + (this.Pylib?? "").GetHashCode()
                      * 23 ) + (this.Pyinf?? "").GetHashCode()
                      * 23 ) + (this.Pysda.GetHashCode() ) 
                      * 23 ) + (this.Pysdm.GetHashCode() ) 
                      * 23 ) + (this.Pysdj.GetHashCode() ) 
                      * 23 ) + (this.Pysfa.GetHashCode() ) 
                      * 23 ) + (this.Pysfm.GetHashCode() ) 
                      * 23 ) + (this.Pysfj.GetHashCode() ) 
                      * 23 ) + (this.Pymju?? "").GetHashCode()
                      * 23 ) + (this.Pymja.GetHashCode() ) 
                      * 23 ) + (this.Pymjm.GetHashCode() ) 
                      * 23 ) + (this.Pymjj.GetHashCode() ) 
                      * 23 ) + (this.Pymjh.GetHashCode() )                    );
           }
        }
    }
}
