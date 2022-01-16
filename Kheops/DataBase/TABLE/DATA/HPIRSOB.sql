﻿CREATE TABLE ZALBINKHEO.HPIRSOB ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPIRSOB de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPIRSOB de ZALBINKHEO. 
	KFATYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFAIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFAALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFAALX. 
	KFAAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFAHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KFARSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFARSQ. 
	KFAOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFAOBJ. 
	KFANATS CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KFANEGA CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KFAFRQE CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KFANBPA NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFANBEX NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFANBVI NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFAGN08 DECIMAL(4, 0) NOT NULL DEFAULT 0 , 
	KFAGN09 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KFAGN10 CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KFANBIN NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFANBIN. 
	KFANBPE NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KFAGCT CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KFANBEM NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KFATYTN CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KFANMDF CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFAFENT CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFAFSVT CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFANMSC CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFALABD CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFANAI NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KFALMA NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFALMA. 
	KFAIFP NUMERIC(6, 3) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFAIFP. 
	KFATHF CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFATU1 CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFATU2 CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFAASC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFAAUTL CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	KFAQMD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFASURF NUMERIC(11, 2) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFASURF. 
	KFAVMC NUMERIC(11, 2) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFAVMC. 
	KFAPROL CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPIRSOB    ; 
  
LABEL ON TABLE ZALBINKHEO.HPIRSOB 
	IS 'KHEOPS InfoSpé Objet - Histo                   KFA' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPIRSOB 
( KFATYP IS 'Type O/P' , 
	KFAIPB IS 'IPB' , 
	KFAALX IS 'ALX' , 
	KFAAVN IS 'Avt' , 
	KFAHIN IS 'Hin' , 
	KFARSQ IS 'Risque' , 
	KFAOBJ IS 'Objet' , 
	KFANATS IS 'Nature du support' , 
	KFANEGA IS 'Type de négatif' , 
	KFAFRQE IS 'Fréquence d''envoi' , 
	KFANBPA IS 'Nb de participants' , 
	KFANBEX IS 'Nombre d''exposants' , 
	KFANBVI IS 'Nb visiteurs' , 
	KFANBPE IS 'Nombre              personnes' , 
	KFAGCT IS 'Catégorie           ou                  groupe assurés' , 
	KFANBEM IS 'Nb d'' émissions' , 
	KFATYTN IS 'Type de tournage' , 
	KFANMDF IS 'Nom du diffuseur' , 
	KFAFENT IS 'Fréquence d''envoi   des rushs (texte)' , 
	KFAFSVT IS 'Fréquence des       Sauvegarde (Texte)' , 
	KFANMSC IS 'Société Post-prod' , 
	KFALABD IS 'Nom du labo developp' , 
	KFANAI IS 'Date                Naissance' , 
	KFAIFP IS 'Taux                Infirm' , 
	KFATHF IS 'Tournage            Hors France         Pays' , 
	KFATU1 IS 'Tournage            USA/Canada          fréquence' , 
	KFATU2 IS 'Tournage            USA/Canada          Nature' , 
	KFAASC IS 'Asc                 O/N' , 
	KFAAUTL IS 'Autre Support' , 
	KFAQMD IS 'Quest               Méd                 O/N' , 
	KFASURF IS 'Superficie' , 
	KFAVMC IS 'Valeur au m²' , 
	KFAPROL IS 'Prof                Lib' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPIRSOB 
( KFATYP TEXT IS 'Type O/P' , 
	KFAIPB TEXT IS 'IPB' , 
	KFAALX TEXT IS 'ALX' , 
	KFAAVN TEXT IS 'N° Avenant' , 
	KFAHIN TEXT IS 'N° Histo par avenant' , 
	KFARSQ TEXT IS 'Risque' , 
	KFAOBJ TEXT IS 'Objet' , 
	KFANATS TEXT IS 'Nature du support' , 
	KFANEGA TEXT IS 'Type de négatif' , 
	KFAFRQE TEXT IS 'Fréquence d''envoi' , 
	KFANBPA TEXT IS 'Nb de participants' , 
	KFANBEX TEXT IS 'Nombre d''exposants' , 
	KFANBVI TEXT IS 'Nb visiteurs' , 
	KFAGN08 TEXT IS 'GEN Nb élèves' , 
	KFAGN09 TEXT IS 'GEN nature études' , 
	KFAGN10 TEXT IS 'GEN durée cycle' , 
	KFANBIN TEXT IS 'Nb invités' , 
	KFANBPE TEXT IS 'Nombre de personnes' , 
	KFAGCT TEXT IS 'Catégorie ou groupe assurés' , 
	KFANBEM TEXT IS 'Nombre d''émissions' , 
	KFATYTN TEXT IS 'Type de tournage' , 
	KFANMDF TEXT IS 'Nom du diffuseur' , 
	KFAFENT TEXT IS 'Fréquence d''envoi des rushs (Texte)' , 
	KFAFSVT TEXT IS 'Fréquence des sauvegardes (Texte)' , 
	KFANMSC TEXT IS 'Société en charge Post-prod' , 
	KFALABD TEXT IS 'Nom du labo de développement' , 
	KFANAI TEXT IS 'Date naissance' , 
	KFALMA TEXT IS 'IA:Report lim âge' , 
	KFAIFP TEXT IS 'IA: Taux infirmité préexistante' , 
	KFATHF TEXT IS 'RC:Tournage hors France : pays' , 
	KFATU1 TEXT IS 'RC:Tournage USA/Canada : fréquence' , 
	KFATU2 TEXT IS 'RC:Tournage USA/Canada : Nature' , 
	KFAASC TEXT IS 'RC:Présence d''un ascenseur (O/N)' , 
	KFAAUTL TEXT IS 'Autre support libellé ExLien KPIDESI' , 
	KFAQMD TEXT IS 'Présence questionnaire médical O/N' , 
	KFASURF TEXT IS 'Superficie' , 
	KFAVMC TEXT IS 'Valeur au m²' , 
	KFAPROL TEXT IS 'Prof libérale O/N (!!rég.taxe)' ) ; 
  