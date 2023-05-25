using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using DG.Tweening;
//クエスト全体を管理
public class QuestManager : MonoBehaviour
{
    public StageUIManager stageUI;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject boss;
    public BattleManager battleManager;
    public PlayerManager player;
    public PlayerUIManager playerUI;
    public SceneTransitionManager sceneTransitionManager;
    public GameObject questBG;
    public Transform dialogUI;
    public RankingScore rankingScore;
    public static int playerAtk = 10;
    public static int score = 0;
    public static int defeatCount = 0;
    

    int[] encountTable = new int[30];//敵に遭遇するテーブル:最大30ステージ
    public int currentStage = 0;//現在のステージ

    public void Start()
    {
        //ランダムエンカウント設定
        Random random = new Random();
        for(int i = 0; i < encountTable.Length; i++)
        {
            encountTable[i] = random.Next(0,7);
        }

        stageUI.UpdateUI(currentStage);
        DialogTextManager.instance.SetScenarios(new string[]{"森についた"});
        player.SetupAtk();
        rankingScore.SetupScore();//score保持
        defeatCount = 0;//街に戻ると連続撃破数はリセット
    }

    IEnumerator Searching()
    {
        DialogTextManager.instance.SetScenarios(new string[]{"探索中..."});
        //背景を大きく
        questBG.transform.DOScale(new Vector3(1.5f,1.5f,1.5f),2f).OnComplete(()=>questBG.transform.localScale = new Vector3(1,1,1));
        //フェードアウト
        SpriteRenderer questBGSR = questBG.GetComponent<SpriteRenderer>();
        questBGSR.DOFade(0,2f).OnComplete(()=> questBGSR.DOFade(1,0));
        //2秒遅延
        yield return new WaitForSeconds(2f);

        currentStage++;

        //進行度をUIに反映
        stageUI.UpdateUI(currentStage);
        
        if(encountTable.Length <= currentStage)
        {
            QuestClear();
        }
        else if (currentStage == encountTable.Length - 1)
        {
            EncountBoss();
        }
        else if(encountTable[currentStage] <= 2)
        {
            EncountEnemy();
        }
        else
        {
            stageUI.ShowButtons();
        }
    }
    //NextButtonを押したときに起こる関数
    public void OnNextButton()
    {
        SoundManager.instance.PlaySE(0);
        stageUI.HideButtons();
        StartCoroutine(Searching());
    }
    public void OnToTownButton()
    {
        //街に戻る回数を記録
        score += 1;
        rankingScore.SetupScore();//score保持
        SoundManager.instance.PlaySE(0);
    }

    void EncountEnemy()
    {
        DialogTextManager.instance.SetScenarios(new string[]{"モンスターに遭遇した！"});
        stageUI.HideButtons();
        //遭遇する敵を決定
        GameObject enemyObject;
        if (encountTable[currentStage] == 0)
        {
            enemyObject = Instantiate(enemy1);
        }
        else if(encountTable[currentStage] == 1)
        {
            enemyObject = Instantiate(enemy2);
        }
        else 
        {
            enemyObject = Instantiate(enemy3);
        }
        EnemyManager enemy= enemyObject.GetComponent<EnemyManager>();
        battleManager.Setup(enemy);
    }

    void EncountBoss()
    {
        DialogTextManager.instance.SetScenarios(new string[]{"ボスモンスターが現れた！"});
        stageUI.HideButtons();
        GameObject enemyObject = Instantiate(boss);
        EnemyManager enemy= enemyObject.GetComponent<EnemyManager>();
        battleManager.SetupBoss(enemy);
    }

    public void EndBattle()
    {
        //3連続撃破でhpを50回復(最大100)
        defeatCount += 1;
        if (defeatCount == 3)
        {
            if (player.hp <= 50)
            {
                player.hp += 50;
            }
            else
            {
                player.hp = 100;
            }
            defeatCount = 0;
        }
        stageUI.ShowButtons();
    }

    void QuestClear()
    {
        DialogTextManager.instance.SetScenarios(new string[]{"ステージ30到達！\nGAME CLEAR！！"});
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(2);
        //ランキング登録
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (score);
        //クリア画面の表示
        stageUI.ShowClearText();
        playerAtk = 10;//ゲームクリアで攻撃力リセット
        score = 0;
    }

    public IEnumerator PlayerDead()
    {
        playerAtk = 10;//GAME OVERで攻撃力リセット
        score = 0;
        DialogTextManager.instance.SetScenarios(new string[]{"GAME OVER"});
        yield return new WaitForSeconds(1f);
        DialogTextManager.instance.SetScenarios(new string[]{"ステージ"+currentStage+"まで到達した！"});
        yield return new WaitForSeconds(2f);
        sceneTransitionManager.LoadTo("Title");
    }

}
