using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tearBullet : MonoBehaviour
{
    private Transform thisTransform;
    private Vector3 targetPos;
    private Vector3 direction;
    private Rigidbody2D _rigid;
    [SerializeField]private Camera mainCamera;
    [SerializeField] float speed = 0.6f;
    // Start is called before the first frame update
    private void Awake() 
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();//TODO?この辺は呼び出す側から渡してもいいかも？
        _rigid = GetComponent<Rigidbody2D>();
        thisTransform = transform;
        targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;

        //速度を求める
        direction = targetPos - transform.position;
        direction.z = 0;
        direction.Normalize();

        //向きを求める
        var angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg+90;
        transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z+angle);
        GetComponent<SpriteRenderer>().enabled = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Contains("wall"))
            Destroy(gameObject);
        if(collision.name.Contains("Enemy")){
            Destroy(gameObject);
            collision.gameObject.GetComponent<Enemy>().Hit(10);
        }
        if(collision.name.Contains("BOSS")){
            Destroy(gameObject);
            collision.gameObject.GetComponent<BossBody>().Hit(10);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //_rigid.velocity = direction * speed * Time.deltaTime;
        var newPos = transform.position + direction * speed * Time.deltaTime;
        transform.position = newPos;
    }
}
