using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField] private List<Slime> slimeTeam;
    [SerializeField] private Vector2[] slimeTeamPos;
    private int maxTeamMembers = 4;

    public List<Slime> SlimeTeam { get => slimeTeam; }

    public int GetTeamMemberCount()
    {
        return slimeTeam.Count;
    }

    public void AddTeamMember(Slime slime)
    {
        if (GetTeamMemberCount() == maxTeamMembers)
        {
            return;
        }

        slimeTeam.Add(slime);
    }

    public void RemoveTeamMember(Slime slime)
    {
        if (GetTeamMemberCount() == 0)
        {
            return;
        }

        slimeTeam.Remove(slime);
    }

    public void PlaceTeamMembers() // Invoke before start of the wave
    {
        int i = 0;
        foreach (Slime supportSlime in slimeTeam)
        {
            supportSlime.gameObject.transform.position = new Vector3(slimeTeamPos[i].x, slimeTeamPos[i].y, 0);
            supportSlime.ResetVitals();
            i++;
        }
    }
}
