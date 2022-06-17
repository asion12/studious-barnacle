using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{

    //ĳ���� Ÿ�ݽ� �� ��ȯ �غ�
    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;
    [Header("�Ӽ�")]
    //ĳ���� �̵��ӵ� ����
    [Tooltip("ĳ���� �̵��ӵ� ����")]
    public float moveSpd = 2.0f;
    //ĳ���� �̵��ӵ� ����
    public float runMoveSpd = 3.5f;
    //ĳ���� �̵����� ȸ�� �ӵ� ����
    public float DirectionRotateSpd = 100.0f;
    //ĳ���� ���� �����̴� ȸ�� �ӵ� ����
    public float BodyRotateSpd = 2.0f;
    //ĳ���� �ӵ� ���� ���� ��
    [Range(0.1f, 50.0f)]
    public float VelocityChangeSpd = 0.1f;
    //ĳ���� ���� �̵� �ӵ� ���� �ʱⰪ
    private Vector3 CurrentVelocitySpd = Vector3.zero;
    //ĳ���� ���� �̵� ���� �ʱⰪ ����
    private Vector3 MoveDirect = Vector3.zero;
    //CharacterController ĳ�� �غ�
    private CharacterController characterCtrl = null;
    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionflages = CollisionFlags.None;
    [Header("�ִϸ��̼� �Ӽ�")]
    public AnimationClip animationClipIdle = null;
    public AnimationClip animationClipWalk = null;
    public AnimationClip animationClipRun = null;
    public AnimationClip animationClipAtk_1 = null;
    public AnimationClip animationClipAtk_2 = null;
    public AnimationClip animationClipAtk_3 = null;
    public AnimationClip animationClipAtk_4 = null;
    
    //animation component ĳ�� �غ�
    private Animation animationPlayer = null;
    //ĳ���� ����
    public enum PlayerState { None, Idle, Walk, Run, Atk }

    [Header("ĳ���� ����")]
    public PlayerState playerState = PlayerState.None;

    //���� ����
    public enum PlayerAttackState { atkStep_1, atkStep_2, atkStep_3, atkStep_4 }

    [Header("���� ����")]
    public PlayerAttackState playerAttackState = PlayerAttackState.atkStep_1;

    //���� ���� ���� Ȱ��ȭ ���θ� Ȯ���ϱ� ���� flag ����
    public bool flagNextAttack = false;
    void Start()
    {
        //CharacterController ĳ��
        characterCtrl = GetComponent<CharacterController>();
        //animation ĳ��
        animationPlayer = GetComponent<Animation>();
        //�ڵ� ��� off
        animationPlayer.playAutomatically = false;
        //���� ������� �ִϸ��̼��� STOP
        animationPlayer.Stop();
        //���°��� �⺻��
        playerState = PlayerState.Idle;
        //�ִϸ��̼� ���¿� Ŭ�� ����
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
    /// ĳ���� �̵� �Լ�
    /// </summary>
    /// 
    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.white;
        
        GUILayout.Label("���� �޺� : " + GameManager.instance.mycombo, labelStyle);
        GUILayout.Label("���� HP : "+GameManager.instance.hp,labelStyle);
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
        //����ī�޶� Transform
        Transform cameraTransform = Camera.main.transform;
        //����ī�޶� �ٶ󺸴� ������ ����󿡼� � �����ΰ�
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        //���� ����
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        //Ű ��
        float vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        //���� ���� ���� ��ǥ��
        Vector3 amount = vertical * forward + Horizontal * right;
        //�̵� ����
        MoveDirect = Vector3.RotateTowards(MoveDirect, amount, DirectionRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        MoveDirect = MoveDirect.normalized;
        //�̵� �ӵ�
        float spd = moveSpd;
        //���࿡ playerState�� Run�̸�
        if (playerState == PlayerState.Run)
        {
            spd = runMoveSpd;
        }
        //�̵��ϴ� ������ ��
        Vector3 moveAmount = (MoveDirect * spd * Time.deltaTime);
        //���� �̵�
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
    /// ���� ���� �Լ� �ۼ�
    /// </summary>
    void BodyDirectionChange()
    {
        //�����̰� �ִ°�?
        if (GetVelocitySpd() > 0.0f)
        {
            //ĳ���� ������ �ٶ� ������? ĳ���� �ӵ� ����
            Vector3 newForward = characterCtrl.velocity;
            newForward.y = 0.0f;
            //ĳ���͸� �������� ������ �����Ѵ�.
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
                                            //������ ��ü
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
    /// �ִϸ��̼� ��� �Լ�
    /// </summary>
    /// <param name="clip">�ִϸ��̼� Ŭ��</param>
    void playAnimationByClip(AnimationClip clip)
    {
        //animationPlayer.clip = clip;
        //animationPlayer.GetClip(clip.name);
        animationPlayer.CrossFade(clip.name);
    }

    /// <summary>
    /// �÷��̾� ���¿� ���� �ִϸ��̼��� ���
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
    /// ���¿� ���� �������ִ� �Լ�
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
    /// ���� ��ư üũ �Լ�
    /// </summary>
    void InputAttackCtrl()
    {
        //���콺 Ŭ�� ����
        if (Input.GetMouseButtonDown(0) == true)
        {
            //�÷��̾ ���ݻ���
            if (playerState != PlayerState.Atk)
            {
                //�÷��̾ ���ݻ��°� �ƴϸ� ���� ���·� �����
                playerState = PlayerState.Atk;
                //���ݻ��� �ʱ�ȭ
                playerAttackState = PlayerAttackState.atkStep_1;
            }
            else
            {
                //�÷��̾� ���°� ������ �� 
                //���� ���¿� ���� �з�
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
    /// CallBack ���� �ִϸ��̼� ����� ������ ȣ�� �Ǵ� �ִϸ��̼� �̺�Ʈ �Լ�
    /// </summary>
    void OnPlayerAttackFinshed()
    {
        //���࿡ flagNextAttack�� true��
        if (flagNextAttack == true)
        {
            //flagNextAttkack �ʱ�ȭ
            flagNextAttack = false;

            //���� ���� �ִϸ��̼� ���¿� ���� ���� �ִϸ��̼� ���°��� �ֱ�
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
    /// �ִϸ��̼� Ŭ�� ����� ������ �ִϸ��̼� �̺�Ʈ �Լ��� ȣ��
    /// </summary>
    /// <param name="clip">�ִϸ��̼� Ŭ��</param>
    /// <param name="funcName">�̺�Ʈ �Լ�</param>
    void SetAnimationEvent(AnimationClip clip, string funcName)
    {
        //���ο� �̺�Ʈ ����
        AnimationEvent newAnimationEvent = new AnimationEvent();

        //�ش� �̺�Ʈ�� ȣ���� funcName
        newAnimationEvent.functionName = funcName;
        newAnimationEvent.time = clip.length - 0.1f;
        clip.AddEvent(newAnimationEvent);
    }

    /// <summary>
    /// ���� �ִϸ��̼� ���
    /// </summary>
    void AtkAnimationCtrl()
    {
        //���� ���ݻ��°�?
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
