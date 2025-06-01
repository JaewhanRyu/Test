using UnityEngine;

public class MonsterRespawn : MonoBehaviour
{
    public BoxCollider2D respawnArea;
    private Bounds bounds;
    private Vector2 respawnPoint;

    void OnEnable()
    {
        ReSpawn();
    }


    void ReSpawn()
    {
        bounds = respawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        respawnPoint = new Vector2(x, y);
    }
}
