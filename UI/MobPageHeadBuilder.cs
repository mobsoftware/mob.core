using Nop.Core.Domain.Seo;
using Nop.Web.Framework.UI;

namespace Mob.Core.UI
{
    public class MobPageHeadBuilder : PageHeadBuilder
    {
        #region fields

        private string _ogTitle;
        private string _ogDescription;
        private string _ogUrl;
        private string _ogType;
        private string _ogImageUrl;

        #endregion

        #region ctor
        public MobPageHeadBuilder(SeoSettings seoSettings) : base(seoSettings)
        {

        }
        #endregion

        #region methods

        public void SetOpenGraphTags(string Title, string Description, string Url, string Type, string ImageUrl)
        {
            _ogTitle = Title;
            _ogDescription = Description;
            _ogUrl = Url;
            _ogType = Type;
            _ogImageUrl = ImageUrl;
        }

        public string GetOpenGraphTitle()
        {
            return _ogTitle;
        }

        public string GetOpenGraphDescription()
        {
            return _ogDescription;

        }
        public string GetOpenGraphUrl()
        {
            return _ogUrl;

        }
        public string GetOpenGraphType()
        {
            return _ogType;

        }
        public string GetOpenGraphImageUrl()
        {
            return _ogImageUrl;

        }
        #endregion
    }
}