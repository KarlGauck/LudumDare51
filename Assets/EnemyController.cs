using UnityEngine;
using UnityEngine.Events;

public class EnemyController: MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
    [SerializeField] private Pathfinding pathfinding;                           // The pathfinding script
    [SerializeField] private Vector3 defaultPosition;                           // Where enemy should go when no targetis nearby
    [SerializeField] private GameObject target;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
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
            // Add a vertical force to the enemy.
            m_Grounded = false;
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

        if (direction.x > 0)
            Move(0.3f);
        if (direction.x < 0)
            Move(-0.3f);
    
    }

}