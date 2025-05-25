using UnityEngine;
using System.Collections;

public class CharacterAutoMove : MonoBehaviour
{
    private CharacterStat characterStat;
    private Rigidbody2D rigid;
    public Transform[] toPortalWayPoints_Town;
    public Transform[] toPortalWayPoints_Field;
    private int currentWayPointIndex = 0;

    public enum MoveState
    {
        GoField,
        InField,
        GoTown,
        InTown
    }

    public MoveState moveState;
    public Coroutine moveCoroutine;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        characterStat = GetComponent<CharacterStat>();
    }


    void FixedUpdate()
    {
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
    }

    bool isFirstMove = true;

    IEnumerator GoField()
    {
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

    IEnumerator InField()
    {
        while (moveState == MoveState.InField)
        {
            while(!isArrivedToFieldArea)
            {
                Vector2 currentPos = transform.position;
                Vector2 targetPos = toPortalWayPoints_Field[currentWayPointIndex].position;
                Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, characterStat.moveSpeed * Time.fixedDeltaTime);
                rigid.MovePosition(newPos);

                if(Vector2.Distance(transform.position, targetPos) < 0.01f)
                {
                    if(currentWayPointIndex < toPortalWayPoints_Field.Length - 1)
                    {
                        currentWayPointIndex++;
                    }             
                }
                yield return new WaitForFixedUpdate();
            }



        }
        yield break;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Field")
        {
            isArrivedToFieldArea = true;
            currentWayPointIndex = 0;
        }
    }

    


}
