using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace ALBINGIA.Framework.Common.IOFile
{
    public class ExportToCSVResult<T> : ContentResult
    {
        #region Variables membres
        private readonly List<T> _workList;
        private readonly string _fileName;
        private readonly string _columns;
        #endregion
        #region Méthodes publiques
        public ExportToCSVResult(List<T> workList, string fileName, string columns)
        {
            _workList = workList;
            _fileName = fileName;
            _columns = columns;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string attachment = "attachment; filename=" + _fileName + ".csv";
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ClearHeaders();
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.AddHeader("content-disposition", attachment);
            context.HttpContext.Response.ContentType = "text/csv";
            context.HttpContext.Response.AddHeader("Pragma", "public");
            context.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-15");
            WriteColumnName();
            foreach (T item in _workList)
            {
                GetProperties(item);
            }

            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Lecture des propriétées publiques d'un objet
        /// </summary>
        /// <param name="elem"></param>
        public void GetProperties(T elem)
        {
            var properties = elem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var stringBuilder = new StringBuilder();
            foreach (var propertyInfo in properties)
            {
               
                if (propertyInfo.CanRead)
                {
                    WriteInfo(propertyInfo, stringBuilder, elem);
                }
            }
            HttpContext.Current.Response.Write(stringBuilder.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);
        }
        #endregion
        #region Méthodes privées
        private void WriteInfo(PropertyInfo propertyInfo, StringBuilder stringBuilder, T elem)
        {
            AddComma(propertyInfo.GetValue(elem, null) == null ? "" : propertyInfo.GetValue(elem, null).ToString(), stringBuilder);
        }

        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            stringBuilder.Append(value.Replace(';', ' '));
            stringBuilder.Append("; ");
        }

        private void WriteColumnName()
        {
            string columnNames = _columns;
            HttpContext.Current.Response.Write(columnNames);
            HttpContext.Current.Response.Write(Environment.NewLine);
        }
        #endregion
    }
}
