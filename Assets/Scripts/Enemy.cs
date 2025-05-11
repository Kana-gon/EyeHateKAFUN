using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] public float hp = 20;
    [SerializeField] public float ATK = 20;
    private Vector3 targetPos;
    private Vector3 direction;
    //private Rigidbody2D _rigid; //未使用
    [SerializeField] float speed = 0.6f;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    public void SetTarget(Transform target){
        //_rigid = GetComponent<Rigidbody2D>();
        thisTransform = transform;
        targetPos = target.position;
        targetPos.z = 0;

        //速度を求める
        direction = targetPos - transform.position;
        direction.z = 0;
        direction.Normalize();
        //Debug.Log(direction);
        // //向きを求める
        // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        // transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + angle);
        GetComponent<SpriteRenderer>().enabled = true;
    }
    public void Hit(float damage){
        //Debug.Log(damage);
        hp -= damage;
        if(hp <= 0){
            //StaticClassData.ScoreNoHitCombo += 1;
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("HIT");
        if (collision.gameObject.name.Contains("wall"))
            Destroy(gameObject);
        if(collision.gameObject.name.Contains("Eye")){
            collision.gameObject.GetComponent<eyes>().Hit(ATK);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("updateing");
        //_rigid.velocity = direction * speed * Time.deltaTime;
        var newPos = transform.position + direction * speed * Time.deltaTime;
        //Debug.Log($"{gameObject.name}:{newPos}");
        transform.position = newPos;
    }
}
