using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPENG
    public partial class KpEng  {
             //HPENG
             //KPENG

            ///<summary>Public empty contructor</summary>
            public KpEng() {}
            ///<summary>Public empty contructor</summary>
            public KpEng(KpEng copyFrom) 
            {
                  this.Kdoid= copyFrom.Kdoid;
                  this.Kdotyp= copyFrom.Kdotyp;
                  this.Kdoipb= copyFrom.Kdoipb;
                  this.Kdoalx= copyFrom.Kdoalx;
                  this.Kdoavn= copyFrom.Kdoavn;
                  this.Kdohin= copyFrom.Kdohin;
                  this.Kdoeco= copyFrom.Kdoeco;
                  this.Kdoact= copyFrom.Kdoact;
                  this.Kdoengid= copyFrom.Kdoengid;
                  this.Kdodatd= copyFrom.Kdodatd;
                  this.Kdodatf= copyFrom.Kdodatf;
                  this.Kdocru= copyFrom.Kdocru;
                  this.Kdocrd= copyFrom.Kdocrd;
                  this.Kdomaju= copyFrom.Kdomaju;
                  this.Kdomajd= copyFrom.Kdomajd;
                  this.Kdonpl= copyFrom.Kdonpl;
                  this.Kdoapp= copyFrom.Kdoapp;
                  this.Kdoeng= copyFrom.Kdoeng;
                  this.Kdoena= copyFrom.Kdoena;
                  this.Kdoobsv= copyFrom.Kdoobsv;
                  this.Kdopcv= copyFrom.Kdopcv;
                  this.Kdolct= copyFrom.Kdolct;
                  this.Kdolca= copyFrom.Kdolca;
                  this.Kdocat= copyFrom.Kdocat;
                  this.Kdocaa= copyFrom.Kdocaa;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdoid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdotyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdoipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdoalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdoavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdohin { get; set; } 
            
            ///<summary> Enregistrement en cours O/N </summary>
            public string Kdoeco { get; set; } 
            
            ///<summary> Enregistrement Actif O/N </summary>
            public string Kdoact { get; set; } 
            
            ///<summary> ID engagement Lien (KPENT YPOCONX..) </summary>
            public Int64 Kdoengid { get; set; } 
            
            ///<summary> Date début </summary>
            public int Kdodatd { get; set; } 
            
            ///<summary> Date Fin </summary>
            public int Kdodatf { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kdocru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdocrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdomaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdomajd { get; set; } 
            
            ///<summary> Nature Police </summary>
            public string Kdonpl { get; set; } 
            
            ///<summary> % Apérition </summary>
            public Decimal Kdoapp { get; set; } 
            
            ///<summary> Engagement Part totale </summary>
            public Int64 Kdoeng { get; set; } 
            
            ///<summary> Engagement Part Albingia </summary>
            public Int64 Kdoena { get; set; } 
            
            ///<summary> Lien KPOBSV </summary>
            public Int64 Kdoobsv { get; set; } 
            
            ///<summary> Part gérée </summary>
            public int Kdopcv { get; set; } 
            
            ///<summary> LCI Part Totale </summary>
            public Int64 Kdolct { get; set; } 
            
            ///<summary> LCI Part Albingia </summary>
            public Int64 Kdolca { get; set; } 
            
            ///<summary> Capitaux Part Totale </summary>
            public Int64 Kdocat { get; set; } 
            
            ///<summary> Capitaux Part Albingia </summary>
            public Int64 Kdocaa { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpEng  x=this,  y=obj as KpEng;
            if( y == default(KpEng) ) return false;
            return (
                    x.Kdoid==y.Kdoid
                    && x.Kdotyp==y.Kdotyp
                    && x.Kdoipb==y.Kdoipb
                    && x.Kdoalx==y.Kdoalx
                    && x.Kdoeco==y.Kdoeco
                    && x.Kdoact==y.Kdoact
                    && x.Kdoengid==y.Kdoengid
                    && x.Kdodatd==y.Kdodatd
                    && x.Kdodatf==y.Kdodatf
                    && x.Kdocru==y.Kdocru
                    && x.Kdocrd==y.Kdocrd
                    && x.Kdomaju==y.Kdomaju
                    && x.Kdomajd==y.Kdomajd
                    && x.Kdonpl==y.Kdonpl
                    && x.Kdoapp==y.Kdoapp
                    && x.Kdoeng==y.Kdoeng
                    && x.Kdoena==y.Kdoena
                    && x.Kdoobsv==y.Kdoobsv
                    && x.Kdopcv==y.Kdopcv
                    && x.Kdolct==y.Kdolct
                    && x.Kdolca==y.Kdolca
                    && x.Kdocat==y.Kdocat
                    && x.Kdocaa==y.Kdocaa  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((
                       17 + (this.Kdoid.GetHashCode() ) 
                      * 23 ) + (this.Kdotyp?? "").GetHashCode()
                      * 23 ) + (this.Kdoipb?? "").GetHashCode()
                      * 23 ) + (this.Kdoalx.GetHashCode() ) 
                      * 23 ) + (this.Kdoeco?? "").GetHashCode()
                      * 23 ) + (this.Kdoact?? "").GetHashCode()
                      * 23 ) + (this.Kdoengid.GetHashCode() ) 
                      * 23 ) + (this.Kdodatd.GetHashCode() ) 
                      * 23 ) + (this.Kdodatf.GetHashCode() ) 
                      * 23 ) + (this.Kdocru?? "").GetHashCode()
                      * 23 ) + (this.Kdocrd.GetHashCode() ) 
                      * 23 ) + (this.Kdomaju?? "").GetHashCode()
                      * 23 ) + (this.Kdomajd.GetHashCode() ) 
                      * 23 ) + (this.Kdonpl?? "").GetHashCode()
                      * 23 ) + (this.Kdoapp.GetHashCode() ) 
                      * 23 ) + (this.Kdoeng.GetHashCode() ) 
                      * 23 ) + (this.Kdoena.GetHashCode() ) 
                      * 23 ) + (this.Kdoobsv.GetHashCode() ) 
                      * 23 ) + (this.Kdopcv.GetHashCode() ) 
                      * 23 ) + (this.Kdolct.GetHashCode() ) 
                      * 23 ) + (this.Kdolca.GetHashCode() ) 
                      * 23 ) + (this.Kdocat.GetHashCode() ) 
                      * 23 ) + (this.Kdocaa.GetHashCode() )                    );
           }
        }
    }
}
