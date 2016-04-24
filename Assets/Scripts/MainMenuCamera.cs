using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuCamera : MonoBehaviour {
    Camera m_camera;

    TextScript m_currentText = null;

    Animator m_animator;

    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_animator = GetComponent<Animator>();
    }

	void Update () {
        RaycastHit hit;
        if(Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            TextScript text = hit.collider.GetComponent<TextScript>();
            if(text == null)
            {
                if(m_currentText != null)
                {
                    m_currentText.Highlight(false);
                    m_currentText = null;
                }
            }
            else
            {
                if(m_currentText != text)
                {
                    if(m_currentText != null)
                        m_currentText.Highlight(false);
                    m_currentText = text;
                }

                if (!m_currentText.Highlighted)
                    m_currentText.Highlight(true);

                if(Input.GetMouseButtonDown(0))
                {
                    m_currentText.Click();
                    Click(m_currentText.Text);
                }
            }
        }
        else
        {
            if(m_currentText != null)
            {
                m_currentText.Highlight(false);
                m_currentText = null;
            }
        }
	}

    private void Click(string _text)
    {
        if (_text == "Exit")
            Application.Quit();
        else if(_text == "StartSingleplayer")
        {
            m_animator.SetTrigger("GoToLevelRoom");
        }
        else if(_text == "StartMultiplayer")
        {

        }

        if(_text == "Level01")
        {
            SceneManager.LoadScene("Vault");
        }
    }
}
