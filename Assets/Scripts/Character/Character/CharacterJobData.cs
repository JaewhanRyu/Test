using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatData", menuName = "RPG/CharacterStatData")]
public class CharacterJobData : ScriptableObject
{
    public string jobName;
    public int level = 1;
    public int maxHp;
    public int attackPower;
    public int defense;
    public float dodgeRate;
    public float attackDelay = 1f;
    public float skillCoolTimeDecreaseRate;
    public float moveSpeed = 2f;
    public float viewRange = 1.5f;
    public float attackRange = 1f;

    public int currentGold = 0;

    public int currentExp = 0;
    public int maxExp = 50;
    
    public float maxExpRateIncreaseRate = 1.2f;
}
