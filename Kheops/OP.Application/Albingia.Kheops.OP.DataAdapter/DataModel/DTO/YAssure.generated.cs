using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YASSURE
    public partial class YAssure  {
             //YASSURE

            ///<summary>Public empty contructor</summary>
            public YAssure() {}
            ///<summary>Public empty contructor</summary>
            public YAssure(YAssure copyFrom) 
            {
                  this.Asias= copyFrom.Asias;
                  this.Asad1= copyFrom.Asad1;
                  this.Asad2= copyFrom.Asad2;
                  this.Asdep= copyFrom.Asdep;
                  this.Ascpo= copyFrom.Ascpo;
                  this.Asvil= copyFrom.Asvil;
                  this.Aspay= copyFrom.Aspay;
                  this.Ascom= copyFrom.Ascom;
                  this.Asreg= copyFrom.Asreg;
                  this.Asfdc= copyFrom.Asfdc;
                  this.Astel= copyFrom.Astel;
                  this.Astlc= copyFrom.Astlc;
                  this.Assir= copyFrom.Assir;
                  this.Asape= copyFrom.Asape;
                  this.Asapg= copyFrom.Asapg;
                  this.Aslig= copyFrom.Aslig;
                  this.Asgrs= copyFrom.Asgrs;
                  this.Ascra= copyFrom.Ascra;
                  this.Ascrm= copyFrom.Ascrm;
                  this.Ascrj= copyFrom.Ascrj;
                  this.Asusr= copyFrom.Asusr;
                  this.Asmja= copyFrom.Asmja;
                  this.Asmjm= copyFrom.Asmjm;
                  this.Asmjj= copyFrom.Asmjj;
                  this.Asins= copyFrom.Asins;
                  this.Aspub= copyFrom.Aspub;
                  this.Asadh= copyFrom.Asadh;
                  this.Asnic= copyFrom.Asnic;
                  this.Asap5= copyFrom.Asap5;
        
            }        
            
            ///<summary> Identifiant Assuré 10/00 </summary>
            public int Asias { get; set; } 
            
            ///<summary> Adresse 1 </summary>
            public string Asad1 { get; set; } 
            
            ///<summary> Adresse 2 </summary>
            public string Asad2 { get; set; } 
            
            ///<summary> Département </summary>
            public string Asdep { get; set; } 
            
            ///<summary> 3 derniers caractères code postal </summary>
            public string Ascpo { get; set; } 
            
            ///<summary> Ville </summary>
            public string Asvil { get; set; } 
            
            ///<summary> Code pays </summary>
            public string Aspay { get; set; } 
            
            ///<summary> Code commune Arrondissement </summary>
            public string Ascom { get; set; } 
            
            ///<summary> Code région </summary>
            public string Asreg { get; set; } 
            
            ///<summary> Code FRANCE DOC  Transit courrier </summary>
            public string Asfdc { get; set; } 
            
            ///<summary> Téléphone </summary>
            public string Astel { get; set; } 
            
            ///<summary> Télécopie </summary>
            public string Astlc { get; set; } 
            
            ///<summary> Siren </summary>
            public int Assir { get; set; } 
            
            ///<summary> NON UTILISE Ancien code APE </summary>
            public string Asape { get; set; } 
            
            ///<summary> Appartenance à un groupe </summary>
            public string Asapg { get; set; } 
            
            ///<summary> Lien avec le groupe </summary>
            public string Aslig { get; set; } 
            
            ///<summary> Groupe de souscription </summary>
            public string Asgrs { get; set; } 
            
            ///<summary> Année de création </summary>
            public int Ascra { get; set; } 
            
            ///<summary> Mois de création </summary>
            public int Ascrm { get; set; } 
            
            ///<summary> Jour de création </summary>
            public int Ascrj { get; set; } 
            
            ///<summary> User  Màj </summary>
            public string Asusr { get; set; } 
            
            ///<summary> Année Màj </summary>
            public int Asmja { get; set; } 
            
            ///<summary> Mois Màj </summary>
            public int Asmjm { get; set; } 
            
            ///<summary> Jour Màj </summary>
            public int Asmjj { get; set; } 
            
            ///<summary> Code INSEE </summary>
            public string Asins { get; set; } 
            
            ///<summary> Assuré public O/N </summary>
            public string Aspub { get; set; } 
            
            ///<summary> Numéro chrono Adresse </summary>
            public int Asadh { get; set; } 
            
            ///<summary> Siret NIC </summary>
            public int Asnic { get; set; } 
            
            ///<summary> Code APE </summary>
            public string Asap5 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YAssure  x=this,  y=obj as YAssure;
            if( y == default(YAssure) ) return false;
            return (
                    x.Asias==y.Asias
                    && x.Asad1==y.Asad1
                    && x.Asad2==y.Asad2
                    && x.Asdep==y.Asdep
                    && x.Ascpo==y.Ascpo
                    && x.Asvil==y.Asvil
                    && x.Aspay==y.Aspay
                    && x.Ascom==y.Ascom
                    && x.Asreg==y.Asreg
                    && x.Asfdc==y.Asfdc
                    && x.Astel==y.Astel
                    && x.Astlc==y.Astlc
                    && x.Assir==y.Assir
                    && x.Asape==y.Asape
                    && x.Asapg==y.Asapg
                    && x.Aslig==y.Aslig
                    && x.Asgrs==y.Asgrs
                    && x.Ascra==y.Ascra
                    && x.Ascrm==y.Ascrm
                    && x.Ascrj==y.Ascrj
                    && x.Asusr==y.Asusr
                    && x.Asmja==y.Asmja
                    && x.Asmjm==y.Asmjm
                    && x.Asmjj==y.Asmjj
                    && x.Asins==y.Asins
                    && x.Aspub==y.Aspub
                    && x.Asadh==y.Asadh
                    && x.Asnic==y.Asnic
                    && x.Asap5==y.Asap5  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((
                       17 + (this.Asias.GetHashCode() ) 
                      * 23 ) + (this.Asad1?? "").GetHashCode()
                      * 23 ) + (this.Asad2?? "").GetHashCode()
                      * 23 ) + (this.Asdep?? "").GetHashCode()
                      * 23 ) + (this.Ascpo?? "").GetHashCode()
                      * 23 ) + (this.Asvil?? "").GetHashCode()
                      * 23 ) + (this.Aspay?? "").GetHashCode()
                      * 23 ) + (this.Ascom?? "").GetHashCode()
                      * 23 ) + (this.Asreg?? "").GetHashCode()
                      * 23 ) + (this.Asfdc?? "").GetHashCode()
                      * 23 ) + (this.Astel?? "").GetHashCode()
                      * 23 ) + (this.Astlc?? "").GetHashCode()
                      * 23 ) + (this.Assir.GetHashCode() ) 
                      * 23 ) + (this.Asape?? "").GetHashCode()
                      * 23 ) + (this.Asapg?? "").GetHashCode()
                      * 23 ) + (this.Aslig?? "").GetHashCode()
                      * 23 ) + (this.Asgrs?? "").GetHashCode()
                      * 23 ) + (this.Ascra.GetHashCode() ) 
                      * 23 ) + (this.Ascrm.GetHashCode() ) 
                      * 23 ) + (this.Ascrj.GetHashCode() ) 
                      * 23 ) + (this.Asusr?? "").GetHashCode()
                      * 23 ) + (this.Asmja.GetHashCode() ) 
                      * 23 ) + (this.Asmjm.GetHashCode() ) 
                      * 23 ) + (this.Asmjj.GetHashCode() ) 
                      * 23 ) + (this.Asins?? "").GetHashCode()
                      * 23 ) + (this.Aspub?? "").GetHashCode()
                      * 23 ) + (this.Asadh.GetHashCode() ) 
                      * 23 ) + (this.Asnic.GetHashCode() ) 
                      * 23 ) + (this.Asap5?? "").GetHashCode()                   );
           }
        }
    }
}
