using UnityEngine;

public class MonsterAuto : MonoBehaviour
{
    public MonsterStat monsterStat;

    public enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Die
    }

    public MonsterState monsterState;
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Coroutine coroutineState;
    public BoxCollider2D boxCollider;
    private Bounds bounds;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bounds = boxCollider.bounds;
    }

    void Start()
    {
        RandomSpawn();
    }

    void FixedUpdate()
    {
        if(coroutineState == null)
        {
            switch(monsterState)
            {
                case MonsterState.Idle:
                    break;
                case MonsterState.Move:
                    break;
                case MonsterState.Attack:
                    break;
                case MonsterState.Die:
                    break;
            }
        }
    }

    void RandomSpawn()
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector3(randomX, randomY, 0);
    }


    
}
