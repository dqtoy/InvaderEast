﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 작성자 : 최대원
/// 스테이지 클리어 시 나타나는 패널의 컨트롤러
/// </summary>
public class StageClearCtrl : MonoBehaviour {

    public void OnClickReturnToTitle()
    {
        Time.timeScale = 1;
        InputManager.Instance.ChangeScene(SceneState.Title);
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        StageManager.Instance.restart();
        StageManager.Instance.NextStage();
    }

}
