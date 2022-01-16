using EmitMapper;
using OP.WSAS400.DTO.Clausier;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleClausier
    {
        /// </value>
        public string Id { get; set; }
        public string Code { get; set; }
        public string Rubrique { get; set; }
        public string SousRubrique { get; set; }
        public string Sequence { get; set; }
        public int Version { get; set; }
        public string Libelle { get; set; }
        public long DateDeb { get; set; }
        public long DateFin { get; set; }
        public int Date { get; set; }
        
        //OrigineAffichage="ClauseInvalide" suite à la selection sinon vide suite au click sur le buton historique
        public string OrigineAffichage { get; set; }
        public bool ClauseValideExist { get; set; }
        public int CodeClauseValide { get; set; }
        public int VersionClauseValide { get; set; }
        public ModeleContexte ModeleContexte { get; set; }  
        public List<ClausierHistorique> Historique { get; set; }
        public string SelectionPossible { get; set; }

        public static explicit operator ModeleClausier(ClausierDto dtoClausier)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ClausierDto, ModeleClausier>().Map(dtoClausier);
        }
    }
}