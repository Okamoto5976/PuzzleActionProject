using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSOTest : MonoBehaviour
{
    [SerializeField] private SaveSO saveData;
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            saveData.AddItem(1);
            Debug.Log("ƒAƒCƒeƒ€’Ç‰Á");
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            foreach (var item in saveData.items)
            {
                Debug.Log("Id : " + item.itemID + item.count + "ŒÂ");
            }
        }
    }
}
