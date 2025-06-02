using UnityEngine;

public class MonsterDie : MonoBehaviour
{
    private Stat stat;
    private AutoFight autoFight;
    private Animator animator;

    void Awake()
    {
        stat = GetComponent<Stat>();
        autoFight = GetComponent<AutoFight>();
        animator = GetComponent<Animator>();
    }


    void Die()
    {
        if(autoFight.autoFightState == AutoFight.AutoFightState.AttackTarget)
        {
            if(stat.currentHp <= 0)
            {
                animator.SetBool("isDeath", false);
                gameObject.SetActive(false);
            }
        }
    }
    

}
