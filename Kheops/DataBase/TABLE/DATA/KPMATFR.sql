CREATE TABLE ZALBINKHEO.KPMATFR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPMATFR de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPMATFR de ZALBINKHEO. 
	KEBTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEBIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KEBALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KEBCHR NUMERIC(7, 0) NOT NULL DEFAULT 0 , 
	KEBTYE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEBRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KEBRSQ. 
	KEBOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KEBOBJ. 
	KEBINV CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KEBVID CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPMATFR    ; 
  
LABEL ON TABLE ZALBINKHEO.KPMATFR 
	IS 'KHEOPS Matrice/formule Risque' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATFR 
( KEBTYP IS 'Type O/P' , 
	KEBIPB IS 'IPB' , 
	KEBALX IS 'ALX' , 
	KEBCHR IS 'Chrono Affichage ID' , 
	KEBTYE IS 'Type enregistrement' , 
	KEBRSQ IS 'Identifiant risque' , 
	KEBOBJ IS 'Identifiant objet' , 
	KEBINV IS 'inventaire O/N' , 
	KEBVID IS 'Non affecté O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPMATFR 
( KEBTYP TEXT IS 'Type O/P' , 
	KEBIPB TEXT IS 'IPB' , 
	KEBALX TEXT IS 'ALX' , 
	KEBCHR TEXT IS 'N° Chrono Affichage ID unique' , 
	KEBTYE TEXT IS 'Type enregistrement  Risque Objet' , 
	KEBRSQ TEXT IS 'Identifiant risque' , 
	KEBOBJ TEXT IS 'Identifiant objet' , 
	KEBINV TEXT IS 'inventaire  O/N' , 
	KEBVID TEXT IS 'Non affecté O/N' ) ; 
  
