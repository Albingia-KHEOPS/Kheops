using System.Web.Mvc;

namespace ALBINGIA.Framework.Common.Extensions
{
    /// <summary>
    /// Classe d'extension de SelectlistItem : prise en compte du Title des éléments de la list
    /// </summary>
    public class AlbSelectListItem : SelectListItem
    {
        public string Title { get; set; }
        public string Descriptif { get; set; }
         
        public static AlbSelectListItem ConvertToAlbSelect(string value, string text, string title = "")
        {
            if (!string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(text))
            {
                title = string.Format("{0} - {1}", value, text);
                text = string.Format("{0} - {1}", value, text);
            }
            else
            {
                title = "";
                text = "";
            }
            return new AlbSelectListItem { Value = value, Text = text, Title = title };
        }


    }

}
