using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossBody : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;
        public void Hit(float damage)
    {
        audioSource.Play();
        transform.parent.GetComponent<BossParent>().Hit(damage);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Eye"))
        {
            collision.gameObject.GetComponent<eyes>().Hit(transform.parent.GetComponent<BossParent>().ATK);
        }
    }
}
