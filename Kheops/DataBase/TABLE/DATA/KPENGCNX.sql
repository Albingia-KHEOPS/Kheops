CREATE TABLE ZALBINKHEO.KPENGCNX ( 
--  SQL150B   10   REUSEDLT(*NO) de la table KPENGCNX de ZALBINKHEO ignoré. 
	KIEPJID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KIEORD NUMERIC(5, 0) NOT NULL DEFAULT 0 , 
	KIEKDOID NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FPENGCNX   ; 
  
LABEL ON TABLE ZALBINKHEO.KPENGCNX 
	IS 'KHEOPS Engagement lien connex                  KIE' ; 
  
LABEL ON COLUMN ZALBINKHEO.KPENGCNX 
( KIEPJID IS 'ID lien YPOCONX' , 
	KIEORD IS 'N°d''ordre' , 
	KIEKDOID IS 'Lien KPENG' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.KPENGCNX 
( KIEPJID TEXT IS 'ID  lien YPOCONX  cd PJIDE' , 
	KIEORD TEXT IS 'N° d''Ordre' , 
	KIEKDOID TEXT IS 'lien KPENG' ) ; 
  
