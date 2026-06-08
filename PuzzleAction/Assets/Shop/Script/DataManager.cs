using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    [HideInInspector] public SaveData data;
    string filepath;
    string fileName = "Data.json";

    void Awake()
    {
        filepath = Application.persistentDataPath + "/" + fileName;   

        if (!File.Exists(filepath))
        {
            data = new SaveData();

            //テスト用データ
            data.ItemID.Add(8);
            data.ItemID.Add(5);
            data.ItemID.Add(7);
            data.ItemID.Add(6);

            data.money = 1000;
            Save(data);
            Debug.Log("初回セーブ作成");
        }
        data = Load(filepath);
        Debug.Log(JsonUtility.ToJson(data,true));
        Debug.Log("ロード完了");
    }
    public void AddItem(int id)
    {
        data.ItemID.Add(id);
        Debug.Log("アイテム追加" + id);
        Save(data);
    }
    public void Save(SaveData data)
    {
        //JSONに変換
        string json = JsonUtility.ToJson(data, true);

        StreamWriter wr = new StreamWriter(filepath, false);
        //JSONに書き込む
        wr.WriteLine(json);
        wr.Close();

        Debug.Log("保存");
    }
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();
        return JsonUtility.FromJson<SaveData>(json);
    }

    void OnDestroy()
    {
        Save(data);   
    }
}


//List<int> inventry;


//inventry[8, 8, 5, 6] もらう

//json書き込む

//saveを取り出すときは

//