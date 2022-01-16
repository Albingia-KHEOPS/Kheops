using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.GestionErreur_MetaData
{
    public abstract class _GestionErreur_MetaData_Base
    {
        #region Message pour le test gestion d'erreur

        [
            Display(Name = " = "),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Resultat { get; set; }

        #endregion

        public _GestionErreur_MetaData_Base()
        {
            this.Resultat = string.Empty;
        }
    }
}