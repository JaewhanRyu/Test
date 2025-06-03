using UnityEngine;
using System.Collections;

public class AutoFight : MonoBehaviour, IFight
{
    private Stat stat;
    private GameObject target;
    
    public enum AutoFightState
    {
        FindTarget,
        AttackTarget
    }

    public AutoFightState autoFightState;
    private Coroutine autoFightCoroutine;
    private CharacterPathMove characterPathMove;
    public LayerMask targetLayer;
    public BoxCollider2D fieldCollider;
    private Bounds bounds;
    private Vector2 randomPos;
    private float time;
    private bool isFirstMove = true;
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;

    void Awake()
    {
        stat = GetComponent<Stat>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(gameObject.tag == "Character")
        {
            characterPathMove = GetComponent<CharacterPathMove>();
        }
    }

    void OnEnable()
    {
        autoFightState = AutoFightState.FindTarget;
        autoFightCoroutine = null;
        target = null;

        if(fieldCollider != null)
        {
            bounds = fieldCollider.bounds;
        }   
    }

    void OnDisable()
    {
        if(autoFightCoroutine != null)
        {
            StopCoroutine(autoFightCoroutine);
            autoFightCoroutine = null;
        }
    }

    void FixedUpdate()
    {
        if (autoFightCoroutine == null)
        {
            switch (autoFightState)
            {
                case AutoFightState.FindTarget:
                    autoFightCoroutine = StartCoroutine(FindTarget());
                    break;
                case AutoFightState.AttackTarget:
                    autoFightCoroutine = StartCoroutine(AttackTarget());
                    break;
            }
        }
    }

    IEnumerator FindTarget()
    {
        isFirstMove = true;
        time = 0;

        while(autoFightState == AutoFightState.FindTarget)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stat.viewRange, targetLayer);

            if (colliders.Length > 0)
            {
                float minDistance = Mathf.Infinity;
                GameObject nearestTarget = null;

                foreach (Collider2D collider in colliders)
               {
                        float distance = Vector2.Distance(transform.position, collider.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestTarget = collider.gameObject;
                        }
                }
               target = nearestTarget;
               autoFightState = AutoFightState.AttackTarget;
               autoFightCoroutine = null;
               yield break;
            }
            else
            {
                if(isFirstMove)
                {
                    RandomSpotSelect();
                    isFirstMove = false;
                }
                
                Vector2 currentPos = transform.position;
                Vector2 targetPos = randomPos;
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, stat.moveSpeed * Time.fixedDeltaTime);
                rigid.MovePosition(newPos);

                float distance = (currentPos - targetPos).sqrMagnitude;
                if(distance <= 0.0001f)
                {
                    time += Time.fixedDeltaTime;
                    if(time >= 5f)
                    {
                        time = 0;
                        isFirstMove = true;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    public IEnumerator AttackTarget()
    {
        isAttacking = false;
        while(autoFightState == AutoFightState.AttackTarget)
        {
            TargetPosAndDieCheck();
            if(target == null)
            {
                autoFightState = AutoFightState.FindTarget;
                autoFightCoroutine = null;
                yield break;
            }

            Vector2 currentPos = transform.position;
            Vector2 targetPos = target.transform.position;
            float distance = Vector2.Distance(currentPos, targetPos);

            if(distance >= stat.attackRange)
            {
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, stat.moveSpeed * Time.fixedDeltaTime);
                rigid.MovePosition(newPos);
            }
            else
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                AnimationFlip();
                StartCoroutine(AttackEnd());
                yield return new WaitForSeconds(stat.attackDelay);
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Field")
        {
            bounds = other.bounds;
        }
    }

    void RandomSpotSelect()
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        randomPos = new Vector2(randomX, randomY);
    }

    void AnimationFlip()
    {
        if(transform.position.x > target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void TargetPosAndDieCheck()
    {
        if(target.transform.position.x < bounds.min.x || target.transform.position.x > bounds.max.x || target.transform.position.y < bounds.min.y || target.transform.position.y > bounds.max.y)
        {
            target = null;
            autoFightState = AutoFightState.FindTarget;
            autoFightCoroutine = null;
        }
        else if(target.GetComponent<Stat>().currentHp <= 0)
        {
            Debug.Log("targetDie");
            target = null;
            autoFightState = AutoFightState.FindTarget;
            autoFightCoroutine = null;
        }
    }

    IEnumerator AttackEnd()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
        yield break;
    }



    public void Attack()
    {
        if(target != null)
        {
            if(target.GetComponent<Stat>().currentHp > 0)
            {
                target.GetComponent<IFight>().Hurt(stat.attackPower);
            }
        }
    }


    public void Hurt(int damage)
    {
        stat.currentHp = Mathf.Max(stat.currentHp - damage, 0);
        stat.UpdateHpBar();
        if(!isAttacking)
        {
            animator.SetTrigger("Hurt");
        }
         if(stat.currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(gameObject.tag == "Character")
        {
            StartCoroutine(CharacterDieCoroutine());
        }
        else if(gameObject.tag == "Monster")
        {
            StartCoroutine(MonsterDieCoroutine());
        }
    }

    public void Skill(int skillDamage)
    {
        Debug.Log("Skill");
    }

    IEnumerator CharacterDieCoroutine()
    {
        StopCoroutine(autoFightCoroutine);
        animator.SetTrigger("Death");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Debug.Log("CharacterDieCoroutine");
        yield return new WaitForSeconds(7f);
        transform.position = characterPathMove.intownInitWayPoints[0].position;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        characterPathMove.moveState = CharacterPathMove.MoveState.InTown;
        characterPathMove.moveCoroutine = null;
        this.enabled = false; 
        yield break;
    }

    IEnumerator MonsterDieCoroutine()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        yield break;
    }
}
