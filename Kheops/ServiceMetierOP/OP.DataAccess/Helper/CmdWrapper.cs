using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Data;
using System.Linq;
using ALBINGIA.Framework.Common.Data;

namespace OP.DataAccess.Helpers
{

  public class AlbSqlCommand
  {
    public CommandType CommandType { get; set; }
    public string CommandText { get; set; }
    public DbParameterCollection Parameters { get; set; }
  }
  public class CmdWrapper
  {
    public AlbSqlCommand Cmd { get; set; }
    public bool HasWhere = false;

    public CmdWrapper()
    {
      Cmd = new AlbSqlCommand();
    }
    public void Where(string command)
    {
      CommandText += AddWhereOrAnd();
      CommandText += command + " ";
    }

    public void Where(string champ, object value, DbType dbType)
    {
      if (value == null || value is string && String.IsNullOrEmpty(value.ToString())) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper() + " = :" + champ.ToLower() + " ";
      AppendParameter(dbType, champ.ToLower(), value);
    }

    public void WhereNotEqual(string champ, string value, DbType dbType)
    {
      if (value != null && !(value is string && String.IsNullOrEmpty(value)))
      {
        CommandText += AddWhereOrAnd();
        CommandText += champ.ToUpper() + " <> :" + champ.ToLower() + " ";
        AppendParameter(dbType, champ.ToLower(), value);
      }
    }

    public void WhereLessOrEqual(string champ, string value, DbType dbType)
    {
      if (string.IsNullOrEmpty(value)) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper() + " <= :" + champ.ToLower() + " ";
      AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), value);
    }

    public void WhereGreaterOrEqual(string champ, string value, DbType dbType)
    {
      if (string.IsNullOrEmpty(value)) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper() + " >= :" + champ.ToLower() + " ";
      AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), value);
    }

    public void WhereIn(string champ, List<string> values, bool whereIn = true)
    {
      if (values == null) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper();
      if (!whereIn)
      {
        CommandText += " NOT";
      }
      CommandText += " IN (";

      for (var i = 0; i < values.Count; i++)
      {
        if (String.IsNullOrEmpty(values[i])) continue;
        CommandText += ":" + champ.ToLower() + i + ", ";
        AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower() + i, values[i]);
      }

      CommandText = CommandText.Substring(0, CommandText.Length - 2);
      CommandText += ") ";
    }

    public void WhereNotIn(string champ, string values)
    {
      if (String.IsNullOrEmpty(values)) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper() + " NOT IN (:" + champ.ToLower() + ") ";
      AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), values);
    }

    public void WhereLike(string champ, string value)
    {
      if (String.IsNullOrEmpty(value)) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper() + " LIKE :" + champ.ToLower() + " ";
      AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), "%" + value + "%");
    }

    public void WhereStartWith(string champ, string value)
    {
      if (String.IsNullOrEmpty(value)) return;
      CommandText += AddWhereOrAnd();
      CommandText += champ.ToUpper() + " LIKE :" + champ.ToLower() + " ";
      AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), value + "%");
    }
    public void WhereLikeOr(string champs, string value)
    {
      if (String.IsNullOrEmpty(value)) return;
      CommandText += AddWhereOrAnd();
      CommandText += "(";
      var champsArray = champs.Split(';');
      foreach (var champ in champsArray)
      {
        CommandText += champ.ToUpper() + " LIKE :" + champ.ToLower() + " ";
        AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), "%" + value + "%");
        CommandText += " OR ";
      }
      CommandText = CommandText.Substring(0, CommandText.Length - 4);
      CommandText += ")";
    }

    private string AddWhereOrAnd()
    {
      string result;
      if (HasWhere)
      {
        result = "AND ";
      }
      else
      {
        HasWhere = true;
        result = " WHERE ";
      }
      return result;
    }

    public void InsertInto(string tableName, IEnumerable<Param> entrees)
    {
      var commandText = "INSERT INTO " + tableName;
      var nomColonne = "(";
      var value = "VALUES (";

      foreach (var param in entrees.Where(param => param.ValueParam != null))
      {
        nomColonne += param.NomColonne + ", ";
        value += param.NameQuery + ", ";
        AppendParameter(param.DbType, param.NomColonne, param.ValueParam);
      }

      nomColonne = nomColonne.Substring(0, nomColonne.Length - 2) + ")";
      value = value.Substring(0, value.Length - 2) + ")";

      commandText += " " + nomColonne + " " + value;
      CommandText = commandText;
    }

    public void Update(string tableName, IEnumerable<Param> entrees)
    {
      var commandText = "UPDATE " + tableName + " SET ";
      foreach (var param in entrees.Where(param => param.ValueParam != null))
      {
        commandText += param.NomColonne + " = " + param.NameQuery + ", ";
        AppendParameter(param.DbType, param.NomColonne, param.ValueParam);
      }

      commandText = commandText.Substring(0, commandText.Length - 2);
      CommandText = commandText;
    }

    public string CommandText
    {
      get { return Cmd.CommandText; }
      set { Cmd.CommandText = value; }
    }

    public CommandType CommandType
    {
      get { return Cmd.CommandType; }
      set { Cmd.CommandType = value; }
    }

    public IDataParameterCollection Parameters
    {
      get { return Cmd.Parameters; }
    }

    public virtual void AppendParameter(DbType type, string name, object value, int size = 0)
    {
      var dbParameter = DbBase.Settings.CreateParameter(name, type, size);
      dbParameter.Value = value;
      if (Cmd.Parameters == null)
      {
        Cmd.Parameters = new EacParameterCollection { dbParameter };
      }
      else
      {
        Cmd.Parameters.Add(dbParameter);
      }
    }
  }

  public class Param
  {
    public DbType DbType;
    public string NomColonne { get; set; }
    public string NameQuery { get { return ":" + NomColonne; } }
    public Object ValueParam { get; set; }

    public Param(DbType dbType, string nomColonne, Object value)
    {
      DbType = dbType;
      NomColonne = nomColonne;
      ValueParam = value;
    }
  }

  public class Outils
  {
    public static Object SiNull(Object argument)
    {
      Object result = String.Empty;
      if (argument != DBNull.Value && argument != null)
      {
        result = argument;
      }
      return result;
    }

    //public static void AddMaxDate(CmdWrapper cmdWrapper, DateTime? maxDate, string champDeb)
    //{
    //  if (maxDate.HasValue)
    //  {
    //    cmdWrapper.Where("(" + champDeb + "A * 10000 + " + champDeb + "M * 100 + " + champDeb + "J) <= " +
    //                     DateHelper.AS400GetDate(maxDate.Value));
    //  }
    //}

    //public static void AddMinDate(CmdWrapper cmdWrapper, DateTime? minDate, string champDeb)
    //{
    //  if (minDate.HasValue)
    //  {
    //    cmdWrapper.Where("(" + champDeb + "A * 10000 + " + champDeb + "M * 100 + " + champDeb + "J) >= " +
    //                     DateHelper.AS400GetDate(minDate.Value));
    //  }
    //}

    //public static string AddMaxDate(DateTime? maxDate, string champDeb)
    //{
    //  string result = string.Empty;

    //  if (maxDate.HasValue)
    //    result = string.Format("({0}A * 10000 + {0}M * 100 + {0}J) <= {1}", champDeb, DateHelper.AS400GetDate(maxDate.Value));

    //  return result;
    //}

    //public static string AddMinDate(DateTime? minDate, string champDeb)
    //{
    //  return string.Format("({0}A * 10000 + {0}M * 100 + {0}J) >= {1}", champDeb, DateHelper.AS400GetDate(minDate.Value));
    //}
  }
}
