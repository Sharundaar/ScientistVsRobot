using UnityEngine;
using System.Collections;

public class CarryController : MonoBehaviour {

    class TriggerVar
    {
        private bool m_data = false;
        public bool Data
        {
            get
            {
                if(m_data)
                {
                    m_data = false;
                    return true;
                }
                return false;
            }
        }

        public void Trigger()
        {
            m_data = true;
        }
    }

    [SerializeField]
    private Transform View;

    [SerializeField]
    private Transform Anchor;

    [SerializeField]
    private float Range = 2.5f;

    [SerializeField]
    private float MaxSpeed = 5.0f;

    [SerializeField]
    private float ShootForce = 10.0f;

    private Rigidbody m_carriedObject = null;

    private TriggerVar m_carry = new TriggerVar();
    private TriggerVar m_release = new TriggerVar();
    private TriggerVar m_shoot = new TriggerVar();

    private int m_carryMask = 0;

    void Start()
    {
        m_carryMask = LayerMask.GetMask("Carryable");
    }

    void Update()
    {
        bool carry = m_carry.Data;

        RaycastHit hit;
        if(Physics.Raycast(View.position, View.forward, out hit, Range, m_carryMask))
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if(body != null && carry)
            {
                m_carriedObject = body;
            }
        }

        if(m_carriedObject != null && m_release.Data)
        {
            m_carriedObject = null;
        }

        if(m_carriedObject != null && m_shoot.Data)
        {
            m_carriedObject.AddForce(Anchor.forward * ShootForce, ForceMode.VelocityChange);
            m_carriedObject = null;
        }
    }

    void FixedUpdate()
    {
        if(m_carriedObject != null)
        {
            Vector3 targetSpeed = (Anchor.position - m_carriedObject.transform.position);
            m_carriedObject.velocity = targetSpeed.normalized * Mathf.Min(MaxSpeed, targetSpeed.magnitude / Time.fixedDeltaTime);
            m_carriedObject.MoveRotation(Quaternion.RotateTowards(m_carriedObject.rotation, Quaternion.LookRotation(Anchor.forward, Anchor.up), 10.0f));
        }
    }

    public void Carry()
    {
        m_carry.Trigger();
    }

    public void Release()
    {
        m_release.Trigger();
    }

    public void Shoot()
    {
        m_shoot.Trigger();
    }

    public bool IsCarrying()
    {
        return m_carriedObject != null;
    }
}
