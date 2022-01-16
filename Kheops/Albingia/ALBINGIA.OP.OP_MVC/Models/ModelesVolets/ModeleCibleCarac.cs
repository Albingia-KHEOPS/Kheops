using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesVolets
{
    [Serializable]
    public class ModeleCibleCarac
    {
      public List<AlbSelectListItem> Cibles { get; set; }
      public List<AlbSelectListItem> Caracteres { get; set; }
    }
}