using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public List<Vector3> nextPos = new List<Vector3>();
    private int nextPosIndex = 0;



    IEnumerator NextButtonClickCoroutine()
    {
        if(nextPos.Count == 0)
        {
            yield break;
        }

        Vector3 originPos = Camera.main.transform.position;

        if(nextPosIndex < nextPos.Count - 1)
        {
            nextPosIndex++;
        }

        Vector3 targetPos = nextPos[nextPosIndex];
        float t = 0f;
        float duration = 1.0f; // 이동에 걸리는 시간(초)

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Camera.main.transform.position = Vector3.Lerp(originPos, targetPos, t);
            yield return null;
        }
        Camera.main.transform.position = targetPos; // 마지막 위치 보정
    }

    IEnumerator PreviousButtonClickCoroutine()
    {
        if(nextPos.Count == 0)
        {
            yield break;
        }

        Vector3 originPos = Camera.main.transform.position;

        if(nextPosIndex > 0)
        {
            nextPosIndex--;
        }

        Vector3 targetPos = nextPos[nextPosIndex];
        float t = 0f;
        float duration = 1.0f; // 이동에 걸리는 시간(초)

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Camera.main.transform.position = Vector3.Lerp(originPos, targetPos, t);
            yield return null;
        }
        Camera.main.transform.position = targetPos; // 마지막 위치 보정
    }

    public void NextButtonClick()
    {
        StartCoroutine(NextButtonClickCoroutine());
    }

    public void PreviousButtonClick()
    {
       StartCoroutine(PreviousButtonClickCoroutine());
    }

}
