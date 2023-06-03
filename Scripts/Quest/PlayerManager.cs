using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int hp;
    public int atk;

    //街に戻ってもATKを保持
    public void updateATK()
    {
        atk = QuestManager.playerAtk;
    }

    //攻撃
    public int Attack(EnemyManager enemy)
    {
        int damage = enemy.Damage(atk);
        return damage;
    }

    //ダメージを受ける
    public int Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }
        return damage;
    }
}
