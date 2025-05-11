using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    public float progress = 0.0f;
    [SerializeField]private GameObject bar;
    [SerializeField]private float maxLength;
    [SerializeField]private float xmin;
    [SerializeField]private float xmax;
    private float progressTarget = 0;
    public IEnumerator progressAutochange;
    bool isTargetSeted = false;
    void Start()
    {
        progressAutochange = ProgressAutochange();
        StartCoroutine(progressAutochange);
    }

    // Update is called once per frame
    void Update()
    {
        if(isTargetSeted){
            progress = Mathf.Lerp(progress,progressTarget,0.5f);
            if(progress == progressTarget)
                isTargetSeted = false;
        }

        Vector3 scale = bar.transform.localScale;
        Vector3 position = bar.transform.localPosition;
        position.x = Mathf.Pow(0.01f * progress, 1 / 2f)*(xmax)+xmin;
        Debug.Log(position);
        scale.x=Mathf.Pow(0.01f*progress,1/2f);
        scale.x *= maxLength;
        bar.transform.localPosition = position;
        bar.transform.localScale = scale;
    }//*おいおい…プログレスバーができちまったよ…

    public void SetProgress(int value){
        progressTarget = value; 
        isTargetSeted = true;
    }
    public void AddProgress(int value){
        progressTarget = progress + value;
        isTargetSeted = true;
    }

    public void StopHeal(){
        StopCoroutine(progressAutochange);
    }
    public void RestartHeal(){
        StartCoroutine(progressAutochange);
    }
    public IEnumerator ProgressAutochange(){
        while(true){
            yield return new WaitForSeconds(0.1f);
            if(progress-0.1f > 0)progress -= 0.1f;
            else progress = 0;
        }
    }
}
