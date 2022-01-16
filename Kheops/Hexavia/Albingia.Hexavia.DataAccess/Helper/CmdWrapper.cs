using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using NLog;

namespace Albingia.Hexavia.DataAccess.Helper
{
    public class CmdWrapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public IDbCommand Cmd { get; set; }
        public bool HasWhere = false;

        public void Where(string command)
        {
            CommandText += AddWhereOrAnd();
            CommandText += command + " ";
        }

        public void Where(string champ, object value, DbType dbType)
        {
            if (value != null && !(value is string && String.IsNullOrEmpty(value.ToString())))
            {
                CommandText += AddWhereOrAnd();
                CommandText += champ.ToUpper() + " = :" + champ.ToLower() + " ";
                AppendParameter(dbType, champ.ToLower(), value);
            }
        }

        public void WhereIn(string champ, List<string> values, bool whereIn = true)
        {
            if (values != null)
            {
                CommandText += AddWhereOrAnd();
                CommandText += champ.ToUpper() ;
                if (!whereIn)
                {
                    CommandText += " NOT";
                }
                CommandText += " IN (";

                for (int i = 0; i < values.Count; i++)
                {
                    if (!String.IsNullOrEmpty(values[i]))
                    {
                        CommandText += ":" + champ.ToLower() + i + ", ";
                        AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower() + i, values[i]);
                    }
                }

                CommandText = CommandText.Substring(0, CommandText.Length - 2);
                CommandText += ") ";

            }

        }

        public void WhereNotIn(string champ, string values)
        {
            if (!String.IsNullOrEmpty(values))
            {
                CommandText += AddWhereOrAnd();
                CommandText += champ.ToUpper() + " NOT IN (:" + champ.ToLower() + ") ";
                AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), values);
            }
        }

        public void WhereLike(string champ, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                CommandText += AddWhereOrAnd();
                CommandText += champ.ToUpper() + " LIKE :" + champ.ToLower() + " ";
                AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), "%" + value + "%");
            }
        }

        public void WhereStartWith(string champ, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                CommandText += AddWhereOrAnd();
                CommandText += champ.ToUpper() + " LIKE :" + champ.ToLower() + " ";
                AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), value + "%");
            }
        }
        public void WhereLikeOr(string champs, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                CommandText += AddWhereOrAnd();
                CommandText += "(";
                string[] champsArray = champs.Split(';');
                foreach (string champ in champsArray)
                {
                    CommandText += champ.ToUpper() + " LIKE :" + champ.ToLower() + " ";
                    AppendParameter(DbType.AnsiStringFixedLength, champ.ToLower(), "%" + value + "%");
                    CommandText += " OR ";
                }
                CommandText = CommandText.Substring(0, CommandText.Length - 4);
                CommandText += ")";
            }
        }

        private string AddWhereOrAnd()
        {
            string result = "";
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
            string commandText = "INSERT INTO " + tableName;
            string nomColonne = "(";
            string value = "VALUES (";

            foreach (Param param in entrees)
            {
                if (param.ValueParam != null)
                {
                    nomColonne += param.NomColonne + ", ";
                    value += param.NameQuery + ", ";
                    this.AppendParameter(param.DbType, param.NomColonne, param.ValueParam);
                }
            }

            nomColonne = nomColonne.Substring(0, nomColonne.Length - 2) + ")";
            value = value.Substring(0, value.Length - 2) + ")";

            commandText += " " + nomColonne + " " + value;
            CommandText = commandText;
        }

        public void Update(string tableName, IEnumerable<Param> entrees)
        {
            string commandText = "UPDATE " + tableName + " SET ";
            foreach (Param param in entrees)
            {
                if (param.ValueParam != null)
                {
                    commandText += param.NomColonne + " = " + param.NameQuery + ", ";
                    this.AppendParameter(param.DbType, param.NomColonne, param.ValueParam);
                }
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
            logger.Info(name + " : " + value.ToString());
            IDbDataParameter dbParameter = Cmd.CreateParameter();
            dbParameter.ParameterName = name;
            dbParameter.DbType = type;
            dbParameter.Value = value;
            dbParameter.Size = size;
            Cmd.Parameters.Add(dbParameter);
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
    }
}
