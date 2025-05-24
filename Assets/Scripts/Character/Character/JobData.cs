using UnityEngine;

[CreateAssetMenu(fileName = "JobData", menuName = "RPG/JobData")]
public class JobData : ScriptableObject
{
    public string jobName;
    public int strength;
    public int intelligence;
    public int dexterity;
    public int vitality;
}
