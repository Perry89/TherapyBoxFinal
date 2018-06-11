using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TherapyBoxDemo.Services
{
    public class ClothesData
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("date")]
        public string date { get; set; }

        [JsonProperty("clothe")]
        public string clothe { get; set; }
    }
    public class RootObject
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("payload")]
        public ObservableCollection<ClothesData> payload { get; set; }

    }
}
