using UnityEngine;
using System.Collections;

public class Plateform : MonoBehaviour, IActivable {

    [SerializeField]
    private bool Activated = false;

    [SerializeField]
    private Vector3 EndPosition;

    [SerializeField]
    private float Frequency = 1.0f;

    private Vector3 m_startPosition;
    private float m_timer = 0;

    // Use this for initialization
    private void Start () {
        m_startPosition = transform.position;
	}
	
	private void FixedUpdate()
    {
        if (Activated)
        {
            m_timer += Time.fixedDeltaTime;
            transform.position = (EndPosition - m_startPosition) * ((Mathf.Sin(m_timer * 2.0f * Mathf.PI * Frequency - Mathf.PI / 2.0f) + 1.0f) / 2.0f) + m_startPosition;
        }

    }

    void OnTriggerEnter(Collider _collider)
    {
        _collider.transform.SetParent(transform);
    }

    void OnTriggerExit(Collider _collider)
    {
        _collider.transform.SetParent(null);
    }

    public void Activate()
    {
        Activated = true;
    }

    public void Deactivate()
    {
        Activated = false;
    }
}
