using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootBallData
{
    public class Championships
    {
        private static Dictionary<string, int> Teams;

        public Championships()
        {
            if (Teams == null || Teams.Count == 0)
                InitializeText();
        }


        private void InitializeText()
        {
            Teams = new Dictionary<string, int>();
            Teams.Add("New England", 1);
            Teams.Add("Cincinnati", 2);
            Teams.Add("Denver", 3);
            Teams.Add("Kansas", 4);
            Teams.Add("New York", 5);
            Teams.Add("Pittsburgh", 6);
            Teams.Add("Houston", 7);
            Teams.Add("Buffalo", 8);
            Teams.Add("Indianapolis", 9);
            Teams.Add("Oakland", 10);
            Teams.Add("Miami", 11);
            Teams.Add("Baltimore", 12);
            Teams.Add("Jacksonville", 13);
            Teams.Add("San Diego", 14);
            Teams.Add("Claveland", 15);
            Teams.Add("Tennessee", 16);            
        }

        public bool DoesTeamExist(string teamName)
        {
            return Teams.Where(x => x.Key.Equals(teamName, StringComparison.InvariantCultureIgnoreCase)).Count() > 0 ? true: false;
            //return Teams.Where(x => x.Key.ToLower() == teamName.ToLower()).Count() > 0 ? true : false;
        }


        public List<string> GetTeams()
        {
            var sorted = Teams.OrderBy(p => p.Value);
            List<string> orderedTeams = new List<string>();
            foreach (var s in sorted)
                orderedTeams.Add(s.Key);

            return orderedTeams;
        }


        public string GetHighestRatedTeam()
        {
            var sorted = Teams.OrderBy(p => p.Value).Select(x => x.Key).FirstOrDefault();
            return sorted.ToString();
        }

        public string GetLowestRatedTeam()
        {
            var sorted = Teams.OrderByDescending(p => p.Value).Select(x => x.Key).FirstOrDefault();
            return sorted.ToString();
        }



        public List<string> GetTopThreeTeams()
        {
            List<string> results = new List<string>();

            var teams = Teams.OrderBy(p => p.Value).Take(3);

            foreach (var team in teams)
            {
                results.Add(team.Key);
            }

            return results;
        }

        public void RemoveTeam(string teamName)
        {
            Teams.Remove(teamName);
        }


        public int GetTeamCount() {
            return Teams.Count;
        }

        public void Reset()
        {
            try
            {
                InitializeText();
            }
            catch (Exception)
            {
                
            }
        }

        public void RemoveText(string name)
        {
            //string Key = "";

            foreach(var key in Teams.Keys)
            {
                if (!string.IsNullOrEmpty(key))
                    Teams.Remove(key);
                //if (key.ToLower().Equals(name.ToLower()))
                //    Key = key;

                //if (!string.IsNullOrEmpty(Key))
                //    Teams.Remove(Key);                
            }
        }

    }
}