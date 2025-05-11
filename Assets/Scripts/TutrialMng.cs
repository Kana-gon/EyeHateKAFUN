using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutrialMng : MonoBehaviour
{
    [SerializeField]private TextMeshPro tutrialText;
    public bool[] keyPushed = {false,false};
    public bool[] onceSwitch = new bool[10];
    public int eventSwitch = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<onceSwitch.Length;i++){
            onceSwitch[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"eventSwitch:{eventSwitch}");
        if(keyPushed[0] == true && keyPushed[1] == true&&eventSwitch == 0)eventSwitch = 1;
        if(eventSwitch == 1){SetTutrialText("左クリック：涙を流す");}
        if(eventSwitch == 2){SetTutrialText("");}
        if(eventSwitch == 3&&onceSwitch[3]==false){SetTutrialText("涙が枯れた。目を閉じて弾数を回復");onceSwitch[3] = true;}
        if(eventSwitch == 4&&onceSwitch[4]==false){SetTutrialText("両目を閉じ続けたり、花粉が直撃すると怒りが貯まる\n怒りすぎるとゲームオーバー"); onceSwitch[4] = true; StartCoroutine(TutrialDisappear(7));}
        if(eventSwitch == 6&&onceSwitch[5] == false) { SetTutrialText("両目を開けていると、怒りは少しずつ回復する"); onceSwitch[5] = true; StartCoroutine(TutrialDisappear(7)); }
        if(eventSwitch == 100) SetTutrialText("");
        if(eventSwitch == 55){ SetTutrialText("大花粉主　カフリエル"); tutrialText.fontSize = 13;StartCoroutine(TutrialDisappear(5));}
        //TODO eventswitchだけでよくね？
        //TODO コルーチンで自動で消えるようにする
    }
    public void SetTutrialText(string setText){
        tutrialText.text = setText;
    }
    public IEnumerator TutrialDisappear(int waitSecond){
        yield return new WaitForSeconds(waitSecond);
        SetTutrialText("");
        tutrialText.fontSize = 6;
        eventSwitch = 100;
    }
}
