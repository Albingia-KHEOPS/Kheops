using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class Kpgaran_GarContrat {
        ///<summary>Public empty contructor</summary>
        public Kpgaran_GarContrat() {}
        ///<summary>Public empty contructor</summary>
        public Kpgaran_GarContrat(Kpgaran_GarContrat copyFrom) 
        {
 
            this.Kdeid= copyFrom.Kdeid;
 
            this.Kdetyp= copyFrom.Kdetyp;
 
            this.Kdeipb= copyFrom.Kdeipb;
 
            this.Kdealx= copyFrom.Kdealx;
 
            this.Kdefor= copyFrom.Kdefor;
 
            this.Kdeopt= copyFrom.Kdeopt;
 
            this.Kdekdcid= copyFrom.Kdekdcid;
 
            this.Kdegaran= copyFrom.Kdegaran;
 
            this.Kdeseq= copyFrom.Kdeseq;
 
            this.Kdeniveau= copyFrom.Kdeniveau;
 
            this.Kdesem= copyFrom.Kdesem;
 
            this.Kdese1= copyFrom.Kdese1;
 
            this.Kdetri= copyFrom.Kdetri;
 
            this.Kdenumpres= copyFrom.Kdenumpres;
 
            this.Kdeajout= copyFrom.Kdeajout;
 
            this.Kdecar= copyFrom.Kdecar;
 
            this.Kdenat= copyFrom.Kdenat;
 
            this.Kdegan= copyFrom.Kdegan;
 
            this.Kdekdfid= copyFrom.Kdekdfid;
 
            this.Kdedefg= copyFrom.Kdedefg;
 
            this.Kdekdhid= copyFrom.Kdekdhid;
 
            this.Kdedatdeb= copyFrom.Kdedatdeb;
 
            this.Kdeheudeb= copyFrom.Kdeheudeb;
 
            this.Kdedatfin= copyFrom.Kdedatfin;
 
            this.Kdeheufin= copyFrom.Kdeheufin;
 
            this.Kdeduree= copyFrom.Kdeduree;
 
            this.Kdeduruni= copyFrom.Kdeduruni;
 
            this.Kdeprp= copyFrom.Kdeprp;
 
            this.Kdetypemi= copyFrom.Kdetypemi;
 
            this.Kdealiref= copyFrom.Kdealiref;
 
            this.Kdecatnat= copyFrom.Kdecatnat;
 
            this.Kdeina= copyFrom.Kdeina;
 
            this.Kdetaxcod= copyFrom.Kdetaxcod;
 
            this.Kdetaxrep= copyFrom.Kdetaxrep;
 
            this.Kdecravn= copyFrom.Kdecravn;
 
            this.Kdecru= copyFrom.Kdecru;
 
            this.Kdecrd= copyFrom.Kdecrd;
 
            this.Kdemajavn= copyFrom.Kdemajavn;
 
            this.Kdeasvalo= copyFrom.Kdeasvalo;
 
            this.Kdeasvala= copyFrom.Kdeasvala;
 
            this.Kdeasvalw= copyFrom.Kdeasvalw;
 
            this.Kdeasunit= copyFrom.Kdeasunit;
 
            this.Kdeasbase= copyFrom.Kdeasbase;
 
            this.Kdeasmod= copyFrom.Kdeasmod;
 
            this.Kdeasobli= copyFrom.Kdeasobli;
 
            this.Kdeinvsp= copyFrom.Kdeinvsp;
 
            this.Kdeinven= copyFrom.Kdeinven;
 
            this.Kdewddeb= copyFrom.Kdewddeb;
 
            this.Kdewhdeb= copyFrom.Kdewhdeb;
 
            this.Kdewdfin= copyFrom.Kdewdfin;
 
            this.Kdewhfin= copyFrom.Kdewhfin;
 
            this.Kdetcd= copyFrom.Kdetcd;
 
            this.Kdemodi= copyFrom.Kdemodi;
 
            this.Kdepind= copyFrom.Kdepind;
 
            this.Kdepcatn= copyFrom.Kdepcatn;
 
            this.Kdepref= copyFrom.Kdepref;
 
            this.Kdepprp= copyFrom.Kdepprp;
 
            this.Kdepemi= copyFrom.Kdepemi;
 
            this.Kdeptaxc= copyFrom.Kdeptaxc;
 
            this.Kdepntm= copyFrom.Kdepntm;
 
            this.Kdeala= copyFrom.Kdeala;
 
            this.Kdepala= copyFrom.Kdepala;
 
            this.Kdealo= copyFrom.Kdealo;
 
            this.Kdgid= copyFrom.Kdgid;
 
            this.Kdgtyp= copyFrom.Kdgtyp;
 
            this.Kdgipb= copyFrom.Kdgipb;
 
            this.Kdgalx= copyFrom.Kdgalx;
 
            this.Kdgfor= copyFrom.Kdgfor;
 
            this.Kdgopt= copyFrom.Kdgopt;
 
            this.Kdggaran= copyFrom.Kdggaran;
 
            this.Kdgkdeid= copyFrom.Kdgkdeid;
 
            this.Kdgnumtar= copyFrom.Kdgnumtar;
 
            this.Kdglcimod= copyFrom.Kdglcimod;
 
            this.Kdglciobl= copyFrom.Kdglciobl;
 
            this.Kdglcivalo= copyFrom.Kdglcivalo;
 
            this.Kdglcivala= copyFrom.Kdglcivala;
 
            this.Kdglcivalw= copyFrom.Kdglcivalw;
 
            this.Kdglciunit= copyFrom.Kdglciunit;
 
            this.Kdglcibase= copyFrom.Kdglcibase;
 
            this.Kdgkdiid= copyFrom.Kdgkdiid;
 
            this.Kdgfrhmod= copyFrom.Kdgfrhmod;
 
            this.Kdgfrhobl= copyFrom.Kdgfrhobl;
 
            this.Kdgfrhvalo= copyFrom.Kdgfrhvalo;
 
            this.Kdgfrhvala= copyFrom.Kdgfrhvala;
 
            this.Kdgfrhvalw= copyFrom.Kdgfrhvalw;
 
            this.Kdgfrhunit= copyFrom.Kdgfrhunit;
 
            this.Kdgfrhbase= copyFrom.Kdgfrhbase;
 
            this.Kdgkdkid= copyFrom.Kdgkdkid;
 
            this.Kdgfmivalo= copyFrom.Kdgfmivalo;
 
            this.Kdgfmivala= copyFrom.Kdgfmivala;
 
            this.Kdgfmivalw= copyFrom.Kdgfmivalw;
 
            this.Kdgfmiunit= copyFrom.Kdgfmiunit;
 
            this.Kdgfmibase= copyFrom.Kdgfmibase;
 
            this.Kdgfmavalo= copyFrom.Kdgfmavalo;
 
            this.Kdgfmavala= copyFrom.Kdgfmavala;
 
            this.Kdgfmavalw= copyFrom.Kdgfmavalw;
 
            this.Kdgfmaunit= copyFrom.Kdgfmaunit;
 
            this.Kdgfmabase= copyFrom.Kdgfmabase;
 
            this.Kdgprimod= copyFrom.Kdgprimod;
 
            this.Kdgpriobl= copyFrom.Kdgpriobl;
 
            this.Kdgprivalo= copyFrom.Kdgprivalo;
 
            this.Kdgprivala= copyFrom.Kdgprivala;
 
            this.Kdgprivalw= copyFrom.Kdgprivalw;
 
            this.Kdgpriunit= copyFrom.Kdgpriunit;
 
            this.Kdgpribase= copyFrom.Kdgpribase;
 
            this.Kdgmntbase= copyFrom.Kdgmntbase;
 
            this.Kdgprimpro= copyFrom.Kdgprimpro;
 
            this.Kdgtmc= copyFrom.Kdgtmc;
 
            this.Kdgtff= copyFrom.Kdgtff;
 
            this.Kdgcmc= copyFrom.Kdgcmc;
 
            this.Kdgcht= copyFrom.Kdgcht;
 
            this.Kdgctt= copyFrom.Kdgctt;
 
            this.Kdckdbid= copyFrom.Kdckdbid;
 
            this.Kdcteng= copyFrom.Kdcteng;
 
            this.Kdckakid= copyFrom.Kdckakid;
 
            this.Kdckaeid= copyFrom.Kdckaeid;
 
            this.Kdckaqid= copyFrom.Kdckaqid;
 
            this.Kdcmodele= copyFrom.Kdcmodele;
 
            this.Kdckarid= copyFrom.Kdckarid;
 
            this.Kdcflag= copyFrom.Kdcflag;
        
        }        



        ///<summary> ID unique </summary>
        public Int64 Kdeid { get; set; } 
        ///<summary> Type O/P </summary>
        public string Kdetyp { get; set; } 
        ///<summary> IPB </summary>
        public string Kdeipb { get; set; } 
        ///<summary> ALX </summary>
        public int Kdealx { get; set; } 
        ///<summary> Formule </summary>
        public int Kdefor { get; set; } 
        ///<summary> Option </summary>
        public int Kdeopt { get; set; } 
        ///<summary> Lien KPOPTD </summary>
        public Int64 Kdekdcid { get; set; } 
        ///<summary> Garantie </summary>
        public string Kdegaran { get; set; } 
        ///<summary> Séquence </summary>
        public Int64 Kdeseq { get; set; } 
        ///<summary> Niveau </summary>
        public int Kdeniveau { get; set; } 
        ///<summary> Séquence garantie maitre </summary>
        public Int64 Kdesem { get; set; } 
        ///<summary> Séquence Niveau 1 </summary>
        public Int64 Kdese1 { get; set; } 
        ///<summary> Tri </summary>
        public string Kdetri { get; set; } 
        ///<summary> N° de présentation </summary>
        public Double Kdenumpres { get; set; } 
        ///<summary> Garantie Ajoutée O/N </summary>
        public string Kdeajout { get; set; } 
        ///<summary> Caractère (Base Obligatoire ...) </summary>
        public string Kdecar { get; set; } 
        ///<summary> Nature paramétrage </summary>
        public string Kdenat { get; set; } 
        ///<summary> Nature retenue </summary>
        public string Kdegan { get; set; } 
        ///<summary> Lien KPGARAP </summary>
        public Int64 Kdekdfid { get; set; } 
        ///<summary> Définition garantie (Maintenance .. </summary>
        public string Kdedefg { get; set; } 
        ///<summary> Lien KPSPEC </summary>
        public Int64 Kdekdhid { get; set; } 
        ///<summary> Date début </summary>
        public int Kdedatdeb { get; set; } 
        ///<summary> Heure début </summary>
        public int Kdeheudeb { get; set; } 
        ///<summary> Fin de garantie Date </summary>
        public int Kdedatfin { get; set; } 
        ///<summary> Heure Fin </summary>
        public int Kdeheufin { get; set; } 
        ///<summary> Durée </summary>
        public int Kdeduree { get; set; } 
        ///<summary> Durée Unité </summary>
        public string Kdeduruni { get; set; } 
        ///<summary> Type Application </summary>
        public string Kdeprp { get; set; } 
        ///<summary> Type émission </summary>
        public string Kdetypemi { get; set; } 
        ///<summary> Alimentation mnt Référence O/N </summary>
        public string Kdealiref { get; set; } 
        ///<summary> Application CATNAT </summary>
        public string Kdecatnat { get; set; } 
        ///<summary> Indexée O/N </summary>
        public string Kdeina { get; set; } 
        ///<summary> Code taxe </summary>
        public string Kdetaxcod { get; set; } 
        ///<summary> Répartition taxe </summary>
        public int Kdetaxrep { get; set; } 
        ///<summary> Avenant de création </summary>
        public int Kdecravn { get; set; } 
        ///<summary> Création User </summary>
        public string Kdecru { get; set; } 
        ///<summary> Création Date </summary>
        public int Kdecrd { get; set; } 
        ///<summary> Avenant de mise à jour </summary>
        public int Kdemajavn { get; set; } 
        ///<summary> Assiette Valeur Origine </summary>
        public Double Kdeasvalo { get; set; } 
        ///<summary> Assiette Valeur actualisée </summary>
        public Double Kdeasvala { get; set; } 
        ///<summary> Assiette Valeur de travail </summary>
        public Double Kdeasvalw { get; set; } 
        ///<summary> Assiette Unité </summary>
        public string Kdeasunit { get; set; } 
        ///<summary> Assiette Base </summary>
        public string Kdeasbase { get; set; } 
        ///<summary> Assiette Modifiable O/N </summary>
        public string Kdeasmod { get; set; } 
        ///<summary> Assiette Obligatoire </summary>
        public string Kdeasobli { get; set; } 
        ///<summary> Inventaire spécifique </summary>
        public string Kdeinvsp { get; set; } 
        ///<summary> Lien KPINVEN </summary>
        public Int64 Kdeinven { get; set; } 
        ///<summary> Date standard Début </summary>
        public int Kdewddeb { get; set; } 
        ///<summary> Heure standard début </summary>
        public int Kdewhdeb { get; set; } 
        ///<summary> Date standard Fin </summary>
        public int Kdewdfin { get; set; } 
        ///<summary> Heure Standard Fin </summary>
        public int Kdewhfin { get; set; } 
        ///<summary> Type de contrôle date </summary>
        public string Kdetcd { get; set; } 
        ///<summary> Flag Modifié  O/N </summary>
        public string Kdemodi { get; set; } 
        ///<summary> Paramétrage Indexation O/N </summary>
        public string Kdepind { get; set; } 
        ///<summary> Paramétrage CATNAT </summary>
        public string Kdepcatn { get; set; } 
        ///<summary> Paramétrage Mnt Ref </summary>
        public string Kdepref { get; set; } 
        ///<summary> Paramétrage Application </summary>
        public string Kdepprp { get; set; } 
        ///<summary> Paramétrage Type émission </summary>
        public string Kdepemi { get; set; } 
        ///<summary> Paramétrage Code Taxe </summary>
        public string Kdeptaxc { get; set; } 
        ///<summary> Paramétrage Nature modifiable </summary>
        public string Kdepntm { get; set; } 
        ///<summary> Type Alimentation </summary>
        public string Kdeala { get; set; } 
        ///<summary> Paramétrage Type Alimentation </summary>
        public string Kdepala { get; set; } 
        ///<summary> Alimentation origine ' ' ou A Auto </summary>
        public string Kdealo { get; set; } 
        ///<summary> ID unique </summary>
        public Int64 Kdgid { get; set; } 
        ///<summary> Type O/P </summary>
        public string Kdgtyp { get; set; } 
        ///<summary> IPB </summary>
        public string Kdgipb { get; set; } 
        ///<summary> ALX </summary>
        public int Kdgalx { get; set; } 
        ///<summary> Formule </summary>
        public int Kdgfor { get; set; } 
        ///<summary> Option </summary>
        public int Kdgopt { get; set; } 
        ///<summary> Garantie </summary>
        public string Kdggaran { get; set; } 
        ///<summary> Lien KPGARAN </summary>
        public Int64 Kdgkdeid { get; set; } 
        ///<summary> Numéro TARIF </summary>
        public int Kdgnumtar { get; set; } 
        ///<summary> LCI Modifiable </summary>
        public string Kdglcimod { get; set; } 
        ///<summary> LCI obligatoire </summary>
        public string Kdglciobl { get; set; } 
        ///<summary> LCI valeur Origine </summary>
        public Double Kdglcivalo { get; set; } 
        ///<summary> LCI Valeur Actualisée </summary>
        public Double Kdglcivala { get; set; } 
        ///<summary> LCI Valeur Travail </summary>
        public Double Kdglcivalw { get; set; } 
        ///<summary> LCI Unité </summary>
        public string Kdglciunit { get; set; } 
        ///<summary> LCI Base </summary>
        public string Kdglcibase { get; set; } 
        ///<summary> Lien KPEXPLCI </summary>
        public Int64 Kdgkdiid { get; set; } 
        ///<summary> Franchise Modifiable </summary>
        public string Kdgfrhmod { get; set; } 
        ///<summary> Franchise Obligatoire </summary>
        public string Kdgfrhobl { get; set; } 
        ///<summary> Franchise Valeur Origine </summary>
        public Double Kdgfrhvalo { get; set; } 
        ///<summary> Franchise Valeur actualisée </summary>
        public Double Kdgfrhvala { get; set; } 
        ///<summary> Franchise Valeur W </summary>
        public Double Kdgfrhvalw { get; set; } 
        ///<summary> Franchise Unité </summary>
        public string Kdgfrhunit { get; set; } 
        ///<summary> Franchise Base </summary>
        public string Kdgfrhbase { get; set; } 
        ///<summary> Lien KPEXPFRH </summary>
        public Int64 Kdgkdkid { get; set; } 
        ///<summary> Franchise Minimum origine </summary>
        public Double Kdgfmivalo { get; set; } 
        ///<summary> Franchise Minimum valeur Actualisé </summary>
        public Double Kdgfmivala { get; set; } 
        ///<summary> Franchise Minimum Valeur travail </summary>
        public Double Kdgfmivalw { get; set; } 
        ///<summary> Franchise Minimum Unité </summary>
        public string Kdgfmiunit { get; set; } 
        ///<summary> Franchise minimum Base </summary>
        public string Kdgfmibase { get; set; } 
        ///<summary> Franchise maximum Valeur Origine </summary>
        public Double Kdgfmavalo { get; set; } 
        ///<summary> Franchise maximum Valeur actualisée </summary>
        public Double Kdgfmavala { get; set; } 
        ///<summary> Franchise Maximum Valeur de travail </summary>
        public Double Kdgfmavalw { get; set; } 
        ///<summary> Franchise Maximum Unité </summary>
        public string Kdgfmaunit { get; set; } 
        ///<summary> Franchise maximum Base </summary>
        public string Kdgfmabase { get; set; } 
        ///<summary> Prime Modifiable O/N </summary>
        public string Kdgprimod { get; set; } 
        ///<summary> Prime Obligatoire </summary>
        public string Kdgpriobl { get; set; } 
        ///<summary> Prime Valeur origine </summary>
        public Double Kdgprivalo { get; set; } 
        ///<summary> Prime Valeur Actualisée </summary>
        public Double Kdgprivala { get; set; } 
        ///<summary> Prime Valeur de travail </summary>
        public Double Kdgprivalw { get; set; } 
        ///<summary> Prime Unité </summary>
        public string Kdgpriunit { get; set; } 
        ///<summary> Prime Base </summary>
        public string Kdgpribase { get; set; } 
        ///<summary> Prime Montant de Base </summary>
        public Double Kdgmntbase { get; set; } 
        ///<summary> Prime Provisionnelle </summary>
        public Double Kdgprimpro { get; set; } 
        ///<summary> Total : Montant Calculé </summary>
        public Double Kdgtmc { get; set; } 
        ///<summary> Total : Montant Forcé </summary>
        public Double Kdgtff { get; set; } 
        ///<summary> Comptant : Montant Calculé </summary>
        public Double Kdgcmc { get; set; } 
        ///<summary> Comptant : Mnt Forcé HT </summary>
        public Double Kdgcht { get; set; } 
        ///<summary> Comptant : Mnt Forcé TTC </summary>
        public Double Kdgctt { get; set; } 
        ///<summary> Lien KPOPT </summary>
        public Int64 Kdckdbid { get; set; } 
        ///<summary> Type enregistrement Volet Bloc </summary>
        public string Kdcteng { get; set; } 
        ///<summary> Lien KVOLET </summary>
        public Int64 Kdckakid { get; set; } 
        ///<summary> Lien KBLOC </summary>
        public Int64 Kdckaeid { get; set; } 
        ///<summary> Lien KCATBLOC </summary>
        public Int64 Kdckaqid { get; set; } 
        ///<summary> Modèle </summary>
        public string Kdcmodele { get; set; } 
        ///<summary> Lien KCATMODELE </summary>
        public Int64 Kdckarid { get; set; } 
        ///<summary> Flag modifié 1/0 </summary>
        public int Kdcflag { get; set; } 
        }
}
