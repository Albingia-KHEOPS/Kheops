using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public partial class KpEntMapper : EntityMap<KpEnt>   {
        public KpEntMapper () {
            Map(p => p.Kaatyp).ToColumn("KAATYP");
            Map(p => p.Kaaipb).ToColumn("KAAIPB");
            Map(p => p.Kaaalx).ToColumn("KAAALX");
            Map(p => p.Kaaavn).ToColumn("KAAAVN");
            Map(p => p.Kaahin).ToColumn("KAAHIN");
            Map(p => p.Kaaboni).ToColumn("KAABONI");
            Map(p => p.Kaabont).ToColumn("KAABONT");
            Map(p => p.Kaaanti).ToColumn("KAAANTI");
            Map(p => p.Kaadesi).ToColumn("KAADESI");
            Map(p => p.Kaaobsv).ToColumn("KAAOBSV");
            Map(p => p.Kaalcivalo).ToColumn("KAALCIVALO");
            Map(p => p.Kaalcivala).ToColumn("KAALCIVALA");
            Map(p => p.Kaalcivalw).ToColumn("KAALCIVALW");
            Map(p => p.Kaalciunit).ToColumn("KAALCIUNIT");
            Map(p => p.Kaalcibase).ToColumn("KAALCIBASE");
            Map(p => p.Kaakdiid).ToColumn("KAAKDIID");
            Map(p => p.Kaafrhvalo).ToColumn("KAAFRHVALO");
            Map(p => p.Kaafrhvala).ToColumn("KAAFRHVALA");
            Map(p => p.Kaafrhvalw).ToColumn("KAAFRHVALW");
            Map(p => p.Kaafrhunit).ToColumn("KAAFRHUNIT");
            Map(p => p.Kaafrhbase).ToColumn("KAAFRHBASE");
            Map(p => p.Kaakdkid).ToColumn("KAAKDKID");
            Map(p => p.Kaaatglci).ToColumn("KAAATGLCI");
            Map(p => p.Kaaatgklc).ToColumn("KAAATGKLC");
            Map(p => p.Kaaatgcap).ToColumn("KAAATGCAP");
            Map(p => p.Kaaatgkca).ToColumn("KAAATGKCA");
            Map(p => p.Kaaatgsur).ToColumn("KAAATGSUR");
            Map(p => p.Kaaatgbcn).ToColumn("KAAATGBCN");
            Map(p => p.Kaaatgkbc).ToColumn("KAAATGKBC");
            Map(p => p.Kaaatgcri).ToColumn("KAAATGCRI");
            Map(p => p.Kaaatgapt).ToColumn("KAAATGAPT");
            Map(p => p.Kaaatgf).ToColumn("KAAATGF");
            Map(p => p.Kaaatgapr).ToColumn("KAAATGAPR");
            Map(p => p.Kaaatgtrt).ToColumn("KAAATGTRT");
            Map(p => p.Kaaatgtrr).ToColumn("KAAATGTRR");
            Map(p => p.Kaaatgtct).ToColumn("KAAATGTCT");
            Map(p => p.Kaaatgtcr).ToColumn("KAAATGTCR");
            Map(p => p.Kaaatgtft).ToColumn("KAAATGTFT");
            Map(p => p.Kaaatgtcm).ToColumn("KAAATGTCM");
            Map(p => p.Kaaatgtfa).ToColumn("KAAATGTFA");
            Map(p => p.Kaaatgctx).ToColumn("KAAATGCTX");
            Map(p => p.Kaaatglcf).ToColumn("KAAATGLCF");
            Map(p => p.Kaaatgcaf).ToColumn("KAAATGCAF");
            Map(p => p.Kaaatgsuf).ToColumn("KAAATGSUF");
            Map(p => p.Kaaatgbcf).ToColumn("KAAATGBCF");
            Map(p => p.Kaalciina).ToColumn("KAALCIINA");
            Map(p => p.Kaaatgfrr).ToColumn("KAAATGFRR");
            Map(p => p.Kaaatgcmt).ToColumn("KAAATGCMT");
            Map(p => p.Kaaatgfat).ToColumn("KAAATGFAT");
            Map(p => p.Kaaatgbas).ToColumn("KAAATGBAS");
            Map(p => p.Kaaatgkba).ToColumn("KAAATGKBA");
            Map(p => p.Kaafrhina).ToColumn("KAAFRHINA");
            Map(p => p.Kaaand).ToColumn("KAAAND");
            Map(p => p.Kaadprj).ToColumn("KAADPRJ");
            Map(p => p.Kaadsta).ToColumn("KAADSTA");
            Map(p => p.Kaaobsf).ToColumn("KAAOBSF");
            Map(p => p.Kaaobsa).ToColumn("KAAOBSA");
            Map(p => p.Kaaobsc).ToColumn("KAAOBSC");
            Map(p => p.Kaaass).ToColumn("KAAASS");
            Map(p => p.Kaaafs).ToColumn("KAAAFS");
            Map(p => p.Kaaxcms).ToColumn("KAAXCMS");
            Map(p => p.Kaacncs).ToColumn("KAACNCS");
            Map(p => p.Kaacible).ToColumn("KAACIBLE");
            Map(p => p.Kaamaxa).ToColumn("KAAMAXA");
            Map(p => p.Kaamaxe).ToColumn("KAAMAXE");
            Map(p => p.Kaaide).ToColumn("KAAIDE");
            Map(p => p.Kaaimed).ToColumn("KAAIMED");
            Map(p => p.Kaaimda).ToColumn("KAAIMDA");
            Map(p => p.Kaaisin).ToColumn("KAAISIN");
            Map(p => p.Kaaavh).ToColumn("KAAAVH");
            Map(p => p.Kaaavds).ToColumn("KAAAVDS");
            Map(p => p.Kaarcp).ToColumn("KAARCP");
            Map(p => p.Kaapaq).ToColumn("KAAPAQ");
            Map(p => p.Kaascat).ToColumn("KAASCAT");
            Map(p => p.Kaaltasp).ToColumn("KAALTASP");
            Map(p => p.Kaareldt).ToColumn("KAARELDT");
            Map(p => p.Kaaquach).ToColumn("KAAQUACH");
            Map(p => p.Kaaquven).ToColumn("KAAQUVEN");
            Map(p => p.Kaaavao).ToColumn("KAAAVAO");
            Map(p => p.Kaaaripk).ToColumn("KAAARIPK");
            Map(p => p.Kaaartyg).ToColumn("KAAARTYG");
            Map(p => p.Kaapkrd).ToColumn("KAAPKRD");
            Map(p => p.Kaasudd).ToColumn("KAASUDD");
            Map(p => p.Kaasudh).ToColumn("KAASUDH");
            Map(p => p.Kaasufd).ToColumn("KAASUFD");
            Map(p => p.Kaasufh).ToColumn("KAASUFH");
            Map(p => p.Kaarsdd).ToColumn("KAARSDD");
            Map(p => p.Kaarsdh).ToColumn("KAARSDH");
            Map(p => p.Kaaattdoc).ToColumn("KAAATTDOC");
        }
    }
  

}