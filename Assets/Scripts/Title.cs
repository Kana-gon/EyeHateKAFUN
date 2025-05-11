using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject fade;
    [SerializeField]AudioSource audio1;
    void Start()
    {
        fade.SetActive(false);
    }

    void OnMouseDown()
    {
        StaticClassData.StartChapter = 0;
        fade.SetActive(true);
        //fade.GetComponent<Animator>().Play("fade")
        audio1.Play();
        Invoke("SceneChangeInvoke", 1.5f);
    }
    private void SceneChangeInvoke(){
        SceneManager.LoadScene("SampleScene");
    }
}
