using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    [Serializable]
    public class ModeleEngagementPeriode
    {
        public Int64 Code { get; set; }
        public string Actif { get; set; }
        public int DateDebut { get; set; }
        public int DateFin { get; set; }
        public float Part { get; set; }
        public Int64 EngagementTotal { get; set; }
        public Int64 EngagementAlbingia { get; set; }
        public string Utilise { get; set; }
        public bool IsReadOnly { get; set; }

        public string INHPENG { get; set; }
        public static explicit operator ModeleEngagementPeriode(EngagementPeriodeDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EngagementPeriodeDto, ModeleEngagementPeriode>().Map(data);
        }
    }
}