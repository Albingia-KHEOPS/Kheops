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

    public  partial class  HpctrlRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KEUID, KEUTYP, KEUIPB, KEUALX, KEUAVN
, KEUHIN, KEUETAPE, KEUETORD, KEUORDR, KEUPERI
, KEURSQ, KEUOBJ, KEUINVEN, KEUINLGN, KEUFOR
, KEUOPT, KEUGAR, KEUMSG, KEUNIVM, KEUCRU
, KEUCRD, KEUCRH FROM HPCTRL
WHERE KEUID = :KEUID
and KEUAVN = :KEUAVN
and KEUHIN = :KEUHIN
";
            const string update=@"UPDATE HPCTRL SET 
KEUID = :KEUID, KEUTYP = :KEUTYP, KEUIPB = :KEUIPB, KEUALX = :KEUALX, KEUAVN = :KEUAVN, KEUHIN = :KEUHIN, KEUETAPE = :KEUETAPE, KEUETORD = :KEUETORD, KEUORDR = :KEUORDR, KEUPERI = :KEUPERI
, KEURSQ = :KEURSQ, KEUOBJ = :KEUOBJ, KEUINVEN = :KEUINVEN, KEUINLGN = :KEUINLGN, KEUFOR = :KEUFOR, KEUOPT = :KEUOPT, KEUGAR = :KEUGAR, KEUMSG = :KEUMSG, KEUNIVM = :KEUNIVM, KEUCRU = :KEUCRU
, KEUCRD = :KEUCRD, KEUCRH = :KEUCRH
 WHERE 
KEUID = :KEUID and KEUAVN = :KEUAVN and KEUHIN = :KEUHIN";
            const string delete=@"DELETE FROM HPCTRL WHERE KEUID = :KEUID AND KEUAVN = :KEUAVN AND KEUHIN = :KEUHIN";
            const string insert=@"INSERT INTO  HPCTRL (
KEUID, KEUTYP, KEUIPB, KEUALX, KEUAVN
, KEUHIN, KEUETAPE, KEUETORD, KEUORDR, KEUPERI
, KEURSQ, KEUOBJ, KEUINVEN, KEUINLGN, KEUFOR
, KEUOPT, KEUGAR, KEUMSG, KEUNIVM, KEUCRU
, KEUCRD, KEUCRH
) VALUES (
:KEUID, :KEUTYP, :KEUIPB, :KEUALX, :KEUAVN
, :KEUHIN, :KEUETAPE, :KEUETORD, :KEUORDR, :KEUPERI
, :KEURSQ, :KEUOBJ, :KEUINVEN, :KEUINLGN, :KEUFOR
, :KEUOPT, :KEUGAR, :KEUMSG, :KEUNIVM, :KEUCRU
, :KEUCRD, :KEUCRH)";
            const string select_GetByAffaire=@"SELECT
KEUID, KEUTYP, KEUIPB, KEUALX, KEUAVN
, KEUHIN, KEUETAPE, KEUETORD, KEUORDR, KEUPERI
, KEURSQ, KEUOBJ, KEUINVEN, KEUINLGN, KEUFOR
, KEUOPT, KEUGAR, KEUMSG, KEUNIVM, KEUCRU
, KEUCRD, KEUCRH FROM HPCTRL
WHERE KEUTYP = :KEUTYP
and KEUIPB = :KEUIPB
and KEUALX = :KEUALX
and KEUAVN = :KEUAVN
";
            #endregion

            public HpctrlRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpCtrl Get(Int64 KEUID, int KEUAVN, int KEUHIN){
                return connection.Query<KpCtrl>(select, new {KEUID, KEUAVN, KEUHIN}).SingleOrDefault();
            }


            public void Insert(KpCtrl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEUID",value.Keuid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEUTYP",value.Keutyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEUIPB",value.Keuipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEUALX",value.Keualx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEUAVN",value.Keuavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEUHIN",value.Keuhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEUETAPE",value.Keuetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEUETORD",value.Keuetord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEUORDR",value.Keuordr, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEUPERI",value.Keuperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEURSQ",value.Keursq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUOBJ",value.Keuobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUINVEN",value.Keuinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEUINLGN",value.Keuinlgn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUFOR",value.Keufor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUOPT",value.Keuopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUGAR",value.Keugar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEUMSG",value.Keumsg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KEUNIVM",value.Keunivm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEUCRU",value.Keucru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEUCRD",value.Keucrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEUCRH",value.Keucrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpCtrl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEUID",value.Keuid);
                    parameters.Add("KEUAVN",value.Keuavn);
                    parameters.Add("KEUHIN",value.Keuhin);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpCtrl value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KEUID",value.Keuid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEUTYP",value.Keutyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEUIPB",value.Keuipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KEUALX",value.Keualx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KEUAVN",value.Keuavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEUHIN",value.Keuhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEUETAPE",value.Keuetape??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEUETORD",value.Keuetord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KEUORDR",value.Keuordr, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEUPERI",value.Keuperi??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEURSQ",value.Keursq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUOBJ",value.Keuobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUINVEN",value.Keuinven, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KEUINLGN",value.Keuinlgn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUFOR",value.Keufor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUOPT",value.Keuopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KEUGAR",value.Keugar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEUMSG",value.Keumsg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:100, scale: 0);
                    parameters.Add("KEUNIVM",value.Keunivm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KEUCRU",value.Keucru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KEUCRD",value.Keucrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KEUCRH",value.Keucrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KEUID",value.Keuid);
                    parameters.Add("KEUAVN",value.Keuavn);
                    parameters.Add("KEUHIN",value.Keuhin);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpCtrl> GetByAffaire(string KEUTYP, string KEUIPB, int KEUALX, int KEUAVN){
                    return connection.EnsureOpened().Query<KpCtrl>(select_GetByAffaire, new {KEUTYP, KEUIPB, KEUALX, KEUAVN}).ToList();
            }
    }
}
