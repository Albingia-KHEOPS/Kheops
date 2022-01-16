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

    public  partial class  UtilisateurRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
UTIUT, UTNOM, UTPNM, UTGRP, UTSGR
, UTHIE, UTBUR, UTBAU, UTBRA, UTIEX
, UTBS1, UTBS2, UTBS3, UTBS4, UTBS5
, UTTEL, UTIDL, UTTPG, UTSOU, UTGEP
, UTGES, UTUSR, UTMJA, UTMJM, UTMJJ
, UTTIT, UTINI, UTOUQ, UTFUT, UTFDP
, UTUSC, UTDVV, UTDGE, UTFAX, UTAEM
, UTPFX, UTINS, UTRSV FROM YUTILIS
WHERE UTIUT = :codeUtilisateur
";
            const string update=@"UPDATE YUTILIS SET 
UTIUT = :UTIUT, UTNOM = :UTNOM, UTPNM = :UTPNM, UTGRP = :UTGRP, UTSGR = :UTSGR, UTHIE = :UTHIE, UTBUR = :UTBUR, UTBAU = :UTBAU, UTBRA = :UTBRA, UTIEX = :UTIEX
, UTBS1 = :UTBS1, UTBS2 = :UTBS2, UTBS3 = :UTBS3, UTBS4 = :UTBS4, UTBS5 = :UTBS5, UTTEL = :UTTEL, UTIDL = :UTIDL, UTTPG = :UTTPG, UTSOU = :UTSOU, UTGEP = :UTGEP
, UTGES = :UTGES, UTUSR = :UTUSR, UTMJA = :UTMJA, UTMJM = :UTMJM, UTMJJ = :UTMJJ, UTTIT = :UTTIT, UTINI = :UTINI, UTOUQ = :UTOUQ, UTFUT = :UTFUT, UTFDP = :UTFDP
, UTUSC = :UTUSC, UTDVV = :UTDVV, UTDGE = :UTDGE, UTFAX = :UTFAX, UTAEM = :UTAEM, UTPFX = :UTPFX, UTINS = :UTINS, UTRSV = :UTRSV
 WHERE 
UTIUT = :codeUtilisateur";
            const string delete=@"DELETE FROM YUTILIS WHERE UTIUT = :codeUtilisateur";
            const string insert=@"INSERT INTO  YUTILIS (
UTIUT, UTNOM, UTPNM, UTGRP, UTSGR
, UTHIE, UTBUR, UTBAU, UTBRA, UTIEX
, UTBS1, UTBS2, UTBS3, UTBS4, UTBS5
, UTTEL, UTIDL, UTTPG, UTSOU, UTGEP
, UTGES, UTUSR, UTMJA, UTMJM, UTMJJ
, UTTIT, UTINI, UTOUQ, UTFUT, UTFDP
, UTUSC, UTDVV, UTDGE, UTFAX, UTAEM
, UTPFX, UTINS, UTRSV
) VALUES (
:UTIUT, :UTNOM, :UTPNM, :UTGRP, :UTSGR
, :UTHIE, :UTBUR, :UTBAU, :UTBRA, :UTIEX
, :UTBS1, :UTBS2, :UTBS3, :UTBS4, :UTBS5
, :UTTEL, :UTIDL, :UTTPG, :UTSOU, :UTGEP
, :UTGES, :UTUSR, :UTMJA, :UTMJM, :UTMJJ
, :UTTIT, :UTINI, :UTOUQ, :UTFUT, :UTFDP
, :UTUSC, :UTDVV, :UTDGE, :UTFAX, :UTAEM
, :UTPFX, :UTINS, :UTRSV)";
            const string select_ListeNom=@"SELECT
UTNOM AS NOM, UTPNM AS PRENOM, B1.BUDBU AS B1NOMBUREAU, B1.BUAD1 AS B1AD1, UTIUT AS UTIUT
 FROM YUTILIS
left join Ybureau b1 on b1.BUIBU = UTBUR
where Lower(UtNom) like @ChaineRecherche or Lower(UtPnm) like @ChaineRecherche2
ORDER BY utnom, utpnm
FETCH FIRST 300 ROWS ONLY
";
            const string select_GetAll=@"SELECT
