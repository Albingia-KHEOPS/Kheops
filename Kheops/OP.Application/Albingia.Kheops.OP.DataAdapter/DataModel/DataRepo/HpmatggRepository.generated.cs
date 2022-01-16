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

    public  partial class  HpmatggRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEETYP, KEEIPB, KEEALX, KEEAVN, KEEHIN
, KEECHR, KEETYE, KEEKDCID, KEEVOLET, KEEKAKID
, KEEBLOC, KEEKAEID, KEEGARAN, KEESEQ, KEENIV
, KEEVID FROM HPMATGG
WHERE KEETYP = :KEETYP
and KEEIPB = :KEEIPB
and KEEALX = :KEEALX
and KEEAVN = :KEEAVN
and KEEHIN = :KEEHIN
and KEECHR = :KEECHR
";
            const string update=@"UPDATE HPMATGG SET 
KEETYP = :KEETYP, KEEIPB = :KEEIPB, KEEALX = :KEEALX, KEEAVN = :KEEAVN, KEEHIN = :KEEHIN, KEECHR = :KEECHR, KEETYE = :KEETYE, KEEKDCID = :KEEKDCID, KEEVOLET = :KEEVOLET, KEEKAKID = :KEEKAKID
, KEEBLOC = :KEEBLOC, KEEKAEID = :KEEKAEID, KEEGARAN = :KEEGARAN, KEESEQ = :KEESEQ, KEENIV = :KEENIV, KEEVID = :KEEVID
 WHERE 
KEETYP = :KEETYP and KEEIPB = :KEEIPB and KEEALX = :KEEALX and KEEAVN = :KEEAVN and KEEHIN = :KEEHIN and KEECHR = :KEECHR";
            const string delete=@"DELETE FROM HPMATGG WHERE KEETYP = :KEETYP AND KEEIPB = :KEEIPB AND KEEALX = :KEEALX AND KEEAVN = :KEEAVN AND KEEHIN = :KEEHIN AND KEECHR = :KEECHR";
            const string insert=@"INSERT INTO  HPMATGG (
KEETYP, KEEIPB, KEEALX, KEEAVN, KEEHIN
, KEECHR, KEETYE, KEEKDCID, KEEVOLET, KEEKAKID
, KEEBLOC, KEEKAEID, KEEGARAN, KEESEQ, KEENIV
, KEEVID
) VALUES (
:KEETYP, :KEEIPB, :KEEALX, :KEEAVN, :KEEHIN
, :KEECHR, :KEETYE, :KEEKDCID, :KEEVOLET, :KEEKAKID
, :KEEBLOC, :KEEKAEID, :KEEGARAN, :KEESEQ, :KEENIV
, :KEEVID)";
            const string select_GetByAffaire=@"SELECT
KEETYP, KEEIPB, KEEALX, KEEAVN, KEEHIN
, KEECHR, KEETYE, KEEKDCID, KEEVOLET, KEEKAKID
, KEEBLOC, KEEKAEID, KEEGARAN, KEESEQ, KEENIV
, KEEVID FROM HPMATGG
WHERE KEETYP = :KEETYP
and KEEIPB = :KEEIPB
and KEEALX = :KEEALX
and KEEAVN = :KEEAVN
";
            #endregion

            public HpmatggRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpMatGg Get(string KEETYP, string KEEIPB, int KEEALX, int KEEAVN, int KEEHIN, int KEECHR){
                return connection.Query<KpMatGg>(select, new {KEETYP, KEEIPB, KEEALX, KEEAVN, KEEHIN, KEECHR}).SingleOrDefault();
            }


            public void Insert(KpMatGg value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEETYP",value.Keetyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEEIPB",value.Keeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEEALX",value.Keealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEEAVN",value.Keeavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEEHIN",value.Keehin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEECHR",value.Keechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEETYE",value.Keetye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEEKDCID",value.Keekdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEEVOLET",value.Keevolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEEKAKID",value.Keekakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEEBLOC",value.Keebloc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEEKAEID",value.Keekaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEEGARAN",value.Keegaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEESEQ",value.Keeseq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEENIV",value.Keeniv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEEVID",value.Keevid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpMatGg value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEETYP",value.Keetyp);
                    parameters.Add("KEEIPB",value.Keeipb);
                    parameters.Add("KEEALX",value.Keealx);
                    parameters.Add("KEEAVN",value.Keeavn);
                    parameters.Add("KEEHIN",value.Keehin);
                    parameters.Add("KEECHR",value.Keechr);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpMatGg value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEETYP",value.Keetyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEEIPB",value.Keeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEEALX",value.Keealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEEAVN",value.Keeavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEEHIN",value.Keehin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEECHR",value.Keechr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KEETYE",value.Keetye??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEEKDCID",value.Keekdcid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEEVOLET",value.Keevolet??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEEKAKID",value.Keekakid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEEBLOC",value.Keebloc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEEKAEID",value.Keekaeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEEGARAN",value.Keegaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEESEQ",value.Keeseq, dbType:DbType.Int64, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEENIV",value.Keeniv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEEVID",value.Keevid??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEETYP",value.Keetyp);
                    parameters.Add("KEEIPB",value.Keeipb);
                    parameters.Add("KEEALX",value.Keealx);
                    parameters.Add("KEEAVN",value.Keeavn);
                    parameters.Add("KEEHIN",value.Keehin);
                    parameters.Add("KEECHR",value.Keechr);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpMatGg> GetByAffaire(string KEETYP, string KEEIPB, int KEEALX, int KEEAVN){
                    return connection.EnsureOpened().Query<KpMatGg>(select_GetByAffaire, new {KEETYP, KEEIPB, KEEALX, KEEAVN}).ToList();
            }
    }
}
