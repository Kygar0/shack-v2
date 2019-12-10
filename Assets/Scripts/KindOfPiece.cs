using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KindOfPiece : MonoBehaviour
{
    public bool isPawn = false;
    public bool isRook = false;
    public bool isBishop = false;
    public bool isKnight = false;
    public bool isKing = false;
    public bool isQueen = false;

    void Start()
    {
        // Just checks the location of the piece then assigns it a name based of its starting position, inflexible but the pieces will always start at the same places
        for (float i = -3; i <= 4; i++)
        {
            if (gameObject.transform.position == new Vector3(i, -3, 0) || gameObject.transform.position == new Vector3(i, 2, 0))
            {
                gameObject.GetComponent<KindOfPiece>().isPawn = true;
            }

        }
        for (float i = -3f; i <= 4; i += 7) 
        {
            if (gameObject.transform.position == new Vector3(i, -4, 0) || gameObject.transform.position == new Vector3(i, 3, 0))
            {
                gameObject.GetComponent<KindOfPiece>().isRook = true;
            }
        }
        for (float i = -1; i <= 2; i += 3) 
            if (gameObject.transform.position == new Vector3(i, -4, 0) || gameObject.transform.position == new Vector3(i, 3, 0))
            {
                gameObject.GetComponent<KindOfPiece>().isBishop = true;
            }
        for (float i = -2; i <= 3; i += 5) 
        {
            if (gameObject.transform.position == new Vector3(i, -4, 0) || gameObject.transform.position == new Vector3(i, 3, 0))
            {
                gameObject.GetComponent<KindOfPiece>().isKnight = true;
            }
        }
        if (gameObject.transform.position == new Vector3(1, -4, 0) || gameObject.transform.position == new Vector3(1, 3, 0))
        {
            gameObject.GetComponent<KindOfPiece>().isKing = true;
        }
        if (gameObject.transform.position == new Vector3(0, -4, 0) || gameObject.transform.position == new Vector3(0, 3, 0))
        {
            gameObject.GetComponent<KindOfPiece>().isQueen = true;
        }


    }




}



