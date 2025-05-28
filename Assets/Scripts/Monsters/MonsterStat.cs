using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public MonsterData monsterData;

    public int currentHp;
    public int maxHp;

    public int attackDamage;
    public int moveSpeed;

    public float dodgeRate;
    public float criticalRate;
    public float criticalDamageRate;

    public int dropExp;
    public int dropGold;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        maxHp = monsterData.maxHp;
        attackDamage = monsterData.attackDamage;
        moveSpeed = monsterData.moveSpeed;
        dodgeRate = monsterData.dodgeRate;
        criticalRate = monsterData.criticalRate;
        criticalDamageRate = monsterData.criticalDamageRate;
        dropExp = monsterData.dropExp;
        dropGold = monsterData.dropGold;

        currentHp = maxHp;
    }
}