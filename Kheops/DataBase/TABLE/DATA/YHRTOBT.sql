CREATE TABLE ZALBINKHEO.YHRTOBT ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YHRTOBT de ZALBINKHEO ignoré. 
	KFIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KFALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFALX. 
	KFAVN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	KFHIN DECIMAL(3, 0) NOT NULL DEFAULT 0 , 
	KFRSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFRSQ. 
	KFOBJ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFOBJ. 
	KFDUV NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFDUV. 
	KFDUU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFDDA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KFDDM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFDDJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFDFA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KFDFM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFDFJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFESV NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KFESV. 
	KFESU CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KFEDA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KFEDM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFEDJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFEFA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KFEFM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFEFJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KFTDF CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FHRTOBT    ; 
  
LABEL ON TABLE ZALBINKHEO.YHRTOBT 
	IS 'H-Poli.RT:Objet_TRC                             KF' ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTOBT 
( KFIPB IS 'N° de police' , 
	KFALX IS 'N° Aliment' , 
	KFAVN IS 'N° avenant' , 
	KFHIN IS 'N° historique avenan' , 
	KFRSQ IS 'Identifiant risque' , 
	KFOBJ IS 'Identifiant objet' , 
	KFDUV IS 'Durée travaux valeur' , 
	KFDUU IS 'Durée travaux Unité' , 
	KFDDA IS 'Dur trav:Année début' , 
	KFDDM IS 'Dur trav: mois début' , 
	KFDDJ IS 'Dur trav: Jour début' , 
	KFDFA IS 'Dur trav : Année fin' , 
	KFDFM IS 'Dur trav: Mois fin' , 
	KFDFJ IS 'Dur trav: Jour fin' , 
	KFESV IS 'Durée Essais valeur' , 
	KFESU IS 'Durée Essais Unité' , 
	KFEDA IS 'Essais Année début' , 
	KFEDM IS 'Essais Mois début' , 
	KFEDJ IS 'Essais Jour début' , 
	KFEFA IS 'Essais Année Fin' , 
	KFEFM IS 'Essais Mois Fin' , 
	KFEFJ IS 'Essais Jour Fin' , 
	KFTDF IS 'Date trav définitive' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YHRTOBT 
( KFIPB TEXT IS 'N° de Police' , 
	KFALX TEXT IS 'N° Aliment' , 
	KFAVN TEXT IS 'N° avenant' , 
	KFHIN TEXT IS 'N° historique par avenant' , 
	KFRSQ TEXT IS 'Identifiant risque' , 
	KFOBJ TEXT IS 'Identifiant objet' , 
	KFDUV TEXT IS 'Durée des travaux : Valeur' , 
	KFDUU TEXT IS 'Durée des travaux: Unité' , 
	KFDDA TEXT IS 'Durée travaux : Année début' , 
	KFDDM TEXT IS 'Durée travaux : Mois début' , 
	KFDDJ TEXT IS 'Durée travaux : Jour début' , 
	KFDFA TEXT IS 'Durée travaux : Année fin' , 
	KFDFM TEXT IS 'Durée travaux : Mois fin' , 
	KFDFJ TEXT IS 'Durée travaux : Jour fin' , 
	KFESV TEXT IS 'Durée des Essais : Valeur' , 
	KFESU TEXT IS 'Durée des Essais : Unité' , 
	KFEDA TEXT IS 'Essais : Année début' , 
	KFEDM TEXT IS 'Essais : Mois début' , 
	KFEDJ TEXT IS 'Essais : Jour début' , 
	KFEFA TEXT IS 'Essais : Année Fin' , 
	KFEFM TEXT IS 'Essais : Mois Fin' , 
	KFEFJ TEXT IS 'Essais : Jour Fin' , 
	KFTDF TEXT IS 'Date travaux définitive (O/N)' ) ; 
  
