using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class CameraController : MonoBehaviour
{
    public List<Vector3> nextPos = new List<Vector3>();
    private int nextPosIndex = 0;

    void Start()
    {
        nextPosIndex = 0;
        if(nextPos.Count > 0)
            transform.position = nextPos[nextPosIndex];
    }

    public void NextButtonClick()
    {
        if(nextPos.Count == 0)
        {
            return;
        }

        if(nextPosIndex < nextPos.Count - 1)
        {
            nextPosIndex++;
        }
        Camera.main.transform.position = nextPos[nextPosIndex];
    }

    public void PreviousButtonClick()
    {
        if(nextPos.Count == 0)
        {
            return;
        }

        if(nextPosIndex > 0)
        {
            nextPosIndex--;
        }
        Debug.Log(nextPosIndex);
        Camera.main.transform.position = nextPos[nextPosIndex];
    }

}
