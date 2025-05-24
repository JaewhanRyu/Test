using UnityEngine;

public class CharacterStat : MonoBehaviour
{
   public JobData jobData;

   public int level = 1;

   public int strength;
   public int intelligence;
   public int dexterity;
   public int vitality;

   public int maxHp;
   public int currentHp;

   public int attackDamage;
   public float attackSpeed;
   public float dodgeRate = 0.1f;
   public float criticalRate = 0.1f;
   public int criticalDamage;
   public float skillDamageRate;
   public float moveSpeed = 2f;

   public int gold = 0;
   public int currentExp = 0;
   public int maxExp = 20;

   public float maxPlayTime = 600f;
   public float currentPlayTime = 0f;
   public float playTimeRecovery = 4f;



   void Awake()
   {
        Initialize();
        StatCulculated();
   }

   void Start()
   {
        currentPlayTime = maxPlayTime;
   }

   void Initialize()
   {
        strength = jobData.strength;
        intelligence = jobData.intelligence;
        dexterity = jobData.dexterity;
        vitality = jobData.vitality;
   }

   void StatCulculated()
   {
       maxHp = 50 + (vitality * 5);
       attackDamage = strength * 1;
       attackSpeed = Mathf.Clamp(attackSpeed, 0.5f, 2f);
       dodgeRate = Mathf.Clamp(dodgeRate, 0f, 0.5f);
       moveSpeed = Mathf.Clamp(moveSpeed, 2f, 4f);
       criticalRate = Mathf.Clamp(criticalRate, 0f, 1f);
       criticalDamage = (int)(attackDamage * (1.5f + (dexterity * 0.01f)));
       skillDamageRate = (intelligence * 1.01f);

       currentHp = maxHp;
   }

}
