//A ServerSideSortable and Pageable (ASP) Razor Web Grid
//Created by JP Del Mundo / jpdelmundo@gmail.com
//Created for ASP.NET MVC3 using Razor Engine

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Web.Mvc;
using System.Web.Helpers;

namespace StratasFair.Business.CommonHelper
{
    public class ASPRazorWebGrid
    {
        private IEnumerable<dynamic> _source;
        public string Sort { get; set; }
        public SortDirection SortDirection { get; set; }
        private string _pageFieldName;
        private string _sortFieldName;
        private string _sortDirectionFieldName;
        public string GridID { get; set; }
        private bool _maintainState { get; set; }
        public bool UsePostBack { get; set; }
        public Pager Pager { get; set; }
        public string Width { get; set; }

        public ASPRazorWebGrid(IEnumerable<dynamic> source = null, string gridId = null, bool maintainState = true, bool usePostBack = false, string width = null)
        {
            _source = source;
            _maintainState = maintainState;
            GridID = gridId;
            UsePostBack = usePostBack;
            Width = width;

            SortDirection = SortDirection.Ascending;
            _pageFieldName = string.Join("_", gridId, "page");
            _sortFieldName = string.Join("_", gridId, "sort");
            _sortDirectionFieldName = string.Join("_", gridId, "sortdir");
        }

        public void SaveGridParameters()
        {
            var gridParams = GridParameters.GetGridParameters(GridID);
            HttpContext.Current.Session[string.Join("_", GridID, "gridparameters")] = gridParams;
        }

        public IHtmlString GetHtml(ASPRazorWebGridColumn[] columns = null, string tableStyle = null, string headerStyle = null, string alternatingRowStyle = null, Pager pager = null)
        {
            //Pager = pager;
            //Pager.Container = this;
            var table = new TagBuilder("table");

            if (!string.IsNullOrEmpty(Width))
                table.MergeAttribute("style", string.Format("width: {0}", Width));

            if (!string.IsNullOrEmpty(tableStyle)) table.Attributes["class"] = tableStyle;

            if (!string.IsNullOrEmpty(tableStyle)) table.Attributes["id"] = "example1";

            if (columns.Count() > 0)
            {
                //create top pager
                //if (Pager != null && (Pager.Position == PagerPosition.Top || Pager.Position == PagerPosition.TopAndBottom))
                //    Pager.AddPager(table, columns.Count());

                //create header
                var headerRow = new TagBuilder("tr");
                if (!string.IsNullOrEmpty(headerStyle)) headerRow.Attributes["class"] = headerStyle;

                foreach (var item in columns)
                {
                    var cell = new TagBuilder("th");

                    var style = "";
                    style += !string.IsNullOrEmpty(item.Width) ? string.Format("width: {0};", item.Width) : "";
                    style += !string.IsNullOrEmpty(item.HeaderHorizontalAlignment) ? string.Format("text-align: {0};", item.HeaderHorizontalAlignment) : "";
                    style += !string.IsNullOrEmpty(item.HeaderVerticalAlignment) ? string.Format("vertical-align: {0};", item.HeaderVerticalAlignment) : "";

                    if (!string.IsNullOrEmpty(style))
                        cell.MergeAttribute("style", style);

                    if (!string.IsNullOrEmpty(item.DataField))
                    {
                        cell.SetInnerText(item.HeaderText ?? item.DataField);
                    }
                    else if (!string.IsNullOrEmpty(item.HeaderText))
                    {
                        cell.SetInnerText(item.HeaderText);
                    }

                    if (_source.Count() > 0 && item.CanSort && (!string.IsNullOrEmpty(item.DataField) || !string.IsNullOrEmpty(item.HeaderText)))
                    {
                        cell.SetInnerText("");
                        var sortTag = new TagBuilder("a");

                        sortTag.SetInnerText(item.HeaderText ?? item.DataField);

                        if (!UsePostBack)
                        {
                            sortTag.MergeAttribute("href", GetSortQuery(item));
                        }
                        else
                        {
                            var uri = new Uri("http://www.dummyuri.org/?" + GetSortQuery(item));
                            var nvc = HttpUtility.ParseQueryString(uri.Query);
                            sortTag.MergeAttribute("href", "javascript:;");
                            sortTag.MergeAttribute("onclick", "$(\"#" + _sortFieldName + "\").val(\"" + nvc[_sortFieldName] + "\");$(\"#" + _sortDirectionFieldName + "\").val(\"" + nvc[_sortDirectionFieldName] + "\");$(\"#" + _pageFieldName + "\").val(\"\");$(this).parents(\"form:first\").submit();");
                        }

                        cell.InnerHtml = sortTag.ToString();

                        //add sort direction glyph
                        var gridParameters = GridParameters.GetGridParameters(GridID);
                        if (gridParameters.Sort != null
                            && (gridParameters.Sort == item.DataField || gridParameters.Sort == item.SortExpression))
                        {
                            var img = new TagBuilder("img");
                            var imgfile = gridParameters.SortDirection == "ASC" ? "asc.gif" : "desc.gif";
                            img.MergeAttribute("src", "/areas/admin/content/images/" + imgfile);
                            img.MergeAttribute("style", "vertical-align: bottom; margin-left: 2px;");

                            cell.InnerHtml += img.ToString();
                        }


                    }

                    headerRow.InnerHtml += cell;
                }

                table.InnerHtml += "<thead>" + headerRow.ToString() + "</thead>";

                //create rows
                bool rowNormal = true;
                table.InnerHtml += " <tbody>";
                foreach (var item in _source)
                {
                    var row = new TagBuilder("tr");

                    if (!rowNormal && !string.IsNullOrEmpty(alternatingRowStyle))
                        row.Attributes["class"] = alternatingRowStyle;

                    rowNormal = !rowNormal;

                    foreach (var col in columns)
                    {
                        var cell = new TagBuilder("td");

                        var style = "";
                        style += !string.IsNullOrEmpty(col.Width) ? string.Format("width: {0};", col.Width) : "";
                        style += !string.IsNullOrEmpty(col.ItemHorizontalAlignment) ? string.Format("text-align: {0};", col.ItemHorizontalAlignment) : "";
                        style += !string.IsNullOrEmpty(col.ItemVerticalAlignment) ? string.Format("vertical-align: {0};", col.ItemVerticalAlignment) : "";

                        if (!string.IsNullOrEmpty(style))
                            cell.MergeAttribute("style", style);

                        if (!string.IsNullOrEmpty(col.DataField))
                        {
                            cell.SetInnerText(GetDynamicMemberValue(item, col.DataField).ToString());
                        }

                        if (col.DataFieldFormat != null)
                        {
                            cell.InnerHtml += col.DataFieldFormat.Invoke(item);
                        }

                        row.InnerHtml += cell;
                    }

                    table.InnerHtml += row.ToString();
                }
                table.InnerHtml += " </tbody>";

                //create bottom pager
                //if (Pager != null && (Pager.Position == PagerPosition.Bottom || Pager.Position == PagerPosition.TopAndBottom))
                //    Pager.AddPager(table, columns.Count());
            }

            if (_maintainState) SaveGridParameters();

            var resultHtml = table.ToString();

            //create hidden field for grid parameters
            if (UsePostBack)
            {
                resultHtml = AddGridParametersPostbackHtml(resultHtml);
            }

            return new HtmlString(resultHtml);
        }

