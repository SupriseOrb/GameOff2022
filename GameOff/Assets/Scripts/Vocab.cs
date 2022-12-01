using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vocab
{
    public static string SEPARATE(string[] pieces)
    {
        string temp = "";
        string seperator = " | ";
        for (int i = 0; i < pieces.Length - 1; i++)
        {
            temp += pieces[i] + seperator;
        }
        temp += pieces[pieces.Length - 1];
        return temp;
    }
    public static string VAR(float amount)
    {
        return "<color=#A6823F>" + amount.ToString("0.00") + "</color>";
    }

    public static string VAR(int amount)
    {
        return "<color=#A6823F>" + amount + "</color>";
    }
    public static string PLAYER_UNIT = "Allied Unit";
    public static string SUMMON = "Allied Summon";
    public static string ENEMY_UNIT = "Enemy Unit";
    public static string LAND = "Land";
    public static string ITEM = "Item";
    public static string SPELL = "Spell";
    public static string INKCOST(int amount)
    {
        return VAR(amount) + " Ink";
    }

    public static string HEALTH(float amount)
    {
        return VAR(amount) + " Health";
    }
    
    public static string INKDELTA(int amount)
    {
        return VAR(amount) + " Ink/Sec";
    } 
    public static string DAMAGE(float amount)
    {
        return VAR(amount) + " Attack Damage";
    }

    public static string STUN_DURATION(float amount)
    {
        return VAR(amount) + "sec Stun";
    }

    public static string PIERCE_AMOUNT(int amount)
    {
        return VAR(amount) + " Pierce Amount";
    }
    
    public static string ATKSPD(float amount)
    {
        return VAR(amount) + " Attacks/Sec";
    }

    public static string COOLDOWN(float amount)
    {
        return VAR(amount) + " Cooldown";
    }

    public static string MINION_HEALTH(int amount)
    {
        return VAR(amount) + " Minion Health";
    }

    public static string MINION_DAMAGE(float amount)
    {
        return VAR(amount) + " Minion Damage";
    }

    public static string MINION_ATKSPD(float amount)
    {
        return VAR(amount) + " Minion Attacks/Sec";
    }

    public static string PUSH_DISTANCE(float amount)
    {
        return VAR(amount) + " Push Distance";
    }

    public static string SLOW_INTENSITY(float amount)
    {
        return VAR(amount) + " Slow Intensity";
    }
}
