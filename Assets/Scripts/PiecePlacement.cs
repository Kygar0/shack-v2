using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePlacement : MonoBehaviour
{
    #region Getting the pieces
    [SerializeField]
    GameObject pawn; 
    [SerializeField]
    GameObject rook; 
    [SerializeField]
    GameObject bishop; 
    [SerializeField]
    GameObject knight;
    [SerializeField]
    GameObject king;
    [SerializeField]
    GameObject queen;
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {

            SetBoard(pawn, rook, bishop, knight, king, queen); // Gives the gameobjects
            
        }
    }

    static void SetBoard(GameObject pawn, GameObject rook, GameObject bishop, GameObject knight, GameObject king, GameObject queen) // Needs the gameobjects
    {
        #region Setts The board
        for (float i = -3; i <= 4; i++) // Pawn, Places One on every tile from -3 to 4
        {
            Instantiate(pawn, new Vector3(i, -3, 0), Quaternion.identity);
            Instantiate(pawn, new Vector3(i, 2, 0), Quaternion.identity);
        }
        for (float i = -3f; i <= 4; i += 7) //Rook, places one at starting postion and then one 7 tiles to the side, with a gap between them of 6 
        {
            Instantiate(rook, new Vector3(i, -4, 0), Quaternion.identity); 
            Instantiate(rook, new Vector3(i, 3, 0), Quaternion.identity);
        }
        for (float i = -1; i <= 2; i += 3) // Bishop, On starting tile 2 and then a gap of 2 tiles then another bishop
        {
            Instantiate(bishop, new Vector3(i, -4, 0), Quaternion.identity);
            Instantiate(bishop, new Vector3(i, 3, 0), Quaternion.identity);
        }
        for (float i = -2; i <= 3; i += 5) // Knight, Gap of 4 tiles
        {
            Instantiate(knight, new Vector3(i, -4, 0), Quaternion.identity);
            Instantiate(knight, new Vector3(i, 3, 0), Quaternion.identity);
        }
        for (float i = 1; i < 2; i++) // Queen, creates a queen at a fixed position
        {
            Instantiate(queen, new Vector3(0, -4, 0), Quaternion.identity);
            Instantiate(queen, new Vector3(0, 3, 0), Quaternion.identity);
        }
        for (float i = 1; i < 2; i += 1) // King, creates a King at a fixed position
        {
            Instantiate(king, new Vector3(1, -4, 0), Quaternion.identity);
            Instantiate(king, new Vector3(1, 3, 0), Quaternion.identity);
        }
        #endregion
    }
}
