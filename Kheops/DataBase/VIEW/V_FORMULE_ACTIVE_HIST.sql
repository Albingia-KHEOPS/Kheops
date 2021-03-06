CREATE OR REPLACE VIEW ZALBINKHEO.V_FORMULE_ACTIVE_HIST("FOR", "IPB", "AVN")
AS
SELECT DISTINCT KDBFOR, JGIPB, JGAVN FROM (
SELECT JGIPB, KDBFOR, JGAVN,
ZALBINKHEO.F_TO_TIMESTAMP_SAFE( JGVFA * 10000 + JGVFM * 100 + JGVFJ    ) FINOBJ,
ZALBINKHEO.F_TO_TIMESTAMP_SAFE( JEAVA * 10000 + JEAVM * 100 + JEAVJ    ) AVRSQ,
ZALBINKHEO.F_TO_TIMESTAMP_SAFE( JEVFA * 10000 + JEVFM * 100 + JEVFJ    ) FINRSQ,
ZALBINKHEO.F_TO_TIMESTAMP_SAFE( KDBAVA * 10000 + KDBAVM * 100 + KDBAVJ ) AVFOR,
ZALBINKHEO.F_TO_TIMESTAMP_SAFE( PBAVA * 10000 + PBAVM * 100 + PBAVJ    ) AVENT
FROM YHRTOBJ AS O
INNER JOIN YHRTRSQ AS R ON R.JERSQ = O.JGRSQ AND R.JEIPB = O.JGIPB AND R.JEAVN = O.JGAVN
INNER JOIN HPOPTAP AS A ON A.KDDIPB=R.JEIPB AND R.JERSQ = A.KDDRSQ AND R.JEAVN = A.KDDAVN AND ( A.KDDPERI<>'OB' OR A.KDDOBJ = O.JGOBJ )
INNER JOIN HPOPT AS OP ON A.KDDIPB=OP.KDBIPB AND A.KDDKDBID = OP.KDBID AND A.KDDAVN = OP.KDBAVN
INNER JOIN YHPBASE AS ENT ON ENT.PBIPB = A.KDDIPB AND ENT.PBALX = A.KDDALX AND ENT.PBAVN=A.KDDAVN
) SUB
WHERE
(
		NOT(SUB.FINOBJ IS NOT NULL AND SUB.AVRSQ IS NOT NULL AND (SUB.FINOBJ < SUB.AVRSQ))
	AND NOT(SUB.FINOBJ IS NOT NULL AND SUB.AVENT IS NOT NULL AND (SUB.FINOBJ < SUB.AVENT))
	AND NOT(SUB.FINRSQ IS NOT NULL AND SUB.AVENT IS NOT NULL AND (SUB.FINRSQ < SUB.AVENT))
	AND NOT(SUB.FINOBJ IS NOT NULL AND SUB.AVFOR IS NOT NULL AND (SUB.FINOBJ < SUB.AVFOR))
	AND NOT(SUB.FINRSQ IS NOT NULL AND SUB.AVFOR IS NOT NULL AND (SUB.FINRSQ < SUB.AVFOR))
);