using UnityEngine;
using System.Collections;


namespace DEngine
{
    public class Button : MonoBehaviour, IInteractable {

	    [SerializeField]
	    private GameObject Target;

        [SerializeField]
        private bool Deactivate = false;

        private Animator m_animator;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

	    public void Interact()
	    {
            if(!Deactivate)
		        Target.GetComponent<IActivable>().Activate();
            else
                Target.GetComponent<IActivable>().Deactivate();
            m_animator.SetTrigger("Push");
	    }
    }
}
