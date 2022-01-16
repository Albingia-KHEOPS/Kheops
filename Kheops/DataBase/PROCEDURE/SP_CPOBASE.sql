﻿CREATE PROCEDURE SP_CPOBASE ( 
	IN P_CODEOFFRE VARCHAR(9) , 
	IN P_VERSION INTEGER , 
	IN P_TYPE VARCHAR(1) , 
	IN P_DATESAISIE VARCHAR(8) , 
	IN P_HEURESAISIE VARCHAR(4) , 
	IN P_DATESYSTEME VARCHAR(8) , 
	IN P_USER VARCHAR(15) , 
	IN P_NEWVERSION INTEGER , 
	IN P_TRAITEMENT VARCHAR(1) ) 
	DYNAMIC RESULT SETS 1 
	LANGUAGE SQL 
	SPECIFIC SP_CPOBASE 
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
  
  
DECLARE V_NEWCODEID INTEGER DEFAULT 0 ; 
DECLARE V_LIENADR INTEGER DEFAULT 0 ; 
  
DECLARE V_YEARSAISIE VARCHAR ( 4 ) DEFAULT '' ; 
DECLARE V_MONTHSAISIE VARCHAR ( 2 ) DEFAULT '' ; 
DECLARE V_DAYSAISIE VARCHAR ( 2 ) DEFAULT '' ; 
  
DECLARE V_YEARSYSTEME VARCHAR ( 4 ) DEFAULT '' ; 
DECLARE V_MONTHSYSTEME VARCHAR ( 2 ) DEFAULT '' ; 
DECLARE V_DAYSYSTEME VARCHAR ( 2 ) DEFAULT '' ; 
  
SET V_YEARSAISIE = SUBSTR ( P_DATESAISIE , 1 , 4 ) ; 
SET V_MONTHSAISIE = SUBSTR ( P_DATESAISIE , 5 , 2 ) ; 
SET V_DAYSAISIE = SUBSTR ( P_DATESAISIE , 7 , 2 ) ; 
  
SET V_YEARSYSTEME = SUBSTR ( P_DATESYSTEME , 1 , 4 ) ; 
SET V_MONTHSYSTEME = SUBSTR ( P_DATESYSTEME , 5 , 2 ) ; 
SET V_DAYSYSTEME = SUBSTR ( P_DATESYSTEME , 7 , 2 ) ; 
  
  
SELECT PBADH INTO V_LIENADR FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE ; 
IF ( V_LIENADR != 0 ) THEN 
CALL SP_SECOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'YADRESS' , V_LIENADR , V_NEWCODEID ) ; 
IF ( V_NEWCODEID = 0 ) THEN 
CALL SP_YCHRONO ( 'ADRESSE_CHRONO' , V_NEWCODEID ) ; 
CALL SP_INCOPID ( P_CODEOFFRE , P_VERSION , P_TYPE , 'YADRESS' , V_LIENADR , V_NEWCODEID ) ; 
END IF ; 
END IF ; 
  
IF ( P_TRAITEMENT <> 'V' ) THEN 
	INSERT INTO YPOBASE ( PBTYP , PBIPB , PBALX , PBTBR , PBIAS , PBREF , PBBRA , PBSBR , PBCAT , PBICT , PBTIL , PBIN5 , PBOCT , PBAD1 , PBAD2 , 
	PBDEP , PBCPO , PBVIL , PBPAY , PBSAA , PBSAM , PBSAJ , PBSAH , PBAPR , PBAPP , PBMO1 , PBMO2 , PBMO3 , PBCLE , PBANT , PBCTD , PBCTU , PBAPO , PBDUR , 
	PBREL , PBRLD , PBATT , PBRMP , PBVRF , PBOLV , PBFOA , PBFOM , PBCOU , PBFOE , PBDEV , PBRGT , PBTCO , PBNAT , PBDST , PBLIE , PBGES , PBSOU , PBORI , 
	PBMAI , PBEFA , PBEFM , PBEFJ , PBOFF , PBOFV , PBIPA , PBALA , PBECM , PBECJ , PBPCV , PBCTA , PBNRQ , PBNPL , PBPER , PBAVN , PBAVC , PBAVA , PBAVM , 
	PBAVJ , PBRSC , PBRSA , PBRSM , PBRSJ , PBMER , PBETA , PBSIT , PBSTA , PBSTM , PBSTJ , PBSTQ , PBEDT , PBTAC , PBTAA , PBTAM , PBTAJ , PBCRU , PBCRA , 
	PBCRM , PBCRJ , PBMJU , PBMJA , PBMJM , PBMJJ , PBDEU , PBDEA , PBDEM , PBDEJ , PBCTP , PBEFH , PBFEA , PBFEM , PBFEJ , PBFEH , PBSTP , PBNVA , PBFEC , 
	PBPOR , PBCON , PBTTR , PBAVK , PBADH , PBSTF , PBORK ) 
	( SELECT PBTYP , PBIPB , P_NEWVERSION , PBTBR , PBIAS , PBREF , PBBRA , PBSBR , PBCAT , PBICT , PBTIL , PBIN5 , PBOCT , PBAD1 , PBAD2 , 
	PBDEP , PBCPO , PBVIL , PBPAY , PBSAA , PBSAM , PBSAJ , PBSAH , PBAPR , PBAPP , PBMO1 , PBMO2 , PBMO3 , PBCLE , PBANT , PBCTD , PBCTU , PBAPO , PBDUR , 
	PBREL , PBRLD , PBATT , PBRMP , PBVRF , PBOLV , PBFOA , PBFOM , PBCOU , PBFOE , PBDEV , PBRGT , PBTCO , PBNAT , PBDST , PBLIE , PBGES , PBSOU , PBORI , 
	PBMAI , PBEFA , PBEFM , PBEFJ , PBOFF , PBOFV , PBIPA , PBALA , PBECM , PBECJ , PBPCV , PBCTA , PBNRQ , PBNPL , PBPER , PBAVN , PBAVC , PBAVA , PBAVM , 
	PBAVJ , PBRSC , PBRSA , PBRSM , PBRSJ , PBMER , 'N' , 'A' , V_YEARSYSTEME , V_MONTHSYSTEME , V_DAYSYSTEME , PBSTQ , 'N' , 'N' , 0 , 0 , 0 , P_USER , V_YEARSYSTEME , 
	V_MONTHSYSTEME , V_DAYSYSTEME , P_USER , V_YEARSYSTEME , V_MONTHSYSTEME , V_DAYSYSTEME , 'SPRINKS' , V_YEARSYSTEME , V_MONTHSYSTEME , V_DAYSYSTEME , PBCTP , PBEFH , PBFEA , PBFEM , PBFEJ , PBFEH , PBSTP , '' , '' , 
	PBPOR , PBCON , PBTTR , PBAVK , V_NEWCODEID , '' , 'KHE' 
	FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE ) ; 
