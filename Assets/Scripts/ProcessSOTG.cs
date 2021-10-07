using UnityEngine;
using System.Collections.Generic;

public class ProcessSOTG : MonoBehaviour
{
    public static ProcessSOTG PS;

    private List<Team> teams;

    private struct Team
    {
        public string name;
        public List<int> scores;

        public float Avg()
        {
            int sum = 0;
            for (int i = 0; i < scores.Count; i++)
            {
                sum += scores[i];
            }
            return (float)sum / scores.Count;
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
        string currentTerm = string.Empty;
        string currentTeam = string.Empty;

        if (currLineElements.Count > 1)
        {
            //Ignore Header
            if (currLineIndex > 1)
                for (int columnIndex = 0; columnIndex < currLineElements.Count; columnIndex++)
                {
                    currentTerm = currLineElements[columnIndex];

                    switch (columnIndex)
                    {
                        case 2:
                            currentTeam = currentTerm;

                            if (!TeamExists(currentTeam))
                            {
                                teams.Add(new Team
                                {
                                    name = currentTeam,
                                    scores = new List<int>()
                                });
                            }
                            break;
                        case 8:
                            teams[TeamIndex(currentTeam)].scores.Add(int.Parse(currentTerm));
                            break;
                    }
                }
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

    public string GetTeam(int _index)
    {
        return teams[_index].name + " " + teams[_index].Avg().ToString("0.00");
    }
}
