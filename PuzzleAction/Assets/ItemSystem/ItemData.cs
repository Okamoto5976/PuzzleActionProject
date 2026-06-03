using UnityEngine;


//[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Datas/ItemData")]
[System.Serializable]
public class ItemData //: ScriptableObject
{
    //�A�C�e���^�C�v
    [Header("��{���")]
    [SerializeField] private int itemID;            //�A�C�e��ID
    [SerializeField] private string itemName;       //�A�C�e����
    [SerializeField] private Sprite itemIcon;       //�A�C�e���A�C�R��   
    [SerializeField] private Object itemPrefab;     //�A�C�e���̃v���n�u
    [SerializeField] private string description;    //�A�C�e������
    [SerializeField] private int maxStack;          //�ő�X�^�b�N��
    [SerializeField] private float dropRate;        //�h���b�v��
    [SerializeField] private int itemGrade;         //�A�C�e���̃O���[�h�i��F1=��ʁA2=���A�A3=�G�s�b�N�Ȃǁj
    [SerializeField] private GameObject dropPrefab; //�h���b�v�A�C�e���̃v���n�u
    [SerializeField] private int dropsize;          //�h���b�v�T�C�Y
    [SerializeField] private ItemType itemType;
    [SerializeField, Min(0)] private int price;


    public int ItemID { get => itemID; }// �A�C�e����ID
    public string ItemName { get => itemName; }// �A�C�e���̖��O
    public Sprite ItemIcon { get => itemIcon; } // �A�C�e���̃A�C�R��
    public string Description { get => description; }// �A�C�e���̐���
    public int MaxStack { get => maxStack; }// �ő�X�^�b�N��
    public float DropRate { get => dropRate; }// �h���b�v���i��F0.1=10%�̊m���Ńh���b�v�j
    public int ItemGrade { get => itemGrade; }// �A�C�e���̃O���[�h�i��F1=��ʁA2=���A�A3=�G�s�b�N�Ȃǁj
    public GameObject ItemPrefab { get => ItemPrefab; }
    public GameObject DropPrefab { get => dropPrefab; }
    public int DropSize { get => dropsize; } // �h���b�v�m���̏d��
    public ItemType ItemType => itemType;
    public int Price => price;
    public bool IsShopCompatible => price > 0;

}




