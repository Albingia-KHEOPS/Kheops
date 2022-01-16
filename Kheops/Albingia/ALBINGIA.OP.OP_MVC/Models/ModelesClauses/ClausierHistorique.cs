using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Clausier;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ClausierHistorique
    {
        public int Code { get; set; }

        public int Version { get; set; }

        public string Libelle { get; set; }

        public string DateDeDebut { get; set; }

        public string DateDeFin { get; set; }

        public bool Valide { get; set; }

        public static explicit operator ClausierHistorique(ClausierDto dtoClauseVersions)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<ClausierDto, ClausierHistorique>().Map(dtoClauseVersions);
            var dateDeb = AlbConvert.ConvertIntToDate(int.Parse(dtoClauseVersions.DateDeb.ToString()));
            var dateFin = AlbConvert.ConvertIntToDate(int.Parse(dtoClauseVersions.DateFin.ToString()));
            if (dateDeb.HasValue)
                model.DateDeDebut = dateDeb.Value.ToShortDateString();
            if (dateFin.HasValue)
                model.DateDeFin = dateFin.Value.ToShortDateString();
            return model;

        }
        public static ClausierHistorique GetClausierHistorique(ClausierDto dtoClauseVersions, int date)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<ClausierDto, ClausierHistorique>().Map(dtoClauseVersions);
            var dateDeDebut = AlbConvert.ConvertIntToDate(int.Parse(dtoClauseVersions.DateDeb.ToString()));
            var dateDeFin = AlbConvert.ConvertIntToDate(int.Parse(dtoClauseVersions.DateFin.ToString()));
            if (dateDeDebut.HasValue)
                model.DateDeDebut = dateDeDebut.Value.ToShortDateString();
            if (dateDeFin.HasValue)
                model.DateDeFin = dateDeFin.Value.ToShortDateString();
            
            int dateDeb = int.Parse(dtoClauseVersions.DateDeb.ToString());
            int dateFin = int.Parse(dtoClauseVersions.DateFin.ToString());
            model.Valide = (dateDeb == 0 || (dateDeb <= date && (dateFin >= date || dateFin == 0))) ? true : false;
            return model;
        }
    }
}