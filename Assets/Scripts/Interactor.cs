
using UnityEngine;

public class Interactor : MonoBehaviour
{
    class TriggerVar
    {
        private bool m_data = false;
        public bool Data
        {
            get
            {
                if (m_data)
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
    private float InteractionCooldown = 1.0f;

    [SerializeField]
    private float Range = 2.5f;

    [SerializeField]
    private Transform View;

    private float m_lastInteractionStamp = 0;

    private TriggerVar m_interact;

    private void Update()
    {
        bool interact = m_interact.Data;

        RaycastHit hit;
        if(Physics.Raycast(View.position, View.forward, out hit, Range))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null && interact && Time.time - m_lastInteractionStamp > InteractionCooldown)
            {
                interactable.Interact();
                m_lastInteractionStamp = Time.time;
            }
        }
    }

    public void Interact()
    {
        m_interact.Trigger();
    }
}