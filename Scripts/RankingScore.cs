using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingScore : MonoBehaviour
{
    public int score;//街に戻った回数
    public void SetupScore()
    {
        score = QuestManager.score;
    }
}
