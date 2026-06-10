using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public Transform player;
    public float fadeDistance = 3f;

    private List<Renderer> allObjects = new List<Renderer>();

    void Update()
    {
        Vector3 dir = player.position - transform.position;
        float distance = Vector3.Distance(transform.position, player.position);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, distance);
        
        HashSet<Renderer> hitRenderers = new HashSet<Renderer>();

        // ƒqƒbƒg‚µ‚½•Ç
        foreach (var hit in hits)
        {
            Renderer r = hit.collider.GetComponent<Renderer>();

            if (r != null && hit.transform != player)
            {
                float d = Vector3.Distance(hit.transform.position, player.position);

                if (d < fadeDistance)
                {
                    SetAlphaSmooth(r, 0.3f);
                    hitRenderers.Add(r);

                    if (!allObjects.Contains(r))
                        allObjects.Add(r);
                }
            }
        }

        // ƒqƒbƒg‚µ‚Ä‚È‚¢•Ç‚ÍŒ³‚É–ß‚·
        foreach (var r in allObjects)
        {
            if (!hitRenderers.Contains(r))
            {
                SetAlphaSmooth(r, 1f);
            }
        }
    }

    void SetAlphaSmooth(Renderer r, float targetAlpha)
    {
        foreach (Material mat in r.materials)
        {
            Color c = mat.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * 5f);
            mat.color = c;
        }
    }
}
