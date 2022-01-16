using System.Configuration;

namespace OP.DataAccess
{
    public class DbBase
    {

     

      public static ConnectionStringSettings Settings
        {
            get
            {
              return ConfigurationManager.ConnectionStrings["EasyCom"];
            }
        }
      public static ConnectionStringSettings SettingsClauseVsto
      {
        get
        {

          return ConfigurationManager.ConnectionStrings["VstoClause"];
        }
      }

      public static ConnectionStringSettings SettingsLAB
      {
          get
          {

              return ConfigurationManager.ConnectionStrings["LAB"];
          }
      }

    }
   
}
