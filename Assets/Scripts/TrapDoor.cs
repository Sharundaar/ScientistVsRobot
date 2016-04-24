using UnityEngine;
using System.Collections;
namespace DEngine
{
    public class TrapDoor : MonoBehaviour, IActivable
    {
        private Animator m_animator;

        bool m_left_rewind = false;
        bool m_right_rewind = false;

        [SerializeField]
        private Rigidbody LeftPane;

        [SerializeField]
        private Rigidbody RightPane;

        private Quaternion m_LeftStartRotation;
        private Quaternion m_RightStartRotation;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
            LeftPane.isKinematic = true;
            RightPane.isKinematic = true;

            m_LeftStartRotation = LeftPane.transform.rotation;
            m_RightStartRotation = RightPane.transform.rotation;
        }

        private void Update()
        {
            if (m_left_rewind)
            {
                if (Quaternion.Angle(m_LeftStartRotation, LeftPane.rotation) < 1)
                {
                    LeftPane.rotation = m_LeftStartRotation;
                    LeftPane.isKinematic = true;
                    LeftPane.GetComponent<HingeJoint>().useMotor = false;
                    m_left_rewind = false;
                }
            }

            if (m_right_rewind)
            {
                if (Quaternion.Angle(m_RightStartRotation, RightPane.rotation) < 1)
                {
                    RightPane.rotation = m_RightStartRotation;
                    RightPane.isKinematic = true;
                    RightPane.GetComponent<HingeJoint>().useMotor = false;
                    m_right_rewind = false;
                }
            }
        }

        public void Activate()
        {
            LeftPane.isKinematic = false;
            RightPane.isKinematic = false;

            if (m_left_rewind)
            {
                LeftPane.GetComponent<HingeJoint>().useMotor = false;
                m_left_rewind = false;
            }

            if (m_right_rewind)
            {
                RightPane.GetComponent<HingeJoint>().useMotor = false;
                m_right_rewind = false;
            }
        }

        public void Deactivate()
        {
            m_left_rewind = true;
            m_right_rewind = true;
            LeftPane.GetComponent<HingeJoint>().useMotor = true;
            RightPane.GetComponent<HingeJoint>().useMotor = true;
        }
    }
}