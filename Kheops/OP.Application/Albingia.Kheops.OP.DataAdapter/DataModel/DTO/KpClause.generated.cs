using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPCLAUSE
    public partial class KpClause  {
             //HPCLAUSE
             //KPCLAUSE

            ///<summary>Public empty contructor</summary>
            public KpClause() {}
            ///<summary>Public empty contructor</summary>
            public KpClause(KpClause copyFrom) 
            {
                  this.Kcaid= copyFrom.Kcaid;
                  this.Kcatyp= copyFrom.Kcatyp;
                  this.Kcaipb= copyFrom.Kcaipb;
                  this.Kcaalx= copyFrom.Kcaalx;
                  this.Kcaavn= copyFrom.Kcaavn;
                  this.Kcahin= copyFrom.Kcahin;
                  this.Kcaetape= copyFrom.Kcaetape;
                  this.Kcaperi= copyFrom.Kcaperi;
                  this.Kcarsq= copyFrom.Kcarsq;
                  this.Kcaobj= copyFrom.Kcaobj;
                  this.Kcainven= copyFrom.Kcainven;
                  this.Kcainlgn= copyFrom.Kcainlgn;
                  this.Kcafor= copyFrom.Kcafor;
                  this.Kcaopt= copyFrom.Kcaopt;
                  this.Kcagar= copyFrom.Kcagar;
                  this.Kcactx= copyFrom.Kcactx;
                  this.Kcaajt= copyFrom.Kcaajt;
                  this.Kcanta= copyFrom.Kcanta;
                  this.Kcakduid= copyFrom.Kcakduid;
                  this.Kcaclnm1= copyFrom.Kcaclnm1;
                  this.Kcaclnm2= copyFrom.Kcaclnm2;
                  this.Kcaclnm3= copyFrom.Kcaclnm3;
                  this.Kcaver= copyFrom.Kcaver;
                  this.Kcatxl= copyFrom.Kcatxl;
                  this.Kcamer= copyFrom.Kcamer;
                  this.Kcadoc= copyFrom.Kcadoc;
                  this.Kcachi= copyFrom.Kcachi;
                  this.Kcachis= copyFrom.Kcachis;
                  this.Kcaimp= copyFrom.Kcaimp;
                  this.Kcacxi= copyFrom.Kcacxi;
                  this.Kcaian= copyFrom.Kcaian;
                  this.Kcaiac= copyFrom.Kcaiac;
                  this.Kcasit= copyFrom.Kcasit;
                  this.Kcasitd= copyFrom.Kcasitd;
                  this.Kcaavnc= copyFrom.Kcaavnc;
                  this.Kcacrd= copyFrom.Kcacrd;
                  this.Kcaavnm= copyFrom.Kcaavnm;
                  this.Kcamajd= copyFrom.Kcamajd;
                  this.Kcaspa= copyFrom.Kcaspa;
                  this.Kcatypo= copyFrom.Kcatypo;
                  this.Kcaaim= copyFrom.Kcaaim;
                  this.Kcatae= copyFrom.Kcatae;
                  this.Kcaelgo= copyFrom.Kcaelgo;
                  this.Kcaelgi= copyFrom.Kcaelgi;
                  this.Kcaxtl= copyFrom.Kcaxtl;
                  this.Kcatypd= copyFrom.Kcatypd;
                  this.Kcaetaff= copyFrom.Kcaetaff;
                  this.Kcaxtlm= copyFrom.Kcaxtlm;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kcaid { get; set; } 
            
            ///<summary> TYP O/P </summary>
            public string Kcatyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kcaipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kcaalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kcaavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kcahin { get; set; } 
            
            ///<summary> Etape de génération </summary>
            public string Kcaetape { get; set; } 
            
            ///<summary> Périmètre  BASE RISQUE .... </summary>
            public string Kcaperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kcarsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kcaobj { get; set; } 
            
            ///<summary> ID KPINVEN </summary>
            public Int64 Kcainven { get; set; } 
            
            ///<summary> N° de ligne inventaire </summary>
            public int Kcainlgn { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kcafor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kcaopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kcagar { get; set; } 
            
            ///<summary> Contexte </summary>
            public string Kcactx { get; set; } 
            
            ///<summary> Ajoutée O/N </summary>
            public string Kcaajt { get; set; } 
            
            ///<summary> Nature de la clause </summary>
            public string Kcanta { get; set; } 
            
            ///<summary> Clause lien KCLAUSE </summary>
            public Int64 Kcakduid { get; set; } 
            
            ///<summary> Code clause Nm1 </summary>
            public string Kcaclnm1 { get; set; } 
            
            ///<summary> Code clause Nm2 </summary>
            public string Kcaclnm2 { get; set; } 
            
            ///<summary> Code Clause Nm3 </summary>
            public int Kcaclnm3 { get; set; } 
            
            ///<summary> N° Version </summary>
            public int Kcaver { get; set; } 
            
            ///<summary> Doc libre  lien KPDOC KPDOCEXT </summary>
            public Int64 Kcatxl { get; set; } 
            
            ///<summary> CLause mère Lien KCLAUSE </summary>
            public Int64 Kcamer { get; set; } 
            
            ///<summary> Document Impression CP CG CS... </summary>
            public string Kcadoc { get; set; } 
            
            ///<summary> Chapitre impression </summary>
            public string Kcachi { get; set; } 
            
            ///<summary> Sous chapitre impression </summary>
            public string Kcachis { get; set; } 
            
            ///<summary> N° Impression </summary>
            public Int64 Kcaimp { get; set; } 
            
            ///<summary> N° ordonnancement </summary>
            public Decimal Kcacxi { get; set; } 
            
            ///<summary> Impression en annexe O/N </summary>
            public string Kcaian { get; set; } 
            
            ///<summary> Code annexe </summary>
            public string Kcaiac { get; set; } 
            
            ///<summary> Code situation </summary>
            public string Kcasit { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kcasitd { get; set; } 
            
            ///<summary> Avenant de création </summary>
            public int Kcaavnc { get; set; } 
            
            ///<summary> Création date </summary>
            public int Kcacrd { get; set; } 
            
            ///<summary> Avenant de modification </summary>
            public int Kcaavnm { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kcamajd { get; set; } 
            
            ///<summary> Clause spécifique Avenant O/N </summary>
            public string Kcaspa { get; set; } 
            
            ///<summary> Typologie clause Anodine sensible </summary>
            public string Kcatypo { get; set; } 
            
            ///<summary> Attribut d'impression Gras souligné </summary>
            public string Kcaaim { get; set; } 
            
            ///<summary> Action enchainée </summary>
            public string Kcatae { get; set; } 
            
            ///<summary> Elément générateur  Origine </summary>
            public string Kcaelgo { get; set; } 
            
            ///<summary> Elément générateur ID </summary>
            public Int64 Kcaelgi { get; set; } 
            
            ///<summary> Texte libre O/N </summary>
            public string Kcaxtl { get; set; } 
            
            ///<summary> Doc Ajouté/Généré/Externe </summary>
            public string Kcatypd { get; set; } 
            
            ///<summary> Etape d'affichage </summary>
            public string Kcaetaff { get; set; } 
            
            ///<summary> Texte libre modifié O/N </summary>
            public string Kcaxtlm { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpClause  x=this,  y=obj as KpClause;
            if( y == default(KpClause) ) return false;
            return (
                    x.Kcaid==y.Kcaid
                    && x.Kcatyp==y.Kcatyp
                    && x.Kcaipb==y.Kcaipb
                    && x.Kcaalx==y.Kcaalx
                    && x.Kcaetape==y.Kcaetape
                    && x.Kcaperi==y.Kcaperi
                    && x.Kcarsq==y.Kcarsq
                    && x.Kcaobj==y.Kcaobj
                    && x.Kcainven==y.Kcainven
                    && x.Kcainlgn==y.Kcainlgn
                    && x.Kcafor==y.Kcafor
                    && x.Kcaopt==y.Kcaopt
                    && x.Kcagar==y.Kcagar
                    && x.Kcactx==y.Kcactx
                    && x.Kcaajt==y.Kcaajt
                    && x.Kcanta==y.Kcanta
                    && x.Kcakduid==y.Kcakduid
                    && x.Kcaclnm1==y.Kcaclnm1
                    && x.Kcaclnm2==y.Kcaclnm2
                    && x.Kcaclnm3==y.Kcaclnm3
                    && x.Kcaver==y.Kcaver
                    && x.Kcatxl==y.Kcatxl
                    && x.Kcamer==y.Kcamer
                    && x.Kcadoc==y.Kcadoc
                    && x.Kcachi==y.Kcachi
                    && x.Kcachis==y.Kcachis
                    && x.Kcaimp==y.Kcaimp
                    && x.Kcacxi==y.Kcacxi
                    && x.Kcaian==y.Kcaian
                    && x.Kcaiac==y.Kcaiac
                    && x.Kcasit==y.Kcasit
                    && x.Kcasitd==y.Kcasitd
                    && x.Kcaavnc==y.Kcaavnc
                    && x.Kcacrd==y.Kcacrd
                    && x.Kcaavnm==y.Kcaavnm
                    && x.Kcamajd==y.Kcamajd
                    && x.Kcaspa==y.Kcaspa
                    && x.Kcatypo==y.Kcatypo
                    && x.Kcaaim==y.Kcaaim
                    && x.Kcatae==y.Kcatae
                    && x.Kcaelgo==y.Kcaelgo
                    && x.Kcaelgi==y.Kcaelgi
                    && x.Kcaxtl==y.Kcaxtl
                    && x.Kcatypd==y.Kcatypd
                    && x.Kcaetaff==y.Kcaetaff
                    && x.Kcaxtlm==y.Kcaxtlm  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kcaid.GetHashCode() ) 
                      * 23 ) + (this.Kcatyp?? "").GetHashCode()
                      * 23 ) + (this.Kcaipb?? "").GetHashCode()
                      * 23 ) + (this.Kcaalx.GetHashCode() ) 
                      * 23 ) + (this.Kcaetape?? "").GetHashCode()
                      * 23 ) + (this.Kcaperi?? "").GetHashCode()
                      * 23 ) + (this.Kcarsq.GetHashCode() ) 
                      * 23 ) + (this.Kcaobj.GetHashCode() ) 
                      * 23 ) + (this.Kcainven.GetHashCode() ) 
                      * 23 ) + (this.Kcainlgn.GetHashCode() ) 
                      * 23 ) + (this.Kcafor.GetHashCode() ) 
                      * 23 ) + (this.Kcaopt.GetHashCode() ) 
                      * 23 ) + (this.Kcagar?? "").GetHashCode()
                      * 23 ) + (this.Kcactx?? "").GetHashCode()
                      * 23 ) + (this.Kcaajt?? "").GetHashCode()
                      * 23 ) + (this.Kcanta?? "").GetHashCode()
                      * 23 ) + (this.Kcakduid.GetHashCode() ) 
                      * 23 ) + (this.Kcaclnm1?? "").GetHashCode()
                      * 23 ) + (this.Kcaclnm2?? "").GetHashCode()
                      * 23 ) + (this.Kcaclnm3.GetHashCode() ) 
                      * 23 ) + (this.Kcaver.GetHashCode() ) 
                      * 23 ) + (this.Kcatxl.GetHashCode() ) 
                      * 23 ) + (this.Kcamer.GetHashCode() ) 
                      * 23 ) + (this.Kcadoc?? "").GetHashCode()
                      * 23 ) + (this.Kcachi?? "").GetHashCode()
                      * 23 ) + (this.Kcachis?? "").GetHashCode()
                      * 23 ) + (this.Kcaimp.GetHashCode() ) 
                      * 23 ) + (this.Kcacxi.GetHashCode() ) 
                      * 23 ) + (this.Kcaian?? "").GetHashCode()
                      * 23 ) + (this.Kcaiac?? "").GetHashCode()
                      * 23 ) + (this.Kcasit?? "").GetHashCode()
                      * 23 ) + (this.Kcasitd.GetHashCode() ) 
                      * 23 ) + (this.Kcaavnc.GetHashCode() ) 
                      * 23 ) + (this.Kcacrd.GetHashCode() ) 
                      * 23 ) + (this.Kcaavnm.GetHashCode() ) 
                      * 23 ) + (this.Kcamajd.GetHashCode() ) 
                      * 23 ) + (this.Kcaspa?? "").GetHashCode()
                      * 23 ) + (this.Kcatypo?? "").GetHashCode()
                      * 23 ) + (this.Kcaaim?? "").GetHashCode()
                      * 23 ) + (this.Kcatae?? "").GetHashCode()
                      * 23 ) + (this.Kcaelgo?? "").GetHashCode()
                      * 23 ) + (this.Kcaelgi.GetHashCode() ) 
                      * 23 ) + (this.Kcaxtl?? "").GetHashCode()
                      * 23 ) + (this.Kcatypd?? "").GetHashCode()
                      * 23 ) + (this.Kcaetaff?? "").GetHashCode()
                      * 23 ) + (this.Kcaxtlm?? "").GetHashCode()                   );
           }
        }
    }
}
