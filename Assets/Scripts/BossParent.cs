using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossParent : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] public float hp = 400;
    [SerializeField] public float ATK = 100;
    [SerializeField] private Image attackLine;
    [SerializeField] private AudioSource audio1;
    [SerializeField] private Transform targetTmp;
    [SerializeField] private GameObject[] enemyMngs = new GameObject[5];
    [SerializeField]private GameObject[] stressMarks = new GameObject[4];
    [SerializeField]private GameObject kemuri;
    private Vector3 targetPos;
    private Vector3 direction;
    [SerializeField] private BossArm RightArm;
    [SerializeField] private BossArm LeftArm;
    [SerializeField] private Animator Animator;
    //[SerializeField] float speed = 0.6f;
    // Start is called before the first frame update
    IEnumerator actionTimer;
    private int chargeVec=0;
    [SerializeField]StressMng stressMng;
    [SerializeField]bool[] heal = new bool[3];
    private void Awake()
    {
        attackLine.enabled = false;
        foreach(var stress in stressMarks){
            stress.SetActive(false);
        }
        kemuri.SetActive(false);
        //gameObject.SetActive(false);
    }
    void OnEnable()
    {
        actionTimer = ActionTimer();
        StartCoroutine(actionTimer);
    }
    // public void SetTarget(Transform target)
    // {
    //     //_rigid = GetComponent<Rigidbody2D>();
    //     thisTransform = transform;
    //     targetPos = target.position;
    //     targetPos.z = 0;

    //     //速度を求める
    //     direction = targetPos - transform.position;
    //     direction.z = 0;
    //     direction.Normalize();
    //     //Debug.Log(direction);
    //     // //向きを求める
    //     // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
    //     // transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle);
    //     //GetComponent<SpriteRenderer>().enabled = true;
    // }
    public void Hit(float damage)
    {
        //Debug.Log(damage);
        //Animator.Play("Damaged");
        hp -= damage;
        if (hp <= 0)
        {
            GameObject.Find("GameMng").GetComponent<GameMng>().isClear = true;
            Destroy(gameObject);
        }
        //Debug.Log("HIT!");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Eye"))
        {
            collision.gameObject.GetComponent<eyes>().Hit(ATK);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(hp < 3500&&hp >= 2500){
            stressMarks[0].SetActive(true);
            LeftArm.waitSecond =2.2f;
            RightArm.waitSecond = 2.2f;
        }else if(hp < 2500 && hp >= 1500){
            stressMarks[1].SetActive(true);
            LeftArm.waitSecond = 1.8f;
            RightArm.waitSecond = 1.8f;
            enemyMngs[0].SetActive(true);
            if(heal[0]==false){stressMng.AddProgress(-40);heal[0] = true;}
        }
        else if(hp <1500&&hp >= 500)
        {
            stressMarks[2].SetActive(true);
            LeftArm.waitSecond = 3.0f;
            RightArm.waitSecond = 3.0f;
            enemyMngs[1].SetActive(true);
            enemyMngs[0].GetComponent<EnemyMng1>().GameOver();
            if (heal[1] == false) { stressMng.AddProgress(-40); heal[1] = true; }

        }
        else if(hp <500){
            stressMarks[3].SetActive(true);
            kemuri.SetActive(true);
            enemyMngs[1].SetActive(false);
            LeftArm.waitSecond = 0.8f;
            RightArm.waitSecond = 0.8f;
            if (heal[2] == false) { stressMng.AddProgress(-40); heal[2] = true; }

        }
        // //Debug.Log("updateing");
        // //_rigid.velocity = direction * speed * Time.deltaTime;
        // var newPos = transform.position + direction * speed * Time.deltaTime;
        // //Debug.Log($"{gameObject.name}:{newPos}");
        // transform.position = newPos;
    }

    private IEnumerator ActionTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(7, 15));
            var chargeTimer = ChargeTimer();
            StartCoroutine(chargeTimer);
        }
    }
    private IEnumerator ChargeTimer(){//突進のほうね
                                      //Debug.Log("CHARGE!");
        attackLine.enabled = true;

        if (chargeVec == 0){

            Animator.Play("Charge");
            chargeVec = 1;
        }else{

            Animator.Play("Charge_2");
            chargeVec = 0;
        }
        yield return new WaitForSeconds(1);
        attackLine.enabled = false;
        //SetTarget(targetTmp);
    }
    void PlayAudio(){
        audio1.Play();

    }
    public void GameOver(){
        StopCoroutine(actionTimer);
        Animator.Play("GameOver");
    }
    public void animEndActiveFalse(){
        gameObject.SetActive(false);
    }
}
