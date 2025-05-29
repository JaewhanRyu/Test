using UnityEngine;
using System.Collections;

public class MonsterAuto : MonoBehaviour
{
    private MonsterStat monsterStat;
    public BoxCollider2D boxCollider;
    public LayerMask enemyLayer;
    private Bounds bounds;

    public GameObject enemy;
    public CharacterStat enemyStat;
    public CharacterAutoMove enemyAutoMove;

    private bool isFindEnemy = false;

    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 randomSpot;
    
    private Coroutine randomMoveCoroutine;
    private Coroutine findEnemyCoroutine;
    private Coroutine attackCoroutine;

    private Vector2 previousPos;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterStat = GetComponent<MonsterStat>();
    }

    void OnEnable()
    {
        RandomSpot();
        transform.position = randomSpot;
        previousPos = transform.position;
    }

    void FixedUpdate()
    {
        AnimationCheck();
        if(!isFindEnemy)
        {
            if(randomMoveCoroutine == null)
            {
                randomMoveCoroutine = StartCoroutine(RandomMove());
            }
            if(findEnemyCoroutine == null)
            {
                findEnemyCoroutine = StartCoroutine(FindEnemy());
            }
        }
        else
        {
            CheckEnemyInArea();
            CheckEnemyDie();
            if(attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
        }
        previousPos = transform.position;
    }

    bool isFirstMove = true;

    IEnumerator RandomMove()
    {
        while(!isFindEnemy)
        {
            if(isFirstMove)
            {
                RandomSpot();
                isFirstMove = false;
            }

            Vector2 currentPos = transform.position;
            Vector2 targetPos = randomSpot;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, monsterStat.moveSpeed * Time.fixedDeltaTime);

            rigid.MovePosition(newPos);

            if(Vector2.Distance(currentPos, targetPos) < 0.01f)
            {
                isFirstMove = true;
                yield return new WaitForSeconds(5f);
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    IEnumerator FindEnemy()
    {
        while(!isFindEnemy)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, monsterStat.viewRange, enemyLayer);

            float minDistance = Mathf.Infinity;
            GameObject target = null;
            Vector2 targetPos = Vector2.zero;

            if(colliders.Length > 0)
            {
                foreach(Collider2D collider in colliders)
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);

                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        target = collider.gameObject;
                    }
                }
                enemy = target;
                enemyStat = enemy.GetComponent<CharacterStat>();
                enemyAutoMove = enemy.GetComponent<CharacterAutoMove>();
                isFindEnemy = true;
                randomMoveCoroutine = null;
                findEnemyCoroutine = null;            
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    IEnumerator Attack()
    {
        while(isFindEnemy)
        {
            Vector2 currentPos = transform.position;
            Vector2 targetPos = enemy.transform.position;

            if(Vector2.Distance(currentPos, targetPos) > monsterStat.attackRange && enemy != null)
            {
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, monsterStat.moveSpeed * Time.fixedDeltaTime);
                rigid.MovePosition(newPos);
            }
            
            if(Vector2.Distance(currentPos, targetPos) <= monsterStat.attackRange && enemy != null)
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(monsterStat.attackDelay);
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    void CheckEnemyInArea()
    {
        float maxY = bounds.max.y;

        if(enemy.transform.position.y > maxY)
        {
            isFindEnemy = false;
            randomMoveCoroutine = null;
            findEnemyCoroutine = null;
            attackCoroutine = null;
            enemy = null;
        }
    }

    void CheckEnemyDie()
    {
        if(enemy == null)
        {
            return;
        }
        else
        {
            if(enemyAutoMove.enabled == false)
            {
                isFindEnemy = false;
                randomMoveCoroutine = null;
                findEnemyCoroutine = null;
                attackCoroutine = null;
                enemy = null;
                enemyStat = null;
                enemyAutoMove = null;
            }
        }
    }

    void RandomSpot()
    {
        bounds = boxCollider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        randomSpot = new Vector2(randomX, randomY);
    }

    void AnimationCheck()
    {
        Vector2 currentPos = transform.position;

        float distanceX = Mathf.Abs(currentPos.x - previousPos.x);
        float distanceY = Mathf.Abs(currentPos.y - previousPos.y);

        if(distanceX > 0.01f || distanceY > 0.01f)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        AnimationFlipX();
    }

    void AnimationFlipX()
    {
        Vector2 currentPos = transform.position;

        if(currentPos.x - previousPos.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    void AttackDamage()
    {
        if(enemy == null)
        {
            return;
        }
        else
        {
            enemyStat.OnDamage((int)(monsterStat.attackDamage ));
        }
    }
        
}
