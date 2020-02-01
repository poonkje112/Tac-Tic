using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(CapsuleCollider2D))]
public class Player : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_deceleration;
    [SerializeField] private float m_groundCellerationMultilier;
    [SerializeField] private float m_airCellerationMultilier;
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_gravity;
    private Vector2 m_velocity;

    [Header("Jump")]
    [SerializeField] private float m_jumpForce;

    [Header("Ground Check")]
    [SerializeField] private float m_castDistance;
    [SerializeField] private float m_jumpGroundDistance;
    [SerializeField] private float m_groundedDistance;
    [SerializeField] private float m_radius;
    [SerializeField] private LayerMask m_groundLayer;
    private bool m_grounded;

    [Header("Components")]
    private Rigidbody2D m_rigidbody;
    private CapsuleCollider2D m_collider;


    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, (m_collider.size.x / 2f) * m_radius, Vector2.down, m_castDistance, m_groundLayer);

        float bottomY = transform.position.y - m_collider.size.y / 2f;
        float groundDistance = bottomY - hit.point.y;

        if (Input.GetKeyDown(KeyCode.Space) && groundDistance <= m_jumpGroundDistance)
        {
            m_velocity.y = m_jumpForce;
        }
        else
        {
            if (groundDistance <= m_groundedDistance)
            {
                m_velocity.y = 0;
                m_grounded = true;
            }
            else
            {
                m_velocity.y -= m_gravity * Time.deltaTime;
                m_grounded = false;
            }
        }
        

        float move = Input.GetAxisRaw("Horizontal") * m_maxSpeed;

        float celleration = (Mathf.Abs(move) <= 0.01f ? m_deceleration : m_acceleration);
        celleration *= m_grounded ? m_groundCellerationMultilier : m_airCellerationMultilier;

        m_velocity.x = Mathf.MoveTowards(m_velocity.x, move, celleration * Time.deltaTime);
        m_rigidbody.velocity = m_velocity;
    }

}


