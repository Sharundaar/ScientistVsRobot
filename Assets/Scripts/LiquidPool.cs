using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiquidPool : MonoBehaviour {

    public enum PoolType
    {
        ACID,
        WATER
    }

    [SerializeField]
    private PoolType Type = PoolType.WATER;

    [SerializeField]
    private float Damper = 0.1f;

    private List<Rigidbody> m_ImmergedBodies = new List<Rigidbody>();

    private int s_RobotLayer;
    private int s_HumanLayer;

    void Awake()
    {
        s_RobotLayer = LayerMask.NameToLayer("RobotPlayer");
        s_HumanLayer = LayerMask.NameToLayer("HumanPlayer");
    }

    void FixedUpdate()
    {
        foreach(var body in m_ImmergedBodies)
        {
            Collider bodyCollider = body.GetComponent<Collider>();

            float imertionRatio = ((transform.position.y+0.5f) - bodyCollider.bounds.min.y) / (bodyCollider.bounds.max.y - bodyCollider.bounds.min.y);
            Vector3 force = -Physics.gravity * imertionRatio;
            force.y -= body.velocity.y * Damper / Time.fixedDeltaTime;

            body.AddForce(force*body.mass, ForceMode.Force);

            if(Type == PoolType.ACID && body.gameObject.layer == s_HumanLayer)
            {
                PlayerController player = body.GetComponent<PlayerController>();
                player.Damage();
            }
            else if(Type == PoolType.WATER && body.gameObject.layer == s_RobotLayer)
            {
                PlayerController player = body.GetComponent<PlayerController>();
                player.Damage();
            }
        }
    }

    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.attachedRigidbody != null)
            m_ImmergedBodies.Add(_collider.attachedRigidbody);
    }

    void OnTriggerExit(Collider _collider)
    {
        if (_collider.attachedRigidbody != null)
            m_ImmergedBodies.Remove(_collider.attachedRigidbody);
    }
}
