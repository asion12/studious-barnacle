using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class a : MonoBehaviour
{
    public int hps = 200;
    public int dam = 1;
    private AudioSource audioss;
    // Start is called before the first frame update
    //해골 상태
    public enum SkullState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    //해골 기본 속성
    [Header("기본 속성")]
    //해골 초기 상태
    public SkullState skullState = SkullState.None;
    //해골 이동 속도
    public float spdMove = 1f;
    //해골이 본 타겟
    public GameObject targetCharactor = null;
    //해골이 본 타겟 위치정보 (매번 안 찾을려고)
    public Transform targetTransform = null;
    //해골이 본 타겟 위치(매번 안 찾을려)
    public Vector3 posTarget = Vector3.zero;

    //해골 애니메이션 컴포넌트 캐싱 
    private Animation skullAnimation = null;
    //해골 트랜스폼 컴포넌트 캐싱
    private Transform skullTransform = null;

    [Header("애니메이션 클립")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    [Header("전투속성")]
    //해골 체력
    public int hp = 200;
    //해골 공격 거리
    public float AtkRange = 1.5f;
    //해골 피격 이펙트
    public GameObject effectDamage = null;

    //해골 다이 이펙트
    public GameObject effectDie = null;

    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;
    void OnAtkAnmationFinished()
    {
        Debug.Log("Atk Animation finished");
    }

    void OnDmgAnmationFinished()
    {
        Debug.Log("Dmg Animation finished");
    }

    void OnDieAnmationFinished()
    {
        Debug.Log("Die Animation finished");
    }

    /// <summary>
    /// 애니메이션 이벤트를 추가해주는 함. 
    /// </summary>
    /// <param name="clip">애니메이션 클립 </param>
    /// <param name="funcName">함수명 </param>
    void OnAnimationEvent(AnimationClip clip, string funcName)
    {
        //애니메이션 이벤트를 만들어 준다
        AnimationEvent retEvent = new AnimationEvent();
        //애니메이션 이벤트에 호출 시킬 함수명
        retEvent.functionName = funcName;
        //애니메이션 클립 끝나기 바로 직전에 호출
        retEvent.time = clip.length - 0.1f;
        //위 내용을 이벤트에 추가 하여라
        clip.AddEvent(retEvent);
    }


    // Start is called before the first frame update
    void Start()
    {
        audioss = GetComponent<AudioSource>();

           //처음 상태 대기상태
           skullState = SkullState.Idle;

        //애니메이, 트랜스폼 컴포넌트 캐싱 : 쓸때마다 찾아 만들지 않게
        skullAnimation = GetComponent<Animation>();
        skullTransform = GetComponent<Transform>();

        //애니메이션 클립 재생 모드 비중
        skullAnimation[IdleAnimClip.name].wrapMode = WrapMode.Loop;
        skullAnimation[MoveAnimClip.name].wrapMode = WrapMode.Loop;
        skullAnimation[AtkAnimClip.name].wrapMode = WrapMode.Once;
        skullAnimation[DamageAnimClip.name].wrapMode = WrapMode.Once;

        ////애니메이션 블랜딩 위해 크게 올림
        //skullAnimation[DamageAnimClip.name].layer = 10;
        //skullAnimation[DieAnimClip.name].wrapMode = WrapMode.Once;
        //skullAnimation[DieAnimClip.name].layer = 10;

        //공격 애니메이션 이벤트 추가
        OnAnimationEvent(AtkAnimClip, "OnAtkAnmationFinished");
        OnAnimationEvent(DamageAnimClip, "OnDmgAnmationFinished");
        OnAnimationEvent(DieAnimClip, "DieAnimClip");

        skinnedMeshRenderer = skullTransform.Find("UD_light_infantry").GetComponent<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// 해골 상태에 따라 동작을 제어하는 함수 
    /// </summary>
    void CkState()
    {
        switch (skullState)
        {
            case SkullState.Idle:
                //이동에 관련된 RayCast값
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
        AnimationCtrl();
    }

    /// <summary>
    /// 해골 상태가 대기 일 때 동작 
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
    /// 해골 상태가 이동 일 때 동 
    /// </summary>
    void setMove()
    {
        //출발점 도착점 두 벡터의 차이 
        Vector3 distance = Vector3.zero;
        //어느 방향을 바라보고 가고 있느냐 
        Vector3 posLookAt = Vector3.zero;

        //해골 상태
        switch (skullState)
        {
            //해골이 돌아다니는 경우
            case SkullState.Move:
                //만약 랜덤 위치 값이 제로가 아니면
                if (posTarget != Vector3.zero)
                {
                    //목표 위치에서 해골 있는 위치 차를 구하고
                    distance = posTarget - skullTransform.position;

                    //만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
                    if (distance.magnitude < AtkRange)
                    {
                        //대기 동작 함수를 호출
                        StartCoroutine(setWait());
                        //여기서 끝냄
                        return;
                    }

                    //어느 방향을 바라 볼 것인. 랜덤 지역
                    posLookAt = new Vector3(posTarget.x,
                                            //타겟이 높이 있을 경우가 있으니 y값 체크
                                            skullTransform.position.y,
                                            posTarget.z);
                }
                break;
            //캐릭터를 향해서 가는 돌아다니는  경우
            case SkullState.GoTarget:
                //목표 캐릭터가 있을 땟
                if (targetCharactor != null)
                {
                    //목표 위치에서 해골 있는 위치 차를 구하고
                    distance = targetCharactor.transform.position - skullTransform.position;
                    //만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
                    if (distance.magnitude < AtkRange)
                    {
                        //공격상태로 변경합니.
                        skullState = SkullState.Atk;
                        //플레이어 와 충돌시 hp담
                        GameManager.instance.hp -= 4;
                      
                        
                       
                        //여기서 끝냄
                        return;
                    }
                    //어느 방향을 바라 볼 것인. 랜덤 지역
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            //타겟이 높이 있을 경우가 있으니 y값 체크
                                            skullTransform.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        //해골 이동할 방향에 크기를 없애고 방향만 가진(normalized)
        Vector3 direction = distance.normalized;

        //방향은 x,z 사용 y는 땅을 파고 들어갈거라 안함
        direction = new Vector3(direction.x, 0f, direction.z);

        //이동량 방향 구하기
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //캐릭터 컨트롤이 아닌 트랜스폼으로 월드 좌표 이용하여 이동
        skullTransform.Translate(amount, Space.World);
        //캐릭터 방향 정하기
        skullTransform.LookAt(posLookAt);

    }

   

    /// <summary>
    /// 대기 상태 동작 함 
    /// </summary>
    /// <returns></returns>
    IEnumerator setWait()
    {
        //해골 상태를 대기 상태로 바꿈
        skullState = SkullState.Wait;
        //대기하는 시간이 오래되지 않게 설정
        float timeWait = Random.Range(1f, 3f);
        //대기 시간을 넣어 준.
        yield return new WaitForSeconds(timeWait);
        //대기 후 다시 준비 상태로 변경
        skullState = SkullState.Idle;
    }

    /// <summary>
    /// 애니메이션을 재생시켜주는 함 
    /// </summary>
    void AnimationCtrl()
    {
        //해골의 상태에 따라서 애니메이션 적용
        switch (skullState)
        {
            //대기와 준비할 때 애니메이션 같.
            case SkullState.Wait:
            case SkullState.Idle:
                //준비 애니메이션 실행
                skullAnimation.CrossFade(IdleAnimClip.name);
                break;
            //랜덤과 목표 이동할 때 애니메이션 같.
            case SkullState.Move:
            case SkullState.GoTarget:
                //이동 애니메이션 실행
                skullAnimation.CrossFade(MoveAnimClip.name);
                break;
            //공격할 때
            case SkullState.Atk:
                
                //공격 애니메이션 실행
                skullAnimation.CrossFade(AtkAnimClip.name);
                break;
            //죽었을 때
            case SkullState.Die:
                //죽을 때도 애니메이션 실행
                skullAnimation.CrossFade(DieAnimClip.name);
                break;
            default:
                break;

        }
    }


    ///<summary>
    ///심야 범위안에 다른 트리거 또는 캐릭터가 들어오면 호출된다
    ///함수 동작은 목표물이 들어오면 목표물을 설정하고 
    ///</summary>>
    ///<param name="target"></param>
    void OnCKTarget(GameObject target)
    {
        //목표 캐릭터에 파라메터로 검출된 오프젝트를 넣는다
        targetCharactor = target;

        //목표 위치에 목표 캐릭터의 위치 값을 넣는다
        targetTransform = target.transform;
        //목표물을 향해 해골이 이동하는 상태로 변경
        skullState = SkullState.GoTarget;

    }

    void effectDamageTween()
    {
        if (effectTweener != null && effectTweener.isComplete == false)
        {
            return;
        }
        else
        {
            Color colorTo = Color.red;
            Debug.Log("a");
            effectTweener = HOTween.To(skinnedMeshRenderer.material, 0.2f, new TweenParms()
                                            //색상을 교체
                                            .Prop("color",colorTo)
                                            .Loops(1, LoopType.Yoyo)
                                            .OnStepComplete(OnDamageTweenFinished)
                                            );
        }
    }

    void OnDamageTweenFinished()
    {
        skinnedMeshRenderer.material.color = Color.white;
    }

    ///<summary>
    /// 해골 땡땨ㅐㅇ
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            hps-=GameManager.instance.damge;
            Debug.Log("적이 들어왔다! 빨강으로!");
            Debug.Log(hps);
            if (hps > 0)
            {
               
                Instantiate(effectDamage, other.transform.position, Quaternion.identity);
                skullAnimation.CrossFade(DamageAnimClip.name);
                effectDamageTween();

            }
            else if(hps<=0)
            {
                
                Debug.Log("end");
                skullState = SkullState.Die;
                GameManager.instance.Gold +=1000;
                GameManager.instance.everyMonsters.Remove(this.gameObject);
                UIManager.Instance.SetText(GameManager.instance.everyMonsters.Count.ToString());
                audioss.Play();
                Destroy(gameObject, 1);
               
            }
        }
    }
    /// <summary>
    /// 해골 상태 공격 모드
    /// </summary>
    void setAtk()
    {
        //해골과 캐릭터간의 우치 거리
        float distance = Vector3.Distance(targetTransform.position, skullTransform.position);
        //공격 거리보다 둘간의 거리가 멀어졌다면 
        if (distance > AtkRange + 0.5f)
        {
            skullState = SkullState.GoTarget;
        }

    }


 

}
