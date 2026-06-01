using UnityEngine;

public enum StatusType
{
    HP,�@�@�@�@�@�@ //�̗�
    Strength,�@�@�@ //�U����
    KnockBack,�@�@�@//�m�b�N�o�b�N��
    Defense,�@�@�@�@//�h��(�_���[�W��m�b�N�o�b�N����߂�j
    Speed,�@�@�@�@�@//�X�s�[�h
    DashSpeed,�@�@�@//�_�b�V��
    CriticalRate,�@ //�N���e�B�J����
    CriticalDamage, //�N���e�B�J���_���[�W �{��1.3�Ƃ�
    Dexterity,�@�@�@ //�������@��{100
    Agility,        //�U�����x(�G�̃X�e�[�^�X�j
    Vision,         //���邳(Player�̃X�e�[�^�X�j
    BreakRate,      //�ꌂ���i9999�_���̊m���j
    Stun,           //�X�^���U���傫���@20���ő�@�ϐ�200���h���ɂ͕K�v
    //��Ԉُ� Resistance�@100������b
    PoisonRes,      //�őϐ�
    StunRes,�@�@�@�@//�X�^���ϐ� �X�^���������Z �X�^���A�m�b�N�o�b�N����̕��A���x
    SlowRes,�@�@�@//�ݑ��ϐ�
    BlindRes�@�@�@�@//�Ӗڑϐ�
}

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Datas/EntityData")]
public class EntityData : ScriptableObject
{
    [SerializeField] private float m_hp;
    [SerializeField] private float m_str;
    [SerializeField] private float m_knockBack;
    [SerializeField] private float m_def;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_dashSpeed;
    [SerializeField] private float m_criticalRate;
    [SerializeField] private float m_criticalDamage;
    [SerializeField] private float m_dex;
    [SerializeField] private float m_agi;
    [SerializeField] private float m_vision;
    [SerializeField] private float m_breakRate;
    [SerializeField] private float m_stun;
    [SerializeField] private float m_recover;
    [SerializeField] private float m_poisonRes;
    [SerializeField] private float m_stunRes;
    [SerializeField] private float m_slowRes;
    [SerializeField] private float m_blindRes;


    public float HP { get => m_hp; }
    public float STR { get => m_str; }
    public float KnockBack { get => m_knockBack; }
    public float DEF { get => m_def; }
    public float Speed { get => m_speed; }
    public float DashSpeed { get => m_dashSpeed; }
    public float CriticalRate { get => m_criticalRate; }
    public float CriticalDamage { get => m_criticalDamage; }
    public float DEX { get => m_dex; }
    public float AGI { get => m_agi; }
    public float Vision { get => m_vision; }
    public float BreakRate { get => m_breakRate; }
    public float Stun { get => m_stun; }
    public float PoisonRes { get => m_poisonRes; }
    public float StunRes { get => m_stunRes; }
    public float SlowRes { get => m_slowRes; }
    public float BlindRes { get => m_blindRes; }
}
