using System;

namespace ALBINGIA.Framework.Common.Models.GridColumnModel
{
    /// <summary>
    /// Representation de la class qui personnifira l'instance finale
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelColumnBase<T> 
    {
        /// <summary>
        /// Conteneur de la class à personnaliser
        /// </summary>
        public T MetaColumnData { get; set; }

        /// <summary>
        /// 
        /// </summary>
         public ModelColumnBase()
        {
            MetaColumnData = Activator.CreateInstance<T>();
        }

    }
}