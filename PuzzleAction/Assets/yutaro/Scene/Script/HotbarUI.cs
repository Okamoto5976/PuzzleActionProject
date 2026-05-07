using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public Hotbar hotbar;
    public Image[] slotImages;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            PlayerItem item = hotbar.slots[i];

            if ((item != null))
            {
                slotImages[i].sprite = item.icon;
                slotImages[i].enabled = true;
            }
            else
            {
                slotImages[i].enabled = false;
            }
        }
    }
}
