﻿CREATE INDEX ZALBINKHEO.YPOBAS0017 
	ON ZALBINKHEO.YPOBAS0005   
	( PBSAA * 1000 + PBSAM * 100 + PBSAJ )   
	  
	RCDFMT YPOBASEDSA ADD ALL COLUMNS ; 
  
LABEL ON INDEX ZALBINKHEO.YPOBAS0017 
	IS 'Ancien nom YPOBASEDSA de ZALBINKHEO, propr ALBINGI' ; 
  
