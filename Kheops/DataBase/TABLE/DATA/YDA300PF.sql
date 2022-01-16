CREATE TABLE ZALBINKHEO.YDA300PF ( 
--  SQL150B   10   REUSEDLT(*NO) de la table YDA300PF de ZALBINKHEO ignoré. 
	W2IPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	W2ALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
--  SQL150D   10   EDTCDE ignoré pour la colonne W2ALX. 
	W2ICT NUMERIC(5, 0) NOT NULL DEFAULT 0 )   
	RCDFMT FDA300PF   ; 
  
LABEL ON TABLE ZALBINKHEO.YDA300PF 
	IS 'Delphi Régule Courtier                          W2' ; 
  
LABEL ON COLUMN ZALBINKHEO.YDA300PF 
( W2IPB IS 'N° de police' , 
	W2ALX IS 'N° Aliment' , 
	W2ICT IS 'Identifiant courtier' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YDA300PF 
( W2IPB TEXT IS 'N° de Police' , 
	W2ALX TEXT IS 'N° Aliment' , 
	W2ICT TEXT IS 'Identifiant courtier' ) ; 
  