        public IHtmlString GetHtml(ASPRazorWebGridColumn[] columns = null, string tableStyle = null, string headerStyle = null, string alternatingRowStyle = null, Pager pager = null, string tableId = null)
        {
            //Pager = pager;
            //Pager.Container = this;
            var table = new TagBuilder("table");

            if (!string.IsNullOrEmpty(Width))
                table.MergeAttribute("style", string.Format("width: {0}", Width));

            if (!string.IsNullOrEmpty(tableStyle)) table.Attributes["class"] = tableStyle;

            if (!string.IsNullOrEmpty(tableStyle)) table.Attributes["id"] = tableId;

            if (columns.Count() > 0)
            {
                //create top pager
                //if (Pager != null && (Pager.Position == PagerPosition.Top || Pager.Position == PagerPosition.TopAndBottom))
                //    Pager.AddPager(table, columns.Count());

                //create header
                var headerRow = new TagBuilder("tr");
                if (!string.IsNullOrEmpty(headerStyle)) headerRow.Attributes["class"] = headerStyle;

                foreach (var item in columns)
                {
                    var cell = new TagBuilder("th");

                    var style = "";
                    style += !string.IsNullOrEmpty(item.Width) ? string.Format("width: {0};", item.Width) : "";
                    style += !string.IsNullOrEmpty(item.HeaderHorizontalAlignment) ? string.Format("text-align: {0};", item.HeaderHorizontalAlignment) : "";
                    style += !string.IsNullOrEmpty(item.HeaderVerticalAlignment) ? string.Format("vertical-align: {0};", item.HeaderVerticalAlignment) : "";

                    if (!string.IsNullOrEmpty(style))
                        cell.MergeAttribute("style", style);

                    if (!string.IsNullOrEmpty(item.DataField))
                    {
                        cell.SetInnerText(item.HeaderText ?? item.DataField);
                    }
                    else if (!string.IsNullOrEmpty(item.HeaderText))
                    {
                        cell.SetInnerText(item.HeaderText);
                    }

                    if (_source.Count() > 0 && item.CanSort && (!string.IsNullOrEmpty(item.DataField) || !string.IsNullOrEmpty(item.HeaderText)))
                    {
                        cell.SetInnerText("");
                        var sortTag = new TagBuilder("a");

                        sortTag.SetInnerText(item.HeaderText ?? item.DataField);

                        if (!UsePostBack)
                        {
                            sortTag.MergeAttribute("href", GetSortQuery(item));
                        }
                        else
                        {
                            var uri = new Uri("http://www.dummyuri.org/?" + GetSortQuery(item));
                            var nvc = HttpUtility.ParseQueryString(uri.Query);
                            sortTag.MergeAttribute("href", "javascript:;");
                            sortTag.MergeAttribute("onclick", "$(\"#" + _sortFieldName + "\").val(\"" + nvc[_sortFieldName] + "\");$(\"#" + _sortDirectionFieldName + "\").val(\"" + nvc[_sortDirectionFieldName] + "\");$(\"#" + _pageFieldName + "\").val(\"\");$(this).parents(\"form:first\").submit();");
                        }

                        cell.InnerHtml = sortTag.ToString();

                        //add sort direction glyph
                        var gridParameters = GridParameters.GetGridParameters(GridID);
                        if (gridParameters.Sort != null
                            && (gridParameters.Sort == item.DataField || gridParameters.Sort == item.SortExpression))
                        {
                            var img = new TagBuilder("img");
                            var imgfile = gridParameters.SortDirection == "ASC" ? "asc.gif" : "desc.gif";
                            img.MergeAttribute("src", "/areas/admin/content/images/" + imgfile);
                            img.MergeAttribute("style", "vertical-align: bottom; margin-left: 2px;");

                            cell.InnerHtml += img.ToString();
                        }


                    }

                    headerRow.InnerHtml += cell;
                }

                table.InnerHtml += "<thead>" + headerRow.ToString() + "</thead>";

                //create rows
                bool rowNormal = true;
                table.InnerHtml += " <tbody>";
                foreach (var item in _source)
                {
                    var row = new TagBuilder("tr");

                    if (!rowNormal && !string.IsNullOrEmpty(alternatingRowStyle))
                        row.Attributes["class"] = alternatingRowStyle;

                    rowNormal = !rowNormal;

                    foreach (var col in columns)
                    {
                        var cell = new TagBuilder("td");

                        var style = "";
                        style += !string.IsNullOrEmpty(col.Width) ? string.Format("width: {0};", col.Width) : "";
                        style += !string.IsNullOrEmpty(col.ItemHorizontalAlignment) ? string.Format("text-align: {0};", col.ItemHorizontalAlignment) : "";
                        style += !string.IsNullOrEmpty(col.ItemVerticalAlignment) ? string.Format("vertical-align: {0};", col.ItemVerticalAlignment) : "";

                        if (!string.IsNullOrEmpty(style))
                            cell.MergeAttribute("style", style);
                        try
                        {
                            if (!string.IsNullOrEmpty(col.DataField))
                            {
                                cell.SetInnerText(GetDynamicMemberValue(item, col.DataField).ToString());
                            }
                        }
                        catch
                        {
                            cell.SetInnerText("");
                        }

                        if (col.DataFieldFormat != null)
                        {
                            cell.InnerHtml += col.DataFieldFormat.Invoke(item);
                        }

                        row.InnerHtml += cell;
                    }

                    table.InnerHtml += row.ToString();
                }
                table.InnerHtml += " </tbody>";

                //create bottom pager
                //if (Pager != null && (Pager.Position == PagerPosition.Bottom || Pager.Position == PagerPosition.TopAndBottom))
                //    Pager.AddPager(table, columns.Count());
            }

            if (_maintainState) SaveGridParameters();

            var resultHtml = table.ToString();

            //create hidden field for grid parameters
            if (UsePostBack)
            {
                resultHtml = AddGridParametersPostbackHtml(resultHtml);
            }

            return new HtmlString(resultHtml);
        }


