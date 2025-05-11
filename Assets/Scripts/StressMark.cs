using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressMark : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }
    public Animator animator;
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        //Debug.Log(animator);
        //DestroyThis();
        //animator.Play("stressAppear");
    }
    public void DestroyThis()
    {
        animator.Play("stressDestroy");
        Destroy(gameObject,0.5f);
    }
}
