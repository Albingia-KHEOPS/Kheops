CREATE TABLE ZALBINKHEO.YPOCOAS ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPOCOAS de ZALBINKHEO ignoré. 
	PHTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PHIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	PHALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne PHALX. 
	PHTAP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PHCIE CHAR(6) CCSID 297 NOT NULL DEFAULT '' , 
	PHINL NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PHPOL CHAR(25) CCSID 297 NOT NULL DEFAULT '' , 
	PHAPP DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	PHCOM DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	PHTXF DECIMAL(5, 2) NOT NULL DEFAULT 0 , 
	PHAFR DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	PHEPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PHEPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PHEPJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PHFPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PHFPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PHFPJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PHIN5 NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	PHTAC CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	PHTAA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	PHTAM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	PHTAJ NUMERIC(2, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPOCOAS    ; 
  
LABEL ON TABLE ZALBINKHEO.YPOCOAS 
	IS 'Polices/Offres : Co_assurance                   PH' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOCOAS 
( PHTYP IS 'Type' , 
	PHIPB IS 'N° de police / Offre' , 
	PHALX IS 'N° Aliment / connexe' , 
	PHTAP IS 'Type A/C Apérit/Coas' , 
	PHCIE IS 'Identifiant Cie' , 
	PHINL IS 'Code interlocuteur' , 
	PHPOL IS 'Référence police' , 
	PHAPP IS '% Apérition' , 
	PHCOM IS 'Part comm apérition' , 
	PHTXF IS 'Taux de frais apérit' , 
	PHAFR IS 'Mnt frais           accessoires' , 
	PHEPA IS 'Effet partici. Année' , 
	PHEPM IS 'Effet partici.Mois' , 
	PHEPJ IS 'Effet partici. Jour' , 
	PHFPA IS 'Fin participat.Année' , 
	PHFPM IS 'Fin particip. Mois' , 
	PHFPJ IS 'Fin particip.Jour' , 
	PHIN5 IS 'Code interlocuteur' , 
	PHTAC IS 'Type accord' , 
	PHTAA IS 'Année Accord' , 
	PHTAM IS 'Mois Accord' , 
	PHTAJ IS 'Jour Accord' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPOCOAS 
( PHTYP TEXT IS 'Type  O Offre  P Police' , 
	PHIPB TEXT IS 'N° de Police / Offre' , 
	PHALX TEXT IS 'N° Aliment ou Connexe' , 
	PHTAP TEXT IS 'Type A/C  Apériteur - Coassureur' , 
	PHCIE TEXT IS 'Identifiant Compagnie' , 
	PHINL TEXT IS 'Code interlocuteur' , 
	PHPOL TEXT IS 'Référence police chez apérit-Coas' , 
	PHAPP TEXT IS '% Apérition' , 
	PHCOM TEXT IS 'Part de commissionnement' , 
	PHTXF TEXT IS 'Taux de frais apérition' , 
	PHAFR TEXT IS 'Montant de frais accessoires' , 
	PHEPA TEXT IS 'Effet participation Année' , 
	PHEPM TEXT IS 'Effet participation Mois' , 
	PHEPJ TEXT IS 'Effet participation Jour' , 
	PHFPA TEXT IS 'Fin de participation Année' , 
	PHFPM TEXT IS 'Fin de participation Mois' , 
	PHFPJ TEXT IS 'Fin de participation Jour' , 
	PHIN5 TEXT IS 'Code interlocuteur sur 5' , 
	PHTAC TEXT IS 'Type accord S Signée N Non signée ..' , 
	PHTAA TEXT IS 'Année Accord' , 
	PHTAM TEXT IS 'Mois Accord' , 
	PHTAJ TEXT IS 'Jour Accord' ) ; 
  
