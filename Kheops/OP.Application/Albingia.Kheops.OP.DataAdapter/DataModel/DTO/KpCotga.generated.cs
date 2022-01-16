using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPCOTGA
    public partial class KpCotga  {
             //HPCOTGA
             //KPCOTGA

            ///<summary>Public empty contructor</summary>
            public KpCotga() {}
            ///<summary>Public empty contructor</summary>
            public KpCotga(KpCotga copyFrom) 
            {
                  this.Kdnid= copyFrom.Kdnid;
                  this.Kdntyp= copyFrom.Kdntyp;
                  this.Kdnipb= copyFrom.Kdnipb;
                  this.Kdnalx= copyFrom.Kdnalx;
                  this.Kdnavn= copyFrom.Kdnavn;
                  this.Kdnhin= copyFrom.Kdnhin;
                  this.Kdnfor= copyFrom.Kdnfor;
                  this.Kdnfoa= copyFrom.Kdnfoa;
                  this.Kdnopt= copyFrom.Kdnopt;
                  this.Kdngaran= copyFrom.Kdngaran;
                  this.Kdnkdeid= copyFrom.Kdnkdeid;
                  this.Kdnnumtar= copyFrom.Kdnnumtar;
                  this.Kdnkdgid= copyFrom.Kdnkdgid;
                  this.Kdntarok= copyFrom.Kdntarok;
                  this.Kdnkdmid= copyFrom.Kdnkdmid;
                  this.Kdnrsq= copyFrom.Kdnrsq;
                  this.Kdntri= copyFrom.Kdntri;
                  this.Kdnht= copyFrom.Kdnht;
                  this.Kdnhf= copyFrom.Kdnhf;
                  this.Kdnkh= copyFrom.Kdnkh;
                  this.Kdnmht= copyFrom.Kdnmht;
                  this.Kdnkht= copyFrom.Kdnkht;
                  this.Kdnmtx= copyFrom.Kdnmtx;
                  this.Kdnktx= copyFrom.Kdnktx;
                  this.Kdnttc= copyFrom.Kdnttc;
                  this.Kdnttf= copyFrom.Kdnttf;
                  this.Kdnmtt= copyFrom.Kdnmtt;
                  this.Kdnktc= copyFrom.Kdnktc;
                  this.Kdncot= copyFrom.Kdncot;
                  this.Kdnkco= copyFrom.Kdnkco;
                  this.Kdncnamht= copyFrom.Kdncnamht;
                  this.Kdncnakht= copyFrom.Kdncnakht;
                  this.Kdncnamtx= copyFrom.Kdncnamtx;
                  this.Kdncnaktx= copyFrom.Kdncnaktx;
                  this.Kdncnamtt= copyFrom.Kdncnamtt;
                  this.Kdncnaktt= copyFrom.Kdncnaktt;
                  this.Kdncnacom= copyFrom.Kdncnacom;
                  this.Kdncnakcm= copyFrom.Kdncnakcm;
                  this.Kdnatgmht= copyFrom.Kdnatgmht;
                  this.Kdnatgkht= copyFrom.Kdnatgkht;
                  this.Kdnatgmtx= copyFrom.Kdnatgmtx;
                  this.Kdnatgktx= copyFrom.Kdnatgktx;
                  this.Kdnatgmtt= copyFrom.Kdnatgmtt;
                  this.Kdnatgktt= copyFrom.Kdnatgktt;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdnid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdntyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdnipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdnalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdnavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdnhin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdnfor { get; set; } 
            
            ///<summary> Formule code Alpha </summary>
            public string Kdnfoa { get; set; } 
            
            ///<summary> Option </summary>
            public int Kdnopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kdngaran { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Kdnkdeid { get; set; } 
            
            ///<summary> Numéro Tarif </summary>
            public int Kdnnumtar { get; set; } 
            
            ///<summary> Lien KPGARTAR </summary>
            public Int64 Kdnkdgid { get; set; } 
            
            ///<summary> Flag coché O/N </summary>
            public string Kdntarok { get; set; } 
            
            ///<summary> Lien KPCOTIS </summary>
            public Int64 Kdnkdmid { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kdnrsq { get; set; } 
            
            ///<summary> Tri </summary>
            public string Kdntri { get; set; } 
            
            ///<summary> HT Hors CN Calculé dev </summary>
            public Decimal Kdnht { get; set; } 
            
            ///<summary> HT Hors CN Forcé dev </summary>
            public Decimal Kdnhf { get; set; } 
            
            ///<summary> HT Hors CN cpt </summary>
            public Decimal Kdnkh { get; set; } 
            
            ///<summary> Total HT Hors CN dev </summary>
            public Decimal Kdnmht { get; set; } 
            
            ///<summary> Total HT Hors CN cpt </summary>
            public Decimal Kdnkht { get; set; } 
            
            ///<summary> Taxes Hors CN dev </summary>
            public Decimal Kdnmtx { get; set; } 
            
            ///<summary> Taxes hors CN cpt </summary>
            public Decimal Kdnktx { get; set; } 
            
            ///<summary> Total TTC Calculé Hors CN dev </summary>
            public Decimal Kdnttc { get; set; } 
            
            ///<summary> Total TTC Forcé Hors CN dev </summary>
            public Decimal Kdnttf { get; set; } 
            
            ///<summary> Total TTC Hors CN dev </summary>
            public Decimal Kdnmtt { get; set; } 
            
            ///<summary> Total TTC Hors CN cpt </summary>
            public Decimal Kdnktc { get; set; } 
            
            ///<summary> Mnt Commissions Hors Catnat dev </summary>
            public Decimal Kdncot { get; set; } 
            
            ///<summary> Mnt Commissions Hors CN Cpt </summary>
            public Decimal Kdnkco { get; set; } 
            
            ///<summary> CATNAT HT dev </summary>
            public Decimal Kdncnamht { get; set; } 
            
            ///<summary> CATNAT HT cpt </summary>
            public Decimal Kdncnakht { get; set; } 
            
            ///<summary> CATNAT Taxes dev </summary>
            public Decimal Kdncnamtx { get; set; } 
            
            ///<summary> CATNAT Taxes cpt </summary>
            public Decimal Kdncnaktx { get; set; } 
            
            ///<summary> CATNAT TTC dev </summary>
            public Decimal Kdncnamtt { get; set; } 
            
            ///<summary> CATNAT TTC cpt </summary>
            public Decimal Kdncnaktt { get; set; } 
            
            ///<summary> CATNAT Commission Dev </summary>
            public Decimal Kdncnacom { get; set; } 
            
            ///<summary> CATNAT Commission cpt </summary>
            public Decimal Kdncnakcm { get; set; } 
            
            ///<summary> ATG HT dev </summary>
            public Decimal Kdnatgmht { get; set; } 
            
            ///<summary> ATG HT cpt </summary>
            public Decimal Kdnatgkht { get; set; } 
            
            ///<summary> ATG Taxes dev </summary>
            public Decimal Kdnatgmtx { get; set; } 
            
            ///<summary> ATG Taxes cpt </summary>
            public Decimal Kdnatgktx { get; set; } 
            
            ///<summary> ATG TTC dev </summary>
            public Decimal Kdnatgmtt { get; set; } 
            
            ///<summary> ATG TTC cpt </summary>
            public Decimal Kdnatgktt { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCotga  x=this,  y=obj as KpCotga;
            if( y == default(KpCotga) ) return false;
            return (
                    x.Kdnid==y.Kdnid
                    && x.Kdntyp==y.Kdntyp
                    && x.Kdnipb==y.Kdnipb
                    && x.Kdnalx==y.Kdnalx
                    && x.Kdnfor==y.Kdnfor
                    && x.Kdnfoa==y.Kdnfoa
                    && x.Kdnopt==y.Kdnopt
                    && x.Kdngaran==y.Kdngaran
                    && x.Kdnkdeid==y.Kdnkdeid
                    && x.Kdnnumtar==y.Kdnnumtar
                    && x.Kdnkdgid==y.Kdnkdgid
                    && x.Kdntarok==y.Kdntarok
                    && x.Kdnkdmid==y.Kdnkdmid
                    && x.Kdnrsq==y.Kdnrsq
                    && x.Kdntri==y.Kdntri
                    && x.Kdnht==y.Kdnht
                    && x.Kdnhf==y.Kdnhf
                    && x.Kdnkh==y.Kdnkh
                    && x.Kdnmht==y.Kdnmht
                    && x.Kdnkht==y.Kdnkht
                    && x.Kdnmtx==y.Kdnmtx
                    && x.Kdnktx==y.Kdnktx
                    && x.Kdnttc==y.Kdnttc
                    && x.Kdnttf==y.Kdnttf
                    && x.Kdnmtt==y.Kdnmtt
                    && x.Kdnktc==y.Kdnktc
                    && x.Kdncot==y.Kdncot
                    && x.Kdnkco==y.Kdnkco
                    && x.Kdncnamht==y.Kdncnamht
                    && x.Kdncnakht==y.Kdncnakht
                    && x.Kdncnamtx==y.Kdncnamtx
                    && x.Kdncnaktx==y.Kdncnaktx
                    && x.Kdncnamtt==y.Kdncnamtt
                    && x.Kdncnaktt==y.Kdncnaktt
                    && x.Kdncnacom==y.Kdncnacom
                    && x.Kdncnakcm==y.Kdncnakcm
                    && x.Kdnatgmht==y.Kdnatgmht
                    && x.Kdnatgkht==y.Kdnatgkht
                    && x.Kdnatgmtx==y.Kdnatgmtx
                    && x.Kdnatgktx==y.Kdnatgktx
                    && x.Kdnatgmtt==y.Kdnatgmtt
                    && x.Kdnatgktt==y.Kdnatgktt  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kdnid.GetHashCode() ) 
                      * 23 ) + (this.Kdntyp?? "").GetHashCode()
                      * 23 ) + (this.Kdnipb?? "").GetHashCode()
                      * 23 ) + (this.Kdnalx.GetHashCode() ) 
                      * 23 ) + (this.Kdnfor.GetHashCode() ) 
                      * 23 ) + (this.Kdnfoa?? "").GetHashCode()
                      * 23 ) + (this.Kdnopt.GetHashCode() ) 
                      * 23 ) + (this.Kdngaran?? "").GetHashCode()
                      * 23 ) + (this.Kdnkdeid.GetHashCode() ) 
                      * 23 ) + (this.Kdnnumtar.GetHashCode() ) 
                      * 23 ) + (this.Kdnkdgid.GetHashCode() ) 
                      * 23 ) + (this.Kdntarok?? "").GetHashCode()
                      * 23 ) + (this.Kdnkdmid.GetHashCode() ) 
                      * 23 ) + (this.Kdnrsq.GetHashCode() ) 
                      * 23 ) + (this.Kdntri?? "").GetHashCode()
                      * 23 ) + (this.Kdnht.GetHashCode() ) 
                      * 23 ) + (this.Kdnhf.GetHashCode() ) 
                      * 23 ) + (this.Kdnkh.GetHashCode() ) 
                      * 23 ) + (this.Kdnmht.GetHashCode() ) 
                      * 23 ) + (this.Kdnkht.GetHashCode() ) 
                      * 23 ) + (this.Kdnmtx.GetHashCode() ) 
                      * 23 ) + (this.Kdnktx.GetHashCode() ) 
                      * 23 ) + (this.Kdnttc.GetHashCode() ) 
                      * 23 ) + (this.Kdnttf.GetHashCode() ) 
                      * 23 ) + (this.Kdnmtt.GetHashCode() ) 
                      * 23 ) + (this.Kdnktc.GetHashCode() ) 
                      * 23 ) + (this.Kdncot.GetHashCode() ) 
                      * 23 ) + (this.Kdnkco.GetHashCode() ) 
                      * 23 ) + (this.Kdncnamht.GetHashCode() ) 
                      * 23 ) + (this.Kdncnakht.GetHashCode() ) 
                      * 23 ) + (this.Kdncnamtx.GetHashCode() ) 
                      * 23 ) + (this.Kdncnaktx.GetHashCode() ) 
                      * 23 ) + (this.Kdncnamtt.GetHashCode() ) 
                      * 23 ) + (this.Kdncnaktt.GetHashCode() ) 
                      * 23 ) + (this.Kdncnacom.GetHashCode() ) 
                      * 23 ) + (this.Kdncnakcm.GetHashCode() ) 
                      * 23 ) + (this.Kdnatgmht.GetHashCode() ) 
                      * 23 ) + (this.Kdnatgkht.GetHashCode() ) 
                      * 23 ) + (this.Kdnatgmtx.GetHashCode() ) 
                      * 23 ) + (this.Kdnatgktx.GetHashCode() ) 
                      * 23 ) + (this.Kdnatgmtt.GetHashCode() ) 
                      * 23 ) + (this.Kdnatgktt.GetHashCode() )                    );
           }
        }
    }
}
