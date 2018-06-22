﻿using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 담당자 : 박상원
/// --------------------------------------------------
/// Character를 상속받아
/// MoveController 스크립트에서 이동 방향을
/// Move 메서드의 매개변수에 전달해주면서
/// Player의 공격을 활성화하여 이동과 함께
/// 공격을 하도록 하였습니다.
/// --------------------------------------------------
/// CharacterSelectSceneCtrl 스크립트에서
/// 캐릭터 선택 씬 화면에서 플레이어 캐릭터 선택시
/// 선택된 캐릭터 타입을 InputManager에 알려주고
/// InputManager에서 GameMediator에 전달하여
/// Character 스크립트를 상속받은 Player 스크립트에
/// 정의되어 있는 ChangePlayer 메서드로 전달되어
/// 게임 화면에 표시해줄 플레이어 캐릭터를 활성화한다.
/// --------------------------------------------------
/// 스크립트 주석친 부분은 최근 무기 발사와 관련하여
/// 테스트 하기 위해 남겨둔 코드입니다.
/// --------------------------------------------------
/// </summary>

public class Player : Character
{
    [SerializeField]
    PlayerType playerType;


    [SerializeField]
    int attackCount;

    [SerializeField]
    float fireRate = 0.2f;
    float timeRate = 0.0f;
    float reloadAmmo = 0.5f;
    float reloadTime = 0.0f;
    float coolTime = 3.0f;
    bool EmptyAmmo = true;
    bool fire = false;

    [SerializeField]
    int magazine;

    [SerializeField]
    int maxMagazine = 2;
    int magazineAddCount;

    [SerializeField]
    private GameObject[] playerModel;

    [SerializeField]
    float CoolTime = 3.0f;


    [SerializeField]
    GameObject barrier;
   
    SubAttackCtrl subAttackCtrl;
    MainAttackCtrl mainAttackCtrl;
    GuaidanceMove homingMove;


    private void Start()
    {
        gameMediator = GameObject.FindGameObjectWithTag("GameMediator").GetComponent<GameMediator>();
        DontDestroyOnLoad(gameObject);
        rigidbody = GetComponent<Rigidbody>();
        subAttackCtrl = FindObjectOfType<SubAttackCtrl>();
        barrier.SetActive(false);
        magazine = maxMagazine;
        mainAttackCtrl = gameObject.GetComponentInChildren<MainAttackCtrl>();
          
    }

    private void Update()
    {
        if (attacking)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = playerDirection * moveSpeed;
        rigidbody.position = new Vector3
        (
            Mathf.Clamp(rigidbody.position.x, xMin, xMax),
            0.0f,
            Mathf.Clamp(rigidbody.position.z, zMin, zMax)
        );
        rigidbody.rotation = Quaternion.Euler(0, 0, rigidbody.velocity.x * -tilt);
    }

    public override void Attack()
    {
        //공격 관련 프리펩은 나중에 하나의
        //오브젝트에 모아놓기로 결정
        if (timeRate <= fireRate)
        {
            timeRate += Time.deltaTime;
            return;
        }
        else if (timeRate >= fireRate)
        {
           
            mainAttackCtrl.Attack(power);
            if (playerType == PlayerType.Deung)
            {
                AmmoSpend();
            }
            timeRate = 0.0f;
            attackCount++;
        }

        if (attackCount == 3 && playerType!=PlayerType.Deung)
        {
            subAttackCtrl.Attack(power);
            //Debug.Log("보조 무장 작동 확인");
            attackCount = 0;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
      
              
    }
 

    public void ChangePlayer(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Sin:
                playerModel[0].SetActive(true);
                playerModel[1].SetActive(false);
                playerModel[2].SetActive(false);
                playerType = type;
                barrier.SetActive(false);
                break;
            case PlayerType.Ho:
                playerModel[0].SetActive(false);
                playerModel[1].SetActive(true);
                playerModel[2].SetActive(false);
                playerType = type;
                barrier.SetActive(false);
                break;
            case PlayerType.Deung:
                playerModel[0].SetActive(false);
                playerModel[1].SetActive(false);
                playerModel[2].SetActive(true);
                playerType = type;
                barrier.SetActive(true);
                break;

            default:
                break;
        }
        mainAttackCtrl.playerType = playerType;
        subAttackCtrl.playerType = playerType;
    }

    public void AddAmmo()
    {
        if (maxMagazine < 5)
        {
            maxMagazine = (int)(maxMagazine + Math.Truncate(power / 10f));
        }
        else
        {
            return;
        }
    }

    public void AmmoSpend()
    {
        if (magazine > 0)
        {
            --magazine;
            //Debug.Log("남은 잔탄 수 확인 : " + magazine);
        }
        else if (EmptyAmmo && magazine==0)
        {
            StartCoroutine(AmmoReload());
        }
    }

    IEnumerator AmmoReload()
    {
        EmptyAmmo = false;
        yield return new WaitForSeconds(5.0f);

        //Debug.Log("재장전 대기 시간 종료");
        magazine = maxMagazine;
        //Debug.Log("재장전 장탄 수 확인 : " + magazine);
        EmptyAmmo = true;
        StopCoroutine(AmmoReload());
    }

  
}
