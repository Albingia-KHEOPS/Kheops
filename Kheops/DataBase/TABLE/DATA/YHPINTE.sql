CREATE TABLE ZALBINKHEO.YHPINTE ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHPINTE de ZALBINKHEO ignoré. 
	PPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PPALX. 
	PPAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PPHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PPIIN NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	PPTYI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	PPINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PPPOL CHAR(25) CCSID 297 NOT NULL DEFAULT '' , 
	PPSYM CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHPINTE    ; 
  
LABEL ON TABLE ZALBINKHEO.YHPINTE 
	IS 'Histo Polices : Intervenants' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPINTE 
( PPIPB IS 'N° de police' , 
	PPALX IS 'N° Aliment' , 
	PPAVN IS 'N° avenant' , 
	PPHIN IS 'N° historique avenan' , 
	PPIIN IS 'Identif. Intervenant' , 
	PPTYI IS 'Type intervenant' , 
	PPINL IS 'Code interlocuteur' , 
	PPPOL IS 'Référence police' , 
	PPSYM IS 'Nature mandat Syndic' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHPINTE 
( PPIPB TEXT IS 'N° de Police' , 
	PPALX TEXT IS 'N° Aliment' , 
	PPAVN TEXT IS 'N° avenant' , 
	PPHIN TEXT IS 'N° historique par avenant' , 
	PPIIN TEXT IS 'Identifiant intervenant' , 
	PPTYI TEXT IS 'Type intervenant (Cie Expert Avoc..)' , 
	PPINL TEXT IS 'Code interlocuteur' , 
	PPPOL TEXT IS 'Référence police chez intervenant' , 
	PPSYM TEXT IS 'Nature mandat du Syndic' ) ; 
  
