using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DEngine
{
    public class MainMenuCamera : MonoBehaviour
    {
        private enum MainMenuState
        {
            MAIN,
            LEVEL,
            MULTIPLAYER,
        }

        private MainMenuState m_state = MainMenuState.MAIN;

        Camera m_camera;

        TextScript m_currentText = null;

        Animator m_animator;

        [SerializeField]
        private GameObject m_multiplayerMenu = null;

        void Start()
        {
            m_camera = GetComponent<Camera>();
            m_animator = GetComponent<Animator>();

            m_multiplayerMenu.SetActive(false);
            m_state = MainMenuState.MAIN;

            InitializeLevels();
        }

        void InitializeLevels()
        {
            foreach(var text in FindObjectsOfType<TextScript>())
            {
                if (text.Text == "Level03")
                    text.Enable(false);
            }
        }

        void Update()
        {
            if (m_state == MainMenuState.MULTIPLAYER)
                return;

            RaycastHit hit;
            if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                TextScript text = hit.collider.GetComponent<TextScript>();
                if (text == null)
                {
                    if (m_currentText != null)
                    {
                        m_currentText.Highlight(false);
                        m_currentText = null;
                    }
                }
                else
                {
                    if(!text.Disabled)
                    {
                        if (m_currentText != text)
                        {
                            if (m_currentText != null)
                                m_currentText.Highlight(false);
                            m_currentText = text;
                        }

                        if (!m_currentText.Highlighted)
                            m_currentText.Highlight(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            m_currentText.Click();
                            Click(m_currentText.Text);
                        }
                    }

                }
            }
            else
            {
                if (m_currentText != null)
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
            else if (_text == "StartSingleplayer")
            {
                m_animator.SetTrigger("GoToLevelRoom");
                m_state = MainMenuState.LEVEL;
            }
            else if (_text == "StartMultiplayer")
            {
                m_multiplayerMenu.SetActive(true);
                m_state = MainMenuState.MULTIPLAYER;
            }

            if (_text == "Level01")
            {
                SceneManager.LoadScene("Level1");
            }
            else if(_text == "Level02")
            {
                SceneManager.LoadScene("Level2");
            }
        }
    }
}