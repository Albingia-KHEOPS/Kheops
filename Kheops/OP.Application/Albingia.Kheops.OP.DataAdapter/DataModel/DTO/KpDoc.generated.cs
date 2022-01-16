using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPDOC
    public partial class KpDoc  {
             //KPDOC
             //KPDOCW

            ///<summary>Public empty contructor</summary>
            public KpDoc() {}
            ///<summary>Public empty contructor</summary>
            public KpDoc(KpDoc copyFrom) 
            {
                  this.Keqid= copyFrom.Keqid;
                  this.Keqtyp= copyFrom.Keqtyp;
                  this.Keqipb= copyFrom.Keqipb;
                  this.Keqalx= copyFrom.Keqalx;
                  this.Keqsua= copyFrom.Keqsua;
                  this.Keqnum= copyFrom.Keqnum;
                  this.Keqsbr= copyFrom.Keqsbr;
                  this.Keqserv= copyFrom.Keqserv;
                  this.Keqactg= copyFrom.Keqactg;
                  this.Keqactn= copyFrom.Keqactn;
                  this.Keqeco= copyFrom.Keqeco;
                  this.Keqavn= copyFrom.Keqavn;
                  this.Keqetap= copyFrom.Keqetap;
                  this.Keqkemid= copyFrom.Keqkemid;
                  this.Keqord= copyFrom.Keqord;
                  this.Keqtgl= copyFrom.Keqtgl;
                  this.Keqtdoc= copyFrom.Keqtdoc;
                  this.Keqkesid= copyFrom.Keqkesid;
                  this.Keqajt= copyFrom.Keqajt;
                  this.Keqtrs= copyFrom.Keqtrs;
                  this.Keqmait= copyFrom.Keqmait;
                  this.Keqlien= copyFrom.Keqlien;
                  this.Keqcdoc= copyFrom.Keqcdoc;
                  this.Keqver= copyFrom.Keqver;
                  this.Keqlib= copyFrom.Keqlib;
                  this.Keqnta= copyFrom.Keqnta;
                  this.Keqdacc= copyFrom.Keqdacc;
                  this.Keqtae= copyFrom.Keqtae;
                  this.Keqnom= copyFrom.Keqnom;
                  this.Keqchm= copyFrom.Keqchm;
                  this.Keqstu= copyFrom.Keqstu;
                  this.Keqsit= copyFrom.Keqsit;
                  this.Keqstd= copyFrom.Keqstd;
                  this.Keqsth= copyFrom.Keqsth;
                  this.Keqenvu= copyFrom.Keqenvu;
                  this.Keqenvd= copyFrom.Keqenvd;
                  this.Keqenvh= copyFrom.Keqenvh;
                  this.Keqtedi= copyFrom.Keqtedi;
                  this.Keqorid= copyFrom.Keqorid;
                  this.Keqtyds= copyFrom.Keqtyds;
                  this.Keqtyi= copyFrom.Keqtyi;
                  this.Keqids= copyFrom.Keqids;
                  this.Keqinl= copyFrom.Keqinl;
                  this.Keqnbex= copyFrom.Keqnbex;
                  this.Keqcru= copyFrom.Keqcru;
                  this.Keqcrd= copyFrom.Keqcrd;
                  this.Keqcrh= copyFrom.Keqcrh;
                  this.Keqmaju= copyFrom.Keqmaju;
                  this.Keqmajd= copyFrom.Keqmajd;
                  this.Keqmajh= copyFrom.Keqmajh;
                  this.Keqstg= copyFrom.Keqstg;
                  this.Keqdimp= copyFrom.Keqdimp;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Keqid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Keqtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Keqipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Keqalx { get; set; } 
            
            ///<summary> Sinistre AA </summary>
            public int Keqsua { get; set; } 
            
            ///<summary> Sinistre N° </summary>
            public int Keqnum { get; set; } 
            
            ///<summary> Sinistre Sbr </summary>
            public string Keqsbr { get; set; } 
            
            ///<summary> Service  (Produ,Sinistre...) </summary>
            public string Keqserv { get; set; } 
            
            ///<summary> Acte de gestion </summary>
            public string Keqactg { get; set; } 
            
            ///<summary> N° acte de gestion </summary>
            public int Keqactn { get; set; } 
            
            ///<summary> En cours O/N </summary>
            public string Keqeco { get; set; } 
            
            ///<summary> N° Avenant </summary>
            public int Keqavn { get; set; } 
            
            ///<summary> Etape </summary>
            public string Keqetap { get; set; } 
            
            ///<summary> Lien KPDOCLD </summary>
            public Int64 Keqkemid { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public Decimal Keqord { get; set; } 
            
            ///<summary> Type de Génération Généré ou Libre </summary>
            public string Keqtgl { get; set; } 
            
            ///<summary> Type Document LETTYP LIBRE CP ..... </summary>
            public string Keqtdoc { get; set; } 
            
            ///<summary> Si Complément   Lien KPDOCTX Texte </summary>
            public Int64 Keqkesid { get; set; } 
            
            ///<summary> Ajouté O/N </summary>
            public string Keqajt { get; set; } 
            
            ///<summary> Transformé O/N </summary>
            public string Keqtrs { get; set; } 
            
            ///<summary> Table Maître ou Origine (KCLAUSE... </summary>
            public string Keqmait { get; set; } 
            
            ///<summary> Lien table maître/Origine (KCLAUSE.. </summary>
            public Int64 Keqlien { get; set; } 
            
            ///<summary> Code document (si lettyp (3 x 5)) </summary>
            public string Keqcdoc { get; set; } 
            
            ///<summary> N°Version </summary>
            public int Keqver { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Keqlib { get; set; } 
            
            ///<summary> Nature de la génération (O/P/S) </summary>
            public string Keqnta { get; set; } 
            
            ///<summary> Document Accompagnant O/N </summary>
            public string Keqdacc { get; set; } 
            
            ///<summary> Action enchainée </summary>
            public string Keqtae { get; set; } 
            
            ///<summary> Nom du document </summary>
            public string Keqnom { get; set; } 
            
            ///<summary> Chemin complet </summary>
            public string Keqchm { get; set; } 
            
            ///<summary> Situation User </summary>
            public string Keqstu { get; set; } 
            
            ///<summary> Situation Code </summary>
            public string Keqsit { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Keqstd { get; set; } 
            
            ///<summary> Situation Heure </summary>
            public int Keqsth { get; set; } 
            
            ///<summary> Envoi User </summary>
            public string Keqenvu { get; set; } 
            
            ///<summary> Envoi Date </summary>
            public int Keqenvd { get; set; } 
            
            ///<summary> Envoi Heure </summary>
            public int Keqenvh { get; set; } 
            
            ///<summary> Typo Edit Originale,Copie,Duplicata </summary>
            public string Keqtedi { get; set; } 
            
            ///<summary> Lien KPDOC original si Copie/Dup </summary>
            public Int64 Keqorid { get; set; } 
            
            ///<summary> Type de destinataire sur document </summary>
            public string Keqtyds { get; set; } 
            
            ///<summary> Type intervenant sur document </summary>
            public string Keqtyi { get; set; } 
            
            ///<summary> Id destinataire sur document </summary>
            public int Keqids { get; set; } 
            
            ///<summary> Code interlocuteur sur document </summary>
            public int Keqinl { get; set; } 
            
            ///<summary> Nombre d'exemplaires supplémentaires </summary>
            public int Keqnbex { get; set; } 
            
            ///<summary> Création User </summary>
            public string Keqcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Keqcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Keqcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Keqmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Keqmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Keqmajh { get; set; } 
            
            ///<summary> Statut génération G à jour/ Modif ap </summary>
            public string Keqstg { get; set; } 
            
            ///<summary> Imprimable  O/N </summary>
            public string Keqdimp { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpDoc  x=this,  y=obj as KpDoc;
            if( y == default(KpDoc) ) return false;
            return (
                    x.Keqid==y.Keqid
                    && x.Keqtyp==y.Keqtyp
                    && x.Keqipb==y.Keqipb
                    && x.Keqalx==y.Keqalx
                    && x.Keqsua==y.Keqsua
                    && x.Keqnum==y.Keqnum
                    && x.Keqsbr==y.Keqsbr
                    && x.Keqserv==y.Keqserv
                    && x.Keqactg==y.Keqactg
                    && x.Keqactn==y.Keqactn
                    && x.Keqeco==y.Keqeco
                    && x.Keqavn==y.Keqavn
                    && x.Keqetap==y.Keqetap
                    && x.Keqkemid==y.Keqkemid
                    && x.Keqord==y.Keqord
                    && x.Keqtgl==y.Keqtgl
                    && x.Keqtdoc==y.Keqtdoc
                    && x.Keqkesid==y.Keqkesid
                    && x.Keqajt==y.Keqajt
                    && x.Keqtrs==y.Keqtrs
                    && x.Keqmait==y.Keqmait
                    && x.Keqlien==y.Keqlien
                    && x.Keqcdoc==y.Keqcdoc
                    && x.Keqver==y.Keqver
                    && x.Keqlib==y.Keqlib
                    && x.Keqnta==y.Keqnta
                    && x.Keqdacc==y.Keqdacc
                    && x.Keqtae==y.Keqtae
                    && x.Keqnom==y.Keqnom
                    && x.Keqchm==y.Keqchm
                    && x.Keqstu==y.Keqstu
                    && x.Keqsit==y.Keqsit
                    && x.Keqstd==y.Keqstd
                    && x.Keqsth==y.Keqsth
                    && x.Keqenvu==y.Keqenvu
                    && x.Keqenvd==y.Keqenvd
                    && x.Keqenvh==y.Keqenvh
                    && x.Keqtedi==y.Keqtedi
                    && x.Keqorid==y.Keqorid
                    && x.Keqtyds==y.Keqtyds
                    && x.Keqtyi==y.Keqtyi
                    && x.Keqids==y.Keqids
                    && x.Keqinl==y.Keqinl
                    && x.Keqnbex==y.Keqnbex
                    && x.Keqcru==y.Keqcru
                    && x.Keqcrd==y.Keqcrd
                    && x.Keqcrh==y.Keqcrh
                    && x.Keqmaju==y.Keqmaju
                    && x.Keqmajd==y.Keqmajd
                    && x.Keqmajh==y.Keqmajh
                    && x.Keqstg==y.Keqstg
                    && x.Keqdimp==y.Keqdimp  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Keqid.GetHashCode() ) 
                      * 23 ) + (this.Keqtyp?? "").GetHashCode()
                      * 23 ) + (this.Keqipb?? "").GetHashCode()
                      * 23 ) + (this.Keqalx.GetHashCode() ) 
                      * 23 ) + (this.Keqsua.GetHashCode() ) 
                      * 23 ) + (this.Keqnum.GetHashCode() ) 
                      * 23 ) + (this.Keqsbr?? "").GetHashCode()
                      * 23 ) + (this.Keqserv?? "").GetHashCode()
                      * 23 ) + (this.Keqactg?? "").GetHashCode()
                      * 23 ) + (this.Keqactn.GetHashCode() ) 
                      * 23 ) + (this.Keqeco?? "").GetHashCode()
                      * 23 ) + (this.Keqavn.GetHashCode() ) 
                      * 23 ) + (this.Keqetap?? "").GetHashCode()
                      * 23 ) + (this.Keqkemid.GetHashCode() ) 
                      * 23 ) + (this.Keqord.GetHashCode() ) 
                      * 23 ) + (this.Keqtgl?? "").GetHashCode()
                      * 23 ) + (this.Keqtdoc?? "").GetHashCode()
                      * 23 ) + (this.Keqkesid.GetHashCode() ) 
                      * 23 ) + (this.Keqajt?? "").GetHashCode()
                      * 23 ) + (this.Keqtrs?? "").GetHashCode()
                      * 23 ) + (this.Keqmait?? "").GetHashCode()
                      * 23 ) + (this.Keqlien.GetHashCode() ) 
                      * 23 ) + (this.Keqcdoc?? "").GetHashCode()
                      * 23 ) + (this.Keqver.GetHashCode() ) 
                      * 23 ) + (this.Keqlib?? "").GetHashCode()
                      * 23 ) + (this.Keqnta?? "").GetHashCode()
                      * 23 ) + (this.Keqdacc?? "").GetHashCode()
                      * 23 ) + (this.Keqtae?? "").GetHashCode()
                      * 23 ) + (this.Keqnom?? "").GetHashCode()
                      * 23 ) + (this.Keqchm?? "").GetHashCode()
                      * 23 ) + (this.Keqstu?? "").GetHashCode()
                      * 23 ) + (this.Keqsit?? "").GetHashCode()
                      * 23 ) + (this.Keqstd.GetHashCode() ) 
                      * 23 ) + (this.Keqsth.GetHashCode() ) 
                      * 23 ) + (this.Keqenvu?? "").GetHashCode()
                      * 23 ) + (this.Keqenvd.GetHashCode() ) 
                      * 23 ) + (this.Keqenvh.GetHashCode() ) 
                      * 23 ) + (this.Keqtedi?? "").GetHashCode()
                      * 23 ) + (this.Keqorid.GetHashCode() ) 
                      * 23 ) + (this.Keqtyds?? "").GetHashCode()
                      * 23 ) + (this.Keqtyi?? "").GetHashCode()
                      * 23 ) + (this.Keqids.GetHashCode() ) 
                      * 23 ) + (this.Keqinl.GetHashCode() ) 
                      * 23 ) + (this.Keqnbex.GetHashCode() ) 
                      * 23 ) + (this.Keqcru?? "").GetHashCode()
                      * 23 ) + (this.Keqcrd.GetHashCode() ) 
                      * 23 ) + (this.Keqcrh.GetHashCode() ) 
                      * 23 ) + (this.Keqmaju?? "").GetHashCode()
                      * 23 ) + (this.Keqmajd.GetHashCode() ) 
                      * 23 ) + (this.Keqmajh.GetHashCode() ) 
                      * 23 ) + (this.Keqstg?? "").GetHashCode()
                      * 23 ) + (this.Keqdimp?? "").GetHashCode()                   );
           }
        }
    }
}
