CREATE TABLE ZALBINKHEO.KPAS400 ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPAS400 de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour KPAS400 de ZALBINKHEO. 
	KHPTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KHPIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KHPALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KHPALX. 
	KHPAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KHPSUA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KHPNUM NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KHPSBR CHAR(3) CCSID 297 NOT NULL DEFAULT '' , 
	KHPACTG CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHPACID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KHPUSR CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHPUSED CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KHPCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KHPCRH NUMERIC(6, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPAS400    ; 
  
LABEL ON TABLE ZALBINKHEO.KPAS400 
	IS 'Passage info Web => AS400                      KHP' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPAS400 
( KHPTYP IS 'Typ' , 
	KHPIPB IS 'IPB' , 
	KHPALX IS 'ALX' , 
	KHPAVN IS 'N° avenant' , 
	KHPSUA IS 'N° Sinistre AA surv' , 
	KHPNUM IS 'N° de sinistre : N°' , 
	KHPSBR IS 'N° sinistre Sous_bra' , 
	KHPACTG IS 'Actde de gestion' , 
	KHPACID IS 'Id lien  divers' , 
	KHPUSR IS 'User du travail' , 
	KHPUSED IS 'User pour édition' , 
	KHPCRD IS 'Date' , 
	KHPCRH IS 'Heure' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPAS400 
( KHPTYP TEXT IS 'Typ' , 
	KHPIPB TEXT IS 'IPB' , 
	KHPALX TEXT IS 'ALX' , 
	KHPAVN TEXT IS 'N° avenant' , 
	KHPSUA TEXT IS 'N° de sinistre : AA surv' , 
	KHPNUM TEXT IS 'N° de sinistre : N°' , 
	KHPSBR TEXT IS 'N° de sinistre : Sous-branche' , 
	KHPACTG TEXT IS 'Acte de gestion' , 
	KHPACID TEXT IS 'Id lien divers (Régule ou autre...)' , 
	KHPUSR TEXT IS 'User du travail' , 
	KHPUSED TEXT IS 'User pour édition' , 
	KHPCRD TEXT IS 'Date' , 
	KHPCRH TEXT IS 'Heure' ) ; 
  
