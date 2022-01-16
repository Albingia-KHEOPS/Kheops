using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class KcatVolet_BrancheCible {
        ///<summary>Public empty contructor</summary>
        public KcatVolet_BrancheCible() {}
        ///<summary>Public empty contructor</summary>
        public KcatVolet_BrancheCible(KcatVolet_BrancheCible copyFrom) 
        {
 
            this.Kapid= copyFrom.Kapid;
 
            this.Kapbra= copyFrom.Kapbra;
 
            this.Kapcible= copyFrom.Kapcible;
 
            this.Kapkaiid= copyFrom.Kapkaiid;
 
            this.Kapvolet= copyFrom.Kapvolet;
 
            this.Kapkakid= copyFrom.Kapkakid;
 
            this.Kapcar= copyFrom.Kapcar;
 
            this.Kapordre= copyFrom.Kapordre;
 
            this.Kakdesc= copyFrom.Kakdesc;
        
        }        



        ///<summary> ID unique </summary>
        public Int64 Kapid { get; set; } 
        ///<summary> Branche </summary>
        public string Kapbra { get; set; } 
        ///<summary> Cible </summary>
        public string Kapcible { get; set; } 
        ///<summary> ID KCIBLEF </summary>
        public Int64 Kapkaiid { get; set; } 
        ///<summary> Volet </summary>
        public string Kapvolet { get; set; } 
        ///<summary> ID KVOLET </summary>
        public Int64 Kapkakid { get; set; } 
        ///<summary> Caractère </summary>
        public string Kapcar { get; set; } 
        ///<summary> N° Ordre </summary>
        public int Kapordre { get; set; } 
        ///<summary> Description </summary>
        public string Kakdesc { get; set; } 
        }
}
