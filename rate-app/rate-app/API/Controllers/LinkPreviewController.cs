using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace rate_app.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkPreviewController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LinkPreviewController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetLinkPreview([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL parameter is required");
            }

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; LinkPreviewAPI/1.0)");

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                var baseUri = new Uri(url);

                var preview = new
                {
                    Title = GetTitle(doc),
                    Description = GetDescription(doc),
                    Image = GetImage(doc, baseUri),
                    Favicon = GetFavicon(doc, baseUri)
                };

                return Ok(preview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to fetch link preview: {ex.Message}" });
            }
        }

        private string GetTitle(HtmlDocument doc)
        {
            var ogTitle = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(ogTitle))
                return ogTitle;

            var twitterTitle = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:title']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(twitterTitle))
                return twitterTitle;

            var metaTitle = doc.DocumentNode.SelectSingleNode("//meta[@name='title']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(metaTitle))
                return metaTitle;

            return doc.DocumentNode.SelectSingleNode("//title")?.InnerText?.Trim();
        }

        private string GetDescription(HtmlDocument doc)
        {
            var ogDescription = doc.DocumentNode.SelectSingleNode("//meta[@property='og:description']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(ogDescription))
                return ogDescription;

            var twitterDescription = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:description']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(twitterDescription))
                return twitterDescription;

            return doc.DocumentNode.SelectSingleNode("//meta[@name='description']")?.GetAttributeValue("content", null);
        }

        private string GetImage(HtmlDocument doc, Uri baseUri)
        {
            var ogImage = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(ogImage))
                return ResolveUrl(ogImage, baseUri);

            var twitterImage = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:image']")?.GetAttributeValue("content", null);
            if (!string.IsNullOrEmpty(twitterImage))
                return ResolveUrl(twitterImage, baseUri);

            var img = doc.DocumentNode.SelectNodes("//img")
                ?.Where(i => !string.IsNullOrEmpty(i.GetAttributeValue("src", null)))
                ?.FirstOrDefault(i =>
                {
                    var src = i.GetAttributeValue("src", "");
                    var width = i.GetAttributeValue("width", "0");
                    var height = i.GetAttributeValue("height", "0");

                    int.TryParse(width, out int w);
                    int.TryParse(height, out int h);

                    return (w >= 100 && h >= 100) || (string.IsNullOrEmpty(width) && string.IsNullOrEmpty(height) && !src.Contains("icon") && !src.Contains("logo"));
                });

            return img != null ? ResolveUrl(img.GetAttributeValue("src", null), baseUri) : null;
        }

        private string GetFavicon(HtmlDocument doc, Uri baseUri)
        {
            var iconSelectors = new[]
            {
                "//link[contains(@rel, 'icon')][@href]",
                "//link[@rel='shortcut icon'][@href]",
                "//link[@rel='apple-touch-icon'][@href]",
                "//link[@rel='apple-touch-icon-precomposed'][@href]"
            };

            foreach (var selector in iconSelectors)
            {
                var iconLink = doc.DocumentNode.SelectSingleNode(selector);
                if (iconLink != null)
                {
                    var href = iconLink.GetAttributeValue("href", null);
                    if (!string.IsNullOrEmpty(href))
                    {
                        return ResolveUrl(href, baseUri);
                    }
                }
            }

            return new Uri(baseUri, "/favicon.ico").ToString();
        }

        private string ResolveUrl(string url, Uri baseUri)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            if (Uri.TryCreate(url, UriKind.Absolute, out _))
                return url;

            if (url.StartsWith("//"))
                return $"{baseUri.Scheme}:{url}";

            return new Uri(baseUri, url).ToString();
        }
    }
}