ELSE 
	INSERT INTO YPOBASE ( PBTYP , PBIPB , PBALX , PBTBR , PBIAS , PBREF , PBBRA , PBSBR , PBCAT , PBICT , PBTIL , PBIN5 , PBOCT , PBAD1 , PBAD2 , 
	PBDEP , PBCPO , PBVIL , PBPAY , PBSAA , PBSAM , PBSAJ , PBSAH , PBAPR , PBAPP , PBMO1 , PBMO2 , PBMO3 , PBCLE , PBANT , PBCTD , PBCTU , PBAPO , PBDUR , 
	PBREL , PBRLD , PBATT , PBRMP , PBVRF , PBOLV , PBFOA , PBFOM , PBCOU , PBFOE , PBDEV , PBRGT , PBTCO , PBNAT , PBDST , PBLIE , PBGES , PBSOU , PBORI , 
	PBMAI , PBEFA , PBEFM , PBEFJ , PBOFF , PBOFV , PBIPA , PBALA , PBECM , PBECJ , PBPCV , PBCTA , PBNRQ , PBNPL , PBPER , PBAVN , PBAVC , PBAVA , PBAVM , 
	PBAVJ , PBRSC , PBRSA , PBRSM , PBRSJ , PBMER , PBETA , PBSIT , PBSTA , PBSTM , PBSTJ , PBSTQ , PBEDT , PBTAC , PBTAA , PBTAM , PBTAJ , PBCRU , PBCRA , 
	PBCRM , PBCRJ , PBMJU , PBMJA , PBMJM , PBMJJ , PBDEU , PBDEA , PBDEM , PBDEJ , PBCTP , PBEFH , PBFEA , PBFEM , PBFEJ , PBFEH , PBSTP , PBNVA , PBFEC , 
	PBPOR , PBCON , PBTTR , PBAVK , PBADH , PBSTF , PBORK ) 
	( SELECT PBTYP , PBIPB , P_NEWVERSION , PBTBR , PBIAS , PBREF , PBBRA , PBSBR , PBCAT , PBICT , PBTIL , PBIN5 , PBOCT , PBAD1 , PBAD2 , 
	PBDEP , PBCPO , PBVIL , PBPAY , PBSAA , PBSAM , PBSAJ , PBSAH , PBAPR , PBAPP , PBMO1 , PBMO2 , PBMO3 , PBCLE , PBANT , PBCTD , PBCTU , PBAPO , PBDUR , 
	PBREL , PBRLD , PBATT , PBRMP , PBVRF , PBOLV , PBFOA , PBFOM , PBCOU , PBFOE , PBDEV , PBRGT , PBTCO , PBNAT , PBDST , PBLIE , PBGES , PBSOU , PBORI , 
	PBMAI , PBEFA , PBEFM , PBEFJ , PBOFF , PBOFV , PBIPA , PBALA , PBECM , PBECJ , PBPCV , PBICT , PBNRQ , PBNPL , PBPER , PBAVN , PBAVC , PBAVA , PBAVM , 
	PBAVJ , PBRSC , PBRSA , PBRSM , PBRSJ , PBMER , 'N' , 'A' , V_YEARSYSTEME , V_MONTHSYSTEME , V_DAYSYSTEME , PBSTQ , 'N' , 'N' , 0 , 0 , 0 , P_USER , V_YEARSYSTEME , 
	V_MONTHSYSTEME , V_DAYSYSTEME , P_USER , V_YEARSYSTEME , V_MONTHSYSTEME , V_DAYSYSTEME , 'SPRINKS' , V_YEARSYSTEME , V_MONTHSYSTEME , V_DAYSYSTEME , PBCTP , PBEFH , PBFEA , PBFEM , PBFEJ , PBFEH , PBSTP , '' , '' , 
	PBPOR , PBCON , PBTTR , PBAVK , V_NEWCODEID , '' , 'KHE' 
	FROM YPOBASE WHERE PBIPB = P_CODEOFFRE AND PBALX = P_VERSION AND PBTYP = P_TYPE ) ; 
END IF ; 
  
END P1  ; 
  

  

