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

    public float attackRange;
    public float viewRange;
    public float attackDelay;

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
        attackRange = monsterData.attackRange;
        viewRange = monsterData.viewRange;
        attackDelay = monsterData.attackDelay;


        currentHp = maxHp;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }
}