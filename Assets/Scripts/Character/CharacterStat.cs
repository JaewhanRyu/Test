using UnityEngine;
using System.Collections;

public class CharacterStat : MonoBehaviour
{
    public JobData jobData;

    public int level;
    public int strength;
    public int intelligence;
    public int dexterity;
    public int vitality;

    public float attackRange;
    public float viewRange;

    public int maxHp;
    public int currentHp;

    public int attackDamage;
    public float dodgeRate;
    public float criticalRate;
    public int criticalDamage;
    public float skillDamageRate;

    public float maxPlayTime;
    public float currentPlayTime;

    public float moveSpeed = 2;

    void Awake()
    {
        Initialize();

        CalculateStat();
    }

    void OnEnable()
    {
        currentHp = maxHp;
        gameObject.GetComponent<CharacterAutoMove>().enabled = true;
    }


    void Initialize()
    {
        level = jobData.level;
        strength = jobData.strength;
        intelligence = jobData.intelligence;
        dexterity = jobData.dexterity;
        vitality = jobData.vitality;

        attackRange = jobData.attackRange;
        viewRange = jobData.viewRange;

        currentPlayTime = maxPlayTime;
    }

    void CalculateStat()
    {
        maxHp = vitality * 5;
        currentHp = maxHp;

        attackDamage = strength * 1;
        dodgeRate = Mathf.Clamp(dodgeRate,0.1f,0.5f);
        criticalRate = Mathf.Clamp(criticalRate,0.1f,1f);
        criticalDamage = (int)(attackDamage * (1.5f + (dexterity * 0.005f)));
        skillDamageRate = intelligence * 0.01f;
    }

    public void OnDamage(int damage)
    {
        if(currentHp > 0)
        {
            currentHp = Mathf.Max(currentHp - damage, 0);
        }
        else
        {
            currentHp = 0;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        gameObject.GetComponent<CharacterAutoMove>().enabled = false;
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        yield break;
    }
}

