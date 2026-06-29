using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    ItemData data;
    Item item; // �A�C�e���̃N���X
    float radius = 3f; // �A�C�e�����E�����߂̔��a
    float timeToReturn = 5f; // �A�C�e���������I�ɖ߂�܂ł̎���
    DropPool pool; // �A�C�e�����Ǘ�����h���b�v�v�[���̃N���X
    private GameObject prefab;
    //public event Action m_event;
    
    ////player�̍��W�����g�̔��a�Rm���Ȃ��Ɂ@�v���C���[����������@�v���C���[�ɃA�C�e����n���B
    private void ItemGet(Collider other)
    {
        if (Vector3.Distance(transform.position, other.transform.position) <= radius)
        {
            if (pool == null)
            {
                Debug.LogError("Pool is not assigned.");
                return;
            }
            if (other.CompareTag("Player"))
            {
                //Add.inventory();
                Return();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity == null) return;

        bool added = entity.ReceiveItem(item);
        if (!added) return; // �C���x���g���������ς����Ȃ�E��Ȃ�

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        CancelInvoke();

        if (pool != null)
        {
            //Pool�ɕԂ�����
            pool.ReturnItem( prefab);
        }
    }
}