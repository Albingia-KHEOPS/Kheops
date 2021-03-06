CREATE TABLE ZALBINKHEO.HPENGVEN ( 
--  SQL150B   10   REUSEDLT(*NO) de la table HPENGVEN de ZALBINKHEO ignoré. 
--  SQL1506   30   Clé ou attribut ignoré pour HPENGVEN de ZALBINKHEO. 
	KDQID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDQTYP CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDQIPB CHAR(9) CCSID 297 NOT NULL DEFAULT '' , 
	KDQALX NUMERIC(4, 0) NOT NULL DEFAULT 0 , 
	KDQAVN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KDQHIN NUMERIC(3, 0) NOT NULL DEFAULT 0 , 
	KDQKDPID NUMERIC(15, 0) NOT NULL DEFAULT 0 , 
	KDQFAM CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDQVEN NUMERIC(2, 0) NOT NULL DEFAULT 0 , 
	KDQENGC NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDQENGF NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDQENGOK CHAR(1) CCSID 297 NOT NULL DEFAULT '' , 
	KDQCRU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDQCRD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDQMAJU CHAR(10) CCSID 297 NOT NULL DEFAULT '' , 
	KDQMAJD NUMERIC(8, 0) NOT NULL DEFAULT 0 , 
	KDQLCT NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDQCAT NUMERIC(13, 0) NOT NULL DEFAULT 0 , 
	KDQKDOID NUMERIC(15, 0) NOT NULL DEFAULT 0 )   
	RCDFMT HPENGVEN   ; 
  
LABEL ON TABLE ZALBINKHEO.HPENGVEN 
	IS 'KHEOPS histo Engagement/Ventil' ; 
  
LABEL ON COLUMN ZALBINKHEO.HPENGVEN 
( KDQID IS 'ID unique' , 
	KDQTYP IS 'Type O/P' , 
	KDQIPB IS 'IPB' , 
	KDQALX IS 'ALX' , 
	KDQAVN IS 'N°  avenant' , 
	KDQHIN IS 'N° histo par avenant' , 
	KDQKDPID IS 'Lien KPENGFAM' , 
	KDQFAM IS 'Famille réassurance' , 
	KDQVEN IS 'Ventilation' , 
	KDQENGC IS 'Engag Calc cpt 100%' , 
	KDQENGF IS 'Engagement Forcé cpt' , 
	KDQENGOK IS 'Dans engagement O/N' , 
	KDQCRU IS 'création User' , 
	KDQCRD IS 'Création Date' , 
	KDQMAJU IS 'MAJ User' , 
	KDQMAJD IS 'MAJ Date' , 
	KDQLCT IS 'LCI Part Totale 100%' , 
	KDQCAT IS 'Capitaux 100%' , 
	KDQKDOID IS 'Lien KPENG' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.HPENGVEN 
( KDQID TEXT IS 'ID unique' , 
	KDQTYP TEXT IS 'Type O/P' , 
	KDQIPB TEXT IS 'IPB' , 
	KDQALX TEXT IS 'ALX' , 
	KDQAVN TEXT IS 'N°  avenant' , 
	KDQHIN TEXT IS 'N° histo par avenant' , 
	KDQKDPID TEXT IS 'Lien KPENGFAM' , 
	KDQFAM TEXT IS 'Famille de réassurance' , 
	KDQVEN TEXT IS 'Ventilation   (KREAVEN)' , 
	KDQENGC TEXT IS 'Engagement Calculé cpt  100%' , 
	KDQENGF TEXT IS 'Engagement Forcé cpt 100 %' , 
	KDQENGOK TEXT IS 'Entre dans engagement O/N' , 
	KDQCRU TEXT IS 'Création User' , 
	KDQCRD TEXT IS 'Création Date' , 
	KDQMAJU TEXT IS 'MAJ User' , 
	KDQMAJD TEXT IS 'MAJ Date' , 
	KDQLCT TEXT IS 'LCI part Totale  100%' , 
	KDQCAT TEXT IS 'Capitaux Part Totale 100%' , 
	KDQKDOID TEXT IS 'Lien KPENG' ) ; 
  
