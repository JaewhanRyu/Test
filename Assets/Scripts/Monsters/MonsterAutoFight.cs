using UnityEngine;
using System.Collections;

public class MonsterAutoFight : MonoBehaviour
{
    private MonsterStat monsterStat;
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public LayerMask enemyLayer;
    private Bounds bounds;
    private Vector2 randomSpot;

    private GameObject enemy;
    private CharacterStat enemyStat;
    
    private bool isFindEnemy = false; //적을 발견했는가?
    private bool isEnemyDie = false; //적이 죽었는가?
    private bool isEnemyInArea = false; //적이 영역안에 있는가?
    private bool isEnemyInAttackRange = false; //적이 공격범위안에 있는가?

    private bool isFirstMove = true; //첫번째 이동인가?

    private Coroutine findEnemyCoroutine;
    private Coroutine autoMoveCoroutine;

    private Coroutine checkEnemyInAreaCoroutine;
    private Coroutine checkEnemyDieCoroutine;
    private Coroutine checkEnemyInAttackRangeCoroutine;
    
    private Vector2 previousPos;

    private float time = 0;

    

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterStat = GetComponent<MonsterStat>();
        bounds = boxCollider.bounds;
    }

    void OnEnable()
    {
        RandomSpot();
        transform.position = randomSpot;
        previousPos = transform.position;
    }

   
   void FixedUpdate()
   {
     WalkCheck();
     if(!isFindEnemy)
     {
        if(autoMoveCoroutine == null)
        {
            autoMoveCoroutine = StartCoroutine(AutoMove());
        }
        if(findEnemyCoroutine == null)
        {
            findEnemyCoroutine = StartCoroutine(FindEnemy());
        }
     }
     else
     {
        if(checkEnemyInAreaCoroutine == null)
        {
            checkEnemyInAreaCoroutine = StartCoroutine(CheckEnemyInArea());
        }
        if(checkEnemyDieCoroutine == null)
        {
            checkEnemyDieCoroutine = StartCoroutine(CheckEnemyDie());
        }
        if(checkEnemyInAttackRangeCoroutine == null)
        {
            checkEnemyInAttackRangeCoroutine = StartCoroutine(CheckEnemyInAttackRange());
        }
     }

     previousPos = transform.position;
   }


   //isFindEnemy가 false일때 적 찾기 & 자동이동동
   IEnumerator FindEnemy()
   {
     while(!isFindEnemy)
     {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, monsterStat.viewRange, enemyLayer);

        float minDistance = Mathf.Infinity;
        GameObject target = null;

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
            InitCoroutine();
            isFindEnemy = true;
        }
        yield return new WaitForSeconds(0.5f);
     }
     yield break;
   }

   IEnumerator AutoMove()
   {
     while(isFindEnemy)
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

        float distance = (newPos - targetPos).sqrMagnitude;

        if(distance < 0.0001f)
        {
            isFirstMove = true;
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForFixedUpdate();
     }
     yield break;
   }
   
    //isFindEnemy가 true일때 적이 죽었는지, 영역안에 있는지 확인 후 -> 사정거리 안에 있을 경우 공격 아니면 적에게 이동 
   IEnumerator CheckEnemyInArea()
   {
     while(isFindEnemy)
     {
        float maxY = bounds.max.y;

        if(enemy.transform.position.y > maxY)
        {
            InitCoroutine();
            isFindEnemy = false;
            enemy = null;
        }

        yield return new WaitForSeconds(0.5f);
     }
     yield break;
   }

   IEnumerator CheckEnemyDie()
   {
     while(isFindEnemy)
     {
        if(enemyStat.currentHp <= 0)
        {
            InitCoroutine();
            isFindEnemy = false;
            enemy = null;
        }
        yield return new WaitForFixedUpdate();
     }
     yield break;
   }

   IEnumerator CheckEnemyInAttackRange()
   {
    time = 0;
     while(isFindEnemy)
     {
        Vector2 currentPos = transform.position;
        Vector2 enemyPos = enemy.transform.position;
        float distance = Vector2.Distance(currentPos, enemyPos);

        time += Time.fixedDeltaTime;

        if(distance <= monsterStat.attackRange)
        { 
            if(time >= monsterStat.attackDelay)
            {
                time = 0;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(currentPos, enemyPos, monsterStat.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);
        }
        yield return new WaitForFixedUpdate();
     }
     yield break;
   }


   void InitCoroutine()
   {
     findEnemyCoroutine = null;
     autoMoveCoroutine = null;
     checkEnemyInAreaCoroutine = null;
     checkEnemyDieCoroutine = null;
     checkEnemyInAttackRangeCoroutine = null;
   }

   void AttackDamage()
   {
      enemyStat.OnDamage(monsterStat.attackDamage);
   }



   void RandomSpot()
   {
     float randomX = Random.Range(bounds.min.x, bounds.max.x);
     float randomY = Random.Range(bounds.min.y, bounds.max.y);

     randomSpot = new Vector2(randomX, randomY);
   }

   void WalkCheck()
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

    
}
