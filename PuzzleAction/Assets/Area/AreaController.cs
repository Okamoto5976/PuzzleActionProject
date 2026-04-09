using UnityEngine;

public enum AreaType
{
    Damage,
    Summon,
    Normal,
    Shop
}

public class AreaController : MonoBehaviour
{
    public AreaType CurrentArea;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExecuteEffect();
        }
    }

    void ExecuteEffect()
    {
        switch (CurrentArea)
        {
            case AreaType.Damage:
                Debug.Log("åpë±É_ÉÅéÛÇØÇΩí…");
                break;
            case AreaType.Summon:
                Debug.Log("è¢ä´");
                    break;
            case AreaType.Normal:
                Debug.Log("Ç»ÇÒÇ‡Ç»Ç¢ñ îíñ°Ç‡Ç»Ç¢ëê");
                    break;
            case AreaType.Shop:
                Debug.Log("îÉÇ¢ï®ÇµÇÊ...ìêÇ›ÇÕÇ≈Ç´Ç»Ç¢ÇÃÇ©Ç»");
                    break;
        }
    }
}
