using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using unityroom.Api;

public class GameMng : MonoBehaviour
{
    [SerializeField] private StressMng stressMng;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject clearImage;
    [SerializeField] private GameObject toTitle;
    [SerializeField] public bool DEBUGMODE = false;
    [SerializeField] private GameObject warningEffect;
    [SerializeField] private GameObject[] bars = new GameObject[4];
    [SerializeField]private AudioSource audio1;
    [SerializeField]private AudioSource audio2;
    [SerializeField] private AudioSource audio3;
    [SerializeField] private GameObject[] enemyMngs = new GameObject[5];
    [SerializeField] private TutrialMng tutrialMng;
    [SerializeField] BGMManager bGMManager;
    private bool[] enemyMngsSwitch = new bool[10];
    private bool isGameOver = false;
    public bool isClear = false;
    public int ScoreNoHitComboMax=0;
    private bool gameOverProcessDone = false;
    private bool clearProcessDone = false;
    [SerializeField] private Animator fadeOut;
    private IEnumerator gameTimer;
    private float bossTimer=0.0f;
    private float liveTimer = 0.0f;
    private bool isBossBattle = false;

    // Update is called once per frame
    void Start()
    {
        gameOverImage.SetActive(false);
        clearImage.SetActive(false);
        warningEffect.SetActive(false);
        toTitle.SetActive(false);
        gameTimer = GameTimer();
        for (int i = 0; i < enemyMngs.Length; i++)
        {
            if (enemyMngs[i] != null)
                enemyMngs[i].SetActive(false);
        }
        foreach (var image in bars)
        {
            image.SetActive(false);
        }
        enemyMngs[0].SetActive(true);
        StartCoroutine(gameTimer);
    }
    void Update()
    {
        if (DEBUGMODE == false)
        {
            if(isBossBattle){
                bossTimer += Time.deltaTime;
            }
            liveTimer += Time.deltaTime;
            if (stressMng.progress > 100)
            {
                isGameOver = true;
            }
            if (isClear && clearProcessDone == false)
            {
                isBossBattle = false;
                UnityroomApiClient.Instance.SendScore(1, bossTimer, ScoreboardWriteMode.HighScoreAsc);
                UnityroomApiClient.Instance.SendScore(2, liveTimer, ScoreboardWriteMode.HighScoreAsc);
                clearProcessDone = true;
                StopCoroutine(gameTimer);
                bGMManager.ChangeBGM(0);
                foreach (var enemyMng in enemyMngs)
                {
                    if (enemyMng != null)
                    {
                        if (enemyMng.GetComponent<EnemyMng>())
                            enemyMng.GetComponent<EnemyMng>().GameOver();
                        if (enemyMng.GetComponent<EnemyMng1>())
                            enemyMng.GetComponent<EnemyMng1>().GameOver();
                        if (enemyMng.GetComponent<BossParent>())
                            enemyMng.GetComponent<BossParent>().GameOver();
                    }
                }
                GameObject.Find("RightEye").GetComponent<eyes>().GameClear();
                GameObject.Find("LeftEye").GetComponent<eyes>().GameClear();
                StartCoroutine(ClearGame());
                //UnityroomApiClient.Instance.SendScore(1, StaticClassData.ScoreNoHitCombo, ScoreboardWriteMode.HighScoreDesc);
                fadeOut.Play("appear");
            }
            else if (isGameOver && gameOverProcessDone == false)
            {
                UnityroomApiClient.Instance.SendScore(2, liveTimer, ScoreboardWriteMode.HighScoreAsc);
                gameOverProcessDone = true;
                bGMManager.ChangeBGM(0);
                StopCoroutine(gameTimer);
                foreach (var enemyMng in enemyMngs)
                {
                    if (enemyMng != null)
                    {
                        if (enemyMng.GetComponent<EnemyMng>())
                            enemyMng.GetComponent<EnemyMng>().GameOver();
                        if (enemyMng.GetComponent<EnemyMng1>())
                            enemyMng.GetComponent<EnemyMng1>().GameOver();
                        if(enemyMng.GetComponent<BossParent>())
                            enemyMng.GetComponent<BossParent>().GameOver();
                    }
                }
                GameObject.Find("RightEye").GetComponent<eyes>().GameOver();
                GameObject.Find("LeftEye").GetComponent<eyes>().GameOver();
                StartCoroutine(GameOverStressAdding());
                //UnityroomApiClient.Instance.SendScore(1, StaticClassData.ScoreNoHitCombo, ScoreboardWriteMode.HighScoreDesc);
                fadeOut.Play("appear");
                //Time.timeScale =0.0f;//!使っちゃいけないらしいので没
            }

            
        }
    }
    IEnumerator ClearGame(){
        audio3.Play();
        yield return new WaitForSeconds(3);
        clearImage.SetActive(true);
        toTitle.SetActive(true);
        fadeOut.Play("disappear");
        bGMManager.ChangeBGM(3);
    }
    IEnumerator GameOverStressAdding()
    {
        var i = 1.0f;
        while (i > 0.00005f)
        {
            audio2.Play();
            //Debug.Log(i);
            i = Mathf.Lerp(i, 0, 0.2f);
            yield return new WaitForSeconds(i);
            stressMng.AddProgress(20);
        }
        gameOverImage.SetActive(true);
        toTitle.SetActive(true);

        fadeOut.Play("disappear");
        bGMManager.ChangeBGM(4);
    }
    IEnumerator GameTimer()
    {
        var timer = StaticClassData.StartChapter;
        while (true)
        {
            //Debug.Log(timer);
            if (timer >= 30 && enemyMngsSwitch[0] == false)
            {
                enemyMngs[1].SetActive(true);
                enemyMngsSwitch[0] = true;
            }
            if (timer >= 60 && enemyMngsSwitch[1] == false)
            {
                enemyMngs[0].SetActive(false);
                enemyMngs[2].SetActive(true);
                enemyMngs[3].SetActive(true);
                //Debug.Log("switched");
                enemyMngsSwitch[1] = true;
            }
            if (timer >= 100 && enemyMngsSwitch[2] == false)
            {
                isBossBattle = true;
                StaticClassData.BossDiscovered = true;
                audio1.Play();
                bGMManager.ChangeBGM(2);
                warningEffect.SetActive(true);
                enemyMngs[1].GetComponent<EnemyMng1>().GameOver();
                enemyMngs[2].SetActive(false);
                enemyMngs[2].GetComponent<EnemyMng>().destroyAllChildren();
                enemyMngs[3].SetActive(false);
                enemyMngs[4].SetActive(true);
                enemyMngsSwitch[2] = true;
                tutrialMng.eventSwitch = 55;
            }
            yield return new WaitForSeconds(5);
            timer += 5;
        }
        //30,60,90

    }
}
