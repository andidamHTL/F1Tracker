using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1Tracker
{
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    //Fahrer-Infos
    public class DriverBib : INotifyPropertyChanged
    {
        public string DriverId { get; set; }
        public string FullDriverName { get; set; }
        public string Nationality { get; set; }
        private TeamBib _team;

        public TeamBib Team
        {
            get { return _team; }
            set 
            { 
                _team = value; 
                OnPropertyChanged();
            }
        }

        public int Podiums { get; set; }

        private int _points;

        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }
        public int Number { get; set; }
        public int WorldChampionships { get; set; }
        public int HighestRaceFinish { get; set; }
        public int HighestGridPosition { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }
        public int FastestLaps { get; set; }

        public int Age 
        {
            get { return GetAge(DateOfBirth); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }

    //Team-Infos
    public class TeamBib
    {
        public string FullTeamName { get; set; }
        public string BaseLocation { get; set; }
        public string TeamChief { get; set; }
        public string TechnicalChief { get; set; }
        public string Chassis { get; set; }
        public string PowerUnit { get; set; }
        public int FirstTeamEntry { get; set; }
        public int WorldChampionships { get; set; }
        public int HighestRaceFinish { get; set; }
        public int PolePositions { get; set; }
        public int FastestLaps { get; set; }


        public override string ToString()
        {
            return FullTeamName;
        }
        //Konstruktor
        public TeamBib(string fullTeamName, string baseLocation, string teamChief, string technicalChief, string chassis, string powerUnit)
        {
            FullTeamName = fullTeamName;
            BaseLocation = baseLocation;
            TeamChief = teamChief;
            TechnicalChief = technicalChief;
            Chassis = chassis;
            PowerUnit = powerUnit;
        }
        public TeamBib()
        {
            
        }
    }

    public class DriversBib
    {
        public List<DriverBib> Drivers { get; set; } = new List<DriverBib>();

        public void AddDriver(DriverBib driver)
        {
            Drivers.Add(driver);
        }

        public bool RemoveDriver(string fullName)
        {
            var driver = Drivers.FirstOrDefault(d => d.FullDriverName == fullName);
            return driver != null && Drivers.Remove(driver);
        }

        public DriverBib FindDriver(string fullName)
        {
            return Drivers.FirstOrDefault(d => d.FullDriverName == fullName);
        }

        public List<DriverBib> ListAllDrivers()
        {
            return Drivers;
        }
    }
    public class TeamsBib
    {
        public List<TeamBib> Teams { get; set; } = new List<TeamBib>();

        public void AddTeam(TeamBib team)
        {
            Teams.Add(team);
        }

        public bool RemoveTeam(string teamName)
        {
            var team = Teams.FirstOrDefault(t => t.FullTeamName == teamName);
            return team != null && Teams.Remove(team);
        }

        public TeamBib FindTeam(string teamName)
        {
            return Teams.FirstOrDefault(t => t.FullTeamName == teamName);
        }

        public List<TeamBib> ListAllTeams()
        {
            return Teams;
        }
    }
}
