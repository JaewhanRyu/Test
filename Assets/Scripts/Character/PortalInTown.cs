using UnityEngine;


public class Portal : MonoBehaviour
{
    private GameObject character;
    private CharacterAutoMove characterAutoMove;
    public Transform[] toFieldArea;
   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            character = other.gameObject;
            characterAutoMove = character.GetComponent<CharacterAutoMove>();
            character.transform.position = toFieldArea[0].position;
            characterAutoMove.moveState = CharacterAutoMove.MoveState.InField;
            characterAutoMove.moveCoroutine = null;
        }
    }

   
}
