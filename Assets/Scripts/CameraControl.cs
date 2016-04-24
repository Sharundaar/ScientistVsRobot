using UnityEngine;
using System.Collections;

namespace DEngine
{
    public class CameraControl : MonoBehaviour {

        private float m_targetAngle = 0;
        public float TargetAngle {
            get { return m_targetAngle; }

            set
            {
                m_targetAngle = Mathf.Clamp(value, -80, 80);
            }
        }

        [SerializeField]
        private Transform View;

        private void Update()
        {
            float currentAngle = View.localRotation.eulerAngles.x % 360 ; // angle between [-180, 180]
            if (currentAngle > 180)
                currentAngle = currentAngle - 360.0f;
            View.Rotate(TargetAngle - currentAngle, 0, 0, Space.Self);
        }
    }
}
