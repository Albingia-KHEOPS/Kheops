using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.RefExprComplexe
{
    public class ExprComplexeDetailDto
    {
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }
        [Column(Name = "CODEEXPR")]
        public string CodeExpr { get; set; }
        [Column(Name = "LIBEXPR")]
        public string LibExpr { get; set; }
        [Column(Name = "MODIFEXPR")]
        public string ModifExpr { get; set; }
        [Column(Name = "DESCREXPR")]
        public string DescrExpr { get; set; }
        [Column(Name = "GUIDDETID")]
        public Int64 GuidDetId { get; set; }
        [Column(Name = "VALDET")]
        public Double ValeurDet { get; set; }
        [Column(Name = "UNITDET")]
        public string UniteDet { get; set; }
        [Column(Name = "TYPEDET")]
        public string TypeDet { get; set; }
        [Column(Name = "CONVALDET")]
        public Double ConValeurDet { get; set; }
        [Column(Name = "CONUNITDET")]
        public string ConUniteDet { get; set; }
        [Column(Name = "CONTYPEDET")]
        public string ConTypeDet { get; set; }
        [Column(Name = "FRANCHISEMINI")]
        public Double FranchiseMini { get; set; }
        [Column(Name = "FRANCHISEMAXI")]
        public Double FranchiseMaxi { get; set; }
        [Column(Name = "LIMITEDEB")]
        public Int32 LimiteDeb { get; set; }
        [Column(Name = "LIMITEFIN")]
        public Int32 LimiteFin { get; set; }
        [Column(Name = "ORDRE")]
        public Int32 Ordre { get; set; }
        [Column(Name = "IND")]
        public Int32 CodeIndice { get; set; }
        [Column(Name = "IVO")]
        public Int32 ValeurIndice { get; set; }
        [Column(Name = "ORIGINE")]
        public string Origine { get; set; }
    }
}
