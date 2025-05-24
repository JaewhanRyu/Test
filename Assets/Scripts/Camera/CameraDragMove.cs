using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    public Transform boundObject;
    private BoxCollider2D boundCollider;

    private Camera cam;
    private Vector3 dragOrigin; //드래그 시작 시 위치

    void Start()
    {
        cam = Camera.main;
        boundCollider = boundObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //PC일 때
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            MoveCamera(difference);
        }
 
        //모바일일 때
        if(Input.touchCount == 1) //손가락 하나 눌렀을 때
        {
            Touch touch = Input.GetTouch(0); //첫번째 손가락 정보

            if(touch.phase == TouchPhase.Began)
            {
                dragOrigin = cam.ScreenToWorldPoint(touch.position);
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(touch.position);
                MoveCamera(difference);
            }
        }
    }

    void MoveCamera(Vector3 moveDelta)
    {
        cam.transform.position += moveDelta;
        ClampCamera();
    }

    void ClampCamera()
    {
        Bounds bounds = boundCollider.bounds;

        float camHeight = cam.orthographicSize * 2f; //세로 전체 크기
        float camWidth = camHeight * cam.aspect; //(세로*비율)

        Vector3 camPos = cam.transform.position; //현재 카메라 위치

        float minX = bounds.min.x + camWidth / 2;
        float maxX = bounds.max.x - camWidth / 2;

        float minY = bounds.min.y + camHeight / 2;
        float maxY = bounds.max.y - camHeight / 2;

        //카메라 위치 제한
        camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
        camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
        cam.transform.position = new Vector3(camPos.x, camPos.y, cam.transform.position.z);
    }
}
