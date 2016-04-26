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

            string textOutput = placeText + " Place: Player " + player.id + " Score:" + player.score;

            place.text.text = textOutput;

        }

        for (int i = 0; i < 4; i++) if (i > playerCount - 1) transform.Find("Rank " + (i + 1)).gameObject.SetActive(false);

        


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
