CREATE TRIGGER ZALBINKHEO.TR_AFTERDELYPAR 
	AFTER DELETE ON ZALBINKHEO.YYYYPAR 
	REFERENCING OLD AS OLDLINE 
	FOR EACH ROW 
	MODE DB2SQL 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *NONE , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = *NONE , 
	DYNDFTCOL = *NO , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	BEGIN ATOMIC 
  
DELETE FROM YKPREDTA . YYYYPAR WHERE YKPREDTA . YYYYPAR . TCON = OLDLINE . TCON AND YKPREDTA . YYYYPAR . TFAM = OLDLINE . TFAM AND YKPREDTA . YYYYPAR . TCOD = OLDLINE . TCOD ; 
END  ; 
  
LABEL ON TABLE ZALBINKHEO.YYYYPAR 
	IS 'Paramètres : Codes  (Séquentiel)' ; 
  
LABEL ON COLUMN ZALBINKHEO.YYYYPAR 
( TCON IS 'Concept' , 
	TFAM IS 'Famille' , 
	TCOD IS 'Code' , 
	TPLIB IS 'Libellé paramètre' , 
	TPCN1 IS 'Complément          Numérique 1' , 
	TPCN2 IS 'Complément          Numérique 2' , 
	TPCA1 IS 'Complément          Alphanumérique 1' , 
	TPCA2 IS 'Complément          Alphanumérique 2' , 
	TPTYP IS 'Type paramètre      (pour restriction)' , 
	TPLIL IS 'Libellé long        Paramètre' , 
	TFILT IS 'FILTRE               ' ) ; 
  
LABEL ON COLUMN ZALBINKHEO.YYYYPAR 
( TCON TEXT IS 'Concept' , 
	TFAM TEXT IS 'Famille' , 
	TCOD TEXT IS 'Code' , 
	TPLIB TEXT IS 'Libellé paramètre' , 
	TPCN1 TEXT IS 'Complément numérique 1' , 
	TPCN2 TEXT IS 'Complément numérique 2' , 
	TPCA1 TEXT IS 'Complément alphanumérique 1' , 
	TPCA2 TEXT IS 'Complément alphanumérique 2' , 
	TPTYP TEXT IS 'Type paramètre (pour restriction)' , 
	TPLIL TEXT IS 'Libellé long paramètre' , 
	TFILT TEXT IS 'FILTRE' ) ; 
  
