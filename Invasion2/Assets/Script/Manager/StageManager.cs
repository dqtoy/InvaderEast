﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 난이도 설정
/// </summary>
public enum Difficult
{
    Easy,
    Normal,
    Hard,
}
/// <summary>
/// 재시작을 위한 델리게이트
/// </summary>
public delegate void Restart();

/// <summary>
/// 담당자 : 최대원
/// 메디에이터를 통해 스테이지 시작 요청 받으면 코루틴 시작, 
/// 코루틴 내부에서 스테이지에 맞게 적 생성, 
/// List를 통해 에너미 관리
/// </summary>

public class StageManager : Singleton<StageManager>
{
    [SerializeField]
    List<GameObject> EnemyList;
    [SerializeField]
    GameObject[] TransformList;
    [SerializeField]
    EnemyFactory Factory;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    BoxCollider Boundary;

    public Restart restart;
    CoroutineCtrl stageCoroutineCtrl;
    
    const int MaxStage = 3;
    int CurrentStage;
    Difficult difficult;
    int transformNumber;
    
    

    //스테이지 패턴 발생 간격
    float callCoroutineTick = 5f;

    protected StageManager()
    {
        CurrentStage = 0;
    }

    private void Start()
    {
        
        enemyPrefab = Resources.Load("Enemy") as GameObject;
        EnemyList = new List<GameObject>();
        CurrentStage = 0;
        stageCoroutineCtrl = new CoroutineCtrl(this, enemyPrefab);
        //  Factory = new EnemyFactory(gameMediator);
        Factory = new EnemyFactory();
        restart += new Restart(SetRestart);
      
    }

    public void SetStage()
    {
        Debug.Log("SetStage");
        TransformList = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
    //    Boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<BoxCollider>();
    }

    public void Spawn(GameObject enemy)
    {
     //   if(enemy.tag == "Enemy")
        {
            transformNumber = Random.Range(0,TransformList.Length-1);
            GameObject SpawnEnemy = Factory.CreateEnemy();
            SpawnEnemy.transform.position = TransformList[transformNumber].transform.position;
            EnemyList.Add(SpawnEnemy);
        }
   //     else Debug.Log("Spawn() 메서드의 tag 불일치!");
    }

    public void RemoveAllEnemy()
    {
        foreach (GameObject item in EnemyList)
        {
            GameMediator.Instance.PutEnemyObject(item.gameObject);
        }
        EnemyList.Clear();
        Debug.Log("RemoveAllEnemy 완료");
    }

    public void RemoveEnemy(GameObject enemy)
    { 
        EnemyList.Remove(enemy);
        GameMediator.Instance.PutEnemyObject(enemy);
    }

    public void NextStage()
    {
        
            if (CurrentStage == 0)
            {
                StartCoroutine(StageCoroutine(CurrentStage));
                CurrentStage++;
            }
            else if (CurrentStage <= MaxStage)
            {
                StopCoroutine(StageCoroutine(CurrentStage));
                StartCoroutine(StageCoroutine(CurrentStage++));
            }
            else
            {
                Debug.Log("다음 스테이지 없음");
            }
        
    }


    public void SetDifficulty(Difficult difficult)
    {
        this.difficult = difficult;
        Debug.Log("Set Difficulty : " + difficult);
    }

    public Difficult GetDifficulty()
    {
        Debug.Log("Get Difficulty : " + difficult);
        return difficult;
    }


    //실질석인 스테이지 타임라인
    IEnumerator StageCoroutine(int stageLevel)
    {
       
        Debug.Log("스테이지 " + stageLevel + " 시작");
        yield return new WaitForSeconds(callCoroutineTick);

        StartCoroutine(stageCoroutineCtrl.StagePattern1());
        yield return new WaitForSeconds(callCoroutineTick);

        StartCoroutine(stageCoroutineCtrl.StagePattern2());
        yield return new WaitForSeconds(callCoroutineTick * 3);

        StartCoroutine(stageCoroutineCtrl.StagePattern3());
        yield return new WaitForSeconds(callCoroutineTick * 2);

        StartCoroutine(stageCoroutineCtrl.StagePattern4());
        yield return new WaitForSeconds(callCoroutineTick);

        StartCoroutine(stageCoroutineCtrl.StagePattern1());
        yield return new WaitForSeconds(callCoroutineTick);

        StartCoroutine(stageCoroutineCtrl.StagePattern4());
        yield return new WaitForSeconds(callCoroutineTick);

        StartCoroutine(stageCoroutineCtrl.StagePattern2());
        yield return new WaitForSeconds(callCoroutineTick * 3);

        StartCoroutine(stageCoroutineCtrl.StagePattern1());
        StartCoroutine(stageCoroutineCtrl.StagePattern1());
        yield return new WaitForSeconds(callCoroutineTick);

        StartCoroutine(stageCoroutineCtrl.StagePattern2());
        StartCoroutine(stageCoroutineCtrl.StagePattern3());
        yield return new WaitForSeconds(callCoroutineTick);

        yield break;
    }

    public void SetRestart()
    {
        CurrentStage = 0;
        StopAllCoroutines();
        StopCoroutine(stageCoroutineCtrl.StagePattern1());
        StopCoroutine(stageCoroutineCtrl.StagePattern2());
        StopCoroutine(stageCoroutineCtrl.StagePattern3());
        StopCoroutine(stageCoroutineCtrl.StagePattern4());
    }
   
} 
