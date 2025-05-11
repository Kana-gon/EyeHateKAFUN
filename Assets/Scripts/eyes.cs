using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class eyes : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;
    [SerializeField] private string pushKey;
    [SerializeField] private Transform eyeBall;
    [SerializeField] private SpriteRenderer backRed;
    [SerializeField] public float eyeBallSpeed = 0.1f;
    [SerializeField] public float reloadTime = 0.1f;
    [SerializeField] public int reviveSpeed = 5;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform tearWave;
    [SerializeField] private eyes anotherEye;
    [SerializeField] private TutrialMng tutrialMng;
    [SerializeField]private AudioSource audio1;
    [SerializeField] private AudioSource audio2;
    [SerializeField]private AudioSource audio3;
    [SerializeField]private Sprite closeEye;
    [SerializeField]private GameMng gameMng;
    private StressMng stressMng;
    public int bulletNumMax = 100;
    private int bulletNum;
    public float HPMax = 100;
    private float HP;
    public float shootRate = 0.1f;

    private Vector3 mousePosition;
    private Vector3 eyeBallDirection;
    private Vector3 eyeBallResetPosition;

    private IEnumerator shootTimer;
    private IEnumerator reloadTimer;
    private IEnumerator reviveTimer;

    private bool isOpening = true;
    private bool canOpenClose = true;

    private Collider2D thisCollider2D;
    void Start()
    {
        bulletNum = bulletNumMax;
        HP = HPMax;
        eyeBallResetPosition = eyeBall.position;
        thisCollider2D = GetComponent<Collider2D>();
        stressMng = GameObject.Find("StressMng").GetComponent<StressMng>();
        shootTimer = ShootTimer();
        reloadTimer = ReloadTimer();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = mainCamera.WorldToScreenPoint(eyeBall.position).z;
        mousePosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        eyeBallTransformUpdate();
        tearWaveUpdate();
        damageUpdate();

        if (pushKey == "J")
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                CloseEye();
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                OpenEye();
            }
        }
        if (pushKey == "F")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CloseEye();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                OpenEye();
            }
        }
        /*
        if (System.Enum.TryParse(pushKey, out KeyCode key))
        {
            if (Input.GetKeyDown(key)) animator.Play("Close");
            if (Input.GetKeyUp(key)) animator.Play("Open");
        }
        っていうのもあるらしい
        */
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("shooting");
            if(tutrialMng.eventSwitch ==1){ tutrialMng.eventSwitch = 2;}
            StartCoroutine(shootTimer);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("shootstop");
            StopCoroutine(shootTimer);
        }
    }

    /// <summary>
    /// 瞳（眼球）の移動なんかの処理
    /// </summary> <summary>
    /// 
    /// </summary>
    private void eyeBallTransformUpdate()//*memo:InverseTransformPointは対象の親のトランスフォームが持つメソッドを使う
    {
        //*ローカル座標とワールド座標に大苦戦　
        //*あとデルタタイムにも
        //*↑プレハブ側のインスペクタで設定した値が優先されてただけかよ！！！！！！！！！！！！！！！！！！！！！！
        var thisLocalPos = eyeBall.localPosition;
        var targetWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var targetLocalPos = eyeBall.transform.parent.InverseTransformPoint(targetWorldPos);

        var newLocalPos = Vector3.Lerp(eyeBall.localPosition, targetLocalPos, eyeBallSpeed * Time.deltaTime);

        newLocalPos.x = Mathf.Clamp(newLocalPos.x, -0.42f, 0.42f);
        newLocalPos.y = Mathf.Clamp(newLocalPos.y, -0.20f, 0.20f);
        newLocalPos.z = 0f;
        eyeBall.localPosition = newLocalPos;

    }

    /// <summary>
    /// 残弾表示（涙の波）の更新処理
    /// </summary> <summary>
    /// 
    /// </summary>
    private void tearWaveUpdate()
    {
        tearWave.localPosition = new Vector3(tearWave.localPosition.x, -0.6f + 1.2f / bulletNumMax * bulletNum, 0);
    }
    private void damageUpdate()
    {

        if (HP <= 0)
        {
            audio2.Play();
            stressMng.AddProgress(20);
            reviveTimer = ReviveTimer();
            CloseEye();
            animator.Play("Damaged");
            canOpenClose = false;
            StartCoroutine(reviveTimer);
            HP = HPMax;
        }
        var color = new Color();
        color = backRed.color;
        color.a = HP / HPMax;
        backRed.color = color;
    }
    public void Hit(float damage)
    {
        
        // if(gameMng.ScoreNoHitComboMax < StaticClassData.ScoreNoHitCombo)
        //     gameMng.ScoreNoHitComboMax = StaticClassData.ScoreNoHitCombo;
        // StaticClassData.ScoreNoHitCombo = 0;
        HP -= damage;
    }
    public void CloseEye()
    {
        if (canOpenClose)
        {
            audio1.Play();
            if (name == "LeftEye")tutrialMng.keyPushed[0] = true;
            else tutrialMng.keyPushed[1] = true;
            if(tutrialMng.eventSwitch == 3)tutrialMng.eventSwitch = 100;
            animator.Play("Close");
            StartCoroutine(reloadTimer);
            thisCollider2D.enabled = false;
            stressMng.StopHeal();
            if(anotherEye.isOpening == false)stressMng.StartStressing();
            isOpening = false;
        }
    }

    public void DamageToCloseEye()
    {
        if (canOpenClose)
        {
            animator.Play("dtoClose");
            GetComponent<SpriteRenderer>().sprite = closeEye;
            StartCoroutine(reloadTimer);
            thisCollider2D.enabled = false;
            stressMng.StopHeal();
            if (anotherEye.isOpening == false) stressMng.StartStressing();
            isOpening = false;
        }
    }
    public void OpenEye()
    {
        if (canOpenClose)
        {
            animator.Play("Open");
            StopCoroutine(reloadTimer);
            thisCollider2D.enabled = true;
            if(anotherEye.isOpening == true)stressMng.RestartHeal();
            stressMng.StopStressing();
            isOpening = true;
        }
    }
    public void GameOver(){
        canOpenClose = false;
        if(isOpening)animator.Play("GameOver");
        else animator.Play("GameOver_0");

        isOpening = false;
        if (reviveTimer != null) StopCoroutine(reviveTimer);
    }
    public void GameClear()
    {
        canOpenClose = false;
        if (isOpening) animator.Play("Close");
        else animator.Play("Close");

        isOpening = false;
        if (reviveTimer != null) StopCoroutine(reviveTimer);
    }
    public void OnGameOverAnimEnded(){
        //Debug.Log("gameOver");
        //if(reviveTimer != null)StopCoroutine(reviveTimer);
    }
    private IEnumerator ReloadTimer()
    {
        while (true)
        {
            if (bulletNum < bulletNumMax)
                bulletNum++;
            yield return new WaitForSeconds(reloadTime);
        }
    }
    private IEnumerator ShootTimer()
    {
        while (true)
        {
            //Debug.Log("shoot");
            if (isOpening && bulletNum > 0)
            {
                bullet.transform.position = eyeBall.transform.position;
                bullet.GetComponent<SpriteRenderer>().enabled = false;
                Instantiate(bullet);
                audio3.Play();
                bulletNum--;
            }
            if(bulletNum <= 0&&tutrialMng.onceSwitch[3]==false&&tutrialMng.eventSwitch != 55){
                tutrialMng.eventSwitch = 3;
            }
            yield return new WaitForSeconds(shootRate);
        }
    }
    private IEnumerator ReviveTimer()
    {
        yield return new WaitForSeconds(3);
        canOpenClose = true;
        if(pushKey == "J"){
            if(Input.GetKey(KeyCode.J)){
                DamageToCloseEye();
            }else{
                OpenEye();
            }
        }
        if (pushKey == "F")
        {
            if (Input.GetKey(KeyCode.F))
            {
                DamageToCloseEye();
            }
            else
            {
                OpenEye();
            }
        }
        //Debug.Log("endcor");
    }
}
