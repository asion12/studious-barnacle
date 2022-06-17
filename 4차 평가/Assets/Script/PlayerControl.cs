using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{

    //캐릭터 타격시 색 변환 준비
    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;
    [Header("속성")]
    //캐릭터 이동속도 설정
    [Tooltip("캐릭터 이동속도 설정")]
    public float moveSpd = 2.0f;
    //캐릭터 이동속도 설정
    public float runMoveSpd = 3.5f;
    //캐릭터 이동방향 회전 속도 설정
    public float DirectionRotateSpd = 100.0f;
    //캐릭터 몸을 움직이는 회전 속도 설정
    public float BodyRotateSpd = 2.0f;
    //캐릭터 속도 변경 증가 값
    [Range(0.1f, 50.0f)]
    public float VelocityChangeSpd = 0.1f;
    //캐릭터 현재 이동 속도 설정 초기값
    private Vector3 CurrentVelocitySpd = Vector3.zero;
    //캐릭터 현재 이동 방향 초기값 설정
    private Vector3 MoveDirect = Vector3.zero;
    //CharacterController 캐싱 준비
    private CharacterController characterCtrl = null;
    //캐릭터 CollisionFlags 초기값 설정
    private CollisionFlags collisionflages = CollisionFlags.None;
    [Header("애니메이션 속성")]
    public AnimationClip animationClipIdle = null;
    public AnimationClip animationClipWalk = null;
    public AnimationClip animationClipRun = null;
    public AnimationClip animationClipAtk_1 = null;
    public AnimationClip animationClipAtk_2 = null;
    public AnimationClip animationClipAtk_3 = null;
    public AnimationClip animationClipAtk_4 = null;
    
    //animation component 캐싱 준비
    private Animation animationPlayer = null;
    //캐릭터 상태
    public enum PlayerState { None, Idle, Walk, Run, Atk }

    [Header("캐릭터 상태")]
    public PlayerState playerState = PlayerState.None;

    //공격 상태
    public enum PlayerAttackState { atkStep_1, atkStep_2, atkStep_3, atkStep_4 }

    [Header("공격 상태")]
    public PlayerAttackState playerAttackState = PlayerAttackState.atkStep_1;

    //다음 연계 공격 활성화 여부를 확인하기 위해 flag 설정
    public bool flagNextAttack = false;
    void Start()
    {
        //CharacterController 캐싱
        characterCtrl = GetComponent<CharacterController>();
        //animation 캐싱
        animationPlayer = GetComponent<Animation>();
        //자동 재생 off
        animationPlayer.playAutomatically = false;
        //현재 재생중인 애니메이션을 STOP
        animationPlayer.Stop();
        //상태값을 기본값
        playerState = PlayerState.Idle;
        //애니메이션 상태에 클립 연결
        animationPlayer[animationClipIdle.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipWalk.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipRun.name].wrapMode = WrapMode.Loop;
        animationPlayer[animationClipAtk_1.name].wrapMode = WrapMode.Once;
        animationPlayer[animationClipAtk_2.name].wrapMode = WrapMode.Once;
        animationPlayer[animationClipAtk_3.name].wrapMode = WrapMode.Once;
        animationPlayer[animationClipAtk_4.name].wrapMode = WrapMode.Once;

        SetAnimationEvent(animationClipAtk_1, "OnPlayerAttackFinshed");
        SetAnimationEvent(animationClipAtk_2, "OnPlayerAttackFinshed");
        SetAnimationEvent(animationClipAtk_3, "OnPlayerAttackFinshed");
        SetAnimationEvent(animationClipAtk_4, "OnPlayerAttackFinshed");
        InvokeRepeating("StackUp", 2f, 10f);
    }

    void Update()
    {
       
        Move();
        a();
        BodyDirectionChange();
        AnimationClipCtrl();
        ckAnimationState();
        InputAttackCtrl();
       
    }
    /// <summary>
    /// 캐릭터 이동 함수
    /// </summary>
    /// 
    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.white;
        
        GUILayout.Label("현재 콤보 : " + GameManager.instance.mycombo, labelStyle);
        GUILayout.Label("현재 HP : "+GameManager.instance.hp,labelStyle);
        GUILayout.Label("Gold : " + GameManager.instance.Gold, labelStyle);
        GUILayout.Label("dam : " + GameManager.instance.damge, labelStyle);
    }
    void StackUp()
    {
        
        if (GameManager.instance.mycombo <= 999)
        {
            GameManager.instance.mycombo += 50;
           
        } 
    }
    private void printSomething(char a)
    {
        Debug.Log(a);
    }
    void Move()
    {
        //메인카메라 Transform
        Transform cameraTransform = Camera.main.transform;
        //메인카메라가 바라보는 방향이 월드상에서 어떤 방향인가
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        //벡터 내적
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        //키 값
        float vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        //방향 벡터 이자 목표점
        Vector3 amount = vertical * forward + Horizontal * right;
        //이동 방향
        MoveDirect = Vector3.RotateTowards(MoveDirect, amount, DirectionRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        MoveDirect = MoveDirect.normalized;
        //이동 속도
        float spd = moveSpd;
        //만약에 playerState가 Run이면
        if (playerState == PlayerState.Run)
        {
            spd = runMoveSpd;
        }
        //이동하는 프레임 양
        Vector3 moveAmount = (MoveDirect * spd * Time.deltaTime);
        //실제 이동
        collisionflages = characterCtrl.Move(moveAmount);
    }

    float GetVelocitySpd()
    {
        if (characterCtrl.velocity == Vector3.zero)
        {
            CurrentVelocitySpd = Vector3.zero;
        }
        else
        {
            Vector3 retVelocitySpd = characterCtrl.velocity;
            retVelocitySpd.y = 0;
            CurrentVelocitySpd = Vector3.Lerp(CurrentVelocitySpd, retVelocitySpd, VelocityChangeSpd * Time.fixedDeltaTime);
        }

        return CurrentVelocitySpd.magnitude;
    }

    /// <summary>
    /// 몸통 방향 함수 작성
    /// </summary>
    void BodyDirectionChange()
    {
        //움직이고 있는가?
        if (GetVelocitySpd() > 0.0f)
        {
            //캐릭터 몸통이 바라볼 전방은? 캐릭터 속도 방향
            Vector3 newForward = characterCtrl.velocity;
            newForward.y = 0.0f;
            //캐릭터를 전방으로 방향을 설정한다.
            transform.forward = Vector3.Lerp(transform.forward, newForward, BodyRotateSpd);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkullSword") == true)
        {
            GameManager.instance.hp -= 10;
            Debug.Log(GameManager.instance.hp);
            if (GameManager.instance.hp>0)
            {
                effectDamageTween();
            }
            else if(GameManager.instance.hp<0)
            {
               
                SceneManager.LoadScene("Manu");
            }
        }
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

            effectTweener = HOTween.To(skinnedMeshRenderer.material, 0.2f, new TweenParms()
                                            //색상을 교체
                                            .Prop("color", colorTo)
                                            .Loops(1, LoopType.Yoyo)
                                            .OnStepComplete(OnDamageTweenFinisheds)
                                            );
        }
    }
   void OnDamageTweenFinisheds()
    {
        skinnedMeshRenderer.material.color = Color.white;
    }
    /// <summary>
    /// 애니메이션 재생 함수
    /// </summary>
    /// <param name="clip">애니메이션 클립</param>
    void playAnimationByClip(AnimationClip clip)
    {
        //animationPlayer.clip = clip;
        //animationPlayer.GetClip(clip.name);
        animationPlayer.CrossFade(clip.name);
    }

    /// <summary>
    /// 플레이어 상태에 맞춘 애니메이션을 재생
    /// </summary>
    void AnimationClipCtrl()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                playAnimationByClip(animationClipIdle);
                break;
            case PlayerState.Walk:
                playAnimationByClip(animationClipWalk);
                break;
            case PlayerState.Run:
                playAnimationByClip(animationClipRun);
                break;
            case PlayerState.Atk:
                break;
        }
    }

    /// <summary>
    /// 상태에 따른 변경해주는 함수
    /// </summary>
    void ckAnimationState()
    {
        float nowSpd = GetVelocitySpd();
        switch (playerState)
        {
            case PlayerState.Idle:
                if (nowSpd > 0.0f)
                {
                    playerState = PlayerState.Walk;
                }
                break;
            case PlayerState.Walk:
                if (nowSpd > 2.0f)
                {
                    playerState = PlayerState.Run;
                }
                else if (nowSpd < 0.1f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Run:
                if (nowSpd < 2f)
                {
                    playerState = PlayerState.Walk;
                }
                if (nowSpd < 0.1f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Atk:
                AtkAnimationCtrl();
                break;
        }
    }

    /// <summary>
    /// 공격 버튼 체크 함수
    /// </summary>
    void InputAttackCtrl()
    {
        //마우스 클릭 감지
        if (Input.GetMouseButtonDown(0) == true)
        {
            //플레이어가 공격상태
            if (playerState != PlayerState.Atk)
            {
                //플레이어가 공격상태가 아니면 공격 상태로 만든다
                playerState = PlayerState.Atk;
                //공격상태 초기화
                playerAttackState = PlayerAttackState.atkStep_1;
            }
            else
            {
                //플레이어 상태가 공격일 때 
                //공격 상태에 따른 분류
                switch (playerAttackState)
                {
                    case PlayerAttackState.atkStep_1:
                        if (animationPlayer[animationClipAtk_1.name].normalizedTime > 0.1f)
                        {
                            flagNextAttack = true;

                        }
                        break;
                    case PlayerAttackState.atkStep_2:
                        if (animationPlayer[animationClipAtk_3.name].normalizedTime > 0.1f)
                        {
                            flagNextAttack = true;
                        }
                        break;
                    case PlayerAttackState.atkStep_3:
                        if (animationPlayer[animationClipAtk_4.name].normalizedTime > 0.1f)
                        {
                            flagNextAttack = true;
                        }
                        break;
                    case PlayerAttackState.atkStep_4:
                        if (animationPlayer[animationClipAtk_1.name].normalizedTime > 0.1f)
                        {
                            flagNextAttack = true;
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// CallBack 공격 애니메이션 재생이 끝나면 호출 되는 애니메이션 이벤트 함수
    /// </summary>
    void OnPlayerAttackFinshed()
    {
        //만약에 flagNextAttack이 true면
        if (flagNextAttack == true)
        {
            //flagNextAttkack 초기화
            flagNextAttack = false;

            //현재 공격 애니메이션 상태에 따른 다음 애니메이션 상태값을 넣기
            switch (playerAttackState)
            {
                case PlayerAttackState.atkStep_1:
                    playerAttackState = PlayerAttackState.atkStep_2;
                    GameManager.instance.mycombo += 10;
                    break;
                case PlayerAttackState.atkStep_2:
                    playerAttackState = PlayerAttackState.atkStep_3;
                    GameManager.instance.mycombo += 10;
                    break;
                case PlayerAttackState.atkStep_3:
                    playerAttackState = PlayerAttackState.atkStep_4;
                    GameManager.instance.mycombo += 10;
                    break;
                case PlayerAttackState.atkStep_4:
                    playerAttackState = PlayerAttackState.atkStep_1;
                    GameManager.instance.mycombo += 10;
                    break;
            }
        }
        else
        {
            playerState = PlayerState.Idle;

            playerAttackState = PlayerAttackState.atkStep_1;
        }
    }

    /// <summary>
    /// 애니메이션 클립 재생이 끝날때 애니메이션 이벤트 함수를 호출
    /// </summary>
    /// <param name="clip">애니메이션 클립</param>
    /// <param name="funcName">이벤트 함수</param>
    void SetAnimationEvent(AnimationClip clip, string funcName)
    {
        //새로운 이벤트 선언
        AnimationEvent newAnimationEvent = new AnimationEvent();

        //해당 이벤트의 호출은 funcName
        newAnimationEvent.functionName = funcName;
        newAnimationEvent.time = clip.length - 0.1f;
        clip.AddEvent(newAnimationEvent);
    }

    /// <summary>
    /// 공격 애니메이션 재생
    /// </summary>
    void AtkAnimationCtrl()
    {
        //만약 공격상태가?
        switch (playerAttackState)
        {
            case PlayerAttackState.atkStep_1:
                playAnimationByClip(animationClipAtk_1);
                break;
            case PlayerAttackState.atkStep_2:
                playAnimationByClip(animationClipAtk_2);
                break;
            case PlayerAttackState.atkStep_3:
                playAnimationByClip(animationClipAtk_3);
                break;
            case PlayerAttackState.atkStep_4:
                playAnimationByClip(animationClipAtk_4);
                break;
        }
    }
    private float gravity = 9.8f;
    private float verticalSpd = 0f;
    void a()
    {
        if ((collisionflages & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= gravity + Time.deltaTime;
        }
    }
}
