
using UnityEngine;

namespace DEngine
{
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

        private TriggerVar m_interact = new TriggerVar();

        private IInteractable m_available = null;

        public delegate void InteractionAvailableEventHandler(object _sender, bool _available);
        public event InteractionAvailableEventHandler AvailabilityChanged;

        private void Update()
        {
            bool interact = m_interact.Data;

            RaycastHit hit;
            if (Physics.Raycast(View.position, View.forward, out hit, Range))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (interact && Time.time - m_lastInteractionStamp > InteractionCooldown)
                    {
                        interactable.Interact();
                        m_lastInteractionStamp = Time.time;
                    }

                    // raycast hit and no interactable history
                    if (m_available == null)
                    {
                        m_available = interactable;
                        OnAvailabilityChanged(true);
                    }
                    else
                    {
                        // raycast hit but the interactable is different from the last one
                        if (m_available != interactable)
                        {
                            m_available = interactable;
                            OnAvailabilityChanged(true);
                        }
                    }
                }
                else
                {
                    // raycast hit but no interactable
                    if (m_available != null)
                    {
                        m_available = null;
                        OnAvailabilityChanged(false);
                    }
                }
            }
            else
            {
                // Raycast doesn't hit and history
                if (m_available != null)
                {
                    m_available = null;
                    OnAvailabilityChanged(false);
                }
            }
        }

        private void OnAvailabilityChanged(bool _availability)
        {
            if (AvailabilityChanged != null)
                AvailabilityChanged(this, _availability);
        }

        public void Interact()
        {
            m_interact.Trigger();
        }
    }
}