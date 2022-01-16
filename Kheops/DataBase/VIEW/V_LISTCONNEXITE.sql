﻿CREATE OR REPLACE VIEW ZALBINKHEO.V_LISTCONNEXITE ( 
	CODECONX , 
	CODEAFFAIRE1 FOR COLUMN CODEA00001 , 
	VERSION1 , 
	TYPE1 , 
	CODEBRANCHE1 FOR COLUMN CODEB00001 , 
	LIBBRANCHE1 FOR COLUMN LIBBR00001 , 
	CODECIBLE1 , 
	LIBCIBLE1 , 
	NUMCONX1 , 
	CODEASS1 , 
	NOMASS1 , 
	AD1_1 , 
	AD2_1 , 
	DEP1 , 
	CP1 , 
	VIL1 , 
	CODEAFFAIRE2 FOR COLUMN CODEA00002 , 
	VERSION2 , 
	CODEBRANCHE2 FOR COLUMN CODEB00002 , 
	LIBBRANCHE2 FOR COLUMN LIBBR00002 , 
	CODECIBLE2 , 
	LIBCIBLE2 , 
	NUMCONX2 , 
	CODEASS2 , 
	NOMASS2 , 
	AD1_2 , 
	AD2_2 , 
	DEP2 , 
	CP2 , 
	VIL2 , 
	CODEOBSV , 
	OBSV ) 
	AS 
	(SELECT CONX1.PJCCX CODECONX, CONX1.PJIPB CODEAFFAIRE1, CONX1.PJALX VERSION1, CONX1.PJTYP TYPE1, BASE1.PBBRA CODEBRANCHE1, PARBRC1.TPLIB LIBBRANCHE1,  
							ENT1.KAACIBLE CODECIBLE1, CIB1.KAHDESC LIBCIBLE1,  
							CONX1.PJCNX NUMCONX1, BASE1.PBIAS CODEASS1, ASSN1.ANNOM NOMASS1,  
								ASS1.ASAD1 AD1_1, ASS1.ASAD2 AD2_1, ASS1.ASDEP DEP1, ASS1.ASCPO CP1, ASS1.ASVIL VIL1,  
							CONX2.PJIPB CODEAFFAIRE2, CONX2.PJALX VERSION2,	BASE2.PBBRA CODEBRANCHE2, PARBRC2.TPLIB LIBBRANCHE2,  
							ENT2.KAACIBLE CODECIBLE2, CIB2.KAHDESC LIBCIBLE2,  
							CONX2.PJCNX NUMCONX2, BASE2.PBIAS CODEASS2, ASSN2.ANNOM NOMASS2,  
								ASS2.ASAD1 AD1_2, ASS2.ASAD2 AD2_2, ASS2.ASDEP DEP2, ASS2.ASCPO CP2, ASS2.ASVIL VIL2,  
							CONX1.PJOBS CODEOBSV, KAJOBSV OBSV  
						FROM ZALBINKHEO.YPOCONX CONX1  
							LEFT JOIN ZALBINKHEO.YPOBASE BASE1 ON BASE1.PBIPB = CONX1.PJIPB AND BASE1.PBALX = CONX1.PJALX AND BASE1.PBTYP = CONX1.PJTYP  
							LEFT JOIN ZALBINKHEO.YYYYPAR PARBRC1 ON PARBRC1.TCON = 'GENER' AND PARBRC1.TFAM = 'BRCHE' AND PARBRC1.TCOD = BASE1.PBBRA  
							LEFT JOIN ZALBINKHEO.KPENT ENT1 ON BASE1.PBIPB = ENT1.KAAIPB AND BASE1.PBALX = ENT1.KAAALX AND BASE1.PBTYP = ENT1.KAATYP  
							LEFT JOIN ZALBINKHEO.KCIBLE CIB1 ON ENT1.KAACIBLE = CIB1.KAHCIBLE  
							LEFT JOIN ZALBINKHEO.YASSNOM ASSN1 ON BASE1.PBIAS = ASSN1.ANIAS AND ASSN1.ANINL = 0 AND ASSN1.ANTNM = 'A'  
							LEFT JOIN ZALBINKHEO.YASSURE ASS1 ON BASE1.PBIAS = ASS1.ASIAS  
							LEFT JOIN ZALBINKHEO.KPOBSV ON CONX1.PJOBS = KAJCHR  
							LEFT JOIN ZALBINKHEO.YPOCONX CONX2 ON CONX1.PJTYP = CONX2.PJTYP AND CONX1.PJCCX = CONX2.PJCCX AND CONX1.PJCNX = CONX2.PJCNX  
								AND (CONX1.PJIPB != CONX2.PJIPB OR (CONX1.PJIPB = CONX2.PJIPB AND CONX1.PJALX != CONX2.PJALX))  
							LEFT JOIN ZALBINKHEO.YPOBASE BASE2 ON BASE2.PBIPB = CONX2.PJIPB AND BASE2.PBALX = CONX2.PJALX AND BASE2.PBTYP = CONX2.PJTYP  
							LEFT JOIN ZALBINKHEO.YYYYPAR PARBRC2 ON PARBRC2.TCON = 'GENER' AND PARBRC2.TFAM = 'BRCHE' AND PARBRC2.TCOD = BASE2.PBBRA  
							LEFT JOIN ZALBINKHEO.KPENT ENT2 ON BASE2.PBIPB = ENT2.KAAIPB AND BASE2.PBALX = ENT2.KAAALX AND BASE2.PBTYP = ENT2.KAATYP  
							LEFT JOIN ZALBINKHEO.KCIBLE CIB2 ON ENT2.KAACIBLE = CIB2.KAHCIBLE  
							LEFT JOIN ZALBINKHEO.YASSNOM ASSN2 ON BASE2.PBIAS = ASSN2.ANIAS AND ASSN2.ANINL = 0 AND ASSN2.ANTNM = 'A'  
							LEFT JOIN ZALBINKHEO.YASSURE ASS2 ON BASE2.PBIAS = ASS2.ASIAS) ; 
  
