using UnityEngine;
using System.Collections.Generic;

public class ProcessSOTG : MonoBehaviour
{
    public static ProcessSOTG PS;

    private List<Team> teams;

    public struct Team
    {
        public string name;
        public List<Score> scores;

        public float Avg()
        {
            int sum = 0;
            for (int i = 0; i < scores.Count; i++)
            {
                sum += scores[i].Sum();
            }
            return (float)sum / scores.Count;
        }
    }

    public struct Score
    {
        public string time;
        public string scoringTeam;
        public int[] partials;
        public string comment;

        public int Sum()
        {
            int s = 0;
            foreach (int p in partials)
                s += p;
            return s;
        }
    }

    private void Awake()
    {
        PS = this;
    }

    private void Start()
    {
        ClearData();
    }

    public void ClearData()
    {
        teams = new List<Team>();
    }

    public void ProcessLineFromCSV(List<string> currLineElements, int currLineIndex)
    {
        if (currLineIndex == 0) return; //Ignore Header

        string currentTerm = string.Empty;
        string currentTeam = string.Empty;

        if (currLineElements.Count > 1)
        {
            currentTeam = currLineElements[2];

            if (!TeamExists(currentTeam))
            {
                teams.Add(new Team
                {
                    name = currentTeam,
                    scores = new List<Score>()
                });
            }

            Team t = teams[TeamIndex(currentTeam)];
            t.scores.Add(new Score()
            {
                time = currLineElements[0],
                scoringTeam = currLineElements[1],
                partials = new int[] { int.Parse(currLineElements[3]), int.Parse(currLineElements[4]), int.Parse(currLineElements[5]), int.Parse(currLineElements[6]), int.Parse(currLineElements[7]) },
                comment = currLineElements[9]
            });
        }
        else
        {
            Debug.LogError("Database line did not fall into one of the expected categories.");
        }
    }

    private int TeamIndex(string _name)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].name == _name) return i;
        }

        Debug.LogError("Team not found");
        return -1;
    }

    private bool TeamExists(string _name)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].name == _name) return true;
        }

        return false;
    }

    public int GetNTeams()
    {
        return teams.Count;
    }

    public Team GetTeam(int _index)
    {
        return teams[_index];
    }
}
