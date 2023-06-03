using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  クリア時のランキングスコア
public class RankingScore : MonoBehaviour
{
    public int score;
    public void SetupScore()
    {
        score = QuestManager.score;
    }
}
