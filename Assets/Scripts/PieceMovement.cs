using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PieceMovement : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject turnManager; 

    PieceMovement pieceMovementScript;
    DarkPiece darkPieceScript; // Makes a variable able to store the DarkPiece script
    public bool whiteToMove = true;

    void Start()
    {
        turnManager = GameObject.FindGameObjectWithTag("TurnManager"); // Stores the TurnManager gameobject in the variable, important bc a universal server is needed for the pieces to check if it's their turn or not and for pieces to easily communicate with eachother

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        darkPieceScript = gameObject.GetComponent<DarkPiece>(); // Stores the DarkPiece script in the variable, easy bc both scirpts are on the same gameobject
        pieceMovementScript = turnManager.GetComponent<PieceMovement>();
    }

    #region Movement
    bool isHeld = false;
    Vector3 placementBeforeMove;
    


    private void OnMouseDown()
    {
        isHeld = true; // Says that something is being held when holding down the mouse button
        placementBeforeMove = gameObject.transform.position;
    }

    private void OnMouseUp()
    {
        isHeld = false; // Says that nothing is being held when the mouse button goes up
        gameObject.transform.position = new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.y), 0); //Snaps the pieces to the center of the square by rounding the number

        //Handles the Turns, moves the piece back if not their turn, also deletes a piece if its is taken
        if (placementBeforeMove != gameObject.transform.position) // If the piece is no longer in its previous position
        {
            
            if (darkPieceScript.isDark == true) // if the piece is dark
            {
                
                if (pieceMovementScript.whiteToMove == true) // if it isn't blacks turn the piece is moved back
                {
                    gameObject.transform.position = placementBeforeMove;
                    print("Can't, It is whites turn " + pieceMovementScript.whiteToMove);
                }
                else if (pieceMovementScript.whiteToMove == false) // if it is the turn is passed normally
                {
                    GameObject[] pieceArray;

                    // (Same as Above) After we check if the piece is allowed to move we want to check if it has taken another piece, for this we put all the placements of the pieces into a array and then check if the current piece has the same position as any of the other pieces
                    pieceArray = GameObject.FindGameObjectsWithTag("Piece"); // Fills the array with the pieces
                    
                    int num = 0;
                    GameObject[] killList = new GameObject[2];
                    foreach (GameObject piece in pieceArray)
                    {
                        if (gameObject.transform.position == piece.transform.position)
                        {
                            killList[num] = piece;

                            num++; // It will trigger on itself, if it triggers again that is some other piece
                            if (num == 2)
                            {
                                foreach (GameObject item in killList)
                                {
                                    if (item.GetComponent<DarkPiece>().isDark == false)
                                    {
                                        item.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                    pieceMovementScript.whiteToMove = true;
                    print("It is now whites turn " + pieceMovementScript.whiteToMove);
                } 
            }
            else if (darkPieceScript.isDark == false) // if the piece is white
            {

                if (pieceMovementScript.whiteToMove == false) // Same thing here as above
                {
                    gameObject.transform.position = placementBeforeMove;
                    print("Can't do that, blacks turn " + pieceMovementScript.whiteToMove);
                }
                else if (pieceMovementScript.whiteToMove == true)
                {
                    GameObject[] pieceArray;

                    // (Same as Above) After we check if the piece is allowed to move we want to check if it has taken another piece, for this we put all the placements of the pieces into a array and then check if the current piece has the same position as any of the other pieces
                    pieceArray = GameObject.FindGameObjectsWithTag("Piece"); // Fills the array with the pieces

                    int num = 0;
                    GameObject[] killList = new GameObject[2];
                    foreach (GameObject piece in pieceArray)
                    {
                        if (gameObject.transform.position == piece.transform.position)
                        {
                            killList[num] = piece;

                            num++; // It will trigger on itself, if it triggers again that is some other piece
                            
                            if (num == 2)
                            {
                                foreach (GameObject item in killList)
                                {
                                    if (item.GetComponent<DarkPiece>().isDark == true)
                                    {
                                        item.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                    pieceMovementScript.whiteToMove = false;
                    print("it is now blacks turn " + pieceMovementScript.whiteToMove);
                }
                
            }
        }
        
    }

    private void Update() // Main Movement function
    {
        if (isHeld == true) // If the mouse button is being pressed
        {
            Vector3 mousePos; // Creates a vector3 variable
            mousePos = Input.mousePosition; // Gets the mouse position, very raw, the Input.mousePosition uses the pixel coordinates of the screen, not usable without translation
            mousePos = Camera.main.ScreenToWorldPoint(mousePos); // Translates the position of the mouse in pixels to a position in transform(?), which is the same as the gameobjects use

            gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0); // Sets the position of the gameobject to the translated mousePos x and y (z is 0 because it doesn't matter, it's 2d)
        }
    }
    #endregion
}
