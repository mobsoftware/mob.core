using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.UI;

namespace Mob.Core.UI
{
    public static class LayoutExtensions
    {
        /// <summary>
        /// Set the open graph tags
        /// </summary>
        /// <param name="html">HTML helper</param>
        public static void SetOpenGraphTags(this HtmlHelper html, string Title, string Description, string Url, string Type, string ImageUrl)
        {
            //http://ogp.me/#types for all types
            var pageHeadBuilder = EngineContext.Current.Resolve<MobPageHeadBuilder>();
            pageHeadBuilder.SetOpenGraphTags(Title, Description, Url, Type, ImageUrl);
        }

        /// <summary>
        /// Gets open graph title
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns>open graph title</returns>
        public static MvcHtmlString MobOpenGraphTitle(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<MobPageHeadBuilder>();
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GetOpenGraphTitle()));
        }

        /// <summary>
        /// Gets open graph description
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns>open graph description</returns>
        public static MvcHtmlString MobOpenGraphDescription(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<MobPageHeadBuilder>();
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GetOpenGraphDescription()));
        }

        /// <summary>
        /// Gets open graph type
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns>open graph type</returns>
        public static MvcHtmlString MobOpenGraphType(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<MobPageHeadBuilder>();
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GetOpenGraphType()));
        }
        /// <summary>
        /// Get open graph url
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns>open graph url</returns>
        public static MvcHtmlString MobOpenGraphUrl(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<MobPageHeadBuilder>();
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GetOpenGraphUrl()));
        }

        /// <summary>
        /// Get open graph image url
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns>open graph image url</returns>
        public static MvcHtmlString MobOpenGraphImageUrl(this HtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<MobPageHeadBuilder>();
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GetOpenGraphImageUrl()));
        }

    }
}