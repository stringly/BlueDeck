using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using OrgChartDemo.Models.ViewModels;

namespace OrgChartDemo.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        public bool PageClassesEnabled { get; set; }
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("ul");
            result.AddCssClass("pagination");
            result.AddCssClass("pagination-sm");
            if (PageModel.TotalPages < 10)
            {
                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    TagBuilder liTag = new TagBuilder("li");
                    TagBuilder tag = new TagBuilder("a");
                    PageUrlValues["page"] = i;
                    tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                    if (PageClassesEnabled)
                    {
                        liTag.AddCssClass(PageClass);
                        liTag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());
                    liTag.InnerHtml.AppendHtml(tag);
                    result.InnerHtml.AppendHtml(liTag);
                }
                output.Content.AppendHtml(result);
            }
            else
            {
                // first page button
                TagBuilder firstLiTag = new TagBuilder("li");
                TagBuilder firstTag = new TagBuilder("a");
                TagBuilder firstFaTag = new TagBuilder("i");
                firstFaTag.AddCssClass("fa");
                firstFaTag.AddCssClass("fa-angle-double-left");
                if (PageModel.CurrentPage == 1)
                {
                    firstLiTag.AddCssClass("disabled");
                }
                else
                {
                    PageUrlValues["page"] = 1;
                    firstTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                }
                firstTag.InnerHtml.AppendHtml(firstFaTag);
                firstLiTag.InnerHtml.AppendHtml(firstTag);
                result.InnerHtml.AppendHtml(firstLiTag);

                // previous page button
                TagBuilder backLiTag = new TagBuilder("li");
                TagBuilder backTag = new TagBuilder("a");
                TagBuilder faTag = new TagBuilder("i");
                faTag.AddCssClass("fa");
                faTag.AddCssClass("fa-angle-left");
                if (PageModel.CurrentPage == 1)
                {
                    backLiTag.AddCssClass("disabled");
                }
                else
                {
                    PageUrlValues["page"] = PageModel.CurrentPage - 1;
                    backTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                }
                backTag.InnerHtml.AppendHtml(faTag);
                backLiTag.InnerHtml.AppendHtml(backTag);
                result.InnerHtml.AppendHtml(backLiTag);
                for (int i = PageModel.CurrentPage - 2; i <= PageModel.CurrentPage + 5; i++)
                {
                    if (i <= 0)
                    {
                        continue;
                    }
                    else if (i > PageModel.TotalPages)
                    {
                        break;
                    }
                    TagBuilder liTag = new TagBuilder("li");
                    TagBuilder tag = new TagBuilder("a");
                    PageUrlValues["page"] = i;
                    tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                    if (PageClassesEnabled)
                    {
                        liTag.AddCssClass(PageClass);
                        liTag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());
                    liTag.InnerHtml.AppendHtml(tag);
                    result.InnerHtml.AppendHtml(liTag);
                }

                // next page button
                TagBuilder nextLiTag = new TagBuilder("li");
                TagBuilder nextTag = new TagBuilder("a");
                TagBuilder nextFaTag = new TagBuilder("i");
                nextFaTag.AddCssClass("fa");
                nextFaTag.AddCssClass("fa-angle-right");
                if (PageModel.CurrentPage == PageModel.TotalPages)
                {
                    nextLiTag.AddCssClass("disabled");
                }
                else
                {
                    PageUrlValues["page"] = PageModel.CurrentPage + 1;
                    nextTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                }
                nextTag.InnerHtml.AppendHtml(nextFaTag);
                nextLiTag.InnerHtml.AppendHtml(nextTag);
                result.InnerHtml.AppendHtml(nextLiTag);

                // last page button
                TagBuilder lastLiTag = new TagBuilder("li");
                TagBuilder lastTag = new TagBuilder("a");
                TagBuilder lastFaTag = new TagBuilder("i");
                lastFaTag.AddCssClass("fa");
                lastFaTag.AddCssClass("fa-angle-double-right");
                if (PageModel.CurrentPage == PageModel.TotalPages)
                {
                    lastLiTag.AddCssClass("disabled");
                }
                else
                {
                    PageUrlValues["page"] = PageModel.TotalPages;
                    lastTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                }
                lastTag.InnerHtml.AppendHtml(lastFaTag);
                lastLiTag.InnerHtml.AppendHtml(lastTag);
                result.InnerHtml.AppendHtml(lastLiTag);


                output.Content.AppendHtml(result);
            }
        }

    }
}
