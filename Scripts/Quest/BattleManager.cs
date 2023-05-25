using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//対戦の管理
public class BattleManager : MonoBehaviour
{
    public Transform cameraObj;
    public Transform dialogUI;
    public QuestManager questManager;
    public PlayerManager player;
    public EnemyManager enemy;
    public PlayerUIManager playerUI;
    public EnemyUIManager enemyUI;
    public bool canAttack = false;

    private void Start()
    {
        enemyUI.gameObject.SetActive(false);
        playerUI.SetupUI(player);
    }

    public void Setup(EnemyManager enemyManager)
    {
        canAttack = true;
        SoundManager.instance.PlayBGM("Battle");
        enemyUI.gameObject.SetActive(true);
        enemy = enemyManager;
        enemy.SetUpEnemy();//敵のHPとATKを決定
        enemy.atk += questManager.currentStage;//ステージを進むにつれて敵を強くする
        enemy.hp += questManager.currentStage;
        enemyUI.SetupUI(enemy);
        enemy.AddEvenListenerOnTap(PlayerAttack);
    }

    //ボス戦
    public void SetupBoss(EnemyManager enemyManager)
    {
        Debug.Log("setupBoss");
        canAttack = true;
        SoundManager.instance.PlayBGM("Boss");
        enemyUI.gameObject.SetActive(true);
        enemy = enemyManager;
        enemyUI.SetupUI(enemy);
        enemy.AddEvenListenerOnTap(PlayerAttack);
    }


    void PlayerAttack()
    {
        if (canAttack == false)
        {
            return;
        }
        canAttack = false;
        StopAllCoroutines();
        SoundManager.instance.PlaySE(1);
        int damage = player.Attack(enemy);
        enemyUI.UpdateUI(enemy);
        DialogTextManager.instance.SetScenarios(new string[]{"プレイヤーの攻撃！\nモンスターに"+damage+"ダメージを与えた！"});
        if(enemy.hp <= 0)
        {
            StartCoroutine(EndBattle());
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);
        SoundManager.instance.PlaySE(1);
        cameraObj.DOShakePosition(0.3f,0.5f,20,0,false,true);
        int damage = enemy.Attack(player);   
        playerUI.UpdateUI(player);
        DialogTextManager.instance.SetScenarios(new string[]{"モンスターの攻撃！\nプレイヤーは"+damage+"ダメージを受けた！"});
        if (player.hp <= 0)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(questManager.PlayerDead());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }
    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2f);
        DialogTextManager.instance.SetScenarios(new string[]{"モンスターを倒した"});
        enemyUI.gameObject.SetActive(false);
        Destroy(enemy.gameObject);
        SoundManager.instance.PlayBGM("Quest");
        questManager.EndBattle();
        QuestManager.playerAtk += 2;//敵を倒すごとに攻撃力+2
        player.updateATK();//プレイヤーの攻撃力を更新
        playerUI.UpdateUI(player);//ATKのUI更新
    }
}
