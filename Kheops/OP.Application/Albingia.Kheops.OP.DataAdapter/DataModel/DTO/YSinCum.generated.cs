using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YSINCUM
    public partial class YSinCum  {
             //YSINCUM

            ///<summary>Public empty contructor</summary>
            public YSinCum() {}
            ///<summary>Public empty contructor</summary>
            public YSinCum(YSinCum copyFrom) 
            {
                  this.Susua= copyFrom.Susua;
                  this.Sunum= copyFrom.Sunum;
                  this.Susbr= copyFrom.Susbr;
                  this.Suipb= copyFrom.Suipb;
                  this.Sualx= copyFrom.Sualx;
                  this.Suavn= copyFrom.Suavn;
                  this.Sutbr= copyFrom.Sutbr;
                  this.Sutsb= copyFrom.Sutsb;
                  this.Sutpo= copyFrom.Sutpo;
                  this.Sutpe= copyFrom.Sutpe;
                  this.Sutpa= copyFrom.Sutpa;
                  this.Sutin= copyFrom.Sutin;
                  this.Sutfr= copyFrom.Sutfr;
                  this.Sutre= copyFrom.Sutre;
                  this.Sutch= copyFrom.Sutch;
                  this.Suapo= copyFrom.Suapo;
                  this.Suape= copyFrom.Suape;
                  this.Suapa= copyFrom.Suapa;
                  this.Suain= copyFrom.Suain;
                  this.Suafr= copyFrom.Suafr;
                  this.Suare= copyFrom.Suare;
                  this.Suach= copyFrom.Suach;
                  this.Sukpo= copyFrom.Sukpo;
                  this.Sukpe= copyFrom.Sukpe;
                  this.Sukpa= copyFrom.Sukpa;
                  this.Sukin= copyFrom.Sukin;
                  this.Sukfr= copyFrom.Sukfr;
                  this.Sukre= copyFrom.Sukre;
                  this.Sukch= copyFrom.Sukch;
                  this.Sucpo= copyFrom.Sucpo;
                  this.Sucpe= copyFrom.Sucpe;
                  this.Sucpa= copyFrom.Sucpa;
                  this.Sucin= copyFrom.Sucin;
                  this.Sucfr= copyFrom.Sucfr;
                  this.Sucre= copyFrom.Sucre;
                  this.Succh= copyFrom.Succh;
                  this.Sumju= copyFrom.Sumju;
                  this.Sumja= copyFrom.Sumja;
                  this.Sumjm= copyFrom.Sumjm;
                  this.Sumjj= copyFrom.Sumjj;
                  this.Sumjh= copyFrom.Sumjh;
                  this.Sutpp= copyFrom.Sutpp;
                  this.Sutpf= copyFrom.Sutpf;
                  this.Suapp= copyFrom.Suapp;
                  this.Suapf= copyFrom.Suapf;
                  this.Sukpp= copyFrom.Sukpp;
                  this.Sukpf= copyFrom.Sukpf;
                  this.Sucpp= copyFrom.Sucpp;
                  this.Sucpf= copyFrom.Sucpf;
        
            }        
            
            ///<summary> N° sinistre : Année de survenance </summary>
            public int Susua { get; set; } 
            
            ///<summary> N° sinistre : N° </summary>
            public int Sunum { get; set; } 
            
            ///<summary> N° sinistre : Sous-branche </summary>
            public string Susbr { get; set; } 
            
            ///<summary> N° de Police </summary>
            public string Suipb { get; set; } 
            
            ///<summary> N° Aliment ou Connexe et version </summary>
            public int Sualx { get; set; } 
            
            ///<summary> SUAVN N° Avenant </summary>
            public int Suavn { get; set; } 
            
            ///<summary> SUTBR Branche </summary>
            public string Sutbr { get; set; } 
            
            ///<summary> SUTSB S-branche </summary>
            public string Sutsb { get; set; } 
            
            ///<summary> Total: Provisions     D.Pol </summary>
            public Decimal Sutpo { get; set; } 
            
            ///<summary> Total: Prévisions     D.Pol </summary>
            public Decimal Sutpe { get; set; } 
            
            ///<summary> Total: Prév.attendues D.Pol </summary>
            public Decimal Sutpa { get; set; } 
            
            ///<summary> Total: Indemnités     D.Pol </summary>
            public Decimal Sutin { get; set; } 
            
            ///<summary> Total: Frais & H.     D.Pol </summary>
            public Decimal Sutfr { get; set; } 
            
            ///<summary> Total: Recours        D.Pol </summary>
            public Decimal Sutre { get; set; } 
            
            ///<summary> Total: Chargement     D.Pol </summary>
            public Decimal Sutch { get; set; } 
            
            ///<summary> Albingia: Provisions  D.Pol </summary>
            public Decimal Suapo { get; set; } 
            
            ///<summary> Albingia: Prévisions  D.Pol </summary>
            public Decimal Suape { get; set; } 
            
            ///<summary> Albingia: Prév.attend.D.Pol </summary>
            public Decimal Suapa { get; set; } 
            
            ///<summary> Albingia: Indemnités  D.Pol </summary>
            public Decimal Suain { get; set; } 
            
            ///<summary> Albingia: Frais & H.  D.Pol </summary>
            public Decimal Suafr { get; set; } 
            
            ///<summary> Albingia: Recours     D.Pol </summary>
            public Decimal Suare { get; set; } 
            
            ///<summary> Albingia: Chargement  D.Pol </summary>
            public Decimal Suach { get; set; } 
            
            ///<summary> Total: Provisions     D.Cpt </summary>
            public Decimal Sukpo { get; set; } 
            
            ///<summary> Total: Prévisions     D.Cpt </summary>
            public Decimal Sukpe { get; set; } 
            
            ///<summary> Total: Prév.attendues D.Cpt </summary>
            public Decimal Sukpa { get; set; } 
            
            ///<summary> Total: Indemnités     D.Cpt </summary>
            public Decimal Sukin { get; set; } 
            
            ///<summary> Total: Frais & H.     D.Cpt </summary>
            public Decimal Sukfr { get; set; } 
            
            ///<summary> Total: Recours        D.Cpt </summary>
            public Decimal Sukre { get; set; } 
            
            ///<summary> Total: Chargement     D.Cpt </summary>
            public Decimal Sukch { get; set; } 
            
            ///<summary> Albingia: Provisions  D.Cpt </summary>
            public Decimal Sucpo { get; set; } 
            
            ///<summary> Albingia: Prévisions  D.Cpt </summary>
            public Decimal Sucpe { get; set; } 
            
            ///<summary> Albingia: Prév.attend.D.Cpt </summary>
            public Decimal Sucpa { get; set; } 
            
            ///<summary> Albingia: Indemnités  D.Cpt </summary>
            public Decimal Sucin { get; set; } 
            
            ///<summary> Albingia: Frais & H.  D.Cpt </summary>
            public Decimal Sucfr { get; set; } 
            
            ///<summary> Albingia: Recours     D.Cpt </summary>
            public Decimal Sucre { get; set; } 
            
            ///<summary> Albingia: Chargement  D.Cpt </summary>
            public Decimal Succh { get; set; } 
            
            ///<summary> MàJ: User </summary>
            public string Sumju { get; set; } 
            
            ///<summary> MàJ: Année </summary>
            public int Sumja { get; set; } 
            
            ///<summary> MàJ: Mois </summary>
            public int Sumjm { get; set; } 
            
            ///<summary> MàJ: Jour </summary>
            public int Sumjj { get; set; } 
            
            ///<summary> MàJ: Heure </summary>
            public int Sumjh { get; set; } 
            
            ///<summary> Total: Prov/Princ.    D.Pol </summary>
            public Decimal Sutpp { get; set; } 
            
            ///<summary> Total: Prov/F&H       D.Pol </summary>
            public Decimal Sutpf { get; set; } 
            
            ///<summary> Albingia: Prov/Princ. D.Pol </summary>
            public Decimal Suapp { get; set; } 
            
            ///<summary> Albingia: Prov/F&H    D.Pol </summary>
            public Decimal Suapf { get; set; } 
            
            ///<summary> Total: Prov/Princ.    D.Cpt </summary>
            public Decimal Sukpp { get; set; } 
            
            ///<summary> Total: Prov/F&H       D.Cpt </summary>
            public Decimal Sukpf { get; set; } 
            
            ///<summary> Albingia: Prov/Princ. D.Cpt </summary>
            public Decimal Sucpp { get; set; } 
            
            ///<summary> Albingia: Prov/F&H    D.Cpt </summary>
            public Decimal Sucpf { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YSinCum  x=this,  y=obj as YSinCum;
            if( y == default(YSinCum) ) return false;
            return (
                    x.Susua==y.Susua
                    && x.Sunum==y.Sunum
                    && x.Susbr==y.Susbr
                    && x.Suipb==y.Suipb
                    && x.Sualx==y.Sualx
                    && x.Suavn==y.Suavn
                    && x.Sutbr==y.Sutbr
                    && x.Sutsb==y.Sutsb
                    && x.Sutpo==y.Sutpo
                    && x.Sutpe==y.Sutpe
                    && x.Sutpa==y.Sutpa
                    && x.Sutin==y.Sutin
                    && x.Sutfr==y.Sutfr
                    && x.Sutre==y.Sutre
                    && x.Sutch==y.Sutch
                    && x.Suapo==y.Suapo
                    && x.Suape==y.Suape
                    && x.Suapa==y.Suapa
                    && x.Suain==y.Suain
                    && x.Suafr==y.Suafr
                    && x.Suare==y.Suare
                    && x.Suach==y.Suach
                    && x.Sukpo==y.Sukpo
                    && x.Sukpe==y.Sukpe
                    && x.Sukpa==y.Sukpa
                    && x.Sukin==y.Sukin
                    && x.Sukfr==y.Sukfr
                    && x.Sukre==y.Sukre
                    && x.Sukch==y.Sukch
                    && x.Sucpo==y.Sucpo
                    && x.Sucpe==y.Sucpe
                    && x.Sucpa==y.Sucpa
                    && x.Sucin==y.Sucin
                    && x.Sucfr==y.Sucfr
                    && x.Sucre==y.Sucre
                    && x.Succh==y.Succh
                    && x.Sumju==y.Sumju
                    && x.Sumja==y.Sumja
                    && x.Sumjm==y.Sumjm
                    && x.Sumjj==y.Sumjj
                    && x.Sumjh==y.Sumjh
                    && x.Sutpp==y.Sutpp
                    && x.Sutpf==y.Sutpf
                    && x.Suapp==y.Suapp
                    && x.Suapf==y.Suapf
                    && x.Sukpp==y.Sukpp
                    && x.Sukpf==y.Sukpf
                    && x.Sucpp==y.Sucpp
                    && x.Sucpf==y.Sucpf  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Susua.GetHashCode() ) 
                      * 23 ) + (this.Sunum.GetHashCode() ) 
                      * 23 ) + (this.Susbr?? "").GetHashCode()
                      * 23 ) + (this.Suipb?? "").GetHashCode()
                      * 23 ) + (this.Sualx.GetHashCode() ) 
                      * 23 ) + (this.Suavn.GetHashCode() ) 
                      * 23 ) + (this.Sutbr?? "").GetHashCode()
                      * 23 ) + (this.Sutsb?? "").GetHashCode()
                      * 23 ) + (this.Sutpo.GetHashCode() ) 
                      * 23 ) + (this.Sutpe.GetHashCode() ) 
                      * 23 ) + (this.Sutpa.GetHashCode() ) 
                      * 23 ) + (this.Sutin.GetHashCode() ) 
                      * 23 ) + (this.Sutfr.GetHashCode() ) 
                      * 23 ) + (this.Sutre.GetHashCode() ) 
                      * 23 ) + (this.Sutch.GetHashCode() ) 
                      * 23 ) + (this.Suapo.GetHashCode() ) 
                      * 23 ) + (this.Suape.GetHashCode() ) 
                      * 23 ) + (this.Suapa.GetHashCode() ) 
                      * 23 ) + (this.Suain.GetHashCode() ) 
                      * 23 ) + (this.Suafr.GetHashCode() ) 
                      * 23 ) + (this.Suare.GetHashCode() ) 
                      * 23 ) + (this.Suach.GetHashCode() ) 
                      * 23 ) + (this.Sukpo.GetHashCode() ) 
                      * 23 ) + (this.Sukpe.GetHashCode() ) 
                      * 23 ) + (this.Sukpa.GetHashCode() ) 
                      * 23 ) + (this.Sukin.GetHashCode() ) 
                      * 23 ) + (this.Sukfr.GetHashCode() ) 
                      * 23 ) + (this.Sukre.GetHashCode() ) 
                      * 23 ) + (this.Sukch.GetHashCode() ) 
                      * 23 ) + (this.Sucpo.GetHashCode() ) 
                      * 23 ) + (this.Sucpe.GetHashCode() ) 
                      * 23 ) + (this.Sucpa.GetHashCode() ) 
                      * 23 ) + (this.Sucin.GetHashCode() ) 
                      * 23 ) + (this.Sucfr.GetHashCode() ) 
                      * 23 ) + (this.Sucre.GetHashCode() ) 
                      * 23 ) + (this.Succh.GetHashCode() ) 
                      * 23 ) + (this.Sumju?? "").GetHashCode()
                      * 23 ) + (this.Sumja.GetHashCode() ) 
                      * 23 ) + (this.Sumjm.GetHashCode() ) 
                      * 23 ) + (this.Sumjj.GetHashCode() ) 
                      * 23 ) + (this.Sumjh.GetHashCode() ) 
                      * 23 ) + (this.Sutpp.GetHashCode() ) 
                      * 23 ) + (this.Sutpf.GetHashCode() ) 
                      * 23 ) + (this.Suapp.GetHashCode() ) 
                      * 23 ) + (this.Suapf.GetHashCode() ) 
                      * 23 ) + (this.Sukpp.GetHashCode() ) 
                      * 23 ) + (this.Sukpf.GetHashCode() ) 
                      * 23 ) + (this.Sucpp.GetHashCode() ) 
                      * 23 ) + (this.Sucpf.GetHashCode() )                    );
           }
        }
    }
}