        private string AddGridParametersPostbackHtml(string resultHtml)
        {
            var form = new TagBuilder("form");
            form.MergeAttribute("method", "post");
            form.MergeAttribute("action", HttpContext.Current.Request.RawUrl);

            var gridParams = GridParameters.GetGridParameters();

            form.InnerHtml += CreateHiddenInput(_pageFieldName, Convert.ToString(gridParams.Page)).ToString();
            form.InnerHtml += CreateHiddenInput(_sortFieldName, gridParams.Sort).ToString();
            form.InnerHtml += CreateHiddenInput(_sortDirectionFieldName, gridParams.SortDirection).ToString();
            form.InnerHtml += resultHtml;

            return form.ToString();
        }

        private TagBuilder CreateHiddenInput(string name, string value)
        {
            var hidden = new TagBuilder("input");
            hidden.MergeAttribute("type", "hidden");
            hidden.MergeAttribute("name", name);
            hidden.MergeAttribute("id", name);
            hidden.MergeAttribute("value", value);

            return hidden;
        }

        private string GetSortQuery(ASPRazorWebGridColumn item)
        {
            var gridParams = GridParameters.GetGridParameters(GridID);
            var querySource = UsePostBack ? HttpContext.Current.Request.Form : HttpContext.Current.Request.QueryString;
            var applySortDirection = SortDirection.Ascending;

            var currentSort = gridParams.Sort;
            if (!string.IsNullOrEmpty(currentSort) && (currentSort == item.DataField || currentSort == item.SortExpression))
            {
                var currentSortDirection = gridParams.SortDirection;
                applySortDirection = currentSortDirection == "DESC" ? SortDirection.Ascending : SortDirection.Descending;
            }

            var sortQuery = "";

            if (!UsePostBack)
            {
                sortQuery += HttpContext.Current.Request.Path + string.Format("?{0}={1}&{2}={3}", UrlEncode(_sortFieldName)
                                                            , UrlEncode(item.SortExpression ?? item.DataField)
                                                            , UrlEncode(_sortDirectionFieldName)
                                                            , UrlEncode(applySortDirection == SortDirection.Ascending ? "ASC" : "DESC"));
            }
            else
            {
                sortQuery = string.Format("{0}={1}&{2}={3}", _sortFieldName
                                                            , item.SortExpression ?? item.DataField
                                                            , _sortDirectionFieldName
                                                            , applySortDirection == SortDirection.Ascending ? "ASC" : "DESC");
            }

            return sortQuery;
        }

