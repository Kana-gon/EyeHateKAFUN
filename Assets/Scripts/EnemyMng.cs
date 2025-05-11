using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMng : MonoBehaviour
{
    // Start is called before the first frame update
    public float waitSecond = 0.5f;
    public float waitSecondRandom = 0;
    public GameObject generateEnemy;
    [SerializeField] private Transform LeftEye;
    [SerializeField] private Transform RightEye;
    private IEnumerator generateCor;

    void OnEnable()
    {
        generateCor = GenerateEnemy();
        StartCoroutine(generateCor);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void destroyAllChildren(){
        
        foreach(Transform child in this.transform){
            Destroy(child.gameObject);
        }
    }
    IEnumerator GenerateEnemy()
    {
        while (true)
        {
            //Debug.Log("GENERATE");
            var generatedObject = Instantiate(generateEnemy,this.transform);
            
            var generatePointDecide = Random.Range(0,4);
            if(generatePointDecide == 0){
                generatedObject.transform.position = new Vector3(Random.Range(-10.5f,10.5f),6.75f,0);
            }else if(generatePointDecide == 1){
                generatedObject.transform.position = new Vector3(Random.Range(-10.5f,10.5f),-6.75f,0);
            }else if(generatePointDecide == 2){
                generatedObject.transform.position = new Vector3(-10.5f, Random.Range(-6.75f,6.75f), 0);
            }
            else if(generatePointDecide == 3){
                generatedObject.transform.position = new Vector3(10.5f, Random.Range(-6.75f, 6.75f), 0);
            }

            //TODO 近い方を狙う
            var targetDecide = Random.Range(0, 2);

            if (Vector3.Distance(generatedObject.transform.position,LeftEye.transform.position) < Vector3.Distance(generatedObject.transform.position, RightEye.transform.position))
            {//左目を狙う
                generatedObject.GetComponent<Enemy>().SetTarget(LeftEye);
                //Debug.Log("Left");
            }
            else
            {//右目を狙う
                generatedObject.GetComponent<Enemy>().SetTarget(RightEye);
                //Debug.Log("Right");
            }
            yield return new WaitForSeconds(waitSecond+(Random.Range(-waitSecond,waitSecond)/2));
            //StopCoroutine(generateCor);//かり
        }
    }

    public void GameOver()
    {
        destroyAllChildren();
        this.gameObject.SetActive(false);
    }
}
