using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 movementOffset;

    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(player);
        Vector2 targetVector = player.position + offset;
        if (player.velocity.x > 0.05f)
            targetVector += movementOffset;
        else if (player.velocity.x < -0.05f)
            targetVector -= movementOffset;
        
        Vector2 cameraPosition = Vector2.SmoothDamp(transform.position, targetVector, ref velocity, 0.2f);
        Vector3 currentPosition = transform.position;
        currentPosition.x = cameraPosition.x;
        currentPosition.y = cameraPosition.y;
        transform.position = currentPosition;
    }
}
