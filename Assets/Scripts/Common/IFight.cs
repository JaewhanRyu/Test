using UnityEngine;

public interface IFight
{
    
    void Attack();
    void Hurt(int damage);
    void Die();
    void Skill(int skillDamage);
}
