using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public  partial class  YcourriRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_CourrierContrat=@"SELECT
DACOU AS DACOU, DAIPB AS DAIPB, DAALX AS DAALX, DAAVN AS DAAVN, DAASI AS DAASI
, DANSI AS DANSI, DASSI AS DASSI, DAARC AS DAARC, DANRC AS DANRC, DATBR AS DATBR
, DASRV AS DASRV, DATTR AS DATTR, DATLT AS DATLT, DALET AS DALET, DAVER AS DAVER
, DAFML AS DAFML, DATDS AS DATDS, DATYI AS DATYI, DAIDS AS DAIDS, DAINL AS DAINL
, DASIT AS DASIT, DASTA AS DASTA, DASTM AS DASTM, DASTJ AS DASTJ, DASPA AS DASPA
, DASPM AS DASPM, DASPJ AS DASPJ, DALBC AS DALBC, DATOR AS DATOR, DATEV AS DATEV
, DATAE AS DATAE, DANCP AS DANCP, DASOU AS DASOU, DAGES AS DAGES, DABUC AS DABUC
, DABUS AS DABUS, DACRU AS DACRU, DACRA AS DACRA, DACRM AS DACRM, DACRJ AS DACRJ
, DAMJU AS DAMJU, DAMJA AS DAMJA, DAMJM AS DAMJM, DAMJJ AS DAMJJ, DALES AS DALES
, DAENV AS DAENV, DACRH AS DACRH, DAMJH AS DAMJH, DACRP AS DACRP, DAMJP AS DAMJP
, DALTO AS DALTO, DANUR AS DANUR, DARFG AS DARFG, DAIN5 AS DAIN5, LGID4 AS LGID4
, LGCHD AS LGCHD, LGDOC AS LGDOC, LGEXT AS LGEXT, LGFPV AS LGFPV, LGCRU AS LGCRU
 FROM YCOURRI
LEFT JOIN ylncchd on LGID1='COURRI' and LGID2 = ' ' and LGID3 = DACOU
WHERE DAIPB = :numeroAffaire
and DAALX = :version
FETCH FIRST 300 ROWS ONLY
";
            #endregion

            public YcourriRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<CourrierContrat> CourrierContrat(string numeroAffaire, int version){
                    return connection.EnsureOpened().Query<CourrierContrat>(select_CourrierContrat, new {numeroAffaire, version}).ToList();
            }
    }
}
