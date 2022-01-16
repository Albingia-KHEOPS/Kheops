CREATE TABLE ZALBINKHEO.YPOINTE ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOINTE de ZALBINKHEO ignoré. 
	PPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PPALX. 
	PPIIN NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	PPTYI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	PPINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PPPOL CHAR(25) CCSID 297 NOT NULL DEFAULT '' , 
	PPSYM CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPOINTE    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOINTE 
	IS 'Police : Intervenant                            PP' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOINTE 
( PPIPB IS 'N° de police' , 
	PPALX IS 'N° Aliment' , 
	PPIIN IS 'Identif. Intervenant' , 
	PPTYI IS 'Type intervenant' , 
	PPINL IS 'Code interlocuteur' , 
	PPPOL IS 'Référence police' , 
	PPSYM IS 'Nature mandat Syndic' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOINTE 
( PPIPB TEXT IS 'N° de Police' , 
	PPALX TEXT IS 'N° Aliment' , 
	PPIIN TEXT IS 'Identifiant intervenant' , 
	PPTYI TEXT IS 'Type intervenant (Cie Expert Avoc..)' , 
	PPINL TEXT IS 'Code interlocuteur' , 
	PPPOL TEXT IS 'Référence police chez intervenant' , 
	PPSYM TEXT IS 'Nature mandat du Syndic' ) ; 
  