        private string UrlEncode(string url)
        {
            return HttpContext.Current.Server.UrlEncode(url);
        }

        private object GetDynamicMemberValue(dynamic obj, string property)
        {
            object val = null;

            if (property.Contains("."))
            {
                var innerProp = property.Split('.');
                var innerObj = obj.GetType().GetProperty(innerProp[0]).GetValue(obj, null);
                innerProp[0] = null;
                val = GetDynamicMemberValue(innerObj, string.Join(".", innerProp).TrimStart('.'));
            }
            else
            {
                try
                {
                    val = obj.GetType().GetProperty(property).GetValue(obj, null);
                }
                catch
                {
                    val = "";
                }
            }

            return val;
        }

        public ASPRazorWebGridColumn CreateColumn(string dataField = null, string headerText = null, Func<dynamic, object> dataFieldFormat = null, bool canSort = false, string sortExpression = null, string width = null, string itemHorizontalAlignment = null, string itemVerticalAlignment = null, string headerHorizontalAlignment = null, string headerVerticalAlignment = null)
        {
            return new ASPRazorWebGridColumn
            {
                DataField = dataField,
                HeaderText = headerText,
                DataFieldFormat = dataFieldFormat,
                CanSort = canSort,
                SortExpression = sortExpression,
                Width = width,
                ItemHorizontalAlignment = itemHorizontalAlignment,
                ItemVerticalAlignment = itemVerticalAlignment,
                HeaderHorizontalAlignment = headerHorizontalAlignment,
                HeaderVerticalAlignment = headerVerticalAlignment
            };
        }
    }

    public class ASPRazorWebGridColumn
    {
        public string DataField { get; set; }
        public string HeaderText { get; set; }
        public Func<dynamic, object> DataFieldFormat { get; set; }
        public bool CanSort { get; set; }
        public string SortExpression { get; set; }
        public string Width { get; set; }
        public string HeaderHorizontalAlignment { get; set; }
        public string HeaderVerticalAlignment { get; set; }
        public string ItemHorizontalAlignment { get; set; }
        public string ItemVerticalAlignment { get; set; }
    }

