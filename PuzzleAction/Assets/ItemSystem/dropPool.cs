using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
//using static UnityEditor.PlayerSettings;



public class PoolItem
{
    public int id;
    public string name;
    public GameObject prefab;
    public int initialCount = 5;
}

public class DropPool : MonoBehaviour
{
    public GameObject prefab;
    //public PoolItem poolItem;
    [SerializeField]
    private List<PoolItem> ItemList = new();
    public PoolItem[] Items;
    //Dictionary<int, ObjectPool<GameObject>> pools;
    private ObjectPool<GameObject> pool;
    public int maxSize;
    public int DefaultCapacity;
    //�����ݒ�
    private void Awake() => pool = new ObjectPool<GameObject>(

            CreateItem,       // ������
            ItemGet,   // Get ��
            ReturnItem, // Release ��
            collectionCheck:true,        // �d���ԋp�Ȃǂ̈��S�`�F�b�N
            defaultCapacity: DefaultCapacity,
            maxSize: maxSize
        );
    //pools = new Dictionary<string,Queue<GameObject>>();



    private GameObject CreateItem()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    private void ItemGet(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void ReturnItem(GameObject obj)
    {
        obj.SetActive(false); // �A�C�e�����A�N�e�B�u�ɂ���
        //pools[].Dequeue();
        //pool[item].Enqueue(prefab); // �A�C�e�����v�[���ɖ߂�
    }



    public void Get()
    {
        
        foreach(var item in ItemList)
        {
            for(int i =0; i < item.initialCount; i++)
            {
                var obj = Instantiate(item.prefab);
                obj.SetActive(false);
                //queue.Enqueue(obj);
            }
 
           
        }
        //return;
    }


    public void ItemDrop(int Index, ItemRecieveData r_data )
    {
        GameObject obj= pool.Get();
        obj.transform.position = r_data.pos;
        obj.transform.rotation=Quaternion.Euler(r_data.dir);   
        object value = Instantiate(prefab, r_data.pos, Quaternion.Euler(r_data.dir));


    }








}
