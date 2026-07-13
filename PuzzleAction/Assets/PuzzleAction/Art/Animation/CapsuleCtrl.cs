using UnityEngine;

public class CapsuleCtrl : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
    anim=GetComponent<Animator>();
    }
    void Update()
    {
        //パラメーターを設定する
        anim.SetBool("Bool", true);
        anim.SetFloat("Speed", 3.5f);
        anim.SetInteger("Number", 1);
        anim.SetTrigger("Trigger");
        anim.ResetTrigger("Trigger");

        //パラメーター管理
        bool b = anim.GetBool("Bool");
        float speed = anim.GetFloat("Speed");
        int number = anim.GetInteger("Number");
        //Triggerは取得できない
    }
}
