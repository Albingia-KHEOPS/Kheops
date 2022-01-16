CREATE PROCEDURE SP_REMOVEINFOTABLE ( 
	IN P_CANEVAS INTEGER , 
	IN P_CODEOFFRE CHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_CANEVASID INTEGER , 
	OUT P_ERROROUT INTEGER ) 
	LANGUAGE SQL 
	SPECIFIC SP_REMOVEINFOTABLE 
	NOT DETERMINISTIC 
	MODIFIES SQL DATA 
	CALLED ON NULL INPUT 
	SET OPTION  ALWBLK = *ALLREAD , 
	ALWCPYDTA = *OPTIMIZE , 
	COMMIT = *CHG , 
	CLOSQLCSR = *ENDMOD , 
	DECRESULT = (31, 31, 00) , 
	DFTRDBCOL = ZALBINKHEO , 
	DYNDFTCOL = *YES , 
	SQLPATH = 'ZALBINKHEO, ZALBINKMOD' , 
	DYNUSRPRF = *USER , 
	SRTSEQ = *HEX   
	P1 : BEGIN ATOMIC 
  
/* --                      EXPLICATIONS PARAMETRES                      --             
               P_CANEVAS        => mettre à 1 si on veut supprimer tous les canevas             
                                                           => mettre à 0 si on veut tout supprimer             
               P_CODEOFFRE   => remplir si on veut supprimer une offre / un contrat / un canevas spécifique             
               P_VERSION         => remplir si on veut cibler une version d'une offre / d'un contrat / d'un canevas             
               P_CANEVASID    => remplir si on veut supprimer un canevas en particulier à partir de son ID             
                --                          FIN EXPLICATIONS PARAMETRES                             --*/ 
  
DECLARE V_CODEIPB CHAR ( 9 ) DEFAULT '%' ; 
DECLARE V_TYPE CHAR ( 1 ) DEFAULT 'O' ; 
  
IF ( P_CANEVAS = 1 ) THEN 
SET V_CODEIPB = 'CV%' ; 
END IF ; 
  
IF ( P_CODEOFFRE != '' ) THEN 
SET V_CODEIPB = P_CODEOFFRE ; 
END IF ; 
  
IF ( P_CANEVASID != 0 ) THEN 
SELECT KGOCNVA , KGOTYP INTO V_CODEIPB , V_TYPE FROM KCANEV WHERE KGOID = P_CANEVASID ; 
END IF ; 
  
IF ( V_CODEIPB != '%' AND V_CODEIPB != 'CV%' ) THEN 
CALL SP_DELETEOFFRE ( V_CODEIPB , 0 , V_TYPE ) ; 
DELETE FROM KCANEV WHERE KGOID = P_CANEVASID ; 
END IF ; 
  
