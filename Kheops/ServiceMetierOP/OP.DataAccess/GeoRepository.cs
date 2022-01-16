using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Common;

namespace OP.DataAccess
{
    public class GeoRepository
    {
        #region Localisation

        public static bool SetAdrToLngLat()
        {
            DbParameter[] param = new DbParameter[1]; 
            param[0] = new EacParameter(" P_TST", "1");
            var adresses = DbBase.Settings.ExecuteList<GeoAdrDto>(CommandType.StoredProcedure, "GETALLADR",param);
            return true;
        }

        public static List<BandeauDto> GetListAdrByPerm(string numOffre, string type, int version, int perimetre, string departement)
        {
            const char unit = 'K';
            var infosFolders = GetBandeau(departement);
            if (!string.IsNullOrEmpty(departement))
                return infosFolders;
            var principalFolder =
                infosFolders.FirstOrDefault(
                    el => el.CodeOffre.Trim() == numOffre.Trim() && el.TypeOffre == type && el.VersionOffre == version);
            if (principalFolder == null )
                return null;
            var lat = Convert.ToDouble(principalFolder.Lat, CultureInfo.GetCultureInfo("en-US").NumberFormat);
            var longi = Convert.ToDouble(principalFolder.Long, CultureInfo.GetCultureInfo("en-US").NumberFormat);
            infosFolders.Remove(principalFolder);
            var adrConcerned = new List<BandeauDto> {principalFolder};
            infosFolders.ForEach(
                el =>
                {
                    var elLat = Convert.ToDouble(el.Lat, CultureInfo.GetCultureInfo("en-US").NumberFormat);
                    var elLongi = Convert.ToDouble(el.Long, CultureInfo.GetCultureInfo("en-US").NumberFormat);
                    if (GeoDistance.GetDistance(lat, longi, elLat, elLongi, unit) <= perimetre)
                        adrConcerned.Add(el);

                });
            return adrConcerned;
        }

        public static List<BandeauDto> GetListAdrInrectangle(double eastLatitude, double eastlongitude, double southWestLatitude, double southWestLongitude)
        {
            var infosFolders = GetBandeau(string.Empty);
            var adrConcerned = new List<BandeauDto> ();
            infosFolders.ForEach(
                el =>
                {
                   
                    var elLat = Convert.ToDouble(el.Lat, CultureInfo.GetCultureInfo("en-US").NumberFormat);
                    var elLongi = Convert.ToDouble(el.Long, CultureInfo.GetCultureInfo("en-US").NumberFormat);

                    if (GeoDistance.IsPointInPolygon(eastLatitude, eastlongitude, southWestLatitude, southWestLongitude, elLat, elLongi))
                        adrConcerned.Add(el);

                });
            return adrConcerned;

        }

        #endregion

        #region private Geo

