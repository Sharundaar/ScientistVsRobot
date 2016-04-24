
using UnityEngine;

public class Interactor : MonoBehaviour
{

    [SerializeField]
    private float InteractionCooldown = 1.0f;

    [SerializeField]
    private float Range = 2.5f;

    [SerializeField]
    private Transform View;

    private float m_lastInteractionStamp = 0;

    public bool Interact { get; set; }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(View.position, View.forward, out hit, Range))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null && Interact && Time.time - m_lastInteractionStamp > InteractionCooldown)
            {
                interactable.Interact();
                m_lastInteractionStamp = Time.time;
            }
        }
    }
}