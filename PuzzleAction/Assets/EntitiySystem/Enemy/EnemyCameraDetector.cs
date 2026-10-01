using UnityEngine;

public class EnemyCameraDetector : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask enemyLayer;
    public LayerMask bossLayer;
    public AudioSource enemyBgm;
    public AudioSource bossBgm;

    private bool isBgmPlaying = false;

    void Update()
    {
        bool bossVisible = IsLayerVisible(bossLayer);
        bool enemyVisible = IsLayerVisible(enemyLayer);

        if (bossVisible)
        {
            if (!bossBgm.isPlaying)
                bossBgm.Play();

            if (!enemyBgm.isPlaying)
                enemyBgm.Stop();
        }
        else if (enemyVisible)
        {
            if (!enemyBgm.isPlaying)
                enemyBgm.Play();

            if (!bossBgm.isPlaying)
                bossBgm.Stop();
        }
        else
        {
            if (enemyBgm.isPlaying)
                enemyBgm.Stop();

            if (bossBgm.isPlaying)
                bossBgm.Stop();
        }
    }
         bool IsLayerVisible(LayerMask layer)
    { 
        Collider[] enemies = Physics.OverlapSphere(mainCamera.transform.position, 100f, enemyLayer);
        
        foreach(var enemy in enemies)
        {
            Vector3 viewportPos=mainCamera.WorldToViewportPoint(enemy.transform.position);

            if(viewportPos.z > 0 && viewportPos.x > 0 &&viewportPos.x<1&&viewportPos.y>0&&viewportPos.y<1)
            {
                return true;
            }
        }
        return false;
    }
}
