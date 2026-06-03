using UnityEngine;

    [System.Serializable] public class SaveData
{
    public const int rankCnt = 3;
    public int[] rank = new int[rankCnt];
}

public class ItemDataTest : MonoBehaviour
{
    private void Start()
    {
        DataManager dm = FindObjectOfType<DataManager>();

        dm.data.rank[0] = 100;
        dm.data.rank[1] = 200;
        dm.data.rank[2] = 300;

        Debug.Log("データのリセット");
    }
}
