using FreshMvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TherapyBoxDemo.Services;

namespace TherapyBoxDemo.PageModels
{
    public class SportsPageModel : FreshBasePageModel
    {
        System.IO.StreamReader reader;
        public SportsPageModel()
        {
            GetCSVFile();
        }
        private async Task GetCSVFile()
        {
            var uri = new Uri(@"http://www.football-data.co.uk/mmz4281/1718/I1.csv");
            var request = (HttpWebRequest)WebRequest.Create(uri);
            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            var responseStream = responseObject.GetResponseStream();
            reader = new StreamReader(responseStream);
            List<SportsData> itemsList = new List<SportsData>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                    itemsList.Add(new SportsData { Div = values[0], Date = values[1], HomeTeam = values[2], AwayTeam = values[3], FTHG = values[4], FTAG = values[5], FTR = values[6], HTHG = values[7], HTAG = values[8], HTR = values[9], HS = values[10], AS = values[11], HST = values[12], AST = values[13], HF = values[14], AF = values[15], HC = values[16], AC = values[17], HY = values[18], AY = values[19], HR = values[20], AR = values[21], B365H = values[22], B365D = values[23], B365A = values[24], BWH = values[25], BWD = values[26], BWA = values[27], IWH = values[28], IWD = values[29], IWA = values[30], LBH = values[31], LBD = values[32], LBA = values[33], PSH = values[34], PSD = values[35], PSA = values[36], WHH = values[37], WHD = values[38], WHA = values[39], VCH = values[40], VCD = values[41], VCA = values[42], Bb1X2 = values[43], BbMxH = values[44], BbAvH = values[45], BbMxD = values[46], BbAvD = values[47], BbMxA = values[48], BbAvA = values[49], BbOU = values[50], BbMxmore25 = values[51], BbAvmore25 = values[52], BbMxless25 = values[53], BbAvless25 = values[54], BbAH = values[55], BbAHh = values[56], BbMxAHH = values[57], BbAvAHH = values[58], BbMxAHA = values[59], BbAvAHA = values[60], PSCH = values[61], PSCD = values[62], PSCA = values[63] });


            }

                TeamList = itemsList;
            List<string> filtered = new List<string>();
            FilteredTeamList = filtered;


        }
        private async Task FilterResults()
        {
            FilteredTeamList = new List<string>();
            var valuehome = TeamList.FindAll(item => item.HomeTeam == TeamNameEntry && item.FTR == "H");
            foreach(var teamawayname in valuehome)
            {
                FilteredTeamList.Add(teamawayname.AwayTeam);
            }
            var valueaway = TeamList.FindAll(item => item.AwayTeam == TeamNameEntry && item.FTR == "A");
            foreach (var teamhomename in valueaway)
            {
                FilteredTeamList.Add(teamhomename.HomeTeam);
            }





        }
        private FreshAwaitCommand _searchCommand;
        public FreshAwaitCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await FilterResults();
                        tcs.SetResult(true);
                    });
                }
                return _searchCommand;
            }

        }
        private List<string> _filteredTeamList;
        public List<string> FilteredTeamList
        {
            get
            {
                return _filteredTeamList;
            }
            set
            {
                _filteredTeamList = value;
                RaisePropertyChanged("FilteredTeamList");
            }
        }
        private List<SportsData> _teamList;
        public List<SportsData> TeamList
        {
            get
            {
                return _teamList;
            }
            set
            {
                _teamList = value;
                RaisePropertyChanged("TeamList");
            }
        }
        private string _teamNameEntry;
        public string TeamNameEntry
        {
            get
            {
                return _teamNameEntry;
            }
            set
            {
                _teamNameEntry = value;
                RaisePropertyChanged("TeamNameEntry");
            }
        }
    }
}
