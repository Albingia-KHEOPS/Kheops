using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.IOFile;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Configuration;
using System.Web.Profile;
using System.Web.Security;
namespace ALBINGIA.OP.OP_MVC.Common
{

    public class ProfileBase : System.Web.Profile.ProfileBase
  {
   
    public AlbProjectFolderInfos AlbUserInfos { get; set; }
  
  }
  public class UserProfile : ProfileBase
  {
    public static UserProfile GetUserProfile(string username)
    {

      return Create(username) as UserProfile;
    }
    public static UserProfile GetUserProfile()
    {
      return Create(Membership.GetUser().UserName) as UserProfile;
    }

  }
  public class AlbProfileProvider : ProfileProvider
  {

    
    //#region Méthodes publiques

    ///// <summary>
    ///// Met à jour le profil utilisateur
    ///// </summary>
    ///// <param name="tabGuid">Guid session</param>
    ///// <param name="userName">Nom Utilisateur connecté</param>
    ///// <param name="offre">numéro offre/contrat</param>
    ///// <param name="version">version</param>
    ///// <param name="type">type O/P</param>
    //public static void SetUserProfil(string tabGuid,string userName, string offre, string version,string type)
    //{
    //  var key = tabGuid+"_"+userName+"_"+offre + "_" + version + "_" + type;
    //  //dynamic profile = ProfileBase.Create(userName);
    //  dynamic profile = HttpContext.Current.Profile;

    //  if(profile.AlbUserInfos==null)
    //  {
    //    profile.AlbUserInfos = new AlbProjectFolderInfos();
    //    profile.AlbUserInfos.CurrentInfoUser.Add(key, AddNewUserInfo(tabGuid,userName, offre, version, type));
    //  }
    //  else
    //  {
    //    var lst=profile.AlbUserInfos as AlbProjectFolderInfos;
    //    if (lst != null)
    //    {
    //      var elem=lst.CurrentInfoUser.Select(s => s.Value.Id==key).ToList();
    //      if (elem.Count==0)
    //      {
    //        profile.AlbUserInfos.CurrentInfoUser.Add(key, AddNewUserInfo(tabGuid, userName, offre, version, type));
    //      }
    //    }
    //  }
    //  profile.Save();
     
    //}

    //private static AlbProjectInfo AddNewUserInfo(string guidTab,string userName, string offre, string version, string type)
    //{
    //  return new AlbProjectInfo
    //    {

    //      Id = guidTab + "_" + userName + "_" + offre + "_" + version + "_" + type,
    //      Folder = offre,
    //      UserName = userName,
    //      ReadOnlyFolder =
    //        !string.IsNullOrEmpty(offre) && !string.IsNullOrEmpty(version) && GetFolderRole(offre, version, type),
    //      ReadOnlyUser = GetUserRole(userName)
    //    };
    //}

    //#endregion
    #region Méthode privées
    private static bool GetUserRole(string userName)
    {
      return false;
    }
    private static bool GetFolderRole(string offre, string version, string type, string numAvn)
    {
      using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
      {
          var servicesClient=client.Channel;
        return servicesClient.OffreEstValide(offre, version,type, numAvn);
      }
    }
    #endregion
    #region Méthodes surchargées
    private string _applicationName;

  
    public override string ApplicationName
    {
      get
      {
        _applicationName = FileContentManager.GetConfigValue("InternalApplicationId");
        return _applicationName;
      }

      set { _applicationName = value; }
    }

    public override SettingsPropertyValueCollection GetPropertyValues(
      SettingsContext context, SettingsPropertyCollection collection)
    {

      var settings = new SettingsPropertyValueCollection();
      foreach (SettingsProperty property in collection)
      {
        var value = new SettingsPropertyValue(collection[property.Name]);
        switch (property.Name)
        {
          case "ReadOnlyFolder":
            value.PropertyValue = false;

            break;
          case "ReadOnlyUser":
            value.PropertyValue = false;
            break;
          case "AlbUserInfos":
            value.PropertyValue = null;
            break;

        }
      }
      return settings;
      
    }
  

    public override
      void SetPropertyValues(System.Configuration.SettingsContext context,
                                           System.Configuration.SettingsPropertyValueCollection collection)
    {
      var settings = new SettingsPropertyValueCollection();
      foreach (SettingsProperty property in collection)
      {

        switch (property.Name)
        {
          case "ReadOnlyFolder":
            //value.PropertyValue = string.Empty;

            break;
          case "ReadOnlyUser":
            //value.PropertyValue = string.Empty;
            break;
          case "AlbUserInfos":
           // property.PropertyValue = null;
            break;
            
        }
      }

    }
    #endregion
    #region Méthodes non implémentées

    public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                               DateTime userInactiveSinceDate)
    {
      throw new NotImplementedException();
    }

    public override int DeleteProfiles(string[] usernames)
    {
      throw new NotImplementedException();
    }

    public override int DeleteProfiles(ProfileInfoCollection profiles)
    {
      throw new NotImplementedException();
    }

    public override ProfileInfoCollection FindInactiveProfilesByUserName(
      ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate,
      int pageIndex, int pageSize, out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption,
                                                                 string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                                 DateTime userInactiveSinceDate, int pageIndex,
                                                                 int pageSize, out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex,
                                                         int pageSize, out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                    DateTime userInactiveSinceDate)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}


