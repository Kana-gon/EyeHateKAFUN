using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;


public class EnemyMng1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float waitSecond = 0.5f;
    public float waitSecondRandom = 0;
    public GameObject generateEnemy;
    [SerializeField] private GameObject[] images = new GameObject[4];
    [SerializeField] private Transform LeftEye;
    [SerializeField] private Transform RightEye;
    private IEnumerator generateCor;


    void OnEnable()
    {
        foreach (var image in images)
        {
            image.SetActive(false);
        }
        generateCor = GenerateEnemy();
        StartCoroutine(generateCor);

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void destroyAllChildren()
    {

        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void GameOver(){
        foreach(var bar in images){
            bar.SetActive(false);
        }
        StopCoroutine(generateCor);
        destroyAllChildren();
        this.gameObject.SetActive(false);
    }
    IEnumerator GenerateEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitSecond + (Random.Range(-waitSecond, waitSecond) / 2));

            var generatedObject = Instantiate(generateEnemy,transform);
            
            var generatePointDecide = Random.Range(0,4);
            //ここを変更
            if(generatePointDecide == 0){//左上
                images[0].SetActive(true);
                generatedObject.transform.position = new Vector3(-4.44f, 6.55f, 0);
            }
            else if(generatePointDecide == 1){//左下
                images[1].SetActive(true);
                generatedObject.transform.position = new Vector3(-5.26f, -7.23f, 0);
            }
            else if(generatePointDecide == 2){//右上
                images[2].SetActive(true);
                generatedObject.transform.position = new Vector3(4.73f, 6.6f, 0);
            }
            else if(generatePointDecide == 3){//右下
                images[3].SetActive(true);
                generatedObject.transform.position = new Vector3(4.62f, -7.06f, 0);
            }

            yield return new WaitForSeconds(1);

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
            foreach (var image in images)
            {
                image.SetActive(false);
            }
            //StopCoroutine(generateCor);//かり
        }
    }
}
