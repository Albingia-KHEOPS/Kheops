using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPOPP
    public partial class KpOpp  {
             //HPOPP
             //KPOPP

            ///<summary>Public empty contructor</summary>
            public KpOpp() {}
            ///<summary>Public empty contructor</summary>
            public KpOpp(KpOpp copyFrom) 
            {
                  this.Kfpid= copyFrom.Kfpid;
                  this.Kfptyp= copyFrom.Kfptyp;
                  this.Kfpipb= copyFrom.Kfpipb;
                  this.Kfpalx= copyFrom.Kfpalx;
                  this.Kfpavn= copyFrom.Kfpavn;
                  this.Kfphin= copyFrom.Kfphin;
                  this.Kfpidcb= copyFrom.Kfpidcb;
                  this.Kfptfi= copyFrom.Kfptfi;
                  this.Kfpdesi= copyFrom.Kfpdesi;
                  this.Kfpref= copyFrom.Kfpref;
                  this.Kfpdech= copyFrom.Kfpdech;
                  this.Kfpmnt= copyFrom.Kfpmnt;
                  this.Kfpcru= copyFrom.Kfpcru;
                  this.Kfpcrd= copyFrom.Kfpcrd;
                  this.Kfpcrh= copyFrom.Kfpcrh;
                  this.Kfpmaju= copyFrom.Kfpmaju;
                  this.Kfpmajd= copyFrom.Kfpmajd;
                  this.Kfpmajh= copyFrom.Kfpmajh;
                  this.Kfptds= copyFrom.Kfptds;
                  this.Kfptyi= copyFrom.Kfptyi;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kfpid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kfptyp { get; set; } 
            
            ///<summary> IPB/ALX </summary>
            public string Kfpipb { get; set; } 
            
            ///<summary> Aliment/version </summary>
            public int Kfpalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kfpavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kfphin { get; set; } 
            
            ///<summary> ID crédit bailleur </summary>
            public int Kfpidcb { get; set; } 
            
            ///<summary> Type de financement </summary>
            public string Kfptfi { get; set; } 
            
            ///<summary> lien KPDESI </summary>
            public Int64 Kfpdesi { get; set; } 
            
            ///<summary> Référence </summary>
            public string Kfpref { get; set; } 
            
            ///<summary> Date échéance </summary>
            public int Kfpdech { get; set; } 
            
            ///<summary> Montant financé </summary>
            public Decimal Kfpmnt { get; set; } 
            
            ///<summary> Création user </summary>
            public string Kfpcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kfpcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kfpcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kfpmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kfpmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kfpmajh { get; set; } 
            
            ///<summary> Type de destinataire(Assuré...) </summary>
            public string Kfptds { get; set; } 
            
            ///<summary> Type intervenant (Cie Expert Avoc..) </summary>
            public string Kfptyi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOpp  x=this,  y=obj as KpOpp;
            if( y == default(KpOpp) ) return false;
            return (
                    x.Kfpid==y.Kfpid
                    && x.Kfptyp==y.Kfptyp
                    && x.Kfpipb==y.Kfpipb
                    && x.Kfpalx==y.Kfpalx
                    && x.Kfpidcb==y.Kfpidcb
                    && x.Kfptfi==y.Kfptfi
                    && x.Kfpdesi==y.Kfpdesi
                    && x.Kfpref==y.Kfpref
                    && x.Kfpdech==y.Kfpdech
                    && x.Kfpmnt==y.Kfpmnt
                    && x.Kfpcru==y.Kfpcru
                    && x.Kfpcrd==y.Kfpcrd
                    && x.Kfpcrh==y.Kfpcrh
                    && x.Kfpmaju==y.Kfpmaju
                    && x.Kfpmajd==y.Kfpmajd
                    && x.Kfpmajh==y.Kfpmajh
                    && x.Kfptds==y.Kfptds
                    && x.Kfptyi==y.Kfptyi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((
                       17 + (this.Kfpid.GetHashCode() ) 
                      * 23 ) + (this.Kfptyp?? "").GetHashCode()
                      * 23 ) + (this.Kfpipb?? "").GetHashCode()
                      * 23 ) + (this.Kfpalx.GetHashCode() ) 
                      * 23 ) + (this.Kfpidcb.GetHashCode() ) 
                      * 23 ) + (this.Kfptfi?? "").GetHashCode()
                      * 23 ) + (this.Kfpdesi.GetHashCode() ) 
                      * 23 ) + (this.Kfpref?? "").GetHashCode()
                      * 23 ) + (this.Kfpdech.GetHashCode() ) 
                      * 23 ) + (this.Kfpmnt.GetHashCode() ) 
                      * 23 ) + (this.Kfpcru?? "").GetHashCode()
                      * 23 ) + (this.Kfpcrd.GetHashCode() ) 
                      * 23 ) + (this.Kfpcrh.GetHashCode() ) 
                      * 23 ) + (this.Kfpmaju?? "").GetHashCode()
                      * 23 ) + (this.Kfpmajd.GetHashCode() ) 
                      * 23 ) + (this.Kfpmajh.GetHashCode() ) 
                      * 23 ) + (this.Kfptds?? "").GetHashCode()
                      * 23 ) + (this.Kfptyi?? "").GetHashCode()                   );
           }
        }
    }
}
