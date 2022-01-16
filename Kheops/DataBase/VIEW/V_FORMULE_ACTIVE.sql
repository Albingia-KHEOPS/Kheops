﻿CREATE OR REPLACE VIEW ZALBINKHEO.V_FORMULE_ACTIVE("FOR", "IPB")
AS
SELECT DISTINCT KDBFOR, JGIPB FROM (
SELECT JGIPB, KDBFOR,
F_TO_TIMESTAMP_SAFE( JGVFA * 10000 + JGVFM * 100 + JGVFJ    ) FINOBJ,
F_TO_TIMESTAMP_SAFE( JEAVA * 10000 + JEAVM * 100 + JEAVJ    ) AVRSQ,
F_TO_TIMESTAMP_SAFE( JEVFA * 10000 + JEVFM * 100 + JEVFJ    ) FINRSQ,
F_TO_TIMESTAMP_SAFE( KDBAVA * 10000 + KDBAVM * 100 + KDBAVJ ) AVFOR,
F_TO_TIMESTAMP_SAFE( PBAVA * 10000 + PBAVM * 100 + PBAVJ    ) AVENT
FROM YPRTOBJ AS O
INNER JOIN YPRTRSQ AS R ON R.JERSQ = O.JGRSQ AND R.JEIPB = O.JGIPB
INNER JOIN KPOPTAP AS A ON A.KDDIPB=R.JEIPB AND R.JERSQ = A.KDDRSQ AND ( A.KDDPERI<>'OB' OR A.KDDOBJ = O.JGOBJ )
INNER JOIN KPOPT AS OP ON A.KDDIPB=OP.KDBIPB AND A.KDDKDBID = OP.KDBID
INNER JOIN YPOBASE AS ENT ON ENT.PBIPB = A.KDDIPB AND ENT.PBALX = A.KDDALX
) SUB
WHERE
(
		NOT(SUB.FINOBJ IS NOT NULL AND SUB.AVRSQ IS NOT NULL AND (SUB.FINOBJ < SUB.AVRSQ))
	AND NOT(SUB.FINOBJ IS NOT NULL AND SUB.AVENT IS NOT NULL AND (SUB.FINOBJ < SUB.AVENT))
	AND NOT(SUB.FINRSQ IS NOT NULL AND SUB.AVENT IS NOT NULL AND (SUB.FINRSQ < SUB.AVENT))
	AND NOT(SUB.FINOBJ IS NOT NULL AND SUB.AVFOR IS NOT NULL AND (SUB.FINOBJ < SUB.AVFOR))
	AND NOT(SUB.FINRSQ IS NOT NULL AND SUB.AVFOR IS NOT NULL AND (SUB.FINRSQ < SUB.AVFOR))
);