using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KPAVTRC
    public partial class KpAvTrc  {
             //KPAVTRC

            ///<summary>Public empty contructor</summary>
            public KpAvTrc() {}
            ///<summary>Public empty contructor</summary>
            public KpAvTrc(KpAvTrc copyFrom) 
            {
                  this.Khoid= copyFrom.Khoid;
                  this.Khotyp= copyFrom.Khotyp;
                  this.Khoipb= copyFrom.Khoipb;
                  this.Khoalx= copyFrom.Khoalx;
                  this.Khoperi= copyFrom.Khoperi;
                  this.Khorsq= copyFrom.Khorsq;
                  this.Khoobj= copyFrom.Khoobj;
                  this.Khofor= copyFrom.Khofor;
                  this.Khoopt= copyFrom.Khoopt;
                  this.Khoetape= copyFrom.Khoetape;
                  this.Khocham= copyFrom.Khocham;
                  this.Khoact= copyFrom.Khoact;
                  this.Khoanv= copyFrom.Khoanv;
                  this.Khonvv= copyFrom.Khonvv;
                  this.Khoavo= copyFrom.Khoavo;
                  this.Khooef= copyFrom.Khooef;
                  this.Khocru= copyFrom.Khocru;
                  this.Khocrd= copyFrom.Khocrd;
                  this.Khocrh= copyFrom.Khocrh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Khoid { get; set; } 
            
            ///<summary> Type  O Offre  P Police </summary>
            public string Khotyp { get; set; } 
            
            ///<summary> N° de Police / Offre </summary>
            public string Khoipb { get; set; } 
            
            ///<summary> N° Aliment ou Connexe </summary>
            public int Khoalx { get; set; } 
            
            ///<summary> Périmètre </summary>
            public string Khoperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Khorsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Khoobj { get; set; } 
            
            ///<summary> Formule </summary>
            public int Khofor { get; set; } 
            
            ///<summary> Option </summary>
            public int Khoopt { get; set; } 
            
            ///<summary> Etape </summary>
            public string Khoetape { get; set; } 
            
            ///<summary> Champ concerné </summary>
            public string Khocham { get; set; } 
            
            ///<summary> Type action (Création/Modif/Xsupp) </summary>
            public string Khoact { get; set; } 
            
            ///<summary> Ancienne Valeur </summary>
            public string Khoanv { get; set; } 
            
            ///<summary> Nouvelle Valeur </summary>
            public string Khonvv { get; set; } 
            
            ///<summary> Avenant Obligatoire O/N </summary>
            public string Khoavo { get; set; } 
            
            ///<summary> Opération à effectuer </summary>
            public string Khooef { get; set; } 
            
            ///<summary> Création User </summary>
            public string Khocru { get; set; } 
            
            ///<summary> Création Date </summary>
            public Int64 Khocrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Khocrh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpAvTrc  x=this,  y=obj as KpAvTrc;
            if( y == default(KpAvTrc) ) return false;
            return (
                    x.Khoid==y.Khoid
                    && x.Khotyp==y.Khotyp
                    && x.Khoipb==y.Khoipb
                    && x.Khoalx==y.Khoalx
                    && x.Khoperi==y.Khoperi
                    && x.Khorsq==y.Khorsq
                    && x.Khoobj==y.Khoobj
                    && x.Khofor==y.Khofor
                    && x.Khoopt==y.Khoopt
                    && x.Khoetape==y.Khoetape
                    && x.Khocham==y.Khocham
                    && x.Khoact==y.Khoact
                    && x.Khoanv==y.Khoanv
                    && x.Khonvv==y.Khonvv
                    && x.Khoavo==y.Khoavo
                    && x.Khooef==y.Khooef
                    && x.Khocru==y.Khocru
                    && x.Khocrd==y.Khocrd
                    && x.Khocrh==y.Khocrh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((
                       17 + (this.Khoid.GetHashCode() ) 
                      * 23 ) + (this.Khotyp?? "").GetHashCode()
                      * 23 ) + (this.Khoipb?? "").GetHashCode()
                      * 23 ) + (this.Khoalx.GetHashCode() ) 
                      * 23 ) + (this.Khoperi?? "").GetHashCode()
                      * 23 ) + (this.Khorsq.GetHashCode() ) 
                      * 23 ) + (this.Khoobj.GetHashCode() ) 
                      * 23 ) + (this.Khofor.GetHashCode() ) 
                      * 23 ) + (this.Khoopt.GetHashCode() ) 
                      * 23 ) + (this.Khoetape?? "").GetHashCode()
                      * 23 ) + (this.Khocham?? "").GetHashCode()
                      * 23 ) + (this.Khoact?? "").GetHashCode()
                      * 23 ) + (this.Khoanv?? "").GetHashCode()
                      * 23 ) + (this.Khonvv?? "").GetHashCode()
                      * 23 ) + (this.Khoavo?? "").GetHashCode()
                      * 23 ) + (this.Khooef?? "").GetHashCode()
                      * 23 ) + (this.Khocru?? "").GetHashCode()
                      * 23 ) + (this.Khocrd.GetHashCode() ) 
                      * 23 ) + (this.Khocrh.GetHashCode() )                    );
           }
        }
    }
}
