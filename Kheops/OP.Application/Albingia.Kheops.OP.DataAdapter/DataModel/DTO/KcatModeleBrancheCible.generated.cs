using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class KcatModeleBrancheCible {
        ///<summary>Public empty contructor</summary>
        public KcatModeleBrancheCible() {}
        ///<summary>Public empty contructor</summary>
        public KcatModeleBrancheCible(KcatModeleBrancheCible copyFrom) 
        {
 
            this.Karid= copyFrom.Karid;
 
            this.Karkaqid= copyFrom.Karkaqid;
 
            this.Kardateapp= copyFrom.Kardateapp;
 
            this.Kartypo= copyFrom.Kartypo;
 
            this.Karmodele= copyFrom.Karmodele;
        
        }        



        ///<summary>  </summary>
        public Int64 Karid { get; set; } 
        ///<summary>  </summary>
        public Int64 Karkaqid { get; set; } 
        ///<summary>  </summary>
        public int Kardateapp { get; set; } 
        ///<summary>  </summary>
        public string Kartypo { get; set; } 
        ///<summary>  </summary>
        public string Karmodele { get; set; } 
        }
}
