using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine
{
    public class IndicatorActivator : MonoBehaviour, IActivable
    {
        [SerializeField]
        private GameObject Target;

        [SerializeField]
        private Renderer RedRenderer;

        [SerializeField]
        private Renderer GreenRenderer;

        private static Color RedEmissionColor = Color.red;
        private static Color RedNonEmissionColor = new Color(0.01f, 0, 0);

        private static Color GreenEmissionColor = Color.green;
        private static Color GreenNonEmissionColor = new Color(0, 0.01f, 0);



        private void Start()
        {
            GreenRenderer.material.SetColor("_EmissionColor", GreenNonEmissionColor);
            RedRenderer.material.SetColor("_EmissionColor", RedEmissionColor);
        }

        public void Activate()
        {
            Target.GetComponent<IActivable>().Activate();

            GreenRenderer.material.SetColor("_EmissionColor", GreenEmissionColor);
            RedRenderer.material.SetColor("_EmissionColor", RedNonEmissionColor);
        }

        public void Deactivate()
        {
            Target.GetComponent<IActivable>().Deactivate();

            GreenRenderer.material.SetColor("_EmissionColor", GreenNonEmissionColor);
            RedRenderer.material.SetColor("_EmissionColor", RedEmissionColor);
        }
    }
}
