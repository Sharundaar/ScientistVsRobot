using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DEngine
{
    public class EndLevelScript : MonoBehaviour, IActivable
    {

        [SerializeField]
        private string NextLevel = "";

        public void Activate()
        {
            SceneManager.LoadScene(NextLevel);
        }

        public void Deactivate()
        {

        }
    }

}
