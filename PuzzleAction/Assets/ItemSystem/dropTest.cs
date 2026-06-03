using UnityEngine;
using UnityEngine.UIElements;

public class dropTest : MonoBehaviour
{

    dropPool dropPool;
    DropdownMenuItem dropItem;
    //アイテムをドロップする
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dropPool.DropItem(transform.position); //スペースキーを押したときにアイテムをドロップする
        }
    }
}
    
