using System.Linq;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.AlbingiaAttributes;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.OP.OP_MVC.Models.WordViewer;
using ALBINGIA.OP.OP_MVC.WSCommonOffre;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
  public class WordViwerController : ControllersBase<WordViewer>
  {
    #region Méthodes publiques
    public ActionResult Index(string id)
    {
      if (string.IsNullOrEmpty(id))
        return null;
      GetWordDocument(id.Split('_')[0], id.Split('_')[1],"true");

      return View(ContentData);

    }
    //#endregion

    //#region Méthodes privées
    [AjaxException]
    public string GetWordDocument(string typeDoc, string wOpenParam, string resolu)
    {
      bool clauseResolu;
      bool.TryParse(resolu,out clauseResolu);
      using (var commonContext = new CommonOffreClient())
      {
        var resFile = commonContext.OpenWordDocument(typeDoc, wOpenParam + MvcApplication.SPLIT_CONST_HTML + MvcApplication.CP_TARGETGENERATEFOLDER, MvcApplication.SPLIT_CONST_HTML, clauseResolu);
        if(string.IsNullOrEmpty(resFile))
          throw new AlbFoncException("Impossible d'afficher le document",true,true);
        if(resFile.ToLower().Contains("erreur"))
          throw new AlbFoncException(resFile,  true, true);
        var elmFilePath = resFile.Split('\\');
        //ContentData.Parameters = elmFilePath[elmFilePath.Count()-1];
        return elmFilePath[elmFilePath.Count() - 1];
      }
    #endregion
    }
  }
}
