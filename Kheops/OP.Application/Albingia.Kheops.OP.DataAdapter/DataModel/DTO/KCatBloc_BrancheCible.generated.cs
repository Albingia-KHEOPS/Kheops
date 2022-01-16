using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class KCatBloc_BrancheCible {
        ///<summary>Public empty contructor</summary>
        public KCatBloc_BrancheCible() {}
        ///<summary>Public empty contructor</summary>
        public KCatBloc_BrancheCible(KCatBloc_BrancheCible copyFrom) 
        {
 
            this.Kaqid= copyFrom.Kaqid;
 
            this.Kaqbra= copyFrom.Kaqbra;
 
            this.Kaqcible= copyFrom.Kaqcible;
 
            this.Kaqbloc= copyFrom.Kaqbloc;
 
            this.Kaqkaeid= copyFrom.Kaqkaeid;
 
            this.Kaqcar= copyFrom.Kaqcar;
 
            this.Kaqordre= copyFrom.Kaqordre;
 
            this.Kaedesc= copyFrom.Kaedesc;
 
            this.Kaqkapid= copyFrom.Kaqkapid;
        
        }        



        ///<summary> ID unique </summary>
        public Int64 Kaqid { get; set; } 
        ///<summary> Branche </summary>
        public string Kaqbra { get; set; } 
        ///<summary> Cible </summary>
        public string Kaqcible { get; set; } 
        ///<summary> Bloc </summary>
        public string Kaqbloc { get; set; } 
        ///<summary> ID KBLOC </summary>
        public Int64 Kaqkaeid { get; set; } 
        ///<summary> Caractère </summary>
        public string Kaqcar { get; set; } 
        ///<summary> N° Ordre </summary>
        public int Kaqordre { get; set; } 
        ///<summary> Description </summary>
        public string Kaedesc { get; set; } 
        ///<summary> ID KCATVOLET </summary>
        public Int64 Kaqkapid { get; set; } 
        }
}
