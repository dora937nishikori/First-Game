using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

//敵を管理
public class EnemyManager : MonoBehaviour
{

    public int hp;
    public int atk;
    public new string name;
    public GameObject hitEffect;
    public QuestManager questManager;


    //関数登録
    Action tapAction;//クリックされたときに実行したい関数

    //敵と遭遇時にHPとATKを決定
    public void SetUpEnemy()
    {
        int hp_rnd = Random.Range(1,21);
        int atk_rnd = Random.Range(1,11);
        hp += hp_rnd;
        atk += atk_rnd;
    }
    
    //攻撃
    public int Attack(PlayerManager player)
    {
        int damage = player.Damage(atk);
        return damage;
    }

    //ダメージを受ける
    public int Damage(int damage)
    {
        Instantiate(hitEffect);
        transform.DOShakePosition(0.3f,0.5f,20,0,false,true);
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }
        return damage;
    }

    //tapActionに関数を登録する関数
    public void AddEvenListenerOnTap(Action action)
    {
        tapAction += action;
    }

    public void OnTap()
    {
        tapAction();
    }
}
