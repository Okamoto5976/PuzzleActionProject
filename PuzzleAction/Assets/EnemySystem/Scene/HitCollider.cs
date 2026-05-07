using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public enum HitType
    {
        EnemyAttack,
    }
    public HitType hitType;

    private void OnTriggerEnter(Collider other)
    { 
      if (other.CompareTag("Player"))
      {
        Debug.Log("“GUŒ‚ƒqƒbƒgI");
      }
                    
    }
}

      

