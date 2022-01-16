CREATE TABLE ZALBINKHEO.YINTNOM ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YINTNOM de ZALBINKHEO ignoré. 
	IMIIN NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	IMINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	IMTNM CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	IMORD NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne IMORD. 
	IMTYI CHAR(2) CCSID 297 NOT NULL DEFAULT '' , 
	IMNOM CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	IMTIT CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FINTNOM    ; 
  
LABEL ON TABLE ZALBINKHEO.YINTNOM 
	IS 'Interv/Interloc. Noms                           IM' ; 
  
LABEL ON COLUMN ZALBINKHEO.YINTNOM 
( IMIIN IS 'Identif. Intervenant' , 
	IMINL IS 'Code interlocuteur' , 
	IMTNM IS 'Type de nom' , 
	IMORD IS 'N° ordre' , 
	IMTYI IS 'Type intervenant' , 
	IMNOM IS 'Nom' , 
	IMTIT IS 'Titre' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YINTNOM 
( IMIIN TEXT IS 'Identifiant intervenant' , 
	IMINL TEXT IS 'Code interlocuteur' , 
	IMTNM TEXT IS 'Type de nom' , 
	IMORD TEXT IS 'N° ordre' , 
	IMTYI TEXT IS 'Type intervenant (Cie Expert Avoc..)' , 
	IMNOM TEXT IS 'Nom' , 
	IMTIT TEXT IS 'Titre' ) ; 
  
