using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[System.Serializable]
public class FieldArea
{
    public int minLevel;
    public int maxLevel;
    public Transform[] wayPoints;
}

public class CharacterStateMachine : MonoBehaviour
{
    public CharacterStat characterStat;
    public FieldArea[] fieldAreas;
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Transform[] currentWayPoint;
    private int currentWayPointIndex = 0;
    private Coroutine currentCoroutine;

    public enum MoveState
    {
        GoField,
        InField,
        GoTown,
        InTown
    }

    public MoveState moveState;

    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Die
    }

    public AnimationState animationState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        previousPos = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        moveState = MoveState.InTown;
    }

    void FixedUpdate()
    {  
        AnimationStateChange();
         //사냥터 왔다갔다 도시 왔다갔다
        if(currentCoroutine == null)
        {
            switch(moveState)
            {
                case MoveState.GoField:
                    currentCoroutine = StartCoroutine(GoField());
                    break;
                case MoveState.InField:
                    currentCoroutine = StartCoroutine(InField());
                    break;
                case MoveState.GoTown:
                    currentCoroutine = StartCoroutine(GoTown());
                    break;
                case MoveState.InTown:
                    currentCoroutine = StartCoroutine(InTown());
                    break;
            }
        }

        previousPos = transform.position;

    }


    void FieldSelect()
    {
        foreach(FieldArea fieldArea in fieldAreas)
        {
            if(characterStat.level >= fieldArea.minLevel && characterStat.level <= fieldArea.maxLevel)
            {
                currentWayPoint = fieldArea.wayPoints;
                break;
            }
        }
    }

    private bool isFirstMove = true;

    IEnumerator GoField()
    {
        FieldSelect();
        while(moveState == MoveState.GoField)
        {
            if(isFirstMove)
            {
                currentWayPointIndex = 0;
                isFirstMove = false;
            }

            Vector2 currentPos = transform.position;
            Vector2 targetPos = currentWayPoint[currentWayPointIndex].position;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, characterStat.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);

            if(Vector2.Distance(currentPos, targetPos) < 0.01f)
            {
                currentWayPointIndex++;
                if(currentWayPointIndex >= currentWayPoint.Length)
                {
                    moveState = MoveState.InField;
                    yield break;
                }    
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    IEnumerator InField()
    {
        while(moveState == MoveState.InField)
        {
            characterStat.currentPlayTime = Mathf.Max(characterStat.currentPlayTime - Time.fixedDeltaTime, 0f);
            if(characterStat.currentPlayTime == 0f)
            {
                moveState = MoveState.GoTown;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    IEnumerator GoTown()
    {
        while(moveState == MoveState.GoTown)
        {
            if(isFirstMove)
            {
                currentWayPointIndex = currentWayPoint.Length - 1;
                isFirstMove = false;
            }

            Vector2 currentPos = transform.position;
            Vector2 targetPos = currentWayPoint[currentWayPointIndex].position;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, characterStat.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);

            if(Vector2.Distance(currentPos, targetPos) < 0.01f)
            {
                currentWayPointIndex--;
                if(currentWayPointIndex < 0)
                {
                    moveState = MoveState.InTown;
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    
    IEnumerator InTown()
    {
        while(moveState == MoveState.InTown)
        {
            characterStat.currentPlayTime = Mathf.Min(characterStat.currentPlayTime + (characterStat.playTimeRecovery * Time.fixedDeltaTime), characterStat.maxPlayTime);
            if(characterStat.currentPlayTime == characterStat.maxPlayTime)
            {
                moveState = MoveState.GoField;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Field")
        {
            currentCoroutine = null;
            moveState = MoveState.InField;
        }
        else if(other.gameObject.tag == "Town")
        {
            currentCoroutine = null;
            moveState = MoveState.InTown;
        }
    }

    private Vector2 previousPos;


    void AnimationStateChange()
    {
        float xDistance = Mathf.Abs(transform.position.x - previousPos.x);
        float yDistance = Mathf.Abs(transform.position.y - previousPos.y);

        if(xDistance > 0.01f || yDistance > 0.01f)
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
        if(animationState == AnimationState.Walk)
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

    


}
