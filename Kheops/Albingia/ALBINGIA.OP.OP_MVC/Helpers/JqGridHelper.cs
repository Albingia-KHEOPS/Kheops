
using System.Text;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Models.JqGridModel;


namespace ALBINGIA.OP.OP_MVC.Helpers
{
    public static class JqGridHelper
    {
        public static MvcHtmlString AlbCreateJqGrid<T>(this HtmlHelper helper, ALbJqGridGridViewModel<T> model, bool paging = false)
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.AppendFormat(
                @"<table id=""{0}"" class=""scroll"" cellpadding=""0"" cellspacing=""0""></table>", model.Id);
            htmlBuilder.AppendFormat(@"<div id=""{0}Pager"" class=""scroll"" style=""text-align:center;""></div>",
                                     model.Id);

            htmlBuilder.AppendFormat(@"<script type=""text/javascript"">");
            htmlBuilder.AppendFormat(@"$(function()");
            htmlBuilder.AppendFormat(@"{{");
            htmlBuilder.AppendFormat(@"$('#{0}').jqGrid({{", model.Id);
            htmlBuilder.AppendFormat(@"url: '{0}',", model.Url);
            htmlBuilder.AppendFormat(@"datatype: 'json',");
            htmlBuilder.AppendFormat(@"mtype: 'POST',");
            htmlBuilder.AppendFormat(@"editurl: '{0}',", model.EditUrl);
            htmlBuilder.AppendFormat(@"onSelectRow: $.jqGridOnSelectRow, ");
            htmlBuilder.AppendFormat(@"rowNum: -1, ");
            htmlBuilder.AppendFormat(@"viewrecords: false, ");

            //Create Columns
            htmlBuilder.AppendFormat(@"colModel: [");
            foreach (var column in model.Columns)
            {
                htmlBuilder.AppendFormat(
                    @"{{ name: '{0}',
                                                index: '{0}',
                                                width: {1},
                                                align: '{2}',
                                                sortable: {3},
                                                hidden: {4},
                                                editable: {5},
                                                editrules: {6},
                                                edittype : '{7}',
                                                editoptions  : {8}

                    }},",
                    column.Name
                    , column.Width != null && column.Width > 0 ? column.Width : 80
                    //, column.Alignment == null ? "left" : column.Alignment.ToString()
                    , column.Alignment.ToString()
                    , column.Sortable == null ? "false" : column.Sortable.Value.ToString().ToLower()
                    , column.Hidden.ToString().ToLower()
                    , column.Editable.HasValue ? column.Editable.Value.ToString().ToLower() : "false"
                    ,
                    column.EditRules != null && column.EditRules.Required.HasValue
                        ? "{ required :" + column.EditRules.Required.Value.ToString().ToLower() + "}"
                        : "{ required : false}"
                    ,
                    column.EditType != null && column.EditType.HasValue
                        ? column.EditType.Value.ToString().ToLower()
                        : "text"
                    ,
                    column.EditOptions != null && column.EditOptions.MaximumLength.HasValue
                        ? "{ maxlengh :" + column.EditOptions.MaximumLength.Value.ToString() + "}"
                        : "{ maxlengh : 200 }"
                          +
                          (!string.IsNullOrEmpty(column.Formatter) ? ", formatter: " + column.Formatter : string.Empty)
                          +
                          (!string.IsNullOrEmpty(column.Formatter) && column.Formatter == "'date'" ? ", formatoptions: { srcformat: 'd/m/Y', newformat:'d/m/Y'}" : string.Empty)
                          +
                          (column.FormatterOptions != null && !string.IsNullOrEmpty(column.FormatterOptions.DecimalSeparator) ? ", formatoptions: { decimalSeparator: ',', thousandsSeparator: '', defaultValue: ''}" : string.Empty)

                    );

            }
            htmlBuilder.AppendFormat(@"],");
            if (paging)
            {

                htmlBuilder.AppendFormat(@"pager: jQuery('#{0}Pager'),", model.Id);
                htmlBuilder.AppendFormat(@"rowNum: 5,");
                htmlBuilder.AppendFormat(@"rowList: [5, 10, 20, 50],");
            }
            htmlBuilder.AppendFormat(@"viewrecords: true,");
            htmlBuilder.AppendFormat(@"imgpath: '/scripts/themes/coffee/images',");
            htmlBuilder.AppendFormat(@"caption: '{0}',", model.Caption);
            htmlBuilder.AppendFormat(@"width: {0}", model.Width);
            htmlBuilder.AppendFormat(@"}});");
            htmlBuilder.AppendFormat(@"}});");
            htmlBuilder.AppendFormat(@"</script>");
            return MvcHtmlString.Create(htmlBuilder.ToString());
        }



    }
}

