using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public string characterName;
    public string jobName;
    public int level;
    public int currentHp;
    public int maxHp;
    public int attackPower;
    public int defense;
    public float dodgeRate;
    public float attackDelay;
    public float skillCoolTimeDecreaseRate;
    public float moveSpeed;
    public float viewRange;
    public float attackRange;

    public int currentGold;

    public int currentExp;
    public int maxExp;
    
    public float maxExpRateIncreaseRate;

    public void OnEnable()
    {
        currentHp = maxHp;
    }

    public void InitStat(CharacterJobData jobData)
    {
        jobName = jobData.jobName;
        level = jobData.level;
        maxHp = jobData.maxHp;
        attackPower = jobData.attackPower;
        defense = jobData.defense;
        dodgeRate = jobData.dodgeRate;
        attackDelay = jobData.attackDelay;  
        skillCoolTimeDecreaseRate = jobData.skillCoolTimeDecreaseRate;
        moveSpeed = jobData.moveSpeed;
        viewRange = jobData.viewRange;
        attackRange = jobData.attackRange;
        currentGold = jobData.currentGold;
        currentExp = jobData.currentExp;
        maxExp = jobData.maxExp;
        maxExpRateIncreaseRate = jobData.maxExpRateIncreaseRate;
    }

    
}
