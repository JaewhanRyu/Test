using UnityEngine;
using System.Collections.Generic;

public class PortalInField : MonoBehaviour
{
    public Transform toTownArea;
    private List<GameObject> characters = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Character")
        {
            characters.Add(other.gameObject);
            
            if(characters != null)
            {
                foreach(GameObject character in characters)
                {
                    if(character.GetComponent<CharacterAutoMove>().moveState == CharacterAutoMove.MoveState.GoTown)
                    {
                        character.transform.position = toTownArea.position;
                        character.GetComponent<CharacterAutoMove>().moveState = CharacterAutoMove.MoveState.InTown;
                        character.GetComponent<CharacterAutoMove>().moveCoroutine = null;
                    }
                }
            }
        }
    }


}
