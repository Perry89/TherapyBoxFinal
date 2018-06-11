using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using CodeHollow.FeedReader;
using FreshMvvm;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite;
using TherapyBoxDemo.Models;
using TherapyBoxDemo.Services;
using Xamarin.Forms;
using static Newtonsoft.Json.JsonConvert;

namespace TherapyBoxDemo.PageModels
{
    public class MainPageModel : FreshBasePageModel
    {
        public enum Units
        {
            Imperial,
            Metric
        }

        const string NewsFeed = "http://feeds.bbci.co.uk/news/rss.xml";
        const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=fc9f6c524fc093759cd28d41fda89a1b";
		private List<TaskItems> list;
		const string cmdText = "Select * FROM sqlite_master WHERE type = 'table' AND name = ?";
        public MainPageModel()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                GetCurrentPosition();
                UpdateNewsFeed();
				PopulateTaskList();
                
               
            });


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
        private double _lat;
        public double lat
        {
            get
            {
                return _lat;
            }
            set
            {
                _lat = value;
                RaisePropertyChanged("lat");
            }
        }
        private double _lon;
        public double lon
        {
            get
            {
                return _lon;
            }
            set
            {
                _lon = value;
                RaisePropertyChanged("lon");
            }
        }
                private bool _taskToggled;
        public bool TaskToggled
        {
            get
            {
                return _taskToggled;
            }
            set
            {
                _taskToggled = value;
                RaisePropertyChanged("TaskToggled");
            }
        }

        private string _weatherImageSource;
        public string WeatherImageSource
        {
            get
            {
                return _weatherImageSource;
            }
            set
            {
                _weatherImageSource = value;
                RaisePropertyChanged("WeatherImageSource");
            }
        }
        private string _temperature;
        public string Temperature
        {
            get
            {
                return _temperature + " degrees";
            }
            set
            {
                _temperature = value;
                RaisePropertyChanged("Temperature");
            }
        }
        private string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                RaisePropertyChanged("Location");
            }
        }
		private List<TaskItems> _taskList;
        public List<TaskItems> TaskList
        {
            get
            {
                return _taskList;
            }
            set
            {
                _taskList = value;
                RaisePropertyChanged("TaskList");
            }
        }
        #region Commands

        private FreshAwaitCommand _newsCommand;
        public FreshAwaitCommand NewsCommand
        {
            get
            {
                if (_newsCommand == null)
                {
                    _newsCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await NewsCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _newsCommand;
            }

        }

        private FreshAwaitCommand _sportCommand;
        public FreshAwaitCommand SportCommand
        {
            get
            {
                if (_sportCommand == null)
                {
                    _sportCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await SportsCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _sportCommand;
            }

        }

        private FreshAwaitCommand _photoCommand;
        public FreshAwaitCommand PhotoCommand
        {
            get
            {
                if (_photoCommand == null)
                {
                    _photoCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await PhotoCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _photoCommand;
            }

        }

        private FreshAwaitCommand _taskCommand;
        public FreshAwaitCommand TaskCommand
        {
            get
            {
                if (_taskCommand == null)
                {
                    _taskCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await TaskCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _taskCommand;
            }

        }
        private FreshAwaitCommand _clothesCommand;
        public FreshAwaitCommand ClothesCommand
        {
            get
            {
                if (_clothesCommand == null)
                {
                    _clothesCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await ClothesCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _clothesCommand;
            }

        }
        #endregion
        #region Private Methods
        public async Task UpdateNewsFeed()
        {

            var httpClient = new HttpClient();
            var feed = NewsFeed;
            var responseString = await httpClient.GetStringAsync(feed);

            var items  = await GetNewsFeed(responseString);
            var item = items.FirstOrDefault();
            if(item.description.Length > 10)
            {
                var shortdesc = item.description;
                shortdesc.Substring(0, 10);
                Description = shortdesc + "...";
            }
            else
            {
                Description = item.description;
            }
            
            if (item.title.Length > 2)
            {
                var shortTitle = item.title;
                shortTitle.Substring(0, 3);
                Title = shortTitle + "...";
            }
            else
            {
                Title = item.title;
            }
            


        }
		public async Task<List<TaskItems>> PopulateTaskList()
		{
			    return await Task.Run(() =>
				{
					list = new List<TaskItems>();
					string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "task.db3");
					var db = new SQLiteConnection(dpPath);
					var cmd = db.CreateCommand(cmdText, typeof(TaskTable).Name);
					var result = cmd.ExecuteScalar<string>();
					if (result == null)
					{
						list.Add(new TaskItems { TaskName = "", TaskToggle = false });
						TaskList = list;
						return TaskList;
					}
					else
					{
						var data = db.Table<TaskTable>();
						int counter = 0;
						foreach (var item in data)
						{
							while (counter < 4)
							{
								counter++;

								if (item.TaskToggle == 0)
								{
									TaskToggled = false;
								}
								else
								{
									TaskToggled = true;
								}
								list.Add(new TaskItems { TaskName = item.TaskName, TaskToggle = TaskToggled });
							}
						}
					    TaskList = list;
						return TaskList;
					}
				});
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
                        }).ToList();
            });


        }
        public async Task<Position> GetCurrentPosition()
        {
            Position position = null;
            WeatherRoot weatherRoot = new WeatherRoot();
            var units = Units.Imperial;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;


                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
               weatherRoot = await GetWeather(position.Latitude, position.Longitude, Units.Imperial);
                Location = weatherRoot.Name;
                Temperature = weatherRoot.DisplayTemp;
                var rain = weatherRoot.MainWeather.Humidity;
                var weather = weatherRoot.Clouds;
                if(weather.CloudinessPercent < 20)
                {
                    WeatherImageSource = "Sun_icon.png";
                }
                else if (rain > 80)
                {
                    WeatherImageSource = "Rain_icon";
                }
                else
                {
                    WeatherImageSource = "Clouds_icon.png";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return position;
        }
        public async Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCoordinatesUri, latitude, longitude, units.ToString().ToLower());
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherRoot>(json);
            }

        }

        private async Task ClothesCommandHandler()
		{
			await CoreMethods.PushPageModel<ClothesPageModel>();
		}
		private async Task TaskCommandHandler()
        {
            await CoreMethods.PushPageModel<TaskPageModel>();
        }
		private async Task PhotoCommandHandler()
        {
            await CoreMethods.PushPageModel<PicturesPageModel>();
        }
		private async Task SportsCommandHandler()
        {
            await CoreMethods.PushPageModel<SportsPageModel>();
        }
		private async Task NewsCommandHandler()
        {
            await CoreMethods.PushPageModel<NewsPageModel>();
        }
        #endregion
	}
}

