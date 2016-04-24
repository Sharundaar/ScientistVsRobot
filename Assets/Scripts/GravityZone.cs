using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityZone : MonoBehaviour {

	private class GravityHistory
	{
		public GravityZone lastZone = null;
		public GravityZone currentZone = null;
	}

	[SerializeField]
	private Vector3 m_gravity = Physics.gravity;

	private List<Rigidbody> m_rigidbodies = new List<Rigidbody>();
	private static Dictionary<Rigidbody, GravityHistory> m_histories = new Dictionary<Rigidbody, GravityHistory>();

	void OnTriggerEnter(Collider _collider)
	{
		Rigidbody body = _collider.attachedRigidbody;
		if(body != null)
		{
			body.useGravity = false;
			m_rigidbodies.Add(body);

			GravityHistory history = null;
			if(!m_histories.TryGetValue(body, out history))
			{
				history = new GravityHistory();
				m_histories.Add(body, history);
			}

            if (history.currentZone != this)
            {
                history.lastZone = history.currentZone;
                history.currentZone = this;
            }

			PlayerController player = _collider.GetComponent<PlayerController>();
			if(player != null)
			{
				player.SetUpDirection(-m_gravity.normalized);
			}
		}

	}

	void OnTriggerExit(Collider _collider)
	{
		Rigidbody body = _collider.attachedRigidbody;
		if(body != null)
		{
            m_rigidbodies.Remove(body);

            GravityHistory history = m_histories[body];
			if (history.currentZone == this)
			{
			    history.currentZone = history.lastZone;
                history.lastZone = null;
			}

            if (history.lastZone == this)
                history.lastZone = null;

            if(history.currentZone == null)
            {
                body.useGravity = true;

                PlayerController player = _collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.SetUpDirection(-Physics.gravity.normalized);
                }
            }

            
        }

	}

	void FixedUpdate()
	{
		foreach(var body in m_rigidbodies)
		{
			if(m_histories[body].currentZone == this)
				body.AddForce(m_gravity * body.mass, ForceMode.Force);
		}
	}
}
