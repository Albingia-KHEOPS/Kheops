CREATE TABLE ZALBINKHEO.YPOECHE ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOECHE de ZALBINKHEO ignoré. 
	PITYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PIIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PIALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PIALX. 
	PIEHA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PIEHM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PIEHJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PIEHE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PIPCR DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	PIPCC DECIMAL(9, 6) NOT NULL DEFAULT 0 , 
	PIPMR DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PIPMC DECIMAL(11, 2) NOT NULL DEFAULT 0 , 
	PIAFR DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	PIIPK NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	PIATT CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPOECHE    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOECHE 
	IS 'Polices/Offres : Echéancier                     PI' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOECHE 
( PITYP IS 'Type' , 
	PIIPB IS 'N° de police / Offre' , 
	PIALX IS 'N° Aliment / connexe' , 
	PIEHA IS 'Année d''échéance' , 
	PIEHM IS 'Mois d''échéance' , 
	PIEHJ IS 'Jour d''échéance' , 
	PIEHE IS 'Emission police 1/2' , 
	PIPCR IS '% de répartition' , 
	PIPCC IS '% répart calculé' , 
	PIPMR IS 'Mnt de répartition' , 
	PIPMC IS 'Mnt répartition calc' , 
	PIAFR IS 'Mnt frais           accessoires' , 
	PIIPK IS 'N° de prime/ police' , 
	PIATT IS 'App taxe Attentat' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOECHE 
( PITYP TEXT IS 'Type  O Offre  P Police' , 
	PIIPB TEXT IS 'N° de Police / Offre' , 
	PIALX TEXT IS 'N° Aliment ou Connexe' , 
	PIEHA TEXT IS 'Année d''échéance' , 
	PIEHM TEXT IS 'Mois d''échéance' , 
	PIEHJ TEXT IS 'Jour d''échéance' , 
	PIEHE TEXT IS 'A l''émission police 1 sinon 2' , 
	PIPCR TEXT IS '% de répartition' , 
	PIPCC TEXT IS '% de répartition calculé' , 
	PIPMR TEXT IS 'Montant de répartition' , 
	PIPMC TEXT IS 'Montant de répartition calculé' , 
	PIAFR TEXT IS 'Montant de frais accessoires' , 
	PIIPK TEXT IS 'N° de prime / Police' , 
	PIATT TEXT IS 'Application taxe Attentat' ) ; 
  
