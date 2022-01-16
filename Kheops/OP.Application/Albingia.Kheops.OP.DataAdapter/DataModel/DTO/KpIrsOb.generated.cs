using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPIRSOB
    public partial class KpIrsOb  {
             //HPIRSOB
             //KPIRSOB

            ///<summary>Public empty contructor</summary>
            public KpIrsOb() {}
            ///<summary>Public empty contructor</summary>
            public KpIrsOb(KpIrsOb copyFrom) 
            {
                  this.Kfatyp= copyFrom.Kfatyp;
                  this.Kfaipb= copyFrom.Kfaipb;
                  this.Kfaalx= copyFrom.Kfaalx;
                  this.Kfaavn= copyFrom.Kfaavn;
                  this.Kfahin= copyFrom.Kfahin;
                  this.Kfarsq= copyFrom.Kfarsq;
                  this.Kfaobj= copyFrom.Kfaobj;
                  this.Kfanats= copyFrom.Kfanats;
                  this.Kfanega= copyFrom.Kfanega;
                  this.Kfafrqe= copyFrom.Kfafrqe;
                  this.Kfanbpa= copyFrom.Kfanbpa;
                  this.Kfanbex= copyFrom.Kfanbex;
                  this.Kfanbvi= copyFrom.Kfanbvi;
                  this.Kfagn08= copyFrom.Kfagn08;
                  this.Kfagn09= copyFrom.Kfagn09;
                  this.Kfagn10= copyFrom.Kfagn10;
                  this.Kfanbin= copyFrom.Kfanbin;
                  this.Kfanbpe= copyFrom.Kfanbpe;
                  this.Kfagct= copyFrom.Kfagct;
                  this.Kfanbem= copyFrom.Kfanbem;
                  this.Kfatytn= copyFrom.Kfatytn;
                  this.Kfanmdf= copyFrom.Kfanmdf;
                  this.Kfafent= copyFrom.Kfafent;
                  this.Kfafsvt= copyFrom.Kfafsvt;
                  this.Kfanmsc= copyFrom.Kfanmsc;
                  this.Kfalabd= copyFrom.Kfalabd;
                  this.Kfanai= copyFrom.Kfanai;
                  this.Kfalma= copyFrom.Kfalma;
                  this.Kfaifp= copyFrom.Kfaifp;
                  this.Kfathf= copyFrom.Kfathf;
                  this.Kfatu1= copyFrom.Kfatu1;
                  this.Kfatu2= copyFrom.Kfatu2;
                  this.Kfaasc= copyFrom.Kfaasc;
                  this.Kfaautl= copyFrom.Kfaautl;
                  this.Kfaqmd= copyFrom.Kfaqmd;
                  this.Kfasurf= copyFrom.Kfasurf;
                  this.Kfavmc= copyFrom.Kfavmc;
                  this.Kfaprol= copyFrom.Kfaprol;
                  this.Kfadepd= copyFrom.Kfadepd;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kfatyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kfaipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kfaalx { get; set; } 
            
            ///<summary> N° Avenant </summary>
            public int Kfaavn { get; set; } 
            
            ///<summary> N° Histo par avenant </summary>
            public int Kfahin { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kfarsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kfaobj { get; set; } 
            
            ///<summary> Nature du support </summary>
            public string Kfanats { get; set; } 
            
            ///<summary> Type de négatif </summary>
            public string Kfanega { get; set; } 
            
            ///<summary> Fréquence d'envoi </summary>
            public string Kfafrqe { get; set; } 
            
            ///<summary> Nb de participants </summary>
            public int Kfanbpa { get; set; } 
            
            ///<summary> Nombre d'exposants </summary>
            public int Kfanbex { get; set; } 
            
            ///<summary> Nb visiteurs </summary>
            public int Kfanbvi { get; set; } 
            
            ///<summary> GEN Nb élèves </summary>
            public int Kfagn08 { get; set; } 
            
            ///<summary> GEN nature études </summary>
            public string Kfagn09 { get; set; } 
            
            ///<summary> GEN durée cycle </summary>
            public string Kfagn10 { get; set; } 
            
            ///<summary> Nb invités </summary>
            public int Kfanbin { get; set; } 
            
            ///<summary> Nombre de personnes </summary>
            public int Kfanbpe { get; set; } 
            
            ///<summary> Catégorie ou groupe assurés </summary>
            public string Kfagct { get; set; } 
            
            ///<summary> Nombre d'émissions </summary>
            public int Kfanbem { get; set; } 
            
            ///<summary> Type de tournage </summary>
            public string Kfatytn { get; set; } 
            
            ///<summary> Nom du diffuseur </summary>
            public string Kfanmdf { get; set; } 
            
            ///<summary> Fréquence d'envoi des rushs (Texte) </summary>
            public string Kfafent { get; set; } 
            
            ///<summary> Fréquence des sauvegardes (Texte) </summary>
            public string Kfafsvt { get; set; } 
            
            ///<summary> Société en charge Post-prod </summary>
            public string Kfanmsc { get; set; } 
            
            ///<summary> Nom du labo de développement </summary>
            public string Kfalabd { get; set; } 
            
            ///<summary> Date naissance </summary>
            public int Kfanai { get; set; } 
            
            ///<summary> IA:Report lim âge </summary>
            public int Kfalma { get; set; } 
            
            ///<summary> IA: Taux infirmité préexistante </summary>
            public Decimal Kfaifp { get; set; } 
            
            ///<summary> RC:Tournage hors France : pays </summary>
            public string Kfathf { get; set; } 
            
            ///<summary> RC:Tournage USA/Canada : fréquence </summary>
            public string Kfatu1 { get; set; } 
            
            ///<summary> RC:Tournage USA/Canada : Nature </summary>
            public string Kfatu2 { get; set; } 
            
            ///<summary> RC:Présence d'un ascenseur (O/N) </summary>
            public string Kfaasc { get; set; } 
            
            ///<summary> Autre support libellé ExLien KPIDESI </summary>
            public string Kfaautl { get; set; } 
            
            ///<summary> Présence questionnaire médical O/N </summary>
            public string Kfaqmd { get; set; } 
            
            ///<summary> Superficie </summary>
            public Decimal Kfasurf { get; set; } 
            
            ///<summary> Valeur au m² </summary>
            public Decimal Kfavmc { get; set; } 
            
            ///<summary> Prof libérale O/N (!!rég.taxe) </summary>
            public string Kfaprol { get; set; } 
            
            ///<summary> Durée déplac.si <>180j </summary>
            public int Kfadepd { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpIrsOb  x=this,  y=obj as KpIrsOb;
            if( y == default(KpIrsOb) ) return false;
            return (
                    x.Kfatyp==y.Kfatyp
                    && x.Kfaipb==y.Kfaipb
                    && x.Kfaalx==y.Kfaalx
                    && x.Kfaavn==y.Kfaavn
                    && x.Kfahin==y.Kfahin
                    && x.Kfarsq==y.Kfarsq
                    && x.Kfaobj==y.Kfaobj
                    && x.Kfanats==y.Kfanats
                    && x.Kfanega==y.Kfanega
                    && x.Kfafrqe==y.Kfafrqe
                    && x.Kfanbpa==y.Kfanbpa
                    && x.Kfanbex==y.Kfanbex
                    && x.Kfanbvi==y.Kfanbvi
                    && x.Kfagn08==y.Kfagn08
                    && x.Kfagn09==y.Kfagn09
                    && x.Kfagn10==y.Kfagn10
                    && x.Kfanbin==y.Kfanbin
                    && x.Kfanbpe==y.Kfanbpe
                    && x.Kfagct==y.Kfagct
                    && x.Kfanbem==y.Kfanbem
                    && x.Kfatytn==y.Kfatytn
                    && x.Kfanmdf==y.Kfanmdf
                    && x.Kfafent==y.Kfafent
                    && x.Kfafsvt==y.Kfafsvt
                    && x.Kfanmsc==y.Kfanmsc
                    && x.Kfalabd==y.Kfalabd
                    && x.Kfanai==y.Kfanai
                    && x.Kfalma==y.Kfalma
                    && x.Kfaifp==y.Kfaifp
                    && x.Kfathf==y.Kfathf
                    && x.Kfatu1==y.Kfatu1
                    && x.Kfatu2==y.Kfatu2
                    && x.Kfaasc==y.Kfaasc
                    && x.Kfaautl==y.Kfaautl
                    && x.Kfaqmd==y.Kfaqmd
                    && x.Kfasurf==y.Kfasurf
                    && x.Kfavmc==y.Kfavmc
                    && x.Kfaprol==y.Kfaprol
                    && x.Kfadepd==y.Kfadepd  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kfatyp?? "").GetHashCode()
                      * 23 ) + (this.Kfaipb?? "").GetHashCode()
                      * 23 ) + (this.Kfaalx.GetHashCode() ) 
                      * 23 ) + (this.Kfaavn.GetHashCode() ) 
                      * 23 ) + (this.Kfahin.GetHashCode() ) 
                      * 23 ) + (this.Kfarsq.GetHashCode() ) 
                      * 23 ) + (this.Kfaobj.GetHashCode() ) 
                      * 23 ) + (this.Kfanats?? "").GetHashCode()
                      * 23 ) + (this.Kfanega?? "").GetHashCode()
                      * 23 ) + (this.Kfafrqe?? "").GetHashCode()
                      * 23 ) + (this.Kfanbpa.GetHashCode() ) 
                      * 23 ) + (this.Kfanbex.GetHashCode() ) 
                      * 23 ) + (this.Kfanbvi.GetHashCode() ) 
                      * 23 ) + (this.Kfagn08.GetHashCode() ) 
                      * 23 ) + (this.Kfagn09?? "").GetHashCode()
                      * 23 ) + (this.Kfagn10?? "").GetHashCode()
                      * 23 ) + (this.Kfanbin.GetHashCode() ) 
                      * 23 ) + (this.Kfanbpe.GetHashCode() ) 
                      * 23 ) + (this.Kfagct?? "").GetHashCode()
                      * 23 ) + (this.Kfanbem.GetHashCode() ) 
                      * 23 ) + (this.Kfatytn?? "").GetHashCode()
                      * 23 ) + (this.Kfanmdf?? "").GetHashCode()
                      * 23 ) + (this.Kfafent?? "").GetHashCode()
                      * 23 ) + (this.Kfafsvt?? "").GetHashCode()
                      * 23 ) + (this.Kfanmsc?? "").GetHashCode()
                      * 23 ) + (this.Kfalabd?? "").GetHashCode()
                      * 23 ) + (this.Kfanai.GetHashCode() ) 
                      * 23 ) + (this.Kfalma.GetHashCode() ) 
                      * 23 ) + (this.Kfaifp.GetHashCode() ) 
                      * 23 ) + (this.Kfathf?? "").GetHashCode()
                      * 23 ) + (this.Kfatu1?? "").GetHashCode()
                      * 23 ) + (this.Kfatu2?? "").GetHashCode()
                      * 23 ) + (this.Kfaasc?? "").GetHashCode()
                      * 23 ) + (this.Kfaautl?? "").GetHashCode()
                      * 23 ) + (this.Kfaqmd?? "").GetHashCode()
                      * 23 ) + (this.Kfasurf.GetHashCode() ) 
                      * 23 ) + (this.Kfavmc.GetHashCode() ) 
                      * 23 ) + (this.Kfaprol?? "").GetHashCode()
                      * 23 ) + (this.Kfadepd.GetHashCode() )                    );
           }
        }
    }
}
