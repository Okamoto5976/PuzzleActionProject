using System;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour
{

    public ItemManager.EffectType Type { get; private set; }
    public float BaseValue;

    // 初期化処理
    public void Initialize( ItemManager.EffectType type, float baseValue)
    { 
        Type = type; // ここでTypeを設定
        BaseValue = baseValue; // ここでBaseValueを設定
        gameObject.SetActive(true); // オブジェクトをアクティブにする
    }

    // プールに戻すとき
    public void Deactivate()
    {
        gameObject.SetActive(false); // オブジェクトを非アクティブにする
    }

    internal void BuffSet(BuffItem.BuffType buffType, float value, float buffDuration)
    {

        throw new NotImplementedException();// ここでbuffTypeとvalueを使用して、エンティティにバフを適用するロジックを実装します。
        
    }
    // エンティティに効果を適用するメソッド
    public void ApplyEffect(float effectValue)
    {
        // ここでエンティティに効果を適用するロジックを実装します。
        // 例えば、攻撃力を上げる場合は、BaseValueにeffectValueを加算するなどの処理を行います。
        BaseValue += effectValue;
    }
    // エンティティにバフを適用するメソッド
    public void ApplyBuff(float buffValue, float buffDuration)
    {
        // ここでエンティティにバフを適用するロジックを実装します。
        // 例えば、攻撃力を上げる場合は、BaseValueにbuffValueを加算し、buffDurationの時間だけその効果を持続させるなどの処理を行います。
        BaseValue += buffValue;
        // バフの持続時間を管理するロジックもここに追加します。
    }
    // エンティティのBaseValueをリセットするメソッド
    internal void BaseValueReset(Entity entity)
    {
        // ここでエンティティのBaseValueを元に戻すロジックを実装します。
        // 例えば、バフの効果が切れたときにBaseValueを初期値にリセットするなどの処理を行います。
        entity.BaseValue = 0; // ここでは仮に0にリセットしていますが、実際には初期値に戻す処理を実装してください。
    }

}