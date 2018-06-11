using System;
using System.Collections.Generic;
using System.Text;

namespace TherapyBoxDemo.Models
{
    public class NewsItem
    {
        private string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        private string _description;
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        private string _media;
        public string media
        {
            get
            {   
                return _media;
            }
            set
            {
                _media = value;
            }
        }
    }
}
