using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    private Vector3 originPos;
    public List<Vector3> nextPos = new List<Vector3>();
    private int nextPosIndex = 0;


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
        Camera.main.transform.position = nextPos[nextPosIndex];
    }
}