        private static List<BandeauDto> GetBandeau(string departement)//string codeOffre, string versionOffre, string typeOffre)
        {
           
            var sql = string.Format(@"SELECT PBIPB CODEOFFRE,PBALX VERSIONOFFRE, PBTYP TYPEOFFRE,PBSAA DATESAISIEANNEE, PBSAM DATESAISIEMOIS, PBSAJ DATESAISIEJOUR, PBSAH DATESAISIEHEURE,
								    PBBRA BRANCHECODE, BRCH.TPLIB BRANCHELIB, PBREF DESCRIPTIF,
                                    KAACIBLE CIBLECODE, KAHDESC CIBLELIB,
                                    PBCTA CODECOURTIERAPPORTEUR, YCApp.TNNOM NOMCOURTIERAPPO,
                                    PBCTP CODECOURTIERPAYEUR, YCPay.TNNOM NOMCOURTIERPAYEUR,
                                    PBICT CODECOURTIERGESTIONNAIRE, YCGest.TNNOM NOMCOURTIERGEST,
                                    ANNOM NOMPRENASSUR,ANIAS CODEPRENASSUR,
                                    PBSOU SOUSCODE, UT1.UTNOM SOUSNOM, UT1.UTPNM SOUSPNM,                       
                                    PBGES GESCODE, UT2.UTNOM GESNOM, UT2.UTPNM GESPNM,             
                                    PBEFA DATEEFFETA,PBEFM DATEEFFETM,  PBEFJ DATEEFFETJ,
                                    PBFEJ FINEFFETJOUR, PBFEM FINEFFETMOIS, PBFEA FINEFFETANNEE,                                     
                                    PBDEV CODEDEVISE,DEVISE.TPLIB LIBDEVISE,
                                    PBNPL NATURECONTRAT,NATURE.TPLIL LIBELLENATURECONTRAT,
                                    PBAPP PARTALBINGIA,PBPCV COUVERTURE,
                                    JDIND CODEINDICEREFERENCE,IND.TPLIB LIBINDICE,
                                    JDIVA VALEUR,
                                    PBETA ETAT, PBSIT CODESITUATION, ETAT.TPLIB LIBETAT,
                                    PBPER PERIODICITECODE,PERD.TPLIB PERIODICITENOM,
                                    JDENC CODEENCAISSEMENT, ENCAISS.TPLIB LIBENCAISSEMENT,
                                    PBECJ ECHJOUR, PBECM ECHMOIS,
                                    KAALCIVALO LCIGENERALE, KAALCIUNIT LCIGENERALEUNIT, KAALCIBASE LCIGENERALETYPE,
                                    KAAFRHVALO FRCHGENERALE, KAAFRHUNIT FRCHGENERALEUNIT, KAAFRHBASE FRCHGENERALETYPE, 
                                    JDTRR TERRITORIALITE, TERRITO.TPLIB TERRITORIALITELIB,
                                    PBSTF CODEMOTIF, MOTIF.TPLIB LIBMOTIF,
                                    PBSTP STOP, SITSTOP.TPLIL STOPLIB, PBCON STOPCONTENTIEUX, CONTSTOP.TPLIL STOPCONTENTIEUXLIB,
                                    PBCTD DUREE, PBCTU DUREEUNITE, DR.TPLIB DUREESTR, 
                                    PBRGT CODEREGIME, RG.TPLIB LIBREGIME, JDCNA SOUMISCATNAT, JDTFF MONTANTREF1, JDTMC MONTANTREF2, JDINA INDEXATION,JDIXL LCI,JDIXC ASSIETTE, JDIXF FRANCHISE,
		                            JDDPV PREAVIS,PBTTR CODEACTION, TR.TPLIB LIBACTION,SIT.TPLIB LIBSITUATION,PBSTJ DATESITJOUR, PBSTM DATESITMOIS, PBSTA DATESITANNEE,PBDEU UCRCODE,UCR.UTNOM UCRNOM, 
		                            PBMJU UUPCODE, UUP.UTNOM UUPNOM,
                                    PBAVN NUMAVENANT,PBAVJ DATEEFFETAVNJOUR,PBAVM DATEEFFETAVNMOIS,PBAVA DATEEFFETAVNANNEE,PBOFF CODEOFFREORIGINE,PBOFV VERSIONOFFREORIGINE,
		                            JDPEJ PROCHECHJ,JDPEM PROCHECHM, JDPEA PROCHECHA,JDEHH HORSCATNAT,JDEHC CATNAT,JDXCM TAUXHORSCATNAT,JDCNC TAUXCATNAT,
		                            SSDTJ DATEAFFAIRENOUVELLEJOUR,SSDTM DATEAFFAIRENOUVELLEMOIS,SSDTA DATEAFFAIRENOUVELLEANNEE,PBDEJ DATECRJOUR, PBDEM DATECRMOIS, PBDEA DATECRANNEE,
		                            PBMJJ DATEUPJOUR, PBMJM DATEUPMOIS, PBMJA DATEUPANNEE,
                                    ADRESSEGESTIONNAIRE.ABPDP6 DPGESTIONNAIRE, ADRESSEGESTIONNAIRE.ABPCP6 CPGESTIONNAIRE, ADRESSEGESTIONNAIRE.ABPVI6 VILLEGESTIONNAIRE,
                                    ADRESSEASSURE.ABPDP6 DPASSURE, ADRESSEASSURE.ABPCP6 CPASSURE, ADRESSEASSURE.ABPVI6 VILLEASSURE ,
                                    ADRESSEAPPORTEUR.ABPDP6 DPAPPORTEUR, ADRESSEAPPORTEUR.ABPCP6 CPAPPORTEUR, ADRESSEAPPORTEUR.ABPVI6 VILLEAPPORTEUR,                                                                  
                                    ADRESSEPAYEUR.ABPDP6 DPPAYEUR, ADRESSEPAYEUR.ABPCP6 CPPAYEUR, ADRESSEPAYEUR.ABPVI6 VILLEPAYEUR,PBLNG LNG, PBLAT LAT
                                                                                                     
                                    FROM YPOLOCZB
                                    LEFT JOIN YPRTENT ON PBIPB = JDIPB AND PBALX = JDALX 
                                    LEFT JOIN KPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP 
                                    LEFT JOIN YPOCOAS ON PBIPB = PHIPB AND PBALX = PHALX                                     
                                    LEFT JOIN KPOBSV ON KAAOBSV = KAJCHR
                                    LEFT JOIN YUTILIS UT1 ON PBSOU = UT1.UTIUT AND UT1.UTSOU ='O' 
                                    LEFT JOIN YUTILIS UT2 ON PBGES = UT2.UTIUT AND UT2.UTGEP ='O' 
                                    LEFT JOIN YUTILIS UCR ON PBDEU = UCR.UTIUT 
                                    LEFT JOIN YUTILIS UUP ON PBMJU = UUP.UTIUT 
                                    LEFT JOIN YCOMPA ON CIICI = PHCIE 
                                    LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
                                    LEFT JOIN YSTAPRO ON SSIPW = PBIPB AND SSALW = PBALX
                                    LEFT JOIN YCOURTN YCGest ON YCGest.TNICT = PBICT AND YCGest.TNXN5 = 0 AND YCGest.TNTNM = 'A'
                                    LEFT JOIN YCOURTI Gestionnaire ON Gestionnaire.TCICT=PBICT   
                                    LEFT JOIN YASSNOM ON ANIAS = PBIAS AND ANINL = 0 AND ANTNM = 'A'                                    
                                    LEFT JOIN YCOURTN YCApp ON YCApp.TNICT = PBCTA AND YCApp.TNXN5 = 0 AND YCApp.TNTNM = 'A'
                                    LEFT JOIN YCOURTN YCPay ON YCPay.TNICT = PBCTP AND YCPay.TNXN5 = 0 AND YCPay.TNTNM = 'A'
                                    {0}
                                    {1}
                                    {2}
                                    {3}
                                    {4}
                                    {4}
                                    {5}   
                                    {6}
                                    {7}
                                    {8}
                                    {9}
                                    {10}      
                                    {11}
                                    {12}
                                    {13}
                                    {14}
                                    LEFT JOIN YADRESS ADRESSEGESTIONNAIRE ON Gestionnaire.TCADH = ADRESSEGESTIONNAIRE.ABPCHR 
                                    LEFT JOIN YASSURE ON PBIAS = ASIAS
                                    LEFT JOIN YADRESS ADRESSEASSURE ON ASADH = ADRESSEASSURE.ABPCHR   
                                    LEFT JOIN YCOURTI Apporteur ON Apporteur.TCICT=PBCTA 
                                    LEFT JOIN YADRESS ADRESSEAPPORTEUR ON Apporteur.TCADH = ADRESSEAPPORTEUR.ABPCHR   
                                    LEFT JOIN YCOURTI Payeur ON Payeur.TCICT = PBCTP
                                    LEFT JOIN YADRESS ADRESSEPAYEUR ON Payeur.TCADH = ADRESSEPAYEUR.ABPCHR {15}",   
                                  
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "TAXRG", "RG", " AND RG.TCOD = PBRGT"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBTTR", "TR", " AND TR.TCOD = PBTTR"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBSIT", "SIT", " AND SIT.TCOD = PBSIT"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBNPL", "NATURE", " AND PBNPL = NATURE.TCOD"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBETA", "ETAT", " AND ETAT.TCOD = PBETA"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "BRCH", " AND BRCH.TCOD = PBBRA AND BRCH.TPCN2 = 1"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "DEVIS", "DEVISE", " AND DEVISE.TCOD = PBDEV"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "INDIC", "IND", " AND IND.TCOD = JDIND"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBPER", "PERD", " AND PERD.TCOD = PBPER"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "FBENC", "ENCAISS", " AND ENCAISS.TCOD = JDENC"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "QATRR", "TERRITO", " AND TERRITO.TCOD = JDTRR"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBSTF", "MOTIF", " AND MOTIF.TCOD = PBSTF"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBSTP", "SITSTOP", " AND SITSTOP.TCOD = PBSTP"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PKREL", "CONTSTOP", " AND CONTSTOP.TCOD = PBCON"),
                                    CommonRepository.BuildJoinYYYYPAR("LEFT", "PRODU", "PBCTU", "DR", " AND DR.TCOD = PBCTU"),string.IsNullOrEmpty(departement)?string.Empty:"WHERE trim(pbdep)='"+departement+"'");

           
            
