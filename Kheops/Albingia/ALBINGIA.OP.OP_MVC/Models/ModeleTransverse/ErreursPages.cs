using ALBINGIA.OP.OP_MVC.Models.MetaModels;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{
    public class ErreursPages : MetaModelsBase
  {
    /// <summary>
    /// nature de l'erreur(Bwoser, 404,500....)
    /// </summary>
    
    public string ErrorType { get; set; }
  }
}