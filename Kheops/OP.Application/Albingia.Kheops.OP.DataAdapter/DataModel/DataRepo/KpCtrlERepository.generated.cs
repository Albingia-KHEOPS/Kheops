using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{

    public partial class KpCtrlERepository : BaseTableRepository
    {

        private IdentifierGenerator idGenerator;

        #region Query texts        
        //ZALBINKHEO
        const string select = @"SELECT
KEVTYP, KEVIPB, KEVALX, KEVETAPE, KEVETORD
, KEVORDR, KEVPERI, KEVRSQ, KEVOBJ, KEVKBEID
, KEVFOR, KEVOPT, KEVNIVM, KEVCRU, KEVCRD
, KEVCRH, KEVMAJU, KEVMAJD, KEVMAJH, KEVTAG
, KEVTAGC FROM KPCTRLE
WHERE KEVTYP = :parKEVTYP
and KEVIPB = :parKEVIPB
and KEVALX = :parKEVALX
and KEVETAPE = :parKEVETAPE
";
        const string update = @"UPDATE KPCTRLE SET 
KEVTYP = :KEVTYP, KEVIPB = :KEVIPB, KEVALX = :KEVALX, KEVETAPE = :KEVETAPE, KEVETORD = :KEVETORD, KEVORDR = :KEVORDR, KEVPERI = :KEVPERI, KEVRSQ = :KEVRSQ, KEVOBJ = :KEVOBJ, KEVKBEID = :KEVKBEID
, KEVFOR = :KEVFOR, KEVOPT = :KEVOPT, KEVNIVM = :KEVNIVM, KEVCRU = :KEVCRU, KEVCRD = :KEVCRD, KEVCRH = :KEVCRH, KEVMAJU = :KEVMAJU, KEVMAJD = :KEVMAJD, KEVMAJH = :KEVMAJH, KEVTAG = :KEVTAG
, KEVTAGC = :KEVTAGC
 WHERE 
KEVTYP = :parKEVTYP and KEVIPB = :parKEVIPB and KEVALX = :parKEVALX and KEVETAPE = :parKEVETAPE";
        const string delete = @"DELETE FROM KPCTRLE WHERE KEVTYP = :parKEVTYP AND KEVIPB = :parKEVIPB AND KEVALX = :parKEVALX AND KEVETAPE = :parKEVETAPE";
        const string insert = @"INSERT INTO  KPCTRLE (
KEVTYP, KEVIPB, KEVALX, KEVETAPE, KEVETORD
, KEVORDR, KEVPERI, KEVRSQ, KEVOBJ, KEVKBEID
, KEVFOR, KEVOPT, KEVNIVM, KEVCRU, KEVCRD
, KEVCRH, KEVMAJU, KEVMAJD, KEVMAJH, KEVTAG
, KEVTAGC
) VALUES (
:KEVTYP, :KEVIPB, :KEVALX, :KEVETAPE, :KEVETORD
, :KEVORDR, :KEVPERI, :KEVRSQ, :KEVOBJ, :KEVKBEID
, :KEVFOR, :KEVOPT, :KEVNIVM, :KEVCRU, :KEVCRD
, :KEVCRH, :KEVMAJU, :KEVMAJD, :KEVMAJH, :KEVTAG
, :KEVTAGC)";
        const string select_GetByAffaire = @"SELECT
KEVTYP, KEVIPB, KEVALX, KEVETAPE, KEVETORD
, KEVORDR, KEVPERI, KEVRSQ, KEVOBJ, KEVKBEID
, KEVFOR, KEVOPT, KEVNIVM, KEVCRU, KEVCRD
, KEVCRH, KEVMAJU, KEVMAJD, KEVMAJH, KEVTAG
, KEVTAGC FROM KPCTRLE
WHERE KEVIPB = :IPB
and KEVALX = :ALX
";
        #endregion

        public KpCtrlERepository(IDbConnection connection, IdentifierGenerator idGenerator) : base(connection)
        {
            this.idGenerator = idGenerator;
        }

        public KpCtrlE Get(string parKEVTYP, string parKEVIPB, int parKEVALX, string parKEVETAPE)
        {
            return connection.Query<KpCtrlE>(select, new { parKEVTYP, parKEVIPB, parKEVALX, parKEVETAPE }).SingleOrDefault();
        }


        public void Insert(KpCtrlE value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("KEVTYP", value.Kevtyp ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KEVIPB", value.Kevipb ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 9, scale: 0);
            parameters.Add("KEVALX", value.Kevalx, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 4, scale: 0);
            parameters.Add("KEVETAPE", value.Kevetape ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KEVETORD", value.Kevetord, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KEVORDR", value.Kevordr, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 3, scale: 0);
            parameters.Add("KEVPERI", value.Kevperi ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KEVRSQ", value.Kevrsq, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KEVOBJ", value.Kevobj, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KEVKBEID", value.Kevkbeid, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 15, scale: 0);
            parameters.Add("KEVFOR", value.Kevfor, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KEVOPT", value.Kevopt, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 5, scale: 0);
            parameters.Add("KEVNIVM", value.Kevnivm ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KEVCRU", value.Kevcru ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KEVCRD", value.Kevcrd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KEVCRH", value.Kevcrh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 6, scale: 0);
            parameters.Add("KEVMAJU", value.Kevmaju ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 10, scale: 0);
            parameters.Add("KEVMAJD", value.Kevmajd, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 8, scale: 0);
            parameters.Add("KEVMAJH", value.Kevmajh, dbType: DbType.Int32, direction: ParameterDirection.Input, size: 6, scale: 0);
            parameters.Add("KEVTAG", value.Kevtag ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);
            parameters.Add("KEVTAGC", value.Kevtagc ?? String.Empty, dbType: DbType.AnsiStringFixedLength, direction: ParameterDirection.Input, size: 1, scale: 0);

            connection.EnsureOpened().Execute(insert, parameters);
        }
        public void Delete(KpCtrlE value)
        {
            var parameters = new DynamicParameters();
            parameters.Add("parKEVTYP", value.Kevtyp);
            parameters.Add("parKEVIPB", value.Kevipb);
            parameters.Add("parKEVALX", value.Kevalx);
            parameters.Add("parKEVETAPE", value.Kevetape);
            connection.EnsureOpened().Execute(delete, parameters);
        }

            public void Update(KpCtrlE value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEVTYP",value.Kevtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEVIPB",value.Kevipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEVALX",value.Kevalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEVETAPE",value.Kevetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEVETORD",value.Kevetord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEVORDR",value.Kevordr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEVPERI",value.Kevperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEVRSQ",value.Kevrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEVOBJ",value.Kevobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEVKBEID",value.Kevkbeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEVFOR",value.Kevfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEVOPT",value.Kevopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEVNIVM",value.Kevnivm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEVCRU",value.Kevcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEVCRD",value.Kevcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEVCRH",value.Kevcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEVMAJU",value.Kevmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEVMAJD",value.Kevmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEVMAJH",value.Kevmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEVTAG",value.Kevtag??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEVTAGC",value.Kevtagc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("parKEVTYP",value.Kevtyp);
                    parameters.Add("parKEVIPB",value.Kevipb);
                    parameters.Add("parKEVALX",value.Kevalx);
                    parameters.Add("parKEVETAPE",value.Kevetape);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpCtrlE> GetByAffaire(string IPB, int ALX){
                    return connection.EnsureOpened().Query<KpCtrlE>(select_GetByAffaire, new {IPB, ALX}).ToList();
            }
    }
}