    public enum PagerPosition
    {
        Top,
        Bottom,
        TopAndBottom
    }

    public enum PagerAlign
    {
        Center,
        Right,
        Left
    }

    public class Pager
    {
        public int RowsPerPage { get; set; }
        public int TotalRows { get; set; }
        public PagerPosition Position { get; set; }
        public PagerAlign Alignment { get; set; }
        private IHtmlString PagerHtml { get; set; }
        public string CssClass { get; set; }
        public ASPRazorWebGrid Container { get; set; }
        public bool UseImageButtons { get; set; }

        public int PageCount { get { return Convert.ToInt32(Math.Ceiling(1.0 * TotalRows / RowsPerPage)); } }
        public int? CurrentPage
        {
            get
            {
                var gridParameters = GridParameters.GetGridParameters(Container.GridID);
                gridParameters.Page = gridParameters.Page > PageCount ? PageCount : gridParameters.Page;
                return gridParameters.Page ?? 1;
            }
        }
        public int NextPage
        {
            get
            {
                return Convert.ToInt32(CurrentPage) + 1 > PageCount ? PageCount : Convert.ToInt32(CurrentPage) + 1;
            }
        }

        private int PrevPage
        {
            get
            {
                return Convert.ToInt32(CurrentPage) - 1 < 1 ? 1 : Convert.ToInt32(CurrentPage) - 1;
            }
        }

        public string GetPagerHtml()
        {
            if (PagerHtml != null) return PagerHtml.ToString();

            if (TotalRows == 0) return "No record found";

            //create dropdownlist
            var pageSelector = new TagBuilder("select");

            if (!Container.UsePostBack)
            {
                pageSelector.MergeAttribute("onchange", "javascript:window.location.href=this.value;");
            }
            else
            {
                pageSelector.MergeAttribute("onchange", "$(\"#" + Container.GridID + "_page" + "\").val($(this).val());$(this).parents(\"form:first\").submit();");
            }

            for (int i = 1; i <= PageCount; i++)
            {
                var option = new TagBuilder("option");
                option.SetInnerText(i.ToString());

                if (!Container.UsePostBack)
                {
                    option.MergeAttribute("value", HttpContext.Current.Request.Path + "?" + GetPageQuery(i));
                }
                else
                {
                    option.MergeAttribute("value", i.ToString());
                }

                if (i == CurrentPage)
                    option.MergeAttribute("selected", "selected");

                pageSelector.InnerHtml += option.ToString();
            }

            var pager = new TagBuilder("div");
            var next = GetPagerNav(NextPage, "Next");
            var last = GetPagerNav(PageCount, "Last");
            var first = GetPagerNav(1, "First");
            var prev = GetPagerNav(PrevPage, "Prev");
            //create first, prev, next, last
            if (TotalRows > RowsPerPage)
            {
                if (CurrentPage == 1)
                {
                    pager.InnerHtml = " Records (" + TotalRows.ToString() + ") " + pageSelector.ToString() + " of " + PageCount.ToString() + " " + next + " " + last;
                }
                if (Math.Ceiling((double)TotalRows / RowsPerPage) == CurrentPage)
                {
                    pager.InnerHtml = first + " " + prev + " Records (" + TotalRows.ToString() + ") " + pageSelector.ToString() + " of " + PageCount.ToString();
                }
                if ((CurrentPage > 1) && CurrentPage < Math.Ceiling((double)TotalRows / RowsPerPage))
                {
                    pager.InnerHtml = first + " " + prev + " Records (" + TotalRows.ToString() + ") " + pageSelector.ToString() + " of " + PageCount.ToString() + " " + next + " " + last;
                }

            }
            else
            {
                pager.InnerHtml = " Records (" + TotalRows.ToString() + ") " + pageSelector.ToString() + " of " + PageCount.ToString();
            }
            PagerHtml = new HtmlString(pager.ToString());

            return PagerHtml.ToString();
        }

        private string UrlEncode(string url)
        {
            return HttpContext.Current.Server.UrlEncode(url);
        }

        private string GetPageQuery(int pageNum)
        {
            var pageQuery = "";

            if (CurrentPage == null)
            {
                pageQuery = string.Format("{0}={1}", UrlEncode(string.Join("_", Container.GridID, "page")), UrlEncode(pageNum.ToString()));
                pageQuery = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString())
                                                    ? pageQuery
                                                    : HttpContext.Current.Request.QueryString.ToString() + "&" + pageQuery;
            }
            else
            {
                var nvc = HttpUtility.ParseQueryString(HttpContext.Current.Request.QueryString.ToString());
                nvc[string.Join("_", Container.GridID, "page")] = UrlEncode(pageNum.ToString());
                pageQuery = nvc.ToString();
            }

