using StratasFair.BusinessEntity.Admin;
using StratasFair.Business.Admin;
using StratasFair.Business.CommonHelper;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratasFair.Business.Admin
{
    public class MenuHelper
    {
        private String _conString = String.Empty;
        public static bool isSelected = false;
        public MenuHelper()
        {
            try
            {
                _conString = SqlHelper.GetConnectionString();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetAllModule(DashBoardModel objDashboard)
        {
            try
            {
                SqlParameter prmParent = SqlHelper.CreateParameter("@ParentId", objDashboard.ParentId);
                SqlParameter prmFlag = SqlHelper.CreateParameter("@Flag", objDashboard.Flag);
                SqlParameter prmRole = SqlHelper.CreateParameter("@RoleId", objDashboard.RoleId);
                SqlParameter[] allParams = { prmParent, prmFlag, prmRole };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetActiveModule", allParams);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable Dt = ds.Tables[0].Copy();
                    ds.Dispose();
                    return Dt;
                }
                else
                {
                    return (DataTable)null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static string GetModuleForRoles()
        {
            try
            {
                MenuHelper objMenu = new MenuHelper();
                DataTable dtMenu = objMenu.GetAllModulesForRole(0, 1, 0);
                StringBuilder objSBFirst = new StringBuilder();
                if (dtMenu != null && dtMenu.Rows.Count > 0)
                {
                    for (int first = 0; first < dtMenu.Rows.Count; first++)
                    {
                        objSBFirst.Append("<tr>");

                        objSBFirst.Append("<td class='FieldCaptionTD'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "' onclick=\"CheckAllCheckBoxes('rptMenu',this.checked,this.id)\" style=\"margin-right:10px; margin-bottom:10px;\" />");
                        objSBFirst.Append("" + dtMenu.Rows[first]["menuName"].ToString() + "");
                        objSBFirst.Append("</td>");


                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chk1_" + first.ToString() + "' />");
                        objSBFirst.Append("</td>");

                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chk2_" + first.ToString() + "' />");
                        objSBFirst.Append("</td>");

                        objSBFirst.Append("<td class='FieldCaptionTD'>");
                        objSBFirst.Append("<input type='hidden' id='" + "hModuleId" + first.ToString() + "'  value='" + dtMenu.Rows[first]["moduleId"].ToString() + "' />");
                        objSBFirst.Append("</td>");

                        objSBFirst.Append("</tr>");

                        DataTable dtSubMenu = objMenu.GetAllModulesForRole(Convert.ToInt32(dtMenu.Rows[first]["moduleId"].ToString()), 2, 0);

                        if (dtSubMenu != null && dtSubMenu.Rows.Count > 0)
                        {
                            for (int second = 0; second < dtSubMenu.Rows.Count; second++)
                            {
                                objSBFirst.Append("<tr>");

                                objSBFirst.Append("<td class='FieldCaptionTD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubMenu',this.checked,this.id)\" style=\"margin-right:10px; margin-bottom:10px;\" />");
                                objSBFirst.Append("" + dtSubMenu.Rows[second]["menuName"].ToString() + "");
                                objSBFirst.Append("</td>");


                                objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkF" + first.ToString() + "_" + second.ToString() + "' />");
                                objSBFirst.Append("</td>");

                                objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkS" + first.ToString() + "_" + second.ToString() + "' />");
                                objSBFirst.Append("</td>");

                                objSBFirst.Append("<td class='FieldCaptionTD'>");
                                objSBFirst.Append("<input type='hidden' id='" + "hSubModuleId" + second.ToString() + "'  value='" + dtSubMenu.Rows[second]["moduleId"].ToString() + "' />");
                                objSBFirst.Append("</td>");

                                objSBFirst.Append("</tr>");


                                DataTable dtSubSubMenu = objMenu.GetAllModulesForRole(Convert.ToInt32(dtSubMenu.Rows[second]["moduleId"].ToString()), 3, 0);

                                if (dtSubSubMenu != null && dtSubSubMenu.Rows.Count > 0)
                                {
                                    for (int third = 0; third < dtSubSubMenu.Rows.Count; third++)
                                    {
                                        objSBFirst.Append("<tr>");

                                        objSBFirst.Append("<td class='FieldCaptionTD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubSubMenu',this.checked,this.id)\" style=\"margin-right:10px; margin-bottom:10px;\" />");
                                        objSBFirst.Append("" + dtSubSubMenu.Rows[third]["menuName"].ToString() + "");
                                        objSBFirst.Append("</td>");


                                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkF" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' />");
                                        objSBFirst.Append("</td>");

                                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkS" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' />");
                                        objSBFirst.Append("</td>");

                                        objSBFirst.Append("<td class='FieldCaptionTD'>");
                                        objSBFirst.Append("<input type='hidden' id='" + "hSubSubModuleId_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "'  value='" + dtSubSubMenu.Rows[third]["moduleId"].ToString() + "' />");
                                        objSBFirst.Append("</td>");

                                        objSBFirst.Append("</tr>");

                                    }
                                }
                            }
                        }
                    }
                }
                return objSBFirst.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static string Navigation(string routeName, string action)
        {
            try
            {
                DataRow[] objParentModules = null;
                StringBuilder objStr = new StringBuilder();
                StringBuilder objStrSubMenu = new StringBuilder();
                string rootPath = ConfigurationManager.AppSettings["WebsiteRootPath"].ToString();


                DashBoardModel objDashboard = new DashBoardModel();
                objDashboard.ParentId = 1;
                objDashboard.RoleId = Convert.ToInt32(AdminSessionData.AdminRoleId);
                objDashboard.Flag = 2;

                MenuHelper objMenu = new MenuHelper();
                DataTable dt = objMenu.GetAllModule(objDashboard);
                if (dt.Rows.Count > 0)
                {
                    objParentModules = dt.Select("parentid=1");
                }

                if (objParentModules != null && objParentModules.Length > 0)
                {
                    int intCount = 1;
                    isSelected = false;
                    string strActiveClass = "";
                    //if (routeName.ToLower() == "dashboard")
                    //  strActiveClass = "";
                    objStr.Append("<ul class=\"sidebar-menu\">");
                    objStr.Append("<li class=\"header\">MAIN NAVIGATION</li>");
                    for (int RowCounter = 0; RowCounter < objParentModules.Length; RowCounter++)
                    {
                        string NavUrl = rootPath + objParentModules[RowCounter]["navigateUrl"].ToString();

                        if (RowCounter != 0)
                        {
                            strActiveClass = "";
                        }
                        if (routeName.ToLower() == "dashboard")
                            strActiveClass = "";

                        if (NavUrl.ToLower().Contains("/" + routeName.ToLower()))
                        {
                            strActiveClass = "active";
                        }
                        if (objParentModules[RowCounter]["childnodecount"].ToString() != "0")
                        {
                            if (objParentModules[RowCounter]["childnodecount"].ToString() != "0")
                            {
                                objStr.Append("<li  class=\"treeview " + strActiveClass + "\">" +
                                    "<a " + ((objParentModules[RowCounter]["menuName"].ToString().IndexOf("?") > 0) ? GetNewCSSClass(objParentModules[RowCounter]["menuName"].ToString()) : GetCSSClass(objParentModules[RowCounter]["menuName"].ToString())) + " href=\"" + NavUrl + "\"  class='" + strActiveClass + "'>" +
                                    "<i " + GetLeftPanelIconInAdminPanel(objParentModules[RowCounter]["menuName"].ToString()) + "></i> <span>" + objParentModules[RowCounter]["menuName"].ToString() + "</span>" +
                                    " <i class=\"fa fa-angle-left pull-right\"></i></a>");
                            }
                            else
                            {
                                //objStr.Append("<li><a " + ((objParentModules[RowCounter]["menuName"].ToString().IndexOf("?") > 0) ? GetNewCSSClass(objParentModules[RowCounter]["menuName"].ToString()) : GetCSSClass(objParentModules[RowCounter]["menuName"].ToString())) + " href=\"" + rootPath + objParentModules[RowCounter]["navigateUrl"].ToString() + "\" ><span>" + objParentModules[RowCounter]["menuName"].ToString() + "</span></a>");
                                //if (objParentModules[RowCounter]["menuName"].ToString().ToLower().Contains(routeName.ToLower()))
                                //{
                                //    objStr.Append("<li><a class=\"active\" href=\"" + rootPath + objParentModules[RowCounter]["navigateUrl"].ToString() + "\" ><span>" + objParentModules[RowCounter]["menuName"].ToString() + "</span></a>");
                                //}
                                objStr.Append("<li><a " + ((objParentModules[RowCounter]["menuName"].ToString().IndexOf("?") > 0) ? GetNewCSSClass(objParentModules[RowCounter]["menuName"].ToString()) : GetCSSClass(objParentModules[RowCounter]["menuName"].ToString())) + " href=\"" + NavUrl + "\" ><span>" + objParentModules[RowCounter]["menuName"].ToString() + "</span></a>");

                            }
                        }
                        else
                        {
                            if (objParentModules[RowCounter]["childnodecount"].ToString() != "0")
                            {
                                objStr.Append("<li class=\"treeview " + strActiveClass + "\"><a " + ((objParentModules[RowCounter]["menuName"].ToString().IndexOf("?") > 0) ? GetNewCSSClass(objParentModules[RowCounter]["menuName"].ToString()) : GetCSSClass(objParentModules[RowCounter]["menuName"].ToString())) + " href=\"" + NavUrl + "\" >" +
                                    "<i " + GetLeftPanelIconInAdminPanel(objParentModules[RowCounter]["menuName"].ToString()) + "></i> <span>" + objParentModules[RowCounter]["menuName"].ToString() + "</span>" +
                                    " <i class=\"fa fa-angle-left pull-right\"></i></a>");
                            }
                            else
                            {
                                objStr.Append("<li class='" + strActiveClass + "'><a " + ((objParentModules[RowCounter]["menuName"].ToString().IndexOf("?") > 0) ? GetNewCSSClass(objParentModules[RowCounter]["menuName"].ToString()) : GetCSSClass(objParentModules[RowCounter]["menuName"].ToString())) + " href=\"" + NavUrl + "\" ><span>" + objParentModules[RowCounter]["menuName"].ToString() + "</span></a>");
                            }
                        }

                        if (objParentModules[RowCounter]["childnodecount"].ToString() != "0")
                        {
                            objStr.Append("<ul  class=\"treeview-menu\">");
                            DataRow[] objChildModules = dt.Select("parentid=" + objParentModules[RowCounter]["moduleId"].ToString());
                            for (int ChildRowCounter = 0; ChildRowCounter < objChildModules.Length; ChildRowCounter++)
                            {
                                string Name = objChildModules[ChildRowCounter]["menuName"].ToString();
                                NavUrl = rootPath + objChildModules[ChildRowCounter]["navigateUrl"].ToString().ToLower();
                                if (action != null || action != "")
                                {
                                    if (NavUrl.Contains("/" + routeName.ToLower() + "/" + action.ToLower()))
                                    {
                                        strActiveClass = "active";
                                        goto label;
                                    }
                                    else if (NavUrl.Contains("/" + routeName.ToLower() + "/"))
                                    {
                                        strActiveClass = "active";
                                    }
                                    else
                                    {
                                        strActiveClass = "";
                                    }
                                }
                            label:
                                objStr.Append("<li class='" + strActiveClass + "'><a href=\"" + NavUrl + "\" ><i class=\"fa fa-angle-double-right\"></i>" + Name + "</a></li>");
                            }
                            objStr.Append("</ul>");
                        }
                        objStr.Append("</li>");
                        intCount++;
                    }
                    objStr.Append("</ul>");
                    isSelected = false;
                }
                return objStr.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static string GetEditModuleForRoles(int RoleId)
        {
            try
            {
                MenuHelper objMenu = new MenuHelper();
                DataTable dtMenu = objMenu.GetAllModulesForRole(0, 4, RoleId);
                StringBuilder objSBFirst = new StringBuilder();
                if (dtMenu != null && dtMenu.Rows.Count > 0)
                {
                    for (int first = 0; first < dtMenu.Rows.Count; first++)
                    {
                        objSBFirst.Append("<tr>");

                        objSBFirst.Append("<td class='FieldCaptionTD'>");
                        if (dtMenu.Rows[first]["IsChecked"].ToString() == "1")
                        {
                            objSBFirst.Append("<input type='checkbox' checked='checked' id='" + "chkHm_" + first.ToString() + "' onclick=\"CheckAllCheckBoxes('rptMenu',this.checked,this.id)\" />");
                        }
                        else
                        {
                            objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "' onclick=\"CheckAllCheckBoxes('rptMenu',this.checked,this.id)\" />");
                        }
                        objSBFirst.Append("" + dtMenu.Rows[first]["menuName"].ToString() + "");
                        objSBFirst.Append("</td>");


                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chk1_" + first.ToString() + "' />");
                        objSBFirst.Append("</td>");

                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chk2_" + first.ToString() + "' />");
                        objSBFirst.Append("</td>");

                        objSBFirst.Append("<td class='FieldCaptionTD'>");
                        objSBFirst.Append("<input type='hidden' id='" + "hModuleId" + first.ToString() + "'  value='" + dtMenu.Rows[first]["moduleId"].ToString() + "' />");
                        objSBFirst.Append("</td>");

                        objSBFirst.Append("</tr>");

                        DataTable dtSubMenu = objMenu.GetAllModulesForRole(Convert.ToInt32(dtMenu.Rows[first]["moduleId"].ToString()), 5, RoleId);

                        if (dtSubMenu != null && dtSubMenu.Rows.Count > 0)
                        {
                            for (int second = 0; second < dtSubMenu.Rows.Count; second++)
                            {
                                objSBFirst.Append("<tr>");

                                objSBFirst.Append("<td class='FieldCaptionTD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                                if (dtSubMenu.Rows[second]["IsChecked"].ToString() == "1")
                                {
                                    objSBFirst.Append("<input type='checkbox' checked='checked' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubMenu',this.checked,this.id)\" />");
                                }
                                else
                                {
                                    objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubMenu',this.checked,this.id)\" />");
                                }
                                objSBFirst.Append("" + dtSubMenu.Rows[second]["menuName"].ToString() + "");
                                objSBFirst.Append("</td>");


                                objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkF" + first.ToString() + "_" + second.ToString() + "' />");
                                objSBFirst.Append("</td>");

                                objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkS" + first.ToString() + "_" + second.ToString() + "' />");
                                objSBFirst.Append("</td>");

                                objSBFirst.Append("<td class='FieldCaptionTD'>");
                                objSBFirst.Append("<input type='hidden' id='" + "hSubModuleId" + second.ToString() + "'  value='" + dtSubMenu.Rows[second]["moduleId"].ToString() + "' />");
                                objSBFirst.Append("</td>");

                                objSBFirst.Append("</tr>");


                                DataTable dtSubSubMenu = objMenu.GetAllModulesForRole(Convert.ToInt32(dtSubMenu.Rows[second]["moduleId"].ToString()), 6, RoleId);

                                if (dtSubSubMenu != null && dtSubSubMenu.Rows.Count > 0)
                                {
                                    for (int third = 0; third < dtSubSubMenu.Rows.Count; third++)
                                    {
                                        objSBFirst.Append("<tr>");

                                        objSBFirst.Append("<td class='FieldCaptionTD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

                                        if (dtSubSubMenu.Rows[third]["IsChecked"].ToString() == "1")
                                        {
                                            objSBFirst.Append("<input type='checkbox' checked='checked' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubSubMenu',this.checked,this.id)\" />");
                                        }
                                        else
                                        {
                                            objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubSubMenu',this.checked,this.id)\" />");
                                        }



                                        objSBFirst.Append("" + dtSubSubMenu.Rows[third]["menuName"].ToString() + "");
                                        objSBFirst.Append("</td>");


                                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkF" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' />");
                                        objSBFirst.Append("</td>");

                                        objSBFirst.Append("<td class='FieldCaptionTD' style='display: none' align='left'>");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkS" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' />");
                                        objSBFirst.Append("</td>");

                                        objSBFirst.Append("<td class='FieldCaptionTD'>");
                                        objSBFirst.Append("<input type='hidden' id='" + "hSubSubModuleId_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "'  value='" + dtSubSubMenu.Rows[third]["moduleId"].ToString() + "' />");
                                        objSBFirst.Append("</td>");

                                        objSBFirst.Append("</tr>");

                                    }
                                }
                            }
                        }
                    }
                }
                return objSBFirst.ToString();
            }
            catch
            {
                throw;
            }
        }


        public static string GetNewEditModuleForRoles(int RoleId)
        {
            try
            {
                MenuHelper objMenu = new MenuHelper();
                DataTable dtMenu = objMenu.GetAllModulesForRole(0, 4, RoleId);
                StringBuilder objSBFirst = new StringBuilder();
                if (dtMenu != null && dtMenu.Rows.Count > 0)
                {
                    for (int first = 0; first < dtMenu.Rows.Count; first++)
                    {
                        //objSBFirst.Append("<tr>");

                        objSBFirst.Append("<li class='FieldCaptionTD'>");
                        if (dtMenu.Rows[first]["IsChecked"].ToString() == "1")
                        {
                            objSBFirst.Append("<input type='checkbox' checked='checked' id='" + "chkHm_" + first.ToString() + "' onclick=\"CheckAllCheckBoxes('rptMenu',this.checked,this.id)\" />");
                        }
                        else
                        {
                            objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "' onclick=\"CheckAllCheckBoxes('rptMenu',this.checked,this.id)\" />");
                        }
                        objSBFirst.Append("<span>" + dtMenu.Rows[first]["menuName"].ToString() + "</span>");
                        objSBFirst.Append("</li>");


                        objSBFirst.Append("<li class='FieldCaptionTD' style='display: none' align='left'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chk1_" + first.ToString() + "' />");
                        objSBFirst.Append("</li>");

                        objSBFirst.Append("<li class='FieldCaptionTD' style='display: none' align='left'>");
                        objSBFirst.Append("<input type='checkbox' id='" + "chk2_" + first.ToString() + "' />");
                        objSBFirst.Append("</li>");

                        objSBFirst.Append("<li class='FieldCaptionTD'>");
                        objSBFirst.Append("<input type='hidden' id='" + "hModuleId" + first.ToString() + "'  value='" + dtMenu.Rows[first]["moduleId"].ToString() + "' />");
                        objSBFirst.Append("</li>");

                        //objSBFirst.Append("</tr>");

                        DataTable dtSubMenu = objMenu.GetAllModulesForRole(Convert.ToInt32(dtMenu.Rows[first]["moduleId"].ToString()), 5, RoleId);

                        if (dtSubMenu != null && dtSubMenu.Rows.Count > 0)
                        {
                            for (int second = 0; second < dtSubMenu.Rows.Count; second++)
                            {
                                objSBFirst.Append("<ul class='subMenu'>");

                                objSBFirst.Append("<li class='FieldCaptionTD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                                if (dtSubMenu.Rows[second]["IsChecked"].ToString() == "1")
                                {
                                    objSBFirst.Append("<input type='checkbox' checked='checked' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubMenu',this.checked,this.id)\" />");
                                }
                                else
                                {
                                    objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubMenu',this.checked,this.id)\" />");
                                }
                                objSBFirst.Append("<span>" + dtSubMenu.Rows[second]["menuName"].ToString() + "</span>");
                                objSBFirst.Append("</li>");


                                objSBFirst.Append("<li class='FieldCaptionTD' style='display: none' align='left'>");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkF" + first.ToString() + "_" + second.ToString() + "' />");
                                objSBFirst.Append("</li>");

                                objSBFirst.Append("<li class='FieldCaptionTD' style='display: none' align='left'>");
                                objSBFirst.Append("<input type='checkbox' id='" + "chkS" + first.ToString() + "_" + second.ToString() + "' />");
                                objSBFirst.Append("</li>");

                                objSBFirst.Append("<li class='FieldCaptionTD'>");
                                objSBFirst.Append("<input type='hidden' id='" + "hSubModuleId" + second.ToString() + "'  value='" + dtSubMenu.Rows[second]["moduleId"].ToString() + "' />");
                                objSBFirst.Append("</li>");

                                objSBFirst.Append("</ul>");


                                DataTable dtSubSubMenu = objMenu.GetAllModulesForRole(Convert.ToInt32(dtSubMenu.Rows[second]["moduleId"].ToString()), 6, RoleId);

                                if (dtSubSubMenu != null && dtSubSubMenu.Rows.Count > 0)
                                {
                                    for (int third = 0; third < dtSubSubMenu.Rows.Count; third++)
                                    {
                                        objSBFirst.Append("<ul>");

                                        objSBFirst.Append("<li class='FieldCaptionTD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

                                        if (dtSubSubMenu.Rows[third]["IsChecked"].ToString() == "1")
                                        {
                                            objSBFirst.Append("<input type='checkbox' checked='checked' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubSubMenu',this.checked,this.id)\" />");
                                        }
                                        else
                                        {
                                            objSBFirst.Append("<input type='checkbox' id='" + "chkHm_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' onclick=\"CheckAllCheckBoxes('rptSubSubMenu',this.checked,this.id)\" />");
                                        }



                                        objSBFirst.Append("<span>" + dtSubSubMenu.Rows[third]["menuName"].ToString() + "</span>");
                                        objSBFirst.Append("</li>");


                                        objSBFirst.Append("<li class='FieldCaptionTD' style='display: none' align='left'>");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkF" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' />");
                                        objSBFirst.Append("</li>");

                                        objSBFirst.Append("<li class='FieldCaptionTD' style='display: none' align='left'>");
                                        objSBFirst.Append("<input type='checkbox' id='" + "chkS" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "' />");
                                        objSBFirst.Append("</li>");

                                        objSBFirst.Append("<li class='FieldCaptionTD'>");
                                        objSBFirst.Append("<input type='hidden' id='" + "hSubSubModuleId_" + first.ToString() + "_" + second.ToString() + "_" + third.ToString() + "'  value='" + dtSubSubMenu.Rows[third]["moduleId"].ToString() + "' />");
                                        objSBFirst.Append("</li>");

                                        objSBFirst.Append("</ul>");

                                    }
                                }
                            }
                        }
                    }
                }
                return objSBFirst.ToString();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetAllModulesForRole(int parentId, int choice, int roleId)
        {
            try
            {
                SqlParameter prmParent = SqlHelper.CreateParameter("@parentId", parentId);
                SqlParameter prmChoice = SqlHelper.CreateParameter("@Choice", choice);
                SqlParameter prmRoleId = SqlHelper.CreateParameter("@RoleId", roleId);
                SqlParameter[] allParams = { prmParent, prmChoice, prmRoleId };
                DataSet ds = SqlHelper.ExecuteDataset(_conString, CommandType.StoredProcedure, "Usp_GetRoleModules", allParams);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable Dt = ds.Tables[0].Copy();
                    ds.Dispose();
                    return Dt;
                }
                else
                {
                    return (DataTable)null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static string GetNewCSSClass(string objTitle)
        {
            try
            {
                string strURL = System.Web.HttpContext.Current.Request.RawUrl.ToString();
                string strTemp = strURL.Substring(strURL.LastIndexOf("/") + 1);
                if (strTemp.IndexOf("&") > 0)
                {
                    strTemp = strTemp.Substring(0, strTemp.IndexOf("&"));
                }
                strTemp = strTemp.ToUpper();
                if (((objTitle.IndexOf(strTemp) != -1) && (strTemp.IndexOf("?") != -1)) && !isSelected)
                {
                    isSelected = true;
                    return "class=\"selected\"";
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                throw;
            }
        }
        public static string GetCSSClass(string objTitle)
        {
            try
            {
                string strURL = System.Web.HttpContext.Current.Request.RawUrl.ToString();
                string strTemp = strURL.Substring(strURL.LastIndexOf("/") + 1);
                //strTemp = strTemp.Substring(0, strTemp.LastIndexOf("."));
                strTemp = strTemp.ToUpper();
                if (objTitle.IndexOf(strTemp) != -1 && !isSelected)
                {
                    isSelected = true;
                    return "class=\"selected\"";
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                throw;
            }
        }


        public static string GetLeftPanelIconInAdminPanel(string title)
        {
            string classSetting = "";
            switch (title.ToLower())
            {
                case "admin user": classSetting = "class=\"fa fa-fw fa-user-plus\"";
                    break;
                case "manage admin user": classSetting = "class=\"fa fa-fw fa-user-plus\"";
                    break;
                case "manage clients": classSetting = "class=\"fa fa-fw fa-users\"";
                    break;
                case "manage user": classSetting = "class=\"fa fa-fw fa-users\"";
                    break;
                case "manage master": classSetting = "class=\"fa fa-fw fa-object-group\"";
                    break;
                case "manage cms": classSetting = "class=\"fa fa-fw fa-sticky-note\"";
                    break;
                default: classSetting = "class=\"fa fa-dashboard\"";
                    break;
            }
            return classSetting;
        }
    }
}