/*            
                --  LECTURE DE YPOBASE / SUPPRESSION YADRESS / SUPPRESSION DE YPOBASE  --            
               FOR CURSOR_PBOBASE AS FREE_LIST CURSOR FOR            
                              SELECT PBIPB CODEOFFRE , PBALX VERSION , PBADH ADRID FROM YPOBASE            
                                            WHERE TRIM ( PBIPB ) LIKE TRIM ( V_CODEIPB )            
               DO            
                              DELETE FROM YADRESS WHERE ABPCHR = ADRID ;            
                              DELETE FROM YPOBASE WHERE TRIM ( PBIPB ) = TRIM ( CODEOFFRE ) AND PBALX = VERSION ;            
               END FOR ;            
                           
                --  SUPPRESSION DE YPRTENT  --            
               DELETE FROM YPRTENT WHERE TRIM ( JDIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPENT  --            
               DELETE FROM KPENT WHERE TRIM ( KAAIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPDESI  --            
               DELETE FROM KPDESI WHERE TRIM ( KADIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPOBSV  --            
               DELETE FROM KPOBSV WHERE TRIM ( KAJIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE YPRTRSQ  --            
               DELETE FROM YPRTRSQ WHERE TRIM ( JEIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPRSQ  --            
               DELETE FROM KPRSQ WHERE TRIM ( KABIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE YPRTOBJ  --            
               DELETE FROM YPRTOBJ WHERE TRIM ( JGIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPOBJ  --            
               DELETE FROM KPOBJ WHERE TRIM ( KACIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  LECTURE DES ADRESSES DE RISQUE/OBJET / SUPPRESSION YADRESS / SUPPRESSION DE YPRTADR  --            
               FOR CURSOR_ADR AS FREE_LIST CURSOR FOR            
                              SELECT JFADH ADRID FROM YPRTADR            
                                            WHERE TRIM ( JFIPB ) = TRIM ( V_CODEIPB )            
               DO            
                              DELETE FROM YADRESS WHERE ABPCHR = ADRID ;            
                              DELETE FROM YPRTADR WHERE JFADH = ADRID ;            
               END FOR ;            
                           
                --  SUPPRESSION DE KPFOR  --            
               DELETE FROM KPFOR WHERE TRIM ( KDAIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                                          
                --  SUPPRESSION DE KPOPT  --            
               DELETE FROM KPOPT WHERE TRIM ( KDBIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                                          
                --  SUPPRESSION DE KPOPTD  --            
               DELETE FROM KPOPTD WHERE TRIM ( KDCIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                                          
                --  SUPPRESSION DE KPOPTDW  --            
               DELETE FROM KPOPTDW WHERE TRIM ( KDCIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                                          
                --  SUPPRESSION DE KPOPTAP  --            
               DELETE FROM KPOPTAP WHERE TRIM ( KDDIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                                          
                --  SUPPRESSION DE KPOPTAPW  --            
               DELETE FROM KPOPTAPW WHERE TRIM ( KDDIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                                          
                --  SUPPRESSION DE KPGARAN  --            
               DELETE FROM KPGARAN WHERE TRIM ( KDEIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPGARAW  --            
               DELETE FROM KPGARAW WHERE TRIM ( KDEIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPGARAH  --            
               DELETE FROM KPGARAH WHERE TRIM ( KDEIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPGARTAR  --            
               DELETE FROM KPGARTAR WHERE TRIM ( KDGIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPGARTAW  --            
               DELETE FROM KPGARTAW WHERE TRIM ( KDGIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  LECTURE DES LCI COMPLEXES / SUPPRESION KPEXPLCID / SUPPRESSION KPEXPLCI  --            
               FOR CURSOR_LCICPX AS FREE_LIST CURSOR FOR            
                              SELECT KDIID LCICPXID FROM KPEXPLCI            
                                            WHERE TRIM ( KDIIPB ) = TRIM ( V_CODEIPB )            
               DO            
                              DELETE FROM KPEXPLCID WHERE KDJKDIID = LCICPXID ;            
                              DELETE FROM KPEXPLCI WHERE KDIID = LCICPXID ;            
               END FOR ;            
                           
                --  LECTURE DES FRANCHISES COMPLEXES / SUPPRESION KPEXPFRHD / SUPPRESSION KPEXPFRH  --            
               FOR CURSOR_FRHCPX AS FREE_LIST CURSOR FOR            
                              SELECT KDKID FRHCPXID FROM KPEXPFRH            
                                            WHERE TRIM ( KDKIPB ) = TRIM ( V_CODEIPB )            
               DO            
                              DELETE FROM KPEXPFRHD WHERE KDLKDKID = FRHCPXID ;            
                              DELETE FROM KPEXPFRH WHERE KDKID = FRHCPXID ;            
               END FOR ;            
                           
                --  SUPPRESSION DE KPGARAP  --            
               DELETE FROM KPGARAP WHERE TRIM ( KDFIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPINVEN  --            
               DELETE FROM KPINVEN WHERE TRIM ( KBEIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPINVED  --            
               DELETE FROM KPINVED WHERE TRIM ( KBFIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPINVAPP  --            
               DELETE FROM KPINVAPP WHERE TRIM ( KBGIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPIOPT  --            
               DELETE FROM KPIOPT WHERE TRIM ( KFCIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPIRSGA  --            
               DELETE FROM KPIRSGA WHERE TRIM ( KFDIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPIRSOB  --            
               DELETE FROM KPIRSOB WHERE TRIM ( KFAIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  LECTURE DES CLAUSES / SUPPRESSION KPCLATXT / SUPPRESSION KPCLAUSE  --            
               FOR CURSOR_CLAUSE AS FREE_LIST CURSOR FOR            
                              SELECT KCAID CLAUSEID , KCATXL CLAUSELIBREID FROM KPCLAUSE            
                                            WHERE TRIM ( KCAIPB ) = TRIM ( V_CODEIPB )            
               DO            
                              IF ( CLAUSELIBREID != 0 ) THEN            
                                            DELETE FROM KPCLATXT WHERE KFOID = CLAUSELIBREID ;            
                              END IF ;            
                              DELETE FROM KPCLAUSE WHERE KCAID = CLAUSEID ;            
               END FOR ;            
                           
                --  LECTURE DES CLAUSES W / SUPPRESSION KPCLATXT / SUPPRESSION KPCLAUSW  --            
               FOR CURSOR_CLAUSW AS FREE_LIST CURSOR FOR            
                              SELECT KCBIPB CODEOFFRE , KCBALX VERSION , KCBTXL CLAUSELIBREID FROM KPCLAUSW            
                                            WHERE TRIM ( KCBIPB ) = TRIM ( V_CODEIPB )            
               DO            
                              IF ( CLAUSELIBREID != 0 ) THEN            
                                            DELETE FROM KPCLATXT WHERE KFOID = CLAUSELIBREID ;            
                              END IF ;            
                              DELETE FROM KPCLAUSW WHERE TRIM ( KCBIPB ) = TRIM ( CODEOFFRE ) AND KCBALX = VERSION ;            
               END FOR ;            
                           
                --  SUPPRESSION DE KPCTRLE  --            
               DELETE FROM KPCTRLE WHERE TRIM ( KEVIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KPCTRLA  --            
               DELETE FROM KPCTRLA WHERE TRIM ( KGTIPB ) LIKE TRIM ( V_CODEIPB ) ;            
                           
                --  SUPPRESSION DE KCANEV  --            
               DELETE FROM KCANEV WHERE TRIM ( KGOCNVA ) LIKE TRIM ( V_CODEIPB ) ;            
                */ 
SET P_ERROROUT = 1 ; 
  
END P1  ; 
  

  

