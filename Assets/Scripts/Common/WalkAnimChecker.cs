using UnityEngine;

public class WalkAnimChecker : MonoBehaviour
{
   private Animator animator;
   private SpriteRenderer spriteRenderer;
   private Vector2 previousPos;

   void Awake()
   {
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
     previousPos = transform.position;
   }

   void WalkCheck()
   {
     Vector2 currentPos = transform.position;

     float xDistance = Mathf.Abs(currentPos.x - previousPos.x);
     float yDistance = Mathf.Abs(currentPos.y - previousPos.y);

     if(xDistance > 0.0001f || yDistance > 0.0001f)
     {
      animator.SetBool("Walk", true);
     }
     else
     {
      animator.SetBool("Walk", false);
     }
     FlipCheck();
   }

   void FlipCheck()
   {
      Vector2 currentPos = transform.position;
      
      if(currentPos.x - previousPos.x > 0)
      {
        spriteRenderer.flipX = false;
      }
      else if(currentPos.x - previousPos.x < 0)
      {
        spriteRenderer.flipX = true;
      }
   }
}
