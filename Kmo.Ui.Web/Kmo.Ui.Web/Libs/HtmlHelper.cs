using Kmo.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kmo
{
    public static class HtmlHelperExtension
    {
        public static IHtmlString BuildMenu(this HtmlHelper helper, List<MenuNodes> nodes)
        {
            
            string output = "";
            foreach (var item in nodes)
            {
                var nodeList = new TagBuilder("li");
                nodeList.AddCssClass("nav-item");

                var listAtribute = new TagBuilder("a");
                
                listAtribute.MergeAttribute("href", item.IsFolder ? "" : (HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + item.Path));
                listAtribute.AddCssClass("nav-link");
                listAtribute.AddCssClass("nav-toggle");

                var nodeIcon = new TagBuilder("i");
                nodeIcon.AddCssClass(item.Icon);

                var nodeTitle = new TagBuilder("span");
                nodeTitle.AddCssClass("title");
                nodeTitle.SetInnerText(" " + item.Name);

                listAtribute.InnerHtml = nodeIcon.ToString();
                listAtribute.InnerHtml += nodeTitle.ToString();

                if (item.IsFolder)
                {
                    var nodeArrow = new TagBuilder("span");
                    nodeArrow.AddCssClass("arrow");

                    listAtribute.InnerHtml += nodeArrow.ToString();
                }

                nodeList.InnerHtml = listAtribute.ToString();

                if (item.ChildNodes.Count > 0)
                {
                    var subNode = new TagBuilder("ul");
                    subNode.AddCssClass("sub-menu");
                    subNode.InnerHtml = BuildSubMenu(item.ChildNodes);

                    nodeList.InnerHtml += subNode.ToString();
                }

                output += nodeList.ToString();
            }


            return helper.Raw(output);
        }

        private static string BuildSubMenu(List<MenuNodes> nodes)
        {
            string output = "";
            foreach (var item in nodes)
            {
                var nodeList = new TagBuilder("li");
                nodeList.AddCssClass("nav-item");

                var listAtribute = new TagBuilder("a");
                listAtribute.MergeAttribute("href", item.IsFolder ? "" : (HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + item.Path));
                listAtribute.AddCssClass("nav-link");
                listAtribute.AddCssClass("nav-toggle");

                var nodeIcon = new TagBuilder("i");
                nodeIcon.AddCssClass(item.Icon);

                var nodeTitle = new TagBuilder("span");
                nodeTitle.AddCssClass("title");
                nodeTitle.SetInnerText(" " + item.Name);

                listAtribute.InnerHtml = nodeIcon.ToString();
                listAtribute.InnerHtml += nodeTitle.ToString();

                if (item.IsFolder)
                {
                    var nodeArrow = new TagBuilder("span");
                    nodeArrow.AddCssClass("arrow");

                    listAtribute.InnerHtml += nodeArrow.ToString();
                }

                nodeList.InnerHtml = listAtribute.ToString();

                if (item.ChildNodes.Count > 0)
                {
                    var subNode = new TagBuilder("ul");
                    subNode.AddCssClass("sub-menu");
                    subNode.InnerHtml = BuildSubMenu(item.ChildNodes);

                    nodeList.InnerHtml += subNode.ToString();
                }

                output += nodeList.ToString();
            }
            return output;
        }

        public static string template1 = "";
    }
}