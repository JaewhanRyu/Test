using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "RPG/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int level;
    public int maxHp;
    public int attackDamage;
    public int moveSpeed;

    public float dodgeRate;
    public float criticalRate;
    public float criticalDamageRate;

    public int dropExp;
    public int dropGold;
}
