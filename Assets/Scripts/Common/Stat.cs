using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    public StatData statData;

    public int health;
    public int strength;
    public int intelligence;
    public int dexterity;

    public int maxHp;
    public int currentHp;
    public int attackPower;
    public float skillDamageRate;

    public float dodgeRate;
    public float criticalRate;
    public float criticalDamage;

    public float moveSpeed;
    public float attackDelay;

    public float viewRange;
    public float attackRange;

<<<<<<< HEAD
    public Image hpBar_fill;
=======
    public Image hpBar;
>>>>>>> 0a8d2b86d624abe277cfcdcd78b05575f199b0f9
    
    
    void Awake()
    {
        Init();
    }

    void Start()
    {
        InitStat();
        UpdateHpBar();
    }


    void Init()
    {
        health = statData.health;
        strength = statData.strength;
        intelligence = statData.intelligence;
        dexterity = statData.dexterity;

        viewRange = statData.viewRange;
        attackRange = statData.attackRange;
        moveSpeed = statData.moveSpeed;
        attackDelay = statData.attackDelay;
    }

    void InitStat()
    {
        maxHp = health * 5;
        currentHp = maxHp;

        attackPower = strength * 2;
        skillDamageRate = intelligence * 0.01f;
        criticalDamage = dexterity * 0.01f;
    }

<<<<<<< HEAD
    public void HpBarUpdate()
    {
        hpBar_fill.fillAmount = (float)currentHp / maxHp;
    }

    
=======
    public void UpdateHpBar()
    {
        hpBar.fillAmount = (float)currentHp / maxHp;
    }
>>>>>>> 0a8d2b86d624abe277cfcdcd78b05575f199b0f9
}
