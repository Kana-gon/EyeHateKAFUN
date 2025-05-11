using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressMng : MonoBehaviour
{
    // Start is called before the first frame update
    public float progress = 0.0f;
    // [SerializeField] private float maxLength;
    // [SerializeField] private float xmin;
    // [SerializeField] private float xmax;
    [SerializeField] private GameObject stressMark;
    [SerializeField] private int stressMaxNum;
    [SerializeField] private TutrialMng tutrialMng;
    private int stressDelta;
    private float progressTarget = 0;
    private List<GameObject> stressMarkList = new List<GameObject>();
    public IEnumerator AutoHeal;
    public IEnumerator AutoStressing;
    bool isTargetSeted = false;
    void Start()
    {
        AutoHeal = ProgressAutochange(0.1f);
        AutoStressing = ProgressAutochange(-0.3f);
        StartCoroutine(AutoHeal);
        stressDelta = 100/stressMaxNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargetSeted)
        {
            progress = Mathf.Lerp(progress, progressTarget, 0.5f);
            if (progress == progressTarget)
                isTargetSeted = false;
        }
        if(progress < 0)progress = 0;
        int stressMarkNum = (int)(progress/stressDelta);
        //Debug.Log(stressMarkNum);
        while(stressMarkList.Count < stressMarkNum){
            if(tutrialMng.onceSwitch[4] == false&&tutrialMng.eventSwitch >= 2&& tutrialMng.eventSwitch != 55){
                tutrialMng.eventSwitch = 4;
            }
            var stressMarkIns = Instantiate(stressMark,this.transform);
            var randomPosition = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4f, 4f), 0);
            while(randomPosition.x > -3.5f && randomPosition.x < 3.5f&&randomPosition.y >-1.5f && randomPosition.y < 1.5f)
                randomPosition = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4f, 4f), 0);
            stressMarkIns.transform.position = randomPosition;
            var randomRote = stressMarkIns.transform.rotation;
            stressMarkIns.transform.rotation = Quaternion.AngleAxis(Random.Range(0,360), Vector3.forward);
            stressMarkList.Add(stressMarkIns);
        }
        if(stressMarkList.Count > stressMarkNum)
        {
            //Debug.Log(stressMarkList[stressMarkList.Count - 1].name); //
            var LastMark = stressMarkList[stressMarkList.Count - 1].GetComponent<StressMark>();
            stressMarkList.RemoveAt(stressMarkList.Count -1); 
            LastMark.DestroyThis();
        }
    }

    public void SetProgress(int value)
    {
        progressTarget = value;
        isTargetSeted = true;
    }
    public void AddProgress(int value)
    {
        progressTarget = progress + value;
        isTargetSeted = true;
    }

    public void StopHeal()
    {
        StopCoroutine(AutoHeal);
    }
    public void RestartHeal()
    {
        StartCoroutine(AutoHeal);
        if(tutrialMng.onceSwitch[5] == false&&tutrialMng.eventSwitch > 3&& tutrialMng.eventSwitch < 10)
        {
            tutrialMng.eventSwitch = 6;
        }
    }

    public void StartStressing(){
        StartCoroutine(AutoStressing);
    }
    public void StopStressing(){
        StopCoroutine(AutoStressing);
    }
    public IEnumerator ProgressAutochange(float delta)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (progress - delta > 0) progress -= delta;
            else progress = 0;
        }
    }
}