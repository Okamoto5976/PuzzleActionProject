using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class InteractSystem
{
    public void TryInteract(Vector3 position, LayerMask layer, Entity entity)
    {

        Collider[] colliders = Physics.OverlapSphere(
            position,
            3f,
            layer
        );

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactable))
            {
                //Debug.Log("Interact");


                interactable.OnInteract(entity);
                return;
            }
        }
    }

    public bool CanInteract(Vector3 position, LayerMask layer)
    {
        Collider[] colliders = Physics.OverlapSphere(
           position,
           3f,
           layer
       );

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactable))
            {
                //Debug.Log("Interact");

                if(interactable != null)
                {
                    return true;
                }
                
            }
        }

        return false;
    }
}
