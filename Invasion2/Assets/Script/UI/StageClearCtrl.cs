﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 작성자 : 최대원
/// 스테이지 클리어 시 나타나는 패널의 컨트롤러
/// </summary>
public class StageClearCtrl : MonoBehaviour {

    [SerializeField]
    Text getScoreText;
    [SerializeField]
    Text maxScoreText;
    const int addScore = 7;

    public void OnClickReturnToTitle()
    {
        GameMediator.Instance.GameOver();
        Time.timeScale = 1;
        GameMediator.Instance.ChangeScene(SceneState.Title);
       
    }

    public void OnClickRestart()
    {
        GameMediator.Instance.GameOver();
        Time.timeScale = 1;
        gameObject.SetActive(false);
        StageManager.Instance.restart();
        StageManager.Instance.NextStage();
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        getScoreText.text = GameMediator.Instance.ReadCurrentScore().ToString();
        maxScoreText.text = GameMediator.Instance.ReadCurrentScore().ToString();
    }

}
