using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PrintScores : MonoBehaviour {

    public RuntimeAnimatorController shifter_anim;
    public RuntimeAnimatorController vegano_anim;
    public RuntimeAnimatorController rich_anim;
    public RuntimeAnimatorController missq_anim;

	// Use this for initialization
	void Start () {
        for (var i = 0; i < 0; i++)
        {
            PlayerPrefs.SetInt("p" + (i + 1) + "score",i*4);
            PlayerPrefs.SetInt("Player" + (i + 1) + "CharSelect",(int)CharCodes.Shifter);
        }

        List<PlayerEnd> players = new List<PlayerEnd>();
        for(var i = 0; i < 4; i++){
            int score = PlayerPrefs.GetInt("p" + (i + 1) + "score");
            CharCodes charCode = (CharCodes) PlayerPrefs.GetInt("Player" + (i + 1) + "CharSelect");

            players.Add(new PlayerEnd((i + 1), charCode, score));
        }

        List<PlayerEnd> sortedPlayers = players.OrderBy(p => p.score).ToList();
        
        List<Place> places = new List<Place>();
        for (var i = 0; i < 4; i++)
        {
            GameObject place = transform.Find("Rank " + (i + 1)).gameObject;
            Animator anim = place.GetComponent<Animator>();
            Text text = place.transform.Find("Text").GetComponent<Text>();

            places.Add(new Place(anim,text));
        }

        for (var i = 0; i < 4 ; i++)
        {
            PlayerEnd player = sortedPlayers[i];
            Place place = places[3-i];

            string placeText = "";
            switch (3-i)
            {
                case 0: placeText = "1st"; break;
                case 1: placeText = "2nd"; break;
                case 2: placeText = "3rd"; break;
                case 3: placeText = "4th"; break;
            }

            switch (player.charCode)
            {
                case CharCodes.MissQ: place.anim.runtimeAnimatorController = missq_anim; break;
                case CharCodes.Shifter: place.anim.runtimeAnimatorController = shifter_anim; break;
                case CharCodes.Vegano: place.anim.runtimeAnimatorController = vegano_anim; break;
                case CharCodes.Rich: place.anim.runtimeAnimatorController = rich_anim; break;
            }

            string textOutput = placeText + " Player " + player.id + " Score " + player.score;

            place.text.text = textOutput;

        }

        


	}
	
}

public class Place
{
    public Text text;
    public Animator anim;

    public Place(Animator a, Text txt) {

        text = txt;
        anim = a;

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
