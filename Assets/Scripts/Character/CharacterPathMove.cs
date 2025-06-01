using UnityEngine;
using System.Collections;


[System.Serializable]
public class FieldPortal
{
    public Transform[] FieldPortalWayPoins;
}
public class CharacterPathMove : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private Stat stat;
    private Rigidbody2D rigidbody2D;
    private AutoFight autoFight;

    public enum MoveState
    {
        InTown,
        GoField,
        InField,
        GoTown,
    }

    public MoveState moveState;
    private Coroutine moveCoroutine;

    void Awake()
    {
        characterInfo = GetComponent<CharacterInfo>();
        stat = GetComponent<Stat>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        autoFight = GetComponent<AutoFight>();
    }

    void FixedUpdate()
    {
        if(moveCoroutine == null)
        {
            switch(moveState)
            {
                case MoveState.InTown:
                    moveCoroutine = StartCoroutine(InTown());
                    break;
                case MoveState.GoField:
                    moveCoroutine = StartCoroutine(GoField());
                    break;
                case MoveState.InField:
                    moveCoroutine = StartCoroutine(InField());
                    autoFight.enabled = true;
                    break;
                case MoveState.GoTown:
                    moveCoroutine = StartCoroutine(GoTown());
                    autoFight.enabled = false;
                    break;
            }
        }
    }

    public Transform[] intownInitWayPoints;
    private int currentInTownInitWayPointIndex;
    private bool isPosInit = false;

    IEnumerator InTown()
    {
        currentInTownInitWayPointIndex = 0;
        while(moveState == MoveState.InTown)
        {
            characterInfo.currentPlayTime = Mathf.Min(characterInfo.currentPlayTime + (Time.fixedDeltaTime * characterInfo.playTimeRecoveryRate), characterInfo.maxPlayTime);   

            if(characterInfo.currentPlayTime == characterInfo.maxPlayTime)
            {
                moveState = MoveState.GoField;
                moveCoroutine = null;
                yield break;
            }

            if(!isInField && !isPosInit)
            {
                Vector2 currentPosition = transform.position;
                Vector2 targetPos = intownInitWayPoints[currentInTownInitWayPointIndex].position;
                Vector2 newPos = Vector2.MoveTowards(currentPosition, targetPos, stat.moveSpeed * Time.fixedDeltaTime);
                rigidbody2D.MovePosition(newPos);

                float distance = (currentPosition - targetPos).sqrMagnitude;

                if(distance < 0.0001f)
                {
                    if(currentInTownInitWayPointIndex < intownInitWayPoints.Length - 1)
                    {
                        currentInTownInitWayPointIndex++;
                    }
                    else
                    {
                        isPosInit = true;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    public Transform[] inTownPortalWayPoint;
    public FieldPortal[] fieldPortals; //현재는 0으로 하나 추후 여러개 추가 가능
    private int currentFieldPortalIndex = 0;
    private int currentInTownPortalWayPointIndex;
    private int currentFieldPortalWayPointIndex;
    private bool isInField = false;

    IEnumerator GoField()
    {
        //타운 포탈(마을안, 사냥터안안)에 따라 플레이어 웨이포인트 따라 이동 달리 만들기기

        isInField = false;
        currentInTownPortalWayPointIndex = 0;
        currentFieldPortalWayPointIndex = 0;

        while(moveState == MoveState.GoField)
        {
            Vector2 currentPosition = transform.position;
            
            if(!isInField)
            {   
                Vector2 targetPos = inTownPortalWayPoint[currentInTownPortalWayPointIndex].position;
                Vector2 newPos = Vector2.MoveTowards(currentPosition, targetPos, stat.moveSpeed * Time.fixedDeltaTime);
                rigidbody2D.MovePosition(newPos);

                float distance = (currentPosition - targetPos).sqrMagnitude;

                if(distance < 0.0001f)
                {
                    if(currentInTownPortalWayPointIndex < inTownPortalWayPoint.Length - 1)
                    {
                        currentInTownPortalWayPointIndex++;
                    }
                }
            }
            else
            {
                Vector2 targetPos = fieldPortals[currentFieldPortalIndex].FieldPortalWayPoins[currentFieldPortalWayPointIndex].position;
                Vector2 newPos = Vector2.MoveTowards(currentPosition, targetPos, stat.moveSpeed * Time.fixedDeltaTime);
                rigidbody2D.MovePosition(newPos);

                float distance = (currentPosition - targetPos).sqrMagnitude;

                if(distance < 0.0001f)
                {
                    if(currentFieldPortalWayPointIndex < fieldPortals[currentFieldPortalIndex].FieldPortalWayPoins.Length - 1)
                    {
                        currentFieldPortalWayPointIndex++;
                  }
               }
            }

            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    IEnumerator GoTown()
    {
        isInField = true;
        currentFieldPortalWayPointIndex = fieldPortals[currentFieldPortalIndex].FieldPortalWayPoins.Length - 1;
        currentInTownPortalWayPointIndex = inTownPortalWayPoint.Length - 1;
        while(moveState == MoveState.GoTown)
        {
            Vector2 currentPosition = transform.position;

            if(isInField)
            {
                Vector2 targetPos = fieldPortals[currentFieldPortalIndex].FieldPortalWayPoins[currentFieldPortalWayPointIndex].position;
                Vector2 newPos = Vector2.MoveTowards(currentPosition, targetPos, stat.moveSpeed * Time.fixedDeltaTime);
                rigidbody2D.MovePosition(newPos);

                float distance = (currentPosition - targetPos).sqrMagnitude;

                if(distance < 0.0001f)
                {
                    if(currentFieldPortalWayPointIndex > 0)
                    {
                        currentFieldPortalWayPointIndex--;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }


    public Transform townPortalPos;
    public Transform[] fieldPortalPos;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "InFieldPortal")
        {
            isInField = true;
            if(moveState == MoveState.GoTown)
            {
                transform.position = townPortalPos.position;
            }
        }
        else if(other.gameObject.tag == "InTownPortal")
        {
            isInField = false;
            if(moveState == MoveState.GoField)
            {
                transform.position = fieldPortalPos[0].position;
            }
            else if(moveState == MoveState.GoTown)
            {
                moveState = MoveState.InTown;
                moveCoroutine = null;
            }
        }
        else if(other.gameObject.tag == "Field")
        {
            moveState = MoveState.InField;
            moveCoroutine = null;
        }
    }

    IEnumerator InField()
    {
        while(moveState == MoveState.InField)
        {
            characterInfo.currentPlayTime = Mathf.Max(characterInfo.currentPlayTime - Time.fixedDeltaTime , 0);

            if(characterInfo.currentPlayTime == 0)
            {
                moveState = MoveState.GoTown;
                moveCoroutine = null;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }


    
    
    
}
