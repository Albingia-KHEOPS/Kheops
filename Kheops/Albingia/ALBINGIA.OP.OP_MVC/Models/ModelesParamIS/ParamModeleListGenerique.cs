using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamIS
{
    public class ParamModeleListGenerique
    { 
        public List<AlbSelectListItem> ListLink { get; set; }
        public List<AlbSelectListItem> ListType { get; set; }
        public List<AlbSelectListItem> ListConvertTo { get; set; }
        public List<AlbSelectListItem> ListHierarchyOrder { get; set; }
        public List<AlbSelectListItem> ListLineBreak { get; set; }
        public List<AlbSelectListItem> ListRequired { get; set; }
        public List<AlbSelectListItem> ListDisabled { get; set; }
        public List<AlbSelectListItem> ListUIControl { get; set; }
    }
}