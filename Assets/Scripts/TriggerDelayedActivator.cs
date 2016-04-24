using UnityEngine;
using System.Collections;

namespace DEngine
{
    public class TriggerDelayedActivator : MonoBehaviour
    {

        [SerializeField]
        private bool JustOnce = false;

        [SerializeField]
        private GameObject Target;

        [SerializeField]
        private bool InvertOnExit = false;

        [SerializeField]
        private float Delay;

        [SerializeField]
        private bool Deactivate = false;

        private bool m_triggered = false;
        private float m_timer = 0;

        void Update()
        {
            if (m_triggered)
            {
                m_timer += Time.deltaTime;
                if (m_timer >= Delay)
                {
                    m_triggered = false;
                    m_timer = 0;

                    if (!Deactivate)
                        Target.GetComponent<IActivable>().Activate();
                    else
                        Target.GetComponent<IActivable>().Deactivate();

                    if (InvertOnExit)
                        Deactivate = !Deactivate;

                    if (JustOnce)
                        Destroy(this);
                }
            }
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (!m_triggered)
                m_triggered = true;
        }

        void OnTriggerExit(Collider _collider)
        {
            if (InvertOnExit && !m_triggered)
            {
                m_triggered = true;
            }
        }
    }
}