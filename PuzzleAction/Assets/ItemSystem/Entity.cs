using UnityEngine;

public class Entity : MonoBehaviour
{
    public Entity entity;
    [SerializeField] private float baseValue;
    [SerializeField] private float speedValue;
    [SerializeField] private float distanceValue;

    public float BaseValue; 
    public string type;
    internal AttackItem.EffectType prefab;
    public Object obj;
    public enum Valueatype
    {
        AttackPower,
        HealPower,
        BuffPower,
        DebuffPower,
        TorapPower
    }
    private void Awake()
    {
        entity = GetComponent<Entity>();
        
        
    }
    public void Deactivate()
    {

    }
    public float BuffSet(float value)
    {
        BaseValue = this.baseValue;
        baseValue += value;
        return baseValue;
    }

    public void BaseValueReset(float value){  baseValue -= value;} 

    public void SetBaseValue() {  baseValue = 0f;}
}
