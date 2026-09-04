using UnityEngine;

public class InteractSystem
{
    public void TryInteract(Vector3 position, LayerMask layer, Entity entity)
    {
        Collider[] colliders = Physics.OverlapSphere(
            position,
            2.0f,
            layer
        );

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract(entity);
                return;
            }
        }
    }
}
