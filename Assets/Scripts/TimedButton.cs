using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine
{
    public class TimedButton : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private GameObject Target;

        [SerializeField]
        private float Delay = 1.0f;

        private bool m_active = false;
        private float m_timer = 0;

        private void Update()
        {
            if(m_active)
            {
                m_timer += Time.deltaTime;
                if (m_timer >= Delay)
                {
                    m_active = false;
                    Target.GetComponent<IActivable>().Deactivate();
                }
            }
        }

        public void Interact()
        {
            if (!m_active)
            {
                Target.GetComponent<IActivable>().Activate();
                m_active = true;
                m_timer = 0;
            }
        }
    }
}
