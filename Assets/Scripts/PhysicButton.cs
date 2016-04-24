using UnityEngine;
using System.Collections;

public class PhysicButton : MonoBehaviour {
    [SerializeField]
    private GameObject Target;

    [SerializeField]
    private Rigidbody PhysicButtonRb;

    [SerializeField]
    private float ActivationDistance = 1.0f;

    private Vector3 m_pbStartPosition;

    private bool m_activated = false;

    private void Start()
    {
        m_pbStartPosition = PhysicButtonRb.transform.position;
    }

    private void FixedUpdate()
    {
        float dist = (PhysicButtonRb.transform.position - m_pbStartPosition).magnitude;
        if(dist >= ActivationDistance)
        {
            if(!m_activated)
            {
                Target.GetComponent<IActivable>().Activate();
                m_activated = true;
            }

            PhysicButtonRb.transform.position = m_pbStartPosition + Vector3.down * ActivationDistance;

        }
        else
        {
            if(m_activated)
            {
                m_activated = false;
                Target.GetComponent<IActivable>().Deactivate();
            }
        }
    }
	
}
