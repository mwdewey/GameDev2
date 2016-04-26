﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class PrintScores : MonoBehaviour {

    public RuntimeAnimatorController shifter_anim;
    public RuntimeAnimatorController vegano_anim;
    public RuntimeAnimatorController rich_anim;
    public RuntimeAnimatorController missq_anim;

    private readonly Color32 P1_Color = new Color32(244, 67, 54, 255);
    private readonly Color32 P2_Color = new Color32(33, 150, 243, 255);
    private readonly Color32 P3_Color = new Color32(76, 175, 80, 255);
    private readonly Color32 P4_Color = new Color32(255, 235, 59, 255);

	// Use this for initialization
	void Start () {
        int playerCount = Input.GetJoystickNames().Length;
        playerCount = 4;

        for (var i = 0; i < 1; i++)
        {
            PlayerPrefs.SetInt("p" + (i + 1) + "score",i*4);
            PlayerPrefs.SetInt("Player" + (i + 1) + "CharSelect",(int)CharCodes.Shifter);
        }

        List<PlayerEnd> players = new List<PlayerEnd>();
        for (var i = 0; i < playerCount; i++)
        {
            int score = PlayerPrefs.GetInt("p" + (i + 1) + "score");
            CharCodes charCode = (CharCodes) PlayerPrefs.GetInt("Player" + (i + 1) + "CharSelect");

            players.Add(new PlayerEnd((i + 1), charCode, score));
        }

        List<PlayerEnd> sortedPlayers = players.OrderBy(p => p.score).ToList();
        
        List<Place> places = new List<Place>();
        for (var i = 0; i < playerCount; i++)
        {
            GameObject place = transform.Find("Rank " + (i + 1)).gameObject;
            Animator anim = place.GetComponent<Animator>();
            Text text = place.transform.Find("Text").GetComponent<Text>();
            Image img = place.transform.Find("background").GetComponent<Image>();

            places.Add(new Place(anim, text, img));
        }

        for (var i = 0; i < playerCount; i++)
        {
            PlayerEnd player = sortedPlayers[i];
            Place place = places[playerCount-1-i];

            string placeText = "";
            switch (playerCount-1-i)
            {
                case 0: placeText = "1st"; break;
                case 1: placeText = "2nd"; break;
                case 2: placeText = "3rd"; break;
                case 3: placeText = "4th"; break;
            }

            switch (player.charCode)
            {
                case CharCodes.MissQ: 
                    place.anim.runtimeAnimatorController = missq_anim; 
                    break;
                case CharCodes.Shifter: 
                    place.anim.runtimeAnimatorController = shifter_anim;
                    place.anim.SetFloat("animSpeed", 15 / 8f);
                    break;
                case CharCodes.Vegano: 
                    place.anim.runtimeAnimatorController = vegano_anim; 
                    break;
                case CharCodes.Rich: 
                    place.anim.runtimeAnimatorController = rich_anim;
                    place.anim.SetFloat("animSpeed",6/8f);
                    break;
            }

            switch (player.id)
            {
                case 1: place.background.color = P1_Color; break;
                case 2: place.background.color = P2_Color; break;
                case 3: place.background.color = P3_Color; break;
                case 4: place.background.color = P4_Color; break;

            }

            if (playerCount - 1 - i == 0) place.background.transform.parent.Find("super background").gameObject.GetComponent<Image>().color = place.background.color;

            string textOutput = placeText + " Place : Score " + player.score;

            place.text.text = textOutput;

        }

        for (int i = 0; i < 4; i++) if (i > playerCount - 1) transform.Find("Rank " + (i + 1)).gameObject.SetActive(false);

        // update leaderboards
        Stats bestStats = new Stats();

        for (int i = 1; i < playerCount+1; i++)
        {
            string pid = i.ToString();
            PlayerStatStruct stats = PlayerStats.getStats(pid);

            if (bestStats.kills.value <= stats.kills) bestStats.kills = new Stat(stats.kills, pid);
            if (bestStats.deaths.value <= stats.deaths) bestStats.deaths = new Stat(stats.deaths, pid);
            if (bestStats.coinsLost.value <= stats.coinsLost) bestStats.coinsLost = new Stat(stats.coinsLost, pid);
            if (bestStats.coinsGained.value <= stats.coinsGained) bestStats.coinsGained = new Stat(stats.coinsGained, pid);
            if (bestStats.attacksDone.value <= stats.attacksDone) bestStats.attacksDone = new Stat(stats.attacksDone, pid);
            if (bestStats.itemsUsed.value <= stats.itemsUsed) bestStats.itemsUsed = new Stat(stats.itemsUsed, pid);
            if (bestStats.damageDone.value <= stats.damageDone) bestStats.damageDone = new Stat(stats.damageDone, pid);
            if (bestStats.damageReceived.value <= stats.damageReceived) bestStats.damageReceived = new Stat(stats.damageReceived, pid);

        }

        GameObject statsObject = transform.Find("Stats").gameObject;
        for (int i = 1; i < 9; i++)
        {
            GameObject statObject = statsObject.transform.Find(i.ToString()).gameObject;

            Text descText = statObject.transform.Find("Text").gameObject.GetComponent<Text>();
            Image icon = statObject.transform.Find("playerIcon").gameObject.GetComponent<Image>();
            Text valueText = statObject.transform.Find("playerIcon").Find("Text").gameObject.GetComponent<Text>();


        }


	}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene("main_menu");
        }
    }
	
}

public class Place
{
    public Text text;
    public Animator anim;
    public Image background;

    public Place(Animator a, Text txt, Image bkgrnd) {

        text = txt;
        anim = a;
        background = bkgrnd;
    }
}

public class PlayerEnd
{
    public int id;
    public CharCodes charCode;
    public int score;

    public PlayerEnd(int _id, CharCodes _c, int _score)
    {
        id = _id;
        charCode = _c;
        score = _score;
    }

}

public class Stats
{
    public Stat kills;
    public Stat deaths;
    public Stat coinsLost;
    public Stat coinsGained;
    public Stat attacksDone;
    public Stat itemsUsed;
    public Stat damageDone;
    public Stat damageReceived;

    public Stats() { }
}

public class Stat
{
    public int value;
    public string pid;

    public Stat(int value_, string pid_) 
    {
        value = value_;
        pid = pid_;
    }
}