LABEL ON COLUMN ZALBINKHEO.V_LISTCONNEXITE 
( CODECONX IS 'Cause de connexité' , 
	CODEAFFAIRE1 IS 'N° de police connexe' , 
	VERSION1 IS 'N° Aliment connexe' , 
	TYPE1 IS 'Type' , 
	CODEBRANCHE1 IS 'Branche' , 
	LIBBRANCHE1 IS 'Libellé' , 
	CODECIBLE1 IS 'Cible' , 
	LIBCIBLE1 IS 'Description' , 
	NUMCONX1 IS 'N° de connexité' , 
	CODEASS1 IS 'Identifiant Assuré' , 
	NOMASS1 IS 'Nom' , 
	AD1_1 IS 'Adresse 1' , 
	AD2_1 IS 'Adresse 2' , 
	DEP1 IS 'Département' , 
	CP1 IS 'Code postal 3 car' , 
	VIL1 IS 'Ville' , 
	CODEAFFAIRE2 IS 'N° de police connexe' , 
	VERSION2 IS 'N° Aliment connexe' , 
	CODEBRANCHE2 IS 'Branche' , 
	LIBBRANCHE2 IS 'Libellé' , 
	CODECIBLE2 IS 'Cible' , 
	LIBCIBLE2 IS 'Description' , 
	NUMCONX2 IS 'N° de connexité' , 
	CODEASS2 IS 'Identifiant Assuré' , 
	NOMASS2 IS 'Nom' , 
	AD1_2 IS 'Adresse 1' , 
	AD2_2 IS 'Adresse 2' , 
	DEP2 IS 'Département' , 
	CP2 IS 'Code postal 3 car' , 
	VIL2 IS 'Ville' , 
	CODEOBSV IS 'Lien KPOBSV' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.V_LISTCONNEXITE 
( CODECONX TEXT IS 'Cause de connexité' , 
	CODEAFFAIRE1 TEXT IS 'N° de Police  connexe' , 
	VERSION1 TEXT IS 'N° Aliment connexe' , 
	TYPE1 TEXT IS 'Type  O Offre  P Police' , 
	CODEBRANCHE1 TEXT IS 'Branche' , 
	LIBBRANCHE1 TEXT IS 'Libellé' , 
	CODECIBLE1 TEXT IS 'Cible' , 
	LIBCIBLE1 TEXT IS 'Description' , 
	NUMCONX1 TEXT IS 'N° de connexité' , 
	CODEASS1 TEXT IS 'Identifiant Assuré 10/00' , 
	NOMASS1 TEXT IS 'Nom' , 
	AD1_1 TEXT IS 'Adresse 1' , 
	AD2_1 TEXT IS 'Adresse 2' , 
	DEP1 TEXT IS 'Département' , 
	CP1 TEXT IS '3 derniers caractères code postal' , 
	VIL1 TEXT IS 'Ville' , 
	CODEAFFAIRE2 TEXT IS 'N° de Police  connexe' , 
	VERSION2 TEXT IS 'N° Aliment connexe' , 
	CODEBRANCHE2 TEXT IS 'Branche' , 
	LIBBRANCHE2 TEXT IS 'Libellé' , 
	CODECIBLE2 TEXT IS 'Cible' , 
	LIBCIBLE2 TEXT IS 'Description' , 
	NUMCONX2 TEXT IS 'N° de connexité' , 
	CODEASS2 TEXT IS 'Identifiant Assuré 10/00' , 
	NOMASS2 TEXT IS 'Nom' , 
	AD1_2 TEXT IS 'Adresse 1' , 
	AD2_2 TEXT IS 'Adresse 2' , 
	DEP2 TEXT IS 'Département' , 
	CP2 TEXT IS '3 derniers caractères code postal' , 
	VIL2 TEXT IS 'Ville' , 
	CODEOBSV TEXT IS 'Lien KPOBSV' ) ; 
  
