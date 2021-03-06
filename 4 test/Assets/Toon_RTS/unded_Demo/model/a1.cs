using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class a1 : MonoBehaviour
{
    public int hps = 200;
    public int dam = 1;
    private AudioSource audi;
   
    // Start is called before the first frame update
    //?ذ? ????
    public enum SkullState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    //?ذ? ?⺻ ?Ӽ?
    [Header("?⺻ ?Ӽ?")]
    //?ذ? ?ʱ? ????
    public SkullState skullState = SkullState.None;
    //?ذ? ?̵? ?ӵ?
    public float spdMove = 1f;
    //?ذ??? ?? Ÿ??
    public GameObject targetCharactor = null;
    //?ذ??? ?? Ÿ?? ??ġ???? (?Ź? ?? ã??????)
    public Transform targetTransform = null;
    //?ذ??? ?? Ÿ?? ??ġ(?Ź? ?? ã????)
    public Vector3 posTarget = Vector3.zero;

    //?ذ? ?ִϸ??̼? ??????Ʈ ĳ?? 
    private Animation skullAnimation = null;
    //?ذ? Ʈ?????? ??????Ʈ ĳ??
    private Transform skullTransform = null;

    [Header("?ִϸ??̼? Ŭ??")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    [Header("?????Ӽ?")]
    //?ذ? ü??
    public int hp = 200;
    //?ذ? ???? ?Ÿ?
    public float AtkRange = 1.5f;
    //?ذ? ?ǰ? ????Ʈ
    public GameObject effectDamage = null;

    //?ذ? ???? ????Ʈ
    public GameObject effectDie = null;

    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;

 


    // Start is called before the first frame update
    void Start()
    {

        audi = GetComponent<AudioSource>();
        //ó?? ???? ????????
        skullState = SkullState.Idle;

        //?ִϸ???, Ʈ?????? ??????Ʈ ĳ?? : ???????? ã?? ?????? ?ʰ?
        skullAnimation = GetComponent<Animation>();
        skullTransform = GetComponent<Transform>();

        

   

       
    }

    /// <summary>
    /// ?ذ? ???¿? ???? ?????? ?????ϴ? ?Լ? 
    /// </summary>
    void CkState()
    {
        switch (skullState)
        {
            case SkullState.Idle:
                //?̵??? ???õ? RayCast??
                setIdle();
                
                break;
            case SkullState.GoTarget:
            case SkullState.Move:
                setMove();
                break;
            case SkullState.Atk:
                setAtk();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        CkState();
     
    }

    /// <summary>
    /// ?ذ? ???°? ???? ?? ?? ???? 
    /// </summary>
    void setIdle()
    {
        if (targetCharactor == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-10f, 10f),
                                    skullTransform.position.y + 1000f,
                                    skullTransform.position.z + Random.Range(-10f, 10f)
                );
            Ray ray = new Ray(posTarget, Vector3.down);
            RaycastHit infoRayCast = new RaycastHit();
            if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
            {
                posTarget.y = infoRayCast.point.y;
            }
            skullState = SkullState.Move;
        }
        else
        {
            skullState = SkullState.GoTarget;
        }
    }

    /// <summary>
    /// ?ذ? ???°? ?̵? ?? ?? ?? 
    /// </summary>
    void setMove()
    {
        //?????? ?????? ?? ?????? ???? 
        Vector3 distance = Vector3.zero;
        //???? ?????? ?ٶ󺸰? ???? ?ִ??? 
        Vector3 posLookAt = Vector3.zero;

        //?ذ? ????
        switch (skullState)
        {
            //?ذ??? ???ƴٴϴ? ????
            case SkullState.Move:
                //???? ???? ??ġ ???? ???ΰ? ?ƴϸ?
                if (posTarget != Vector3.zero)
                {
                    //??ǥ ??ġ???? ?ذ? ?ִ? ??ġ ???? ???ϰ?
                    distance = posTarget - skullTransform.position;

                    //???࿡ ?????̴? ???? ?ذ??? ??ǥ?? ?? ???? ???? ???? 
                    if (distance.magnitude < AtkRange)
                    {
                        //???? ???? ?Լ??? ȣ??
                        StartCoroutine(setWait());
                        //???⼭ ????
                        return;
                    }

                    //???? ?????? ?ٶ? ?? ????. ???? ????
                    posLookAt = new Vector3(posTarget.x,
                                            //Ÿ???? ???? ???? ???찡 ?????? y?? üũ
                                            skullTransform.position.y,
                                            posTarget.z);
                }
                break;
            //ĳ???͸? ???ؼ? ???? ???ƴٴϴ?  ????
            case SkullState.GoTarget:
                //??ǥ ĳ???Ͱ? ???? ??
                if (targetCharactor != null)
                {
                    //??ǥ ??ġ???? ?ذ? ?ִ? ??ġ ???? ???ϰ?
                    distance = targetCharactor.transform.position - skullTransform.position;
                    //???࿡ ?????̴? ???? ?ذ??? ??ǥ?? ?? ???? ???? ???? 
                    if (distance.magnitude < AtkRange)
                    {
                        //???ݻ??·? ?????մ?.
                        skullState = SkullState.Atk;

                        //?÷??̾? ?? ?浹?? hp??

                       GameManager.instance.hp -= dam;
                        
                       
                        //???⼭ ????
                        return;
                    }
                    //???? ?????? ?ٶ? ?? ????. ???? ????
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            //Ÿ???? ???? ???? ???찡 ?????? y?? üũ
                                            skullTransform.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        //?ذ? ?̵??? ???⿡ ũ?⸦ ???ְ? ???⸸ ????(normalized)
        Vector3 direction = distance.normalized;

        //?????? x,z ???? y?? ???? ?İ? ????Ŷ? ????
        direction = new Vector3(direction.x, 0f, direction.z);

        //?̵??? ???? ???ϱ?
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //ĳ???? ??Ʈ???? ?ƴ? Ʈ?????????? ???? ??ǥ ?̿??Ͽ? ?̵?
        skullTransform.Translate(amount, Space.World);
        //ĳ???? ???? ???ϱ?
        skullTransform.LookAt(posLookAt);

    }

   

    /// <summary>
    /// ???? ???? ???? ?? 
    /// </summary>
    /// <returns></returns>
    IEnumerator setWait()
    {
        //?ذ? ???¸? ???? ???·? ?ٲ?
        skullState = SkullState.Wait;
        //?????ϴ? ?ð??? ???????? ?ʰ? ????
        float timeWait = Random.Range(1f, 3f);
        //???? ?ð??? ?־? ??.
        yield return new WaitForSeconds(timeWait);
        //???? ?? ?ٽ? ?غ? ???·? ????
        skullState = SkullState.Idle;
    }

    /// <summary>
    /// ?ִϸ??̼??? ?????????ִ? ?? 
    /// </summary>
 


    ///<summary>
    ///?ɾ? ?????ȿ? ?ٸ? Ʈ???? ?Ǵ? ĳ???Ͱ? ???????? ȣ???ȴ?
    ///?Լ? ?????? ??ǥ???? ???????? ??ǥ???? ?????ϰ? 
    ///</summary>>
    ///<param name="target"></param>
    void OnCKTarget(GameObject target)
    {
        //??ǥ ĳ???Ϳ? ?Ķ????ͷ? ?????? ??????Ʈ?? ?ִ´?
        targetCharactor = target;

        //??ǥ ??ġ?? ??ǥ ĳ?????? ??ġ ???? ?ִ´?
        targetTransform = target.transform;
        //??ǥ???? ???? ?ذ??? ?̵??ϴ? ???·? ????
        skullState = SkullState.GoTarget;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            hps-=GameManager.instance.damge;
            Debug.Log("???? ?????Դ?! ????????!");
            Debug.Log(hps);
            if (hps > 0)
            {
               
              

            }
            else
            {
                
                Debug.Log("end");
                skullState = SkullState.Die;
                GameManager.instance.Gold += 1000;
                GameManager.instance.everyMonsters.Remove(this.gameObject);
                UIManager.Instance.SetText(GameManager.instance.everyMonsters.Count.ToString());
                audi.Play();
                Destroy(gameObject, 1);
               
               
            }
        }
    }
    /// <summary>
    /// ?ذ? ???? ???? ????
    /// </summary>
    void setAtk()
    {
        //?ذ??? ĳ???Ͱ??? ??ġ ?Ÿ?
        float distance = Vector3.Distance(targetTransform.position, skullTransform.position);
        //???? ?Ÿ????? ?Ѱ??? ?Ÿ??? ?־????ٸ? 
        if (distance > AtkRange + 0.5f)
        {
            skullState = SkullState.GoTarget;
        }

    }


 

}