UTIUT, UTNOM, UTPNM, UTGRP, UTSGR
, UTHIE, UTBUR, UTBAU, UTBRA, UTIEX
, UTBS1, UTBS2, UTBS3, UTBS4, UTBS5
, UTTEL, UTIDL, UTTPG, UTSOU, UTGEP
, UTGES, UTUSR, UTMJA, UTMJM, UTMJJ
, UTTIT, UTINI, UTOUQ, UTFUT, UTFDP
, UTUSC, UTDVV, UTDGE, UTFAX, UTAEM
, UTPFX, UTINS, UTRSV FROM YUTILIS
";
            #endregion

            public UtilisateurRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public Utilisateur Get(string codeUtilisateur){
                return connection.Query<Utilisateur>(select, new {codeUtilisateur}).SingleOrDefault();
            }

            public string NewId () {
                return idGenerator.NewId("UTIUT").ToString() ;
            }

            public void Insert(Utilisateur value){
                    if(value.Utiut == default(string)) {
                        value.Utiut = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("UTIUT",value.Utiut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTNOM",value.Utnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("UTPNM",value.Utpnm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("UTGRP",value.Utgrp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTSGR",value.Utsgr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTHIE",value.Uthie, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTBUR",value.Utbur??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTBAU",value.Utbau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTBRA",value.Utbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTIEX",value.Utiex??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTBS1",value.Utbs1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS2",value.Utbs2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS3",value.Utbs3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS4",value.Utbs4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS5",value.Utbs5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTTEL",value.Uttel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("UTIDL",value.Utidl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTTPG",value.Uttpg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTSOU",value.Utsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTGEP",value.Utgep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTGES",value.Utges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTUSR",value.Utusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTMJA",value.Utmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("UTMJM",value.Utmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTMJJ",value.Utmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTTIT",value.Uttit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTINI",value.Utini??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("UTOUQ",value.Utouq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTFUT",value.Utfut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTFDP",value.Utfdp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTUSC",value.Utusc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTDVV",value.Utdvv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTDGE",value.Utdge??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTFAX",value.Utfax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("UTAEM",value.Utaem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:75, scale: 0);
                    parameters.Add("UTPFX",value.Utpfx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("UTINS",value.Utins??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTRSV",value.Utrsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(Utilisateur value){
                    var parameters = new DynamicParameters();
                    parameters.Add("codeUtilisateur",value.Utiut);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(Utilisateur value){
                    var parameters = new DynamicParameters();
                    parameters.Add("UTIUT",value.Utiut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTNOM",value.Utnom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("UTPNM",value.Utpnm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:20, scale: 0);
                    parameters.Add("UTGRP",value.Utgrp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTSGR",value.Utsgr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTHIE",value.Uthie, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTBUR",value.Utbur??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTBAU",value.Utbau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTBRA",value.Utbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTIEX",value.Utiex??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTBS1",value.Utbs1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS2",value.Utbs2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS3",value.Utbs3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS4",value.Utbs4??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTBS5",value.Utbs5??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("UTTEL",value.Uttel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("UTIDL",value.Utidl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTTPG",value.Uttpg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTSOU",value.Utsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTGEP",value.Utgep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTGES",value.Utges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTUSR",value.Utusr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTMJA",value.Utmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("UTMJM",value.Utmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTMJJ",value.Utmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTTIT",value.Uttit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTINI",value.Utini??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("UTOUQ",value.Utouq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTFUT",value.Utfut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTFDP",value.Utfdp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTUSC",value.Utusc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("UTDVV",value.Utdvv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTDGE",value.Utdge??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("UTFAX",value.Utfax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:13, scale: 0);
                    parameters.Add("UTAEM",value.Utaem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:75, scale: 0);
                    parameters.Add("UTPFX",value.Utpfx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("UTINS",value.Utins??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("UTRSV",value.Utrsv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("codeUtilisateur",value.Utiut);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<Utilisateur_ListeNom> ListeNom(string chaineRecherche1, string chaineRecherche2){
                    return connection.EnsureOpened().Query<Utilisateur_ListeNom>(select_ListeNom, new {chaineRecherche1, chaineRecherche2}).ToList();
            }
            public IEnumerable<Utilisateur> GetAll(){
                    return connection.EnsureOpened().Query<Utilisateur>(select_GetAll).ToList();
            }
    }
}
