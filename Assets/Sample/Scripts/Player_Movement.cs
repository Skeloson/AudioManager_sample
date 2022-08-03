using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof( Rigidbody2D ) )]
public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    float Speed = 5f;

    [SerializeField]
    protected float m_deadMagnitude = 0.15f;
    Rigidbody2D m_body = default;
    private Vector2 Velocity = Vector2.zero;

    [SerializeField]
    Animator m_animator = default;

    int m_dirX_hash = Animator.StringToHash( "dir_x" );
    int m_dirY_hash = Animator.StringToHash( "dir_y" );
    int m_speed_hash = Animator.StringToHash( "speed" );

    public Vector3 Forward { get; private set; }
    public Vector3 Right => Quaternion.AngleAxis( -90, Vector3.forward ) * Forward;
    
    public void Move(InputAction.CallbackContext context)
    {
        Move( context.ReadValue<Vector2>() );
    }
    
    private void Move(Vector2 p_direction)
    {
        if( Mathf.Abs( p_direction.magnitude ) > m_deadMagnitude )
        {
            m_body.velocity = p_direction;

            if( m_animator )
            {
                m_animator.SetFloat( m_dirX_hash, p_direction.normalized.x );
                m_animator.SetFloat( m_dirY_hash, p_direction.normalized.y );
                m_animator.SetFloat( m_speed_hash, p_direction.magnitude );
            }
        }
        else
        {
            if( m_animator )
            {
                m_animator.SetFloat( m_speed_hash, 0 );
            }

            m_body.velocity = new Vector2( 0, 0 );
        }
    }

    void Start()
    {
        Forward = transform.up;
        m_body = GetComponent<Rigidbody2D>();
        m_body.gravityScale = 0;
        m_body.freezeRotation = true;
        m_body.drag = 0;
    }

    private void OnDisable()
    {
        m_body.velocity = new Vector2( 0, 0 );

        if( m_animator )
        {
            m_animator.SetFloat( m_speed_hash, 0 );
        }
    }
}
