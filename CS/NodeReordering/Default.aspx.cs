using DevExpress.Web.ASPxTreeList;
using NodeReordering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NodeReordering
{
    public partial class Default : System.Web.UI.Page
    {
        DataHelper dataHelper = new DataHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            TreeList.DataSource = dataHelper.Data;
            TreeList.DataBind();
            TreeList.ClientSideEvents.Init = string.Format(
                "function (s, e) {{ s.reorderHelper = new ReorderHelper(s, '{0}', '{1}'); }}",
                Page.ResolveClientUrl("~/Content/Images/004_54.gif"),
                Page.ResolveClientUrl("~/Content/Images/004_32.gif")
                );
        }

        protected void TreeList_DataBinding(object sender, EventArgs e)
        {

        }

        protected void TreeList_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            string[] callbackParams = e.Argument.Split('|');
            bool isSwap = false;
            int draggedKey = -1;
            int targetKey = -1;
            if (callbackParams.Length == 3 &&
                Int32.TryParse(callbackParams[0], out draggedKey) &&
                Int32.TryParse(callbackParams[1], out targetKey) &&
                Boolean.TryParse(callbackParams[2], out isSwap) &&
                isSwap)
            {
                dataHelper.SwapDataItems(dataHelper.Data.Find(i => i.Key == draggedKey), dataHelper.Data.Find(i => i.Key == targetKey));

                TreeList.DataSource = dataHelper.Data;
                ((ASPxTreeList)sender).DataBind();
            }
        }

        protected void TreeList_CustomJSProperties(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomJSPropertiesEventArgs e)
        {
            ASPxTreeList tree = (ASPxTreeList)sender;
            e.Properties["cpSiblingKeys"] = tree.GetAllNodes().ToDictionary(n => n.Key, n => n.ParentNode.ChildNodes.OfType<TreeListNode>().Select(c => c.Key));
        }

        protected void TreeList_ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
        {
            if (e.Node != null && e.NewParentNode != null)
            {
                SampleDataItem child = dataHelper.Data.Find(i => i.Key == Convert.ToInt32(e.Node.Key));
                SampleDataItem newParent = dataHelper.Data.Find(i => i.Key == Convert.ToInt32(e.NewParentNode.Key));
                child.ParentKey = newParent.Key;

                TreeList.DataSource = dataHelper.Data;
                ((ASPxTreeList)sender).DataBind();
                e.Handled = true;
            }
            
        }
    }
}