            return DbBase.Settings.ExecuteList<BandeauDto>(CommandType.Text, sql);

            //if (result != null && result.Any())
            //{
            //    toReturn = result.FirstOrDefault();
            //    int cp = 0;
            //    toReturn.CPCourtierGest = (Int32.TryParse(toReturn.DepartementGestionnaire + toReturn.CodePostalGestionnaire.ToString().PadLeft(3, '0'), out cp) ? cp : cp).ToString();
            //    toReturn.VilleCourtierGest = toReturn.VilleGestionnaire;

            //    cp = 0;
            //    toReturn.CPPreneurAssurance = (Int32.TryParse(toReturn.DepartementAssure + toReturn.CodePostalAssure.ToString().PadLeft(3, '0'), out cp) ? cp : cp).ToString();
            //    toReturn.VillePreneurAssurance = toReturn.VilleAssure;

            //    cp = 0;
            //    toReturn.VilleCourtierAppo = (Int32.TryParse(toReturn.DepartementApporteur + toReturn.CodePostalApporteur.ToString().PadLeft(3, '0'), out cp) ? cp : cp).ToString() + " " + toReturn.VilleApporteur;

            //    cp = 0;
            //    toReturn.VilleCourtierPayeur = (Int32.TryParse(toReturn.DepartementPayeur + toReturn.CodePostalPayeur.ToString().PadLeft(3, '0'), out cp) ? cp : cp).ToString() + " " + toReturn.VilleCourtierPayeur;

            //    toReturn.HasDoubleSaisie = PoliceRepository.GetDoubleSaisie(codeOffre, int.Parse(versionOffre), typeOffre);

            //    if (typeOffre == AlbConstantesMetiers.TYPE_CONTRAT)
            //    {
            //        var delegation = DelegationRepository.Obtenir(toReturn.CodeCourtierApporteur);
            //        if (delegation != null)
            //        {
            //            toReturn.NomDelegation = delegation.Nom;
            //            toReturn.NomInspecteur = delegation.Inspecteur != null ? delegation.Inspecteur.Nom : string.Empty;
            //            toReturn.Secteur = delegation.Secteur;
            //            toReturn.LibSecteur = delegation.LibSecteur;
            //        }

            //        toReturn.MontantStatistique = AffaireNouvelleRepository.GetMontantStatistique(codeOffre, versionOffre);
            //    }


            //}
            
        }
        #endregion

       
    }
}
