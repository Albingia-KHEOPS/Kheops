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

    public  partial class  YsincouRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_CourrierSinistreContrat=@"SELECT
SHSUA AS SHSUA, SHNUM AS SHNUM, SHSBR AS SHSBR, SHNUC AS SHNUC, SHTBR AS SHTBR
, SHSRV AS SHSRV, SHTTR AS SHTTR, SHELG AS SHELG, SHCDS AS SHCDS, SHLET AS SHLET
, SHNAE AS SHNAE, SHTEV AS SHTEV, SHTAE AS SHTAE, SHAFF AS SHAFF, SHLBC AS SHLBC
, SHTLT AS SHTLT, SHTDS AS SHTDS, SHTYI AS SHTYI, SHIDS AS SHIDS, SHINL AS SHINL
, SHSIT AS SHSIT, SHSTA AS SHSTA, SHSTM AS SHSTM, SHSTJ AS SHSTJ, SHSPA AS SHSPA
, SHSPM AS SHSPM, SHSPJ AS SHSPJ, SHNCP AS SHNCP, SHAJT AS SHAJT, SHTVL AS SHTVL
, SHCRU AS SHCRU, SHCRA AS SHCRA, SHCRM AS SHCRM, SHCRJ AS SHCRJ, SHMJU AS SHMJU
, SHMJA AS SHMJA, SHMJM AS SHMJM, SHMJJ AS SHMJJ, SHLES AS SHLES, SHENV AS SHENV
, SHCHR AS SHCHR, SHRCD AS SHRCD, SHRCC AS SHRCC, SHCOU AS SHCOU, SHCDO AS SHCDO
, SHACT AS SHACT, SHTRF AS SHTRF, SHCOP AS SHCOP, SHRFG AS SHRFG, SHIN5 AS SHIN5
, LGID4 AS LGID4, LGCHD AS LGCHD, LGDOC AS LGDOC, LGEXT AS LGEXT, LGFPV AS LGFPV
, LGCRU AS LGCRU FROM YSINCOU
left join ysinist on sisua = shsua and sinum=shnum and sisbr = shsbr
left join ylncchd on lgid1 ='SINCOU' and lgid2 = digits(shsua) concat  '_' concat digits(shnum)
WHERE SIIPB = :numeroAffaire
and SIALX = :version
FETCH FIRST 200 ROWS ONLY
";
            #endregion

            public YsincouRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<CourrierSinistreContrat> CourrierSinistreContrat(string numeroAffaire, int version){
                    return connection.EnsureOpened().Query<CourrierSinistreContrat>(select_CourrierSinistreContrat, new {numeroAffaire, version}).ToList();
            }
    }
}
