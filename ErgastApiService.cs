using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace F1Tracker
{
    public class ErgastApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "http://ergast.com/api/f1";

        public async Task<List<DriverBib>> GetAllDriversAsync(int season)
        {
            var driversList = new List<DriverBib>();
            string url = $"{BaseUrl}/{season}/drivers.json";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<DriversResponse>(content);

                    // Transform the data to DriverBib objects
                    foreach (var driver in data.MRData.DriverTable.Drivers)
                    {
                        driversList.Add(new DriverBib
                        {
                            DriverId = driver.DriverId,
                            FullDriverName = $"{driver.FirstName} {driver.LastName}",
                            Nationality = driver.Nationality,
                            DateOfBirth = DateTime.Parse(Convert.ToString(driver.DateOfBirth)),
                            PlaceOfBirth = driver.PlaceOfBirth,
                            WorldChampionships = driver.WorldChampionships,
                            Number = driver.Number
                        });
                    }
                }
                else
                {
                    throw new HttpRequestException($"Failed to load drivers: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return driversList;
        }
        public async Task<TeamBib> GetDriversTeamAsync(string driverId, int season)
        {
            var driversTeam = new TeamBib();
            string url = $"{BaseUrl}/{season}/drivers/{driverId}/constructors.json";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<TeamsResponse>(content);

                var team = data.MRData.ConstructorTable.Constructors[0];

                driversTeam.FullTeamName = team.Name;
                driversTeam.BaseLocation = team.Nationality;
            }
            else
            {
                throw new HttpRequestException($"Failed to load coresponging team: {response.ReasonPhrase}");
            }

            return driversTeam;
        }

        public async Task<int> GetDriversPointsAsync(string driverId, int season)
        {
            int points = 0;
            string url = $"{BaseUrl}/{season}/drivers/{driverId}/results.json";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PointsResponse>(content);

                foreach (var race in data.MRData.RaceTable.Races)
                {
                    foreach (var result in race.Results)
                    {
                        points += result.Points;
                    }
                }
            }
            else
            {
                throw new HttpRequestException($"Failed to load coresponging points: {response.ReasonPhrase}");
            }

            return points;
        }

        public async Task<List<TeamBib>> GetAllTeamsAsync()
        {
            var teamsList = new List<TeamBib>();
            string url = $"{BaseUrl}constructors.json";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<TeamsResponse>(content);

                // Transform the data to TeamBib objects
                foreach (var team in data.MRData.ConstructorTable.Constructors)
                {
                    teamsList.Add(new TeamBib
                    {
                        FullTeamName = team.Name,
                        BaseLocation = team.Nationality,
                        TeamChief = team.TeamChief,
                        TechnicalChief = team.TechnicalChief,
                        Chassis = team.Chassis,
                        PowerUnit = team.PowerUnit
                    });
                }
            }
            else
            {
                throw new HttpRequestException($"Failed to load teams: {response.ReasonPhrase}");
            }

            return teamsList;
        }
    }

    // Response-Klassen müssen der JSON-Struktur der Ergast API entsprechen

    public class DriversResponse
    {
        public MRData MRData { get; set; }
    }

    public class TeamsResponse
    {
        public MRData MRData { get; set; }
    }

    public class PointsResponse
    {
        public MRData MRData { get; set; }
    }

    public class MRData
    {
        public DriverTable DriverTable { get; set; }
        public ConstructorTable ConstructorTable { get; set; }
        
        public RaceTable RaceTable { get; set; }
    }

    public class DriverTable
    {
        public Driver[] Drivers { get; set; }
    }

    public class ConstructorTable
    {
        public Team[] Constructors { get; set; }
    }

    public class RaceTable
    {
        public Race[] Races { get; set; }
    }

    public class Driver
    {
        public string DriverId { get; set; }

        [JsonProperty("givenName")]
        public string FirstName { get; set; }
        [JsonProperty("familyName")]
        public string LastName { get; set; }

        [JsonProperty("permanentNumber")]
        public int Number { get; set; }
        public string Team { get; set; }
        public string Nationality { get; set; }
        public int Podiums { get; set; }
        public double Points { get; set; }
        public int GrandPrixEntries { get; set; }
        public int WorldChampionships { get; set; }
        public int HighestRaceFinish { get; set; }
        public int HighestGridPosition { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int FastestLaps { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string TeamChief { get; set; }
        public string TechnicalChief { get; set; }
        public string Chassis { get; set; }
        public string PowerUnit { get; set; }
        public int FirstTeamEntry { get; set; }
        public int WorldChampionships { get; set; }
        public int HighestRaceFinish { get; set; }
        public int PolePositions { get; set; }
        public int FastestLaps { get; set; }

        // ... Weitere Eigenschaften, falls vorhanden
    }

    public class Race
    {
        public Result[] Results { get; set; } 
    }
    public class Result
    {
        [JsonProperty("points")]
        public int Points { get; set; }
    }
}
