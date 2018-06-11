using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using FreshMvvm;
using TherapyBoxDemo.Models;
using Xamarin.Forms;

namespace TherapyBoxDemo.PageModels
{
	public class NewsPageModel : FreshBasePageModel
    {
        const string NewsFeed = "http://feeds.bbci.co.uk/news/rss.xml";
        XNamespace media = XNamespace.Get("http://search.yahoo.com/mrss/");
        public NewsPageModel()
        {
            UpdateNewsFeed();

        }
        public async Task UpdateNewsFeed()
        {

            var httpClient = new HttpClient();
            var feed = NewsFeed;
            var responseString = await httpClient.GetStringAsync(feed);

            var items = await GetNewsFeed(responseString);
            var item = items.FirstOrDefault();
            var imageSource = item.media;
            Title = item.title;
            Description = item.description;
            Uri uri = new Uri(imageSource);
            ImageSource = uri;






        }

        public async Task<List<NewsItem>> GetNewsFeed(string rss)
        {
            return await Task.Run(() =>
            {
                var xdoc = XDocument.Parse(rss);
                var id = 0;
                return (from item in xdoc.Descendants("item")
                        select new NewsItem
                        {
                            title = (string)item.Element("title"),
                            description = (string)item.Element("description"),
                            media = (string)item.Element(media + "thumbnail") != null ? item.Element(media + "thumbnail").Attribute("url").Value : ""
                        }).ToList();
            });


        }
        private Uri _imageSource;
        public Uri ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                RaisePropertyChanged("ImageSource");
            }

        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
    }
}

