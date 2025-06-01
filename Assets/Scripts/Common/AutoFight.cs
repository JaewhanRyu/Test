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
    public LayerMask targetLayer;
    public BoxCollider2D fieldCollider;
    private Bounds bounds;
    private Vector2 randomPos;
    private float time;
    private bool isFirstMove = true;
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        stat = GetComponent<Stat>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    IEnumerator AttackTarget()
    {
        while(autoFightState == AutoFightState.AttackTarget)
        {
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
                animator.SetTrigger("Attack");
                FlipAnimation();
                yield return new WaitForSeconds(stat.attackDelay);
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    void FlipAnimation()
    {
        if(transform.position.x - target.transform.position.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(transform.position.x - target.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
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



    public void Attack(int damage)
    {
        Debug.Log("Attack");
    }

    public void Hurt(int damage)
    {
        Debug.Log("Hurt");
    }

    public void Skill(int skillDamage)
    {
        Debug.Log("Skill");
    }
}
