CREATE TABLE ZALBINKHEO.YCOURTN ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YCOURTN de ZALBINKHEO ignoré. 
	TNICT NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	TNINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	TNTNM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TNORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne TNORD. 
	TNTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TNNOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	TNTIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	TNXN5 NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FCOURTN    ; 
  
LABEL ON TABLE ZALBINKHEO.YCOURTN 
	IS 'Noms Courtiers/Interlocuteurs                   TN' ; 
  
LABEL ON COLUMN ZALBINKHEO.YCOURTN 
( TNICT IS 'Identifiant courtier' , 
	TNINL IS 'Code interlocuteur  Courtier' , 
	TNTNM IS 'Type de nom' , 
	TNORD IS 'N° ordre' , 
	TNTYP IS 'Type Prospect/Court' , 
	TNNOM IS 'Nom' , 
	TNTIT IS 'Titre' , 
	TNXN5 IS 'Code interlocuteur' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YCOURTN 
( TNICT TEXT IS 'Identifiant courtier' , 
	TNINL TEXT IS 'Code interlocuteur Courtier' , 
	TNTNM TEXT IS 'Type de nom' , 
	TNORD TEXT IS 'N° ordre' , 
	TNTYP TEXT IS 'Type Prospect/Courtier' , 
	TNNOM TEXT IS 'Nom' , 
	TNTIT TEXT IS 'Titre' , 
	TNXN5 TEXT IS 'Code interlocuteur sur 5' ) ; 
  
