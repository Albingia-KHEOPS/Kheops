CREATE TABLE ZALBINKHEO.YYYYCHR ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YYYYCHR de ZALBINKHEO ignoré. 
	YHCLE CHAR(40) CCSID 297 NOT NULL DEFAULT '' , 
	YHNUD DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne YHNUD. 
	YHNUF DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne YHNUF. 
	YHNUU DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne YHNUU. 
	YHNUA DECIMAL(7, 0) NOT NULL DEFAULT 0 , 
	YHOBS CHAR(40) CCSID 297 NOT NULL DEFAULT '' )   
	RCDFMT FYYYCHR    ; 
  
LABEL ON TABLE ZALBINKHEO.YYYYCHR 
	IS 'BASE : Numérotation chronologique (séquentiel)  YH' ; 
  
LABEL ON COLUMN ZALBINKHEO.YYYYCHR 
( YHCLE IS 'Clé                 Numéro              Chronologique' , 
	YHNUD IS 'N°                  Début               Tranche' , 
	YHNUF IS 'N°                  Fin                 Tranche' , 
	YHNUU IS 'Dernier             N°                  Utilisé' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YYYYCHR 
( YHCLE TEXT IS 'Clé du Numéro chronologique' , 
	YHNUD TEXT IS 'N° chrono. de début de tranche' , 
	YHNUF TEXT IS 'N° chrono. de fin de tranche' , 
	YHNUU TEXT IS 'Dernier n° chrono. utilisé' , 
	YHNUA TEXT IS 'Début alerte au n°' , 
	YHOBS TEXT IS 'observation' ) ; 
  
