using UnityEngine;
using System.Collections;

namespace DEngine
{
    public class TextScript : MonoBehaviour
    {

        [SerializeField]
        private string m_text;

        public string Text
        {
            get { return m_text; }
            private set { m_text = value; }
        }

        public Color NormalColor = Color.gray;
        public Color HighlightColor = Color.white;
        public Color ClickColor = Color.green;
        public Color DisableColor = Color.black;

        private Renderer m_renderer;


        public bool Highlighted { get; set; }
        public bool Disabled { get; set; }

        private void Start()
        {
            m_renderer = GetComponent<Renderer>();
            m_renderer.material.color = NormalColor;
        }

        public void Enable(bool _enable)
        {
            Disabled = !_enable;

            if (Disabled)
                m_renderer.material.color = DisableColor;
            else
                StartCoroutine(Normal(0.0f));
        }

        public void Highlight(bool _enable)
        {
            if (Disabled)
                return;

            if (_enable)
                m_renderer.material.color = HighlightColor;
            else
                m_renderer.material.color = NormalColor;

            Highlighted = _enable;
        }

        public void Click()
        {
            if (Disabled)
                return;

            m_renderer.material.color = ClickColor;
            StartCoroutine(Normal(0.1f));
        }

        private IEnumerator Normal(float _seconds)
        {
            yield return new WaitForSeconds(_seconds);
            m_renderer.material.color = Highlighted ? HighlightColor : NormalColor;
        }
    }

}