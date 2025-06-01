using UnityEngine;

public interface IFight
{
    
    void Attack(int damage);
    void Hurt(int damage);
    void Skill(int skillDamage);
}
