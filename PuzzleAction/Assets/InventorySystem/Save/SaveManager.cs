using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private SaveData m_saveData;

    [SerializeField] private InventorySystem m_inventorySystem;

    private void Start()
    {

    }

    public void SaveToJson()
    {
        //JSON用の箱を作る
        SaveFileData fileData = new SaveFileData();

        // SaveDataの中身をコピーする
        fileData.activeItems = m_saveData.activeItems;
        fileData.passiveItems = m_saveData.passiveItems;

        //JSON文字列に変換する
        string json = JsonUtility.ToJson(fileData, true);

        string path = Application.persistentDataPath + "/save.json";

        // save.jsonに書き込む
        File.WriteAllText(path, json);

        Debug.Log("保存完了：" + path);
    }

    public void ClearSaveData()
    {
        // SaveDataの中身を削除
        m_saveData.activeItems.Clear();
        m_saveData.passiveItems.Clear();

        // セーブファイルを削除
        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        Debug.Log("SaveDataを削除しました");
    }

    public void LoadFromJson()
    {
        string path = Application.persistentDataPath + "/save.json";

        // セーブファイルが存在しなければ終了
        if (!File.Exists(path))
        {
            Debug.Log("セーブデータなし");
            return;
        }

        // jsonを読み込む
        string json = File.ReadAllText(path);

        // json → SaveFileDataに変換
        SaveFileData fileData = JsonUtility.FromJson<SaveFileData>(json);

        // SaveDataに渡す
        m_saveData.activeItems = fileData.activeItems;
        m_saveData.passiveItems = fileData.passiveItems;

        // Inventoryに反映
        m_inventorySystem.LoadInventory();

        Debug.Log("ロード完了");
    }
}
