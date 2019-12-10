using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kungscript : MonoBehaviour
{
    [SerializeField]
    GameObject text;

    GameObject[] pieces;     //förbereder en lista på alla befintliga pjäser      

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pieces = GameObject.FindGameObjectsWithTag("Piece");  //stoppar in pjäser i listan när man trycker space
        }

        if (Input.GetMouseButtonUp(0))      //aktiveras när mus 1 går upp
        {

            foreach (GameObject piece in pieces)     //letar reda på bitar i listan pieces 
            {
                if (piece.GetComponent<KindOfPiece>().isKing == true)  // kollar om biten är kungen
                {
                    if (piece.GetComponent<DarkPiece>().isDark == true && piece.activeSelf == false) //kollar om pjäsen är svart och om den är död
                    {
                        text.GetComponent<Text>().text = "White Wins!" + "\n(Press Enter To Restart)";    //om pjäsen är svart och död skriver spelet "white winns" och "press enter restart"
                    }
                    else if (piece.GetComponent<DarkPiece>().isDark == false && piece.activeSelf == false)     //kollar om pjäsen är vit och om den är död
                    {
                        text.GetComponent<Text>().text = "Black wins" + "\n(Press Enter To Restart)";      //om pjäsen är vit och död skriver spelet "black winns" och "press enter restart"
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))   //kollar om man trycker på enter
        {
            foreach (GameObject item in pieces)
            {
                Destroy(item);                       //förstör alla pjäser i listan pieces
            }
            text.GetComponent<Text>().text = "";     //tar bort förra texten
            GameObject.Find("TurnManager").GetComponent<PieceMovement>().whiteToMove = true;   //gör så att vit börgar spelet
        }

    }
}
