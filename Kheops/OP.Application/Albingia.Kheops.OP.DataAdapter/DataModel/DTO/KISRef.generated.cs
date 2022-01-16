using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.KISREF
    public partial class KISRef  {
             //KISREF

            ///<summary>Public empty contructor</summary>
            public KISRef() {}
            ///<summary>Public empty contructor</summary>
            public KISRef(KISRef copyFrom) 
            {
                  this.Kgbnmid= copyFrom.Kgbnmid;
                  this.Kgbdesc= copyFrom.Kgbdesc;
                  this.Kgblib= copyFrom.Kgblib;
                  this.Kgbtypz= copyFrom.Kgbtypz;
                  this.Kgbmapp= copyFrom.Kgbmapp;
                  this.Kgbtypc= copyFrom.Kgbtypc;
                  this.Kgbpres= copyFrom.Kgbpres;
                  this.Kgbtypu= copyFrom.Kgbtypu;
                  this.Kgbobli= copyFrom.Kgbobli;
                  this.Kgbscraff= copyFrom.Kgbscraff;
                  this.Kgbscrctr= copyFrom.Kgbscrctr;
                  this.Kgbobsv= copyFrom.Kgbobsv;
                  this.Kgbnmbd= copyFrom.Kgbnmbd;
                  this.Kgblngz= copyFrom.Kgblngz;
                  this.Kgbsaid2= copyFrom.Kgbsaid2;
                  this.Kgbscid2= copyFrom.Kgbscid2;
                  this.Kgbnref= copyFrom.Kgbnref;
                  this.Kgbdval= copyFrom.Kgbdval;
                  this.Kgbvdec= copyFrom.Kgbvdec;
                  this.Kgbvdecn= copyFrom.Kgbvdecn;
                  this.Kgbvucon= copyFrom.Kgbvucon;
                  this.Kgbvdatd= copyFrom.Kgbvdatd;
                  this.Kgbvheud= copyFrom.Kgbvheud;
                  this.Kgbvdatf= copyFrom.Kgbvdatf;
                  this.Kgbvheuf= copyFrom.Kgbvheuf;
                  this.Kgbvtxt= copyFrom.Kgbvtxt;
                  this.Kgbkfbid= copyFrom.Kgbkfbid;
                  this.Kgbvufam= copyFrom.Kgbvufam;
        
            }        
            
            ///<summary> Nom de zone   ID </summary>
            public string Kgbnmid { get; set; } 
            
            ///<summary> Description </summary>
            public string Kgbdesc { get; set; } 
            
            ///<summary> Libellé Affiché </summary>
            public string Kgblib { get; set; } 
            
            ///<summary> Type de zone </summary>
            public string Kgbtypz { get; set; } 
            
            ///<summary> Mappage O/N </summary>
            public string Kgbmapp { get; set; } 
            
            ///<summary> Type de conversion   B Bool D date H heure N num.. </summary>
            public string Kgbtypc { get; set; } 
            
            ///<summary> Type de présentation </summary>
            public int Kgbpres { get; set; } 
            
            ///<summary> Type UI de contrôle </summary>
            public string Kgbtypu { get; set; } 
            
            ///<summary> Obligatoire O/N </summary>
            public string Kgbobli { get; set; } 
            
            ///<summary> Présence Script Affichage O/N </summary>
            public string Kgbscraff { get; set; } 
            
            ///<summary> Présence Script Contrôle  O/N </summary>
            public string Kgbscrctr { get; set; } 
            
            ///<summary> Observation </summary>
            public string Kgbobsv { get; set; } 
            
            ///<summary> Nom de la colonne BD </summary>
            public string Kgbnmbd { get; set; } 
            
            ///<summary> LONGUEUR </summary>
            public string Kgblngz { get; set; } 
            
            ///<summary> Script Affichage- lien KHTSCRIPT </summary>
            public string Kgbsaid2 { get; set; } 
            
            ///<summary> Script Contrôle - lien KHTSCRIPT </summary>
            public string Kgbscid2 { get; set; } 
            
            ///<summary> N° de référence </summary>
            public int Kgbnref { get; set; } 
            
            ///<summary> KGBDVAL </summary>
            public string Kgbdval { get; set; } 
            
            ///<summary> Valeur numérique O/N </summary>
            public string Kgbvdec { get; set; } 
            
            ///<summary> Valeur numériquenb nb décimales (0à4) </summary>
            public int Kgbvdecn { get; set; } 
            
            ///<summary> unité concept </summary>
            public string Kgbvucon { get; set; } 
            
            ///<summary> Date début O/N </summary>
            public string Kgbvdatd { get; set; } 
            
            ///<summary> heure début O/N </summary>
            public string Kgbvheud { get; set; } 
            
            ///<summary> Date finO/N </summary>
            public string Kgbvdatf { get; set; } 
            
            ///<summary> heure fin O/N </summary>
            public string Kgbvheuf { get; set; } 
            
            ///<summary> texte O/N </summary>
            public string Kgbvtxt { get; set; } 
            
            ///<summary> Dési O/N </summary>
            public string Kgbkfbid { get; set; } 
            
            ///<summary> unité famille </summary>
            public string Kgbvufam { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KISRef  x=this,  y=obj as KISRef;
            if( y == default(KISRef) ) return false;
            return (
                    x.Kgbnmid==y.Kgbnmid
                    && x.Kgbdesc==y.Kgbdesc
                    && x.Kgblib==y.Kgblib
                    && x.Kgbtypz==y.Kgbtypz
                    && x.Kgbmapp==y.Kgbmapp
                    && x.Kgbtypc==y.Kgbtypc
                    && x.Kgbpres==y.Kgbpres
                    && x.Kgbtypu==y.Kgbtypu
                    && x.Kgbobli==y.Kgbobli
                    && x.Kgbscraff==y.Kgbscraff
                    && x.Kgbscrctr==y.Kgbscrctr
                    && x.Kgbobsv==y.Kgbobsv
                    && x.Kgbnmbd==y.Kgbnmbd
                    && x.Kgblngz==y.Kgblngz
                    && x.Kgbsaid2==y.Kgbsaid2
                    && x.Kgbscid2==y.Kgbscid2
                    && x.Kgbnref==y.Kgbnref
                    && x.Kgbdval==y.Kgbdval
                    && x.Kgbvdec==y.Kgbvdec
                    && x.Kgbvdecn==y.Kgbvdecn
                    && x.Kgbvucon==y.Kgbvucon
                    && x.Kgbvdatd==y.Kgbvdatd
                    && x.Kgbvheud==y.Kgbvheud
                    && x.Kgbvdatf==y.Kgbvdatf
                    && x.Kgbvheuf==y.Kgbvheuf
                    && x.Kgbvtxt==y.Kgbvtxt
                    && x.Kgbkfbid==y.Kgbkfbid
                    && x.Kgbvufam==y.Kgbvufam  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((
                       17 + (this.Kgbnmid?? "").GetHashCode()
                      * 23 ) + (this.Kgbdesc?? "").GetHashCode()
                      * 23 ) + (this.Kgblib?? "").GetHashCode()
                      * 23 ) + (this.Kgbtypz?? "").GetHashCode()
                      * 23 ) + (this.Kgbmapp?? "").GetHashCode()
                      * 23 ) + (this.Kgbtypc?? "").GetHashCode()
                      * 23 ) + (this.Kgbpres.GetHashCode() ) 
                      * 23 ) + (this.Kgbtypu?? "").GetHashCode()
                      * 23 ) + (this.Kgbobli?? "").GetHashCode()
                      * 23 ) + (this.Kgbscraff?? "").GetHashCode()
                      * 23 ) + (this.Kgbscrctr?? "").GetHashCode()
                      * 23 ) + (this.Kgbobsv?? "").GetHashCode()
                      * 23 ) + (this.Kgbnmbd?? "").GetHashCode()
                      * 23 ) + (this.Kgblngz?? "").GetHashCode()
                      * 23 ) + (this.Kgbsaid2?? "").GetHashCode()
                      * 23 ) + (this.Kgbscid2?? "").GetHashCode()
                      * 23 ) + (this.Kgbnref.GetHashCode() ) 
                      * 23 ) + (this.Kgbdval?? "").GetHashCode()
                      * 23 ) + (this.Kgbvdec?? "").GetHashCode()
                      * 23 ) + (this.Kgbvdecn.GetHashCode() ) 
                      * 23 ) + (this.Kgbvucon?? "").GetHashCode()
                      * 23 ) + (this.Kgbvdatd?? "").GetHashCode()
                      * 23 ) + (this.Kgbvheud?? "").GetHashCode()
                      * 23 ) + (this.Kgbvdatf?? "").GetHashCode()
                      * 23 ) + (this.Kgbvheuf?? "").GetHashCode()
                      * 23 ) + (this.Kgbvtxt?? "").GetHashCode()
                      * 23 ) + (this.Kgbkfbid?? "").GetHashCode()
                      * 23 ) + (this.Kgbvufam?? "").GetHashCode()                   );
           }
        }
    }
}
