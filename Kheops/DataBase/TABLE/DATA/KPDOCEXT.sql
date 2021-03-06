CREATE TABLE ZALBINKHEO.KPDOCEXT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPDOCEXT de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPDOCEXT de ZALBINKHEO. 
	KERID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KERTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KERIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KERALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KERSUA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KERNUM NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KERSBR CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	KERSERV CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KERACTG CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KERAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KERORD NUMERIC(7, 2) NOT NULL DEFAULT 0 , 
	KERTYPO CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KERLIB CHAR(255) CCSID 297 NOT NULL DEFAULT '' , 
	KERNOM CHAR(255) CCSID 297 NOT NULL DEFAULT '' , 
	KERCHM CHAR(255) CCSID 297 NOT NULL DEFAULT '' , 
	KERSTU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KERSIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KERSTD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KERSTH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KERCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KERCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KERCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 , 
	KERREF CHAR(100) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPDOCEXT   ; 
  
LABEL ON TABLE ZALBINKHEO.KPDOCEXT 
	IS 'Documents Externes                             KER' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPDOCEXT 
( KERID IS 'ID unique' , 
	KERTYP IS 'Type O/P' , 
	KERIPB IS 'IPB' , 
	KERALX IS 'ALX' , 
	KERSUA IS 'Sinistre AA' , 
	KERNUM IS 'Sinistre N°' , 
	KERSBR IS 'Sinistre Sbr' , 
	KERSERV IS 'Service' , 
	KERACTG IS 'Acte de gestion' , 
	KERAVN IS 'N° Avenant' , 
	KERORD IS 'N° Ordre' , 
	KERTYPO IS 'Typologie' , 
	KERLIB IS 'Libellé' , 
	KERNOM IS 'Nom du document' , 
	KERCHM IS 'Chemin complet' , 
	KERSTU IS 'Situation User' , 
	KERSIT IS 'Situation Code' , 
	KERSTD IS 'Situation Date' , 
	KERSTH IS 'Situation Heure' , 
	KERCRU IS 'Création user' , 
	KERCRD IS 'Création Date' , 
	KERCRH IS 'Création Heure' , 
	KERREF IS 'Référence' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPDOCEXT 
( KERID TEXT IS 'ID unique' , 
	KERTYP TEXT IS 'Type O/P' , 
	KERIPB TEXT IS 'IPB' , 
	KERALX TEXT IS 'ALX' , 
	KERSUA TEXT IS 'Sinistre AA' , 
	KERNUM TEXT IS 'Sinistre N°' , 
	KERSBR TEXT IS 'Sinistre Sbr' , 
	KERSERV TEXT IS 'Service  (Produ,Sinistre...)' , 
	KERACTG TEXT IS 'Acte de gestion' , 
	KERAVN TEXT IS 'N° Avenant' , 
	KERORD TEXT IS 'N° Ordre' , 
	KERTYPO TEXT IS 'Typologie : inventaire .....' , 
	KERLIB TEXT IS 'Libellé' , 
	KERNOM TEXT IS 'Nom du document' , 
	KERCHM TEXT IS 'Chemin complet' , 
	KERSTU TEXT IS 'Situation User' , 
	KERSIT TEXT IS 'Situation Code' , 
	KERSTD TEXT IS 'Situation Date' , 
	KERSTH TEXT IS 'Situation Heure' , 
	KERCRU TEXT IS 'Création User' , 
	KERCRD TEXT IS 'Création Date' , 
	KERCRH TEXT IS 'Création Heure' , 
	KERREF TEXT IS 'Référence' ) ; 
  
