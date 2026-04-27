using UnityEngine;

public class enm : MonoBehaviour
{
 public void TakeDamage(int amount)
    {
        Debug.Log($"{gameObject.name}が{amount}のダメージをウケた");
    }
}