            return pageQuery;
        }

        private string GetPagerNav(int pageNum)
        {
            return GetPagerNav(pageNum, pageNum.ToString());
        }

        private string GetPagerNav(int pageNum, string altText)
        {
            var nav = new TagBuilder("a");
            var img = new TagBuilder("img");
            var imgfile = "";

            switch (altText)
            {
                case "Prev":
                    imgfile = "navprev.gif";
                    break;
                case "First":
                    imgfile = "navfirst.gif";
                    break;
                case "Next":
                    imgfile = "navnext.gif";
                    break;
                case "Last":
                    imgfile = "navlast.gif";
                    break;
                default:
                    break;
            }

            img.MergeAttribute("src", "/content/admin/images/" + imgfile);
            img.MergeAttribute("style", "vertical-align: absmiddle;");
            img.MergeAttribute("alt", imgfile);

            if (UseImageButtons)
                nav.InnerHtml = img.ToString();
            else
                nav.SetInnerText(altText);

            if (!Container.UsePostBack)
            {
                nav.MergeAttribute("href", HttpContext.Current.Request.Path + "?" + GetPageQuery(pageNum));
            }
            else
            {
                var nvc = HttpUtility.ParseQueryString(string.Format("{0}={1}", Container.GridID + "_page", pageNum.ToString()));
                nav.MergeAttribute("href", "javascript:;");
                nav.MergeAttribute("onclick", "$(\"#" + Container.GridID + "_page" + "\").val(\"" + nvc[Container.GridID + "_page"] + "\");$(this).parents(\"form:first\").submit();");
            }

            return nav.ToString();
        }

        public void AddPager(TagBuilder table, int columnCount)
        {
            var row = new TagBuilder("tr");

            if (!string.IsNullOrEmpty(CssClass))
                row.MergeAttribute("class", CssClass);

            var cell = new TagBuilder("td");

            cell.MergeAttribute("align", Alignment.ToString());

            if (columnCount > 1)
                cell.MergeAttribute("colspan", columnCount.ToString());

            cell.InnerHtml = GetPagerHtml();
            row.InnerHtml = cell.ToString();

            table.InnerHtml += row;
        }
    }

    public class GridParameters
    {
        public int? Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }

        public static GridParameters GetGridParameters(string gridID, List<string> validSortColumns = null)
        {
            var paramSource = HttpContext.Current.Request;
            var queryKeys = HttpContext.Current.Request.QueryString.AllKeys;
            var formKeys = HttpContext.Current.Request.Form.AllKeys;
            var pageKey = string.Join("_", gridID, "page");
            var sortKey = string.Join("_", gridID, "sort");
            var sortDirKey = string.Join("_", gridID, "sortdir");

            GridParameters parameters = (GridParameters)HttpContext.Current.Session[string.Join("_", gridID, "gridparameters")];
            if (parameters == null) parameters = new GridParameters();

            if (queryKeys.Contains(pageKey) || formKeys.Contains(pageKey))
            {
                try
                {
                    parameters.Page = Int32.Parse(paramSource[pageKey]);
                }
                catch { }
            }

            if (queryKeys.Contains(sortKey) || formKeys.Contains(sortKey))
                parameters.Sort = paramSource[sortKey];

            if (queryKeys.Contains(sortDirKey) || formKeys.Contains(sortDirKey))
                parameters.SortDirection = paramSource[sortDirKey];

            if (validSortColumns != null
                && validSortColumns.Count > 0
                && !validSortColumns.Contains(paramSource[sortKey]))
            {
                parameters.Sort =
                parameters.SortDirection = null;
            }

            parameters.Sort = parameters.Sort == "" ? null : parameters.Sort;
            parameters.SortDirection = parameters.SortDirection == "" ? null : parameters.SortDirection;

            return parameters;
        }

        public static GridParameters GetGridParameters(List<string> validSortColumns = null)
        {
            return GetGridParameters(null, validSortColumns);
        }

        public static void ResetGridParameters(string gridGridID)
        {
            HttpContext.Current.Session[string.Join("_", gridGridID, "gridparameters")] = null;
        }

        public static void ResetGridParameters()
        {
            ResetGridParameters(null);
        }
    }
}