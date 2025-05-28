using UnityEngine;
using System.Collections;

[System.Serializable]
public class FieldArea
{
    public Transform[] FieldWayPoints;
}


public class CharacterAutoMove : MonoBehaviour
{
    private CharacterStat characterStat;
    private Rigidbody2D rigid;
    public Transform[] toPortalWayPoints_Town;
    public FieldArea[] fieldAreas;
    private int currentWayPointIndex = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public enum MoveState
    {
        GoField,
        InField,
        GoTown,
        InTown
    }

    public MoveState moveState;
    public Coroutine moveCoroutine;

    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Die
    }

    private AnimationState animationState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        characterStat = GetComponent<CharacterStat>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        previousPos = transform.position;
    }


    void FixedUpdate()
    {
        WalkCheck();
        if (moveCoroutine == null)
        {
            switch (moveState)
            {
                case MoveState.GoField:
                    moveCoroutine = StartCoroutine(GoField());
                    break;
                case MoveState.InField:
                    moveCoroutine = StartCoroutine(InField());
                    break;
                case MoveState.GoTown:
                    break;
                case MoveState.InTown:
                    break;
            }
        }
        previousPos = transform.position;
    }

    bool isFirstMove = true;

    IEnumerator GoField()
    {
        yield return new WaitForSeconds(1f); //나중에 삭제!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        while (moveState == MoveState.GoField)
        {
            if (isFirstMove)
            {
                currentWayPointIndex = 0;
                isFirstMove = false;
            }

            Vector2 currentPos = transform.position;
            Vector2 targetPos = toPortalWayPoints_Town[currentWayPointIndex].position;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, characterStat.moveSpeed * Time.fixedDeltaTime);

            rigid.MovePosition(newPos);

            if (Vector2.Distance(transform.position, targetPos) < 0.01f)
            {
                if (currentWayPointIndex < toPortalWayPoints_Town.Length - 1)
                {
                    currentWayPointIndex++;
                }
                else
                {
                    moveState = MoveState.InField;
                    moveCoroutine = null;
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    bool isArrivedToFieldArea = false;

    //0번 필드영역 나중에 추가시 수정
    private int currentFieldAreaIndex = 0;
    private Coroutine randomMoveCoroutine;
    IEnumerator InField()
    {
        while (moveState == MoveState.InField)
        {
            while(!isArrivedToFieldArea)
            {
                Debug.Log("사냥터 이동");
                Vector2 currentPos = transform.position;
                Vector2 targetPos = fieldAreas[0].FieldWayPoints[currentWayPointIndex].position;
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, characterStat.moveSpeed * Time.fixedDeltaTime);
                rigid.MovePosition(newPos);

                if(Vector2.Distance(transform.position, targetPos) < 0.01f)
                {
                    if(currentWayPointIndex < fieldAreas[0].FieldWayPoints.Length - 1)
                    {
                        currentFieldAreaIndex++;
                    }             
                }
                yield return new WaitForFixedUpdate();
            }

            characterStat.currentPlayTime = Mathf.Max(characterStat.currentPlayTime - Time.fixedDeltaTime, 0);

            if(characterStat.currentPlayTime == 0)
            {
                moveState = MoveState.GoTown;
                moveCoroutine = null;
                yield break;
            }

            if(randomMoveCoroutine == null)
            {
                randomMoveCoroutine = StartCoroutine(RandomMove());
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    private Bounds fieldBounds;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Field")
        {
            isArrivedToFieldArea = true;
            currentWayPointIndex = 0;
            fieldBounds = other.bounds;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Field")
        {
            isArrivedToFieldArea = false;
            randomMoveCoroutine = null;
            StopCoroutine(randomMoveCoroutine);
        }
    }

    private Vector2 randomPos;

    IEnumerator RandomMove()
    {
        bool firstMove = true;
        while(moveState == MoveState.InField)
        {
            Vector2 targetPos;

            if(firstMove)
            {
                float randomX = Random.Range(fieldBounds.min.x, fieldBounds.max.x);
                float randomY = Random.Range(fieldBounds.min.y, fieldBounds.max.y);
                randomPos = new Vector2(randomX, randomY);
                firstMove = false;
            }

            Vector2 currentPos = transform.position;
            targetPos = randomPos;

            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, characterStat.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);

            float distance = (newPos - targetPos).sqrMagnitude;

            if(distance < 0.0001f)
            {
                firstMove = true;
                yield return new WaitForSeconds(5f);
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;     
    }

    private Vector2 previousPos;

    void WalkCheck()
    {
        Vector2 currentPos = transform.position;

        float x = Mathf.Abs(currentPos.x - previousPos.x);
        float y = Mathf.Abs(currentPos.y - previousPos.y);

        if(x > 0.01f || y > 0.01f)
        {
            animationState = AnimationState.Walk;
            animator.SetBool("Walk", true);
        }
        else
        {
            animationState = AnimationState.Idle;
            animator.SetBool("Walk", false);
        }
        AnimationFlip();
    }

    void AnimationFlip()
    {
        if(transform.position.x - previousPos.x >= 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    


}
