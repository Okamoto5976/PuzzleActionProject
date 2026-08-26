using UnityEngine;

public class TestTexture : MonoBehaviour
{
    [SerializeField] private Renderer m_renderer;

    private void Start()
    {
        // 10×10のマップ
        bool[,] map = new bool[10, 10];

        // テスト用に適当に形を作る
        map[4, 4] = true;
        map[5, 4] = true;
        map[6, 4] = true;

        map[4, 5] = true;
        map[5, 5] = true;
        map[6, 5] = true;

        map[5, 6] = true;
        map[5, 7] = true;

        // Textureを作る
        Texture2D texture = new Texture2D(10, 10);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Color color = map[x, y] ? Color.white : Color.black;

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        // Rendererに表示
        //m_renderer.material.mainTexture = texture;
        m_renderer.material.SetTexture("_MainTex", texture);
        
    }

}
