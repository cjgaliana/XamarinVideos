using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Tools.Evolve2015Extractor
{
    public static class ExtractorHelper
    {
        public static List<EvolveSession> ExtractSessions(string html)
        {
            var sessionList = new List<EvolveSession>();
            var items = ExtractMainItems(html);

            foreach (var item in items)
            {
                var session = ExtractSessionData(item);
                sessionList.Add(session);
            }

            return sessionList;
        }

        private static EvolveSession ExtractSessionData(string item)
        {
            var evolveSession = new EvolveSession
            {
                Title = ExtractTitle(item),
                Author = ExtractAuthor(item),
                Description = ExtractDescription(item),
                Thumbnail = ExtractThumbnail(item),
                Track = ExtractTrack(item),
                YoutubeID = ExtractYoutubeID(item)
            };

            return evolveSession;
        }

        private static string ExtractYoutubeID(string item)
        {
            var document = new HtmlDocument();
            document.LoadHtml(item);

            var youtubeId =
                document.DocumentNode.Descendants()
                    .FirstOrDefault(d => d.GetAttributeValue("class", "") == "panel panel-default");

            return youtubeId != null
                ? youtubeId.GetAttributeValue("data-youtubeid", null)
                : string.Empty;
        }

        private static string ExtractTrack(string item)
        {
            var document = new HtmlDocument();
            document.LoadHtml(item);

            var track =
                document.DocumentNode.Descendants()
                    .FirstOrDefault(d => d.GetAttributeValue("class", "") == "panel panel-default");

            return track != null
                ? track.GetAttributeValue("data-track", null)
                : string.Empty;
        }

        private static string ExtractThumbnail(string item)
        {
            var document = new HtmlDocument();
            document.LoadHtml(item);

            var imageSrc =
                document.DocumentNode.Descendants()
                    .FirstOrDefault(d => d.GetAttributeValue("class", "") == "img-thumbnail");

            return imageSrc != null
                ? imageSrc.GetAttributeValue("data-src", null)
                : string.Empty;
        }

        private static string ExtractDescription(string item)
        {
            var document = new HtmlDocument();
            document.LoadHtml(item);

            var span = document.DocumentNode
                .Descendants()
                .FirstOrDefault(d => d.GetAttributeValue("class", "") == "cell width-8");

            if (span != null)
            {
                var description = span.Descendants().FirstOrDefault(x => x.Name == "p");
                if (description != null)
                {
                    return description.InnerText;
                }
            }

            return string.Empty;
        }

        private static string ExtractAuthor(string item)
        {
            var document = new HtmlDocument();
            document.LoadHtml(item);

            var authorTag =
                document.DocumentNode.Descendants().Where(d => d.GetAttributeValue("class", "") == "show-on-mobile");
            var author = authorTag.FirstOrDefault();
            return author != null
                ? author.InnerText
                : string.Empty;
        }

        private static string ExtractTitle(string item)
        {
            var document = new HtmlDocument();
            document.LoadHtml(item);

            var div =
                document.DocumentNode.Descendants()
                    .Where(d => d.GetAttributeValue("class", "") == "panel panel-default");
            var title = div.FirstOrDefault();
            return title != null
                ? title.GetAttributeValue("data-title", null)
                : string.Empty;
        }

        private static IEnumerable<string> ExtractMainItems(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var rawItems =
                document.DocumentNode.Descendants()
                    .Where(d => d.GetAttributeValue("class", "") == "panel panel-default");

            return rawItems.Select(x => x.OuterHtml);
        }
    }
}