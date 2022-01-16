CREATE TABLE ZALBINKHEO.YPRTPER ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YPRTPER de ZALBINKHEO ignoré. 
	KAIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KAALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KAALX. 
	KARSQ NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne KARSQ. 
	KAFOR NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KATYP NUMERIC(1, 0) NOT NULL DEFAULT 0 , 
	KADPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KADPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KADPJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KAFPA NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KAFPM NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KAPFJ NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KATPE CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KAIVA DECIMAL(7, 2) NOT NULL DEFAULT 0 , 
	KAVAA DECIMAL(11, 0) NOT NULL DEFAULT 0 , 
	KACOP CHAR(1) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FPRTPER    ; 
  
LABEL ON TABLE ZALBINKHEO.YPRTPER 
	IS 'Poli.RT : Périodes calcul                       KA' ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTPER 
( KAIPB IS 'N° de police / Offre' , 
	KAALX IS 'N° Aliment' , 
	KARSQ IS 'Identifiant risque' , 
	KAFOR IS 'Identifiant formule' , 
	KATYP IS 'Type enregistement' , 
	KADPA IS 'Année Début période' , 
	KADPM IS 'Mois début période' , 
	KADPJ IS 'Jour début période' , 
	KAFPA IS 'Année Fin période' , 
	KAFPM IS 'Mois  Fin de période' , 
	KAPFJ IS 'Jour Fin période' , 
	KATPE IS 'Type de période' , 
	KAIVA IS 'Valeur Indice Périod' , 
	KAVAA IS 'Valeur Assiette' , 
	KACOP IS 'Période complète O/N' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YPRTPER 
( KAIPB TEXT IS 'N° de Police' , 
	KAALX TEXT IS 'N° Aliment' , 
	KARSQ TEXT IS 'Identifiant risque' , 
	KAFOR TEXT IS 'Identifiant formule' , 
	KATYP TEXT IS 'Type Enregist.1 En cours 2 Proch Ech' , 
	KADPA TEXT IS 'Année Début de période' , 
	KADPM TEXT IS 'Mois  Début de période' , 
	KADPJ TEXT IS 'Jour  Début de période' , 
	KAFPA TEXT IS 'Année Fin de période' , 
	KAFPM TEXT IS 'Mois  Fin de période' , 
	KAPFJ TEXT IS 'Jour  Fin de période' , 
	KATPE TEXT IS 'Type de période N afNouv;Terme;Ech P' , 
	KAIVA TEXT IS 'Valeur de l''indice de la période' , 
	KAVAA TEXT IS 'Valeur de l''assiette en cours' , 
	KACOP TEXT IS 'Période complète O/N' ) ; 
  
