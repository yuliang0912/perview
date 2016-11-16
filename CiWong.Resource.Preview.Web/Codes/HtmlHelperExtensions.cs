using System;
using System.Web.Mvc;
using System.Text;
using CiWong.Resource.Preview.Common;

namespace CiWong.Resource.Preview.Web
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Scripts(this HtmlHelper helper, string directoryName, params string[] files)
        {
            Func<string, string> buildStyleUrl = (fileName) =>
            {
                return BuildUrl(directoryName, fileName);
            };

            return Scripts(helper, buildStyleUrl, files);
        }

        public static MvcHtmlString Styles(this HtmlHelper helper, string directoryName, params string[] files)
        {
            Func<string, string> buildStyleUrl = (fileName) =>
            {
                return BuildUrl(directoryName, fileName);
            };

            return Styles(helper, buildStyleUrl, files);
        }

		public static MvcHtmlString Style(this HtmlHelper helper, string directoryName, string file, string otherAttr)
		{
			return new MvcHtmlString(string.Format("<link href=\"{0}\" {1} rel=\"stylesheet\" />", BuildUrl(directoryName, file), otherAttr));
		}

        private static MvcHtmlString Scripts(this HtmlHelper helper, Func<string, string> buildStyleUrl, params string[] files)
        {
            return RenderFiles(helper, "<script src=\"{0}\" type=\"text/javascript\"></script>\n", buildStyleUrl, files);
        }

        private static MvcHtmlString Styles(this HtmlHelper helper, Func<string, string> buildStyleUrl, params string[] files)
        {
            return RenderFiles(helper, "<link href=\"{0}\" rel=\"stylesheet\" />\n", buildStyleUrl, files);
        }

        private static MvcHtmlString RenderFiles(this HtmlHelper helper, string tagTemplate, Func<string, string> buildUrl, string[] files = null)
        {
            if (files == null || files.Length == 0 || buildUrl == null)
            {
                return new MvcHtmlString(string.Empty);
            }

            var builder = new StringBuilder();
            foreach (var file in files)
            {
                builder.AppendFormat(tagTemplate, buildUrl(file));
            }
            return new MvcHtmlString(builder.ToString().TrimEnd('\n'));
        }

        private static string BuildUrl(string directoryName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            var url = SiteSettings.Instance.Style.Domain;
            if (!string.IsNullOrWhiteSpace(directoryName))
            {
                url += "/" + directoryName.TrimStart('/');
            }

            return url.TrimEnd('/') + "/" + fileName;
        }
    }
}