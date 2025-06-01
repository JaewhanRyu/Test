using UnityEngine;

[CreateAssetMenu(fileName = "StatData", menuName = "RPG/StatData")]
public class StatData : ScriptableObject
{
    public int health;
    public int strength;
    public int intelligence;
    public int dexterity;

    public float viewRange;
    public float attackRange;

    public float moveSpeed;
    public float attackDelay;
}
