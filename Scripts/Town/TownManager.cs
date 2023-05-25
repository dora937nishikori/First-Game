using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    public Transform dialogUI;

    void Start()
    {
        DialogTextManager.instance.SetScenarios(new string[]{"街についた"});  
    }
    public void OnToQuestButton()
    {
        SoundManager.instance.PlaySE(0);
    }
}
