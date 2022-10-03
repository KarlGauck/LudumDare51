using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemyController: MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_GroundCheckLeft;                       // A position marking where to check if the ground is continuing on the left hand side
    [SerializeField] private Transform m_GroundCheckRight;                      // A position marking where to check if the ground is continuing on the right hand side
    [SerializeField] private Transform m_ObstacleCheckRight;                    // A position marking where to check for an obstacle on the right hand side
    [SerializeField] private Transform m_ObstacleCheckLeft;                     // A position marking where to check for an obstacle on the left hand side
    [SerializeField] private PathfindingOwn pathfinding;                           // The pathfinding script
    [SerializeField] private Vector3 defaultPosition;                           // Where enemy should go when no targetis nearby
    [SerializeField] private GameObject target;

//test
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_GroundLeft;          // Weather or not the ground continues on the left hand side;
    private bool m_GroundRight;         // Weather or not the ground continues on the right hand side;
    private bool m_ObstacleRight;
    private bool m_ObstacleLeft;
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
        
        colliders = Physics2D.OverlapCircleAll(m_GroundCheckLeft.position, k_CeilingRadius, m_WhatIsGround);
        m_GroundLeft = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
                m_GroundLeft = true;
        }

        colliders = Physics2D.OverlapCircleAll(m_GroundCheckRight.position, k_CeilingRadius, m_WhatIsGround);
        m_GroundRight = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
                m_GroundRight = true;
        }

        colliders = Physics2D.OverlapCircleAll(m_ObstacleCheckLeft.position, k_CeilingRadius, m_WhatIsGround);
        m_ObstacleLeft = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
                m_ObstacleLeft = true;
        }

        colliders = Physics2D.OverlapCircleAll(m_ObstacleCheckRight.position, k_CeilingRadius, m_WhatIsGround);
        m_ObstacleRight = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
                m_ObstacleRight = true;
        }
	}


	public void Move(float move)
	{
		//only control the player if grounded
		if (m_Grounded)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the enemy right and the enemy is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the enemy.
				Flip();
			}
			// Otherwise if the input is moving the enemy left and the enemy is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		
	}

    public void Jump(float xVel, float yVel)
    {
        // If the enemy should jump...

        if (m_Grounded)
        {
            m_Grounded = false;
            float jumpForce = 10.0f;
            m_Rigidbody2D.AddForce(new Vector2(xVel*jumpForce, yVel*jumpForce*3));
            //m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

	private void Flip()
	{
		// Switch the way the enemy is labelled as facing.
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f,180f, 0f);
	}
    
    void Update()
    {
        // Check if enemy can see target
        bool isTargetInSight = Physics2D.Raycast(target.transform.position, (target.transform.position - transform.position).normalized);

        // Determine where to go
        Vector3? targetDirection = pathfinding.getDirection(transform.position, target.transform.position);
        if (targetDirection == null || !isTargetInSight)
            targetDirection = pathfinding.getDirection(transform.position, defaultPosition);
        
        if (targetDirection == null)
            return;
        Vector3 direction = (Vector3)targetDirection;        

        // Weather or not the enemy can walk in this direction
        bool isWalkable = (direction.x > 0 && (m_GroundRight && !m_ObstacleRight) || (direction.x < 0 && (m_GroundLeft && !m_ObstacleLeft)));

        if (isWalkable)
        {
            if (direction.x > 0)
                Move(0.3f);
            if (direction.x < 0)
                Move(-0.3f);
        }
        else
        {
            Vector3? targetPos = pathfinding.getNextPosition(transform.position, target.transform.position);
            if (targetPos == null)
                return;
            JumpData data = pathfinding.CalculateBallisticTrajectory(transform.position, (Vector3)targetPos, 0.3f);
            if (data == null)
                return;
            Jump((float)(data.velocity * Math.Cos(data.angle)), (float)(data.velocity * Math.Sin(data.angle)));
        }
   
    }

}