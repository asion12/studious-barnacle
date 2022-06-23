using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{//¿¡¼Â
    public GameObject mainSlime;
    public Button idleBut, walkBut, jumpBut, attackBut, damageBut0, damageBut1, damageBut2;
    public Camera cam;
    //
    public GameObject eff = null;
    public GameObject eff2 = null;
    public GameObject eff3 = null;
    public static GameManager instance = null;
    public float mycombo = 1;
    public int hp = 100;
    public int Gold = 1000000;
    public int damge = 50;
    public int sword = 0;
    public int maxcombo = 900;
    public int skullhp = 100;
    public int popo = 0;
    public int Item1 = 0;
    public int Item2 = 0;
    public int Item3 = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
  public void eff1()
    {
        if (Item1>0)
        {
            
                
               eff.gameObject.SetActive(true);
            eff2.gameObject.SetActive(false);
            eff3.gameObject.SetActive(false);

        }
    }
    public void eff22()
    {
        if (Item2 > 0)
        {

            Debug.Log("tlqkf?");
            eff.gameObject.SetActive(false);
            eff2.gameObject.SetActive(true);
            eff3.gameObject.SetActive(false);


        }
    }
    public void eff33()
    {
        if (Item3 > 0)
        {

            Debug.Log("tlqkf?");
            eff.gameObject.SetActive(false);
            eff2.gameObject.SetActive(false);
            eff3.gameObject.SetActive(true);

        }
    }
public void dies()
    {
       
            if (GameManager.instance.hp <= 0)
            {
                SceneManager.LoadScene("a");
            }
        }
    private void Start()
    {
        idleBut.onClick.AddListener(delegate { Idle(); });
        walkBut.onClick.AddListener(delegate { ChangeStateTo(SlimeAnimationState.Walk); });
        jumpBut.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Jump); });
        attackBut.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Attack); });
        damageBut0.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<EnemyAi>().damType = 0; });
        damageBut1.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<EnemyAi>().damType = 1; });
        damageBut2.onClick.AddListener(delegate { LookAtCamera(); ChangeStateTo(SlimeAnimationState.Damage); mainSlime.GetComponent<EnemyAi>().damType = 2; });
    }
    private void Update()
    {
        dies();
    }
    void Idle()
    {
        LookAtCamera();
        mainSlime.GetComponent<EnemyAi>().CancelGoNextDestination();
        ChangeStateTo(SlimeAnimationState.Idle);
    }
    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (mainSlime == null) return;
        if (state == mainSlime.GetComponent<EnemyAi>().currentState) return;

        mainSlime.GetComponent<EnemyAi>().currentState = state;
    }
    void LookAtCamera()
    {
        mainSlime.transform.rotation = Quaternion.Euler(new Vector3(mainSlime.transform.rotation.x, cam.transform.rotation.y, mainSlime.transform.rotation.z));
    }
}
