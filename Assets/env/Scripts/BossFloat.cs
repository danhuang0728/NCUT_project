using UnityEngine;

public class BossFloat : MonoBehaviour
{
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;
    public int mainHp = 20;
    public GameObject monster;
    private Vector3 startPos;
    
    // 添加 Animator 引用
    private Animator eyesAnimator_blue;
    private Animator eyesAnimator_green;
    private Animator eyesAnimator_red;
    private Animator eyesAnimator_purple;
    private Animator eyesAnimator_yellow;

    void Start()
    {
        startPos = transform.position;
        // 獲取 eyes_close_blue 物件上的 Animator 組件
        // 假設 eyes_close_blue 是子物件
        eyesAnimator_blue = transform.Find("eyes/eyeball_blue_0/eyes_close_blue").GetComponent<Animator>();
        eyesAnimator_green = transform.Find("eyes/eyeball_green_0/eyes_close_green").GetComponent<Animator>();
        eyesAnimator_red = transform.Find("eyes/eyeball_red_0/eyes_close_red").GetComponent<Animator>();
        eyesAnimator_purple = transform.Find("eyes/eyeball_purple_0/eyes_close_purple").GetComponent<Animator>();
        eyesAnimator_yellow = transform.Find("eyes/eyeball_yellow_0/eyes_close_yellow").GetComponent<Animator>();
        
        // 如果 eyes_close_blue 不在這個路徑，你需要調整路徑
        // 或者，你可以通過 Inspector 拖曳指定：
        // public Animator eyesAnimator; // 改為 public 然後在 Inspector 中指定
    }

    void Update()
    {
        // 浮動移動代碼
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // 檢查血量並控制動畫器
        if (mainHp <= 18 && eyesAnimator_blue != null)
        {
            eyesAnimator_blue.enabled = true;
        }
        
        if (mainHp <= 16 && eyesAnimator_green != null)
        {
            eyesAnimator_green.enabled = true;
        }

        if (mainHp <= 14 && eyesAnimator_red != null)
        {
            eyesAnimator_red.enabled = true;
        }

        if (mainHp <= 12 && eyesAnimator_purple != null)
        {
            eyesAnimator_purple.enabled = true;
        }

        if (mainHp <= 10 && eyesAnimator_yellow != null)
        {
            eyesAnimator_yellow.enabled = true;
        }



        if (mainHp <= 0)
        {
            MonsterDead(monster);
        }
    }

    public void MonsterDead(GameObject monster)
    {
        monster.SetActive(false);
    }
}