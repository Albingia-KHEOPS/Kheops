using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class UtilisateurMapper : EntityMap<Utilisateur>   {
        public UtilisateurMapper () {
            Map(p => p.Utiut).ToColumn("UTIUT");
            Map(p => p.Utnom).ToColumn("UTNOM");
            Map(p => p.Utpnm).ToColumn("UTPNM");
            Map(p => p.Utgrp).ToColumn("UTGRP");
            Map(p => p.Utsgr).ToColumn("UTSGR");
            Map(p => p.Uthie).ToColumn("UTHIE");
            Map(p => p.Utbur).ToColumn("UTBUR");
            Map(p => p.Utbau).ToColumn("UTBAU");
            Map(p => p.Utbra).ToColumn("UTBRA");
            Map(p => p.Utiex).ToColumn("UTIEX");
            Map(p => p.Utbs1).ToColumn("UTBS1");
            Map(p => p.Utbs2).ToColumn("UTBS2");
            Map(p => p.Utbs3).ToColumn("UTBS3");
            Map(p => p.Utbs4).ToColumn("UTBS4");
            Map(p => p.Utbs5).ToColumn("UTBS5");
            Map(p => p.Uttel).ToColumn("UTTEL");
            Map(p => p.Utidl).ToColumn("UTIDL");
            Map(p => p.Uttpg).ToColumn("UTTPG");
            Map(p => p.Utsou).ToColumn("UTSOU");
            Map(p => p.Utgep).ToColumn("UTGEP");
            Map(p => p.Utges).ToColumn("UTGES");
            Map(p => p.Utusr).ToColumn("UTUSR");
            Map(p => p.Utmja).ToColumn("UTMJA");
            Map(p => p.Utmjm).ToColumn("UTMJM");
            Map(p => p.Utmjj).ToColumn("UTMJJ");
            Map(p => p.Uttit).ToColumn("UTTIT");
            Map(p => p.Utini).ToColumn("UTINI");
            Map(p => p.Utouq).ToColumn("UTOUQ");
            Map(p => p.Utfut).ToColumn("UTFUT");
            Map(p => p.Utfdp).ToColumn("UTFDP");
            Map(p => p.Utusc).ToColumn("UTUSC");
            Map(p => p.Utdvv).ToColumn("UTDVV");
            Map(p => p.Utdge).ToColumn("UTDGE");
            Map(p => p.Utfax).ToColumn("UTFAX");
            Map(p => p.Utaem).ToColumn("UTAEM");
            Map(p => p.Utpfx).ToColumn("UTPFX");
            Map(p => p.Utins).ToColumn("UTINS");
            Map(p => p.Utrsv).ToColumn("UTRSV");
        }
    }
  

}
