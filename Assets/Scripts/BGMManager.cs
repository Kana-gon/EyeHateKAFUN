using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]AudioSource audio1;
    [SerializeField] AudioSource audio2;
    [SerializeField] AudioSource audio3;
    [SerializeField] AudioSource audio4;
    public void ChangeBGM(int id){
        audio2.Stop(); audio1.Stop();
        audio3.Stop();audio4.Stop();
        if (id == 2){audio2.Play();}
        if(id == 3){audio3.Play(); }
        if (id == 4) { audio4.Play();}
    }
}
