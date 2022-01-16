﻿CREATE TABLE ZALBINKHEO.HPCLAUSE ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPCLAUSE de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPCLAUSE de ZALBINKHEO. 
	KCAID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KCATYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCAIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KCAALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KCAAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KCAHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KCAETAPE CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCAPERI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCARSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCAOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCAINVEN NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KCAINLGN NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCAFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCAOPT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCAGAR CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCACTX CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCAAJT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCANTA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCAKDUID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KCACLNM1 CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KCACLNM2 CHAR(5) CCSID 297 NOT NULL DEFAULT '' , 
	KCACLNM3 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCAVER NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KCATXL NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KCAMER NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KCADOC CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCACHI CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCACHIS CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCAIMP NUMERIC(10, 0) NOT NULL DEFAULT 0 , 
	KCACXI NUMERIC(7, 3) NOT NULL DEFAULT 0 , 
	KCAIAN CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCAIAC CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCASIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCASITD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KCAAVNC NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KCACRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KCAAVNM NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KCAMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KCASPA CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCATYPO CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCAAIM CHAR(20) CCSID 297 NOT NULL DEFAULT '' , 
	KCATAE CHAR(15) CCSID 297 NOT NULL DEFAULT '' , 
	KCAELGO CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCAELGI NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KCAXTL CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCATYPD CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KCAETAFF CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KCAXTLM CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHPCLAUS   ; 
  
LABEL ON TABLE ZALBINKHEO.HPCLAUSE 
	IS 'KHEOPS histo O/P  Clauses' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPCLAUSE 
( KCAID IS 'ID unique' , 
	KCATYP IS 'TYP O/P' , 
	KCAIPB IS 'IPB' , 
	KCAALX IS 'ALX' , 
	KCAAVN IS 'N° avenant' , 
	KCAHIN IS 'N° histo            par avenant' , 
	KCAETAPE IS 'Etape de génération' , 
	KCAPERI IS 'Périmètre' , 
	KCARSQ IS 'Risque' , 
	KCAOBJ IS 'Objet' , 
	KCAINVEN IS 'ID KPINVEN' , 
	KCAINLGN IS 'N° de ligne invent' , 
	KCAFOR IS 'Formule' , 
	KCAOPT IS 'Option' , 
	KCAGAR IS 'Garantie' , 
	KCACTX IS 'Contexte' , 
	KCAAJT IS 'Ajoutée O/N' , 
	KCANTA IS 'Nature de la clause' , 
	KCAKDUID IS 'Clause lien KCLAUSE' , 
	KCACLNM1 IS 'Code clause Nm1' , 
	KCACLNM2 IS 'Code Clause Nm2' , 
	KCACLNM3 IS 'Code Clause Nm3' , 
	KCAVER IS 'N° version' , 
	KCATXL IS 'Lien clause libre' , 
	KCAMER IS 'Clause Mère' , 
	KCADOC IS 'Document impression' , 
	KCACHI IS 'Chapitre impression' , 
	KCACHIS IS 'Sous chapitre impres' , 
	KCAIMP IS 'N° Impression' , 
	KCACXI IS 'N° Ordonnancement' , 
	KCAIAN IS 'Impress annexe O/N' , 
	KCAIAC IS 'Code annexe' , 
	KCASIT IS 'Code situation' , 
	KCASITD IS 'Situation Date' , 
	KCAAVNC IS 'Avenantde création' , 
	KCACRD IS 'Création date' , 
	KCAAVNM IS 'Avenant de modif' , 
	KCAMAJD IS 'MAj Date' , 
	KCASPA IS 'Clause specif avn' , 
	KCATYPO IS 'Typologie clause' , 
	KCAAIM IS 'Attribut impression' , 
	KCATAE IS 'Action Enchainée' , 
	KCAELGO IS 'Elément Générat Ori' , 
	KCAELGI IS 'Elém Gén  ID' , 
	KCAXTL IS 'Texte libre O/N' , 
	KCATYPD IS 'Doc Ajouté/Génér/Ext' , 
	KCAETAFF IS 'Etape d''affichage' , 
	KCAXTLM IS 'Texte libre modifié' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPCLAUSE 
( KCAID TEXT IS 'ID unique' , 
	KCATYP TEXT IS 'TYP O/P' , 
	KCAIPB TEXT IS 'IPB' , 
	KCAALX TEXT IS 'ALX' , 
	KCAAVN TEXT IS 'N° avenant' , 
	KCAHIN TEXT IS 'N° histo par avenant' , 
	KCAETAPE TEXT IS 'Etape de génération' , 
	KCAPERI TEXT IS 'Périmètre  BASE RISQUE ....' , 
	KCARSQ TEXT IS 'Risque' , 
	KCAOBJ TEXT IS 'Objet' , 
	KCAINVEN TEXT IS 'ID KPINVEN' , 
	KCAINLGN TEXT IS 'N° de ligne inventaire' , 
	KCAFOR TEXT IS 'Formule' , 
	KCAOPT TEXT IS 'Option' , 
	KCAGAR TEXT IS 'Garantie' , 
	KCACTX TEXT IS 'Contexte' , 
	KCAAJT TEXT IS 'Ajoutée O/N' , 
	KCANTA TEXT IS 'Nature de la clause' , 
	KCAKDUID TEXT IS 'Clause lien KCLAUSE' , 
	KCACLNM1 TEXT IS 'Code clause Nm1' , 
	KCACLNM2 TEXT IS 'Code clause Nm2' , 
	KCACLNM3 TEXT IS 'Code Clause Nm3' , 
	KCAVER TEXT IS 'N° Version' , 
	KCATXL TEXT IS 'Doc libre  lien KPDOC KPDOCEXT' , 
	KCAMER TEXT IS 'CLause mère Lien KCLAUSE' , 
	KCADOC TEXT IS 'Document Impression CP CG CS...' , 
	KCACHI TEXT IS 'Chapitre impression' , 
	KCACHIS TEXT IS 'Sous chapitre impression' , 
	KCAIMP TEXT IS 'N° Impression' , 
	KCACXI TEXT IS 'N° ordonnancement' , 
	KCAIAN TEXT IS 'Impression en annexe O/N' , 
	KCAIAC TEXT IS 'Code annexe' , 
	KCASIT TEXT IS 'Code situation' , 
	KCASITD TEXT IS 'Situation Date' , 
	KCAAVNC TEXT IS 'Avenant de création' , 
	KCACRD TEXT IS 'Création date' , 
	KCAAVNM TEXT IS 'Avenant de modification' , 
	KCAMAJD TEXT IS 'Maj Date' , 
	KCASPA TEXT IS 'Clause spécifique Avenant O/N' , 
	KCATYPO TEXT IS 'Typologie clause Anodine sensible' , 
	KCAAIM TEXT IS 'Attribut d''impression Gras souligné' , 
	KCATAE TEXT IS 'Action enchainée' , 
	KCAELGO TEXT IS 'Elément générateur  Origine' , 
	KCAELGI TEXT IS 'Elément générateur ID' , 
	KCAXTL TEXT IS 'Texte libre O/N' , 
	KCATYPD TEXT IS 'Doc Ajouté/Généré/Externe' , 
	KCAETAFF TEXT IS 'Etape d''affichage' , 
	KCAXTLM TEXT IS 'Texte libre modifié O/N' ) ; 
  
