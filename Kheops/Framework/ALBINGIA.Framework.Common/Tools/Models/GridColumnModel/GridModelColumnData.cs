using System.Collections.Generic;

namespace ALBINGIA.Framework.Common.Models.GridColumnModel
{
    /// <summary>
    /// Class de Conteneur générique selon un type de class ayant pour but la grille finale
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridModelColumnData<T> : List<ModelColumnBase<T>>
    {
        //récupération des paramètre d'identifiant d'une colonne les plus utilisés
        public string CodeOffre;
        public int? Version;
        /// <summary>
        /// Entete pour les tableau inversé mis sous forme de colonne
        /// </summary>
        public List<string> Entetes { get; set; }
        public GridModelColumnData()
            : base()
        {
        }
        /// <summary>
        /// Instance avec la liste des données
        /// </summary>
        /// <param name="risques"></param>
        public GridModelColumnData(List<ModelColumnBase<T>> risques) : base() 
        {
            this.AddRange(risques);
        }

        /// <summary>
        /// Instance identifiant + liste des données
        /// </summary>
        /// <param name="risques"></param>
        public GridModelColumnData(string codeOffre, int? version, List<ModelColumnBase<T>> risques)
            : base()
        {
            this.CodeOffre = codeOffre;
            this.Version = version;
            this.AddRange(risques);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="entetes"></param>
        /// <param name="risques"></param>
        public GridModelColumnData(int capacity, List<string> entetes, List<ModelColumnBase<T>> risques)
            : base(capacity) 
        {
            this.Entetes = entetes;
            this.AddRange(risques);
        }
    }
}