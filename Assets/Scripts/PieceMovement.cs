using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PieceMovement : MonoBehaviour
{
   
    [SerializeField]
    GameObject board;
    [SerializeField]
    bool pawnHasMoved = false;

    
    Rigidbody2D rb;
    GameObject turnManager;
    


    PieceMovement pieceMovementScript; // -||-
    DarkPiece darkPieceScript; // Makes a variable able to store the DarkPiece script
    PiecePlacement piecePlacementScript; // -||- (typ)
    KindOfPiece kindOfPieceScript;

    public bool whiteToMove = true;
    public static ParticleSystem deathParticles;

    void Start()
    {
        turnManager = GameObject.FindGameObjectWithTag("TurnManager"); // Stores the TurnManager gameobject in the variable, important bc a universal server is needed for the pieces to check if it's their turn or not and for pieces to easily communicate with eachother
        board = GameObject.FindGameObjectWithTag("Board"); 
        
        piecePlacementScript = board.GetComponent<PiecePlacement>();
        darkPieceScript = gameObject.GetComponent<DarkPiece>(); // Stores the DarkPiece script in the variable, easy bc both scirpts are on the same gameobject
        kindOfPieceScript = gameObject.GetComponent<KindOfPiece>();
        pieceMovementScript = turnManager.GetComponent<PieceMovement>();
        

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    
    bool isHeld = false;
    Vector3 placementBeforeMove;
    int pNum = 1;

    private void OnMouseDown()
    {
        isHeld = true; // Says that something is being held when holding down the mouse button
        placementBeforeMove = gameObject.transform.position;
        

    }
    private void Update() // Makes piece follows the mouse
    {
        if (isHeld == true) // If the mouse button is being pressed
        {
            Vector3 mousePos; // Creates a vector3 variable
            mousePos = Input.mousePosition; // Gets the mouse position, very raw, the Input.mousePosition uses the pixel coordinates of the screen, not usable without translation
            mousePos = Camera.main.ScreenToWorldPoint(mousePos); // Translates the position of the mouse in pixels to a position in transform(?), which is the same as the gameobjects use

            gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0); // Sets the position of the gameobject to the translated mousePos x and y (z is 0 because it doesn't matter, it's 2d)
        }
    }
     void OnMouseUp()
     {

        isHeld = false; // Says that nothing is being held when the mouse button goes up
        gameObject.transform.position = new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.y), 0); //Snaps the pieces to the center of the square by rounding the number

        print(gameObject.transform.position);
        if (gameObject.transform.position.x < -3 || gameObject.transform.position.x > 4 || gameObject.transform.position.y > 3 || gameObject.transform.position.y < -4)
        {
            gameObject.transform.position = placementBeforeMove; // Moves the piece back if it is not on the board
        }

        //Handles the Turns, moves the piece back if not their turn, also deletes a piece if its is taken
        if (placementBeforeMove != gameObject.transform.position) // If the piece is no longer in its previous position
        {
            bool legalMove = true;
            bool dubblePlacement = false;

            #region Dark
            if (darkPieceScript.isDark == true) // if the piece is dark
            {
                if (pieceMovementScript.whiteToMove == true) // if it isn't blacks turn the piece is moved back
                {
                    gameObject.transform.position = placementBeforeMove;
                    print("Can't, It is whites turn " + pieceMovementScript.whiteToMove);
                }
                else if (pieceMovementScript.whiteToMove == false)
                {

                    //1. we check if the move is legal

                    #region LegalMove
                    Vector3 movementDifference;
                    movementDifference = gameObject.transform.position - placementBeforeMove;



                    if (kindOfPieceScript.isPawn == true)
                    {
                        bool pawnTook = false;
                        if ((movementDifference.x == 1 && movementDifference.y == -1) || (movementDifference.x == -1 && movementDifference.y == -1))
                        {
                            GameObject[] pawnPieceArray;

                            pawnPieceArray = GameObject.FindGameObjectsWithTag("Piece"); // Fills the array with the pieces
                            print(pawnPieceArray[10]);

                            int pawnNum = 0;
                            GameObject[] pawnKillList = new GameObject[2];
                            foreach (GameObject piece in pawnPieceArray)
                            {
                                if (gameObject.transform.position == piece.transform.position)
                                {
                                    pawnKillList[pawnNum] = piece;

                                    pawnNum++; // It will trigger on itself, if it triggers again that is some other piece
                                    if (pawnNum == 2)
                                    {
                                        foreach (GameObject item in pawnKillList)
                                        {
                                            if (item.GetComponent<DarkPiece>().isDark == false)
                                            {
                                                item.SetActive(false); // If the pawn takes something
                                                ParticleSystem newPSystem = Instantiate(deathParticles, item.transform.position, Quaternion.identity); // When something dies we play the designated particlesystem, so we instantiate it then play it, loop is turned off, will reoccur in following segments where we take pieces                                        
                                                newPSystem.Play();
                                                pawnTook = true;
                                            }
                                            else if (item.GetComponent<DarkPiece>().isDark == true)
                                            {
                                                pawnNum++;
                                                if (pawnNum == 4) // If both are black
                                                {
                                                    gameObject.transform.position = placementBeforeMove;
                                                    dubblePlacement = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (pawnTook == false)
                        {
                            if (pawnHasMoved == true) // If the pawn has moved before it moves one spaces
                            {
                                if (movementDifference.y < -1 || movementDifference.y > 0 || movementDifference.x != 0)
                                {
                                    gameObject.transform.position = placementBeforeMove;
                                    legalMove = false;
                                }
                            }
                            else if (pawnHasMoved == false) // If it hasn't then it is allowed to move 2 spaces
                            {
                                if (movementDifference.y < -2 || movementDifference.y > 0 || movementDifference.x != 0) // If the pawn moves more than 2 then it doesn't move and can still move 2
                                {
                                    gameObject.transform.position = placementBeforeMove;
                                    legalMove = false;
                                }
                            }
                        }

                    }
                    else if (kindOfPieceScript.isRook == true)
                    {
                        if (movementDifference.x != 0 && movementDifference.y != 0) //if the rook has moved in both directions, if there is a difference on both
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isBishop == true)
                    {
                        if (movementDifference.x == movementDifference.y || movementDifference.x == -movementDifference.y || -movementDifference.x == movementDifference.y)
                        {
                            //Just Continues
                        }
                        else // In all other cases the move must be illegal
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isKnight == true)
                    {
                        if (movementDifference == new Vector3(2, 1, 0) || movementDifference == new Vector3(-2, 1, 0) || movementDifference == new Vector3(2, -1, 0) || movementDifference == new Vector3(-2, -1, 0) || movementDifference == new Vector3(1, 2, 0) || movementDifference == new Vector3(-1, 2, 0) || movementDifference == new Vector3(1, -2, 0) || movementDifference == new Vector3(-1, -2, 0))
                        {
                            // All possible moves for the knight
                        }
                        else
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isKing == true)
                    {
                        if (movementDifference.x < -1 || movementDifference.x > 1 || movementDifference.y < -1 || movementDifference.y > 1)
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isQueen == true)
                    {
                        if (movementDifference.x == movementDifference.y || movementDifference.x == -movementDifference.y || -movementDifference.x == movementDifference.y)
                        {
                            // If it moves like a bishop we're happy
                        }
                        else if ((movementDifference.x == 0 && movementDifference.y != 0) || (movementDifference.x != 0 && movementDifference.y == 0))
                        {
                            // If it moves like a rook we're happy
                        }
                        else // But if it doesn't do any of those then that's illegal
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    #endregion

                    //2. we check if it takes something
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
                                        ParticleSystem newPSystem = Instantiate(deathParticles, item.transform.position, Quaternion.identity);
                                        newPSystem.Play();
                                    }
                                    else if (item.GetComponent<DarkPiece>().isDark == true)
                                    {
                                        num++;
                                        if (num == 4) // If both are black
                                        {
                                            gameObject.transform.position = placementBeforeMove;
                                            dubblePlacement = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (legalMove == true && dubblePlacement == false)
                {
                    pieceMovementScript.whiteToMove = true;
                    pawnHasMoved = true;
                    print("It is now whites turn " + pieceMovementScript.whiteToMove);
                }
                else
                {
                    gameObject.transform.position = placementBeforeMove;
                }
            }
            #endregion 

            #region White
            else if (darkPieceScript.isDark == false) // if the piece is white
            {

                if (pieceMovementScript.whiteToMove == false) // Same thing here as above
                {
                    gameObject.transform.position = placementBeforeMove;
                    print("Can't do that, blacks turn " + pieceMovementScript.whiteToMove);
                }
                else if (pieceMovementScript.whiteToMove == true)
                {
                    //1. we check if the move is legal

                    Vector3 movementDifference;
                    movementDifference = gameObject.transform.position - placementBeforeMove;
                    print(movementDifference);

                    #region LegalMoves
                    if (kindOfPieceScript.isPawn == true)
                    {
                        bool pawnTook = false;
                        if ((movementDifference.x == 1 && movementDifference.y == 1) || (movementDifference.x == -1 && movementDifference.y == 1))
                        {
                            GameObject[] pawnPieceArray;


                            // (Same as Above) After we check if the piece is allowed to move we want to check if it has taken another piece, for this we put all the placements of the pieces into a array and then check if the current piece has the same position as any of the other pieces
                            pawnPieceArray = GameObject.FindGameObjectsWithTag("Piece"); // Fills the array with the pieces


                            int pawnNum = 0;
                            GameObject[] pawnKillList = new GameObject[2];
                            foreach (GameObject piece in pawnPieceArray)
                            {
                                if (gameObject.transform.position == piece.transform.position)
                                {
                                    pawnKillList[pawnNum] = piece;

                                    pawnNum++; // It will trigger on itself, if it triggers again that is some other piece
                                    if (pawnNum == 2)
                                    {
                                        foreach (GameObject item in pawnKillList)
                                        {
                                            if (item.GetComponent<DarkPiece>().isDark == true)
                                            {
                                                item.SetActive(false); // If the pawn takes something
                                                ParticleSystem newPSystem = Instantiate(deathParticles, item.transform.position, Quaternion.identity);
                                                newPSystem.Play();
                                                pawnTook = true;
                                            }
                                            else if (item.GetComponent<DarkPiece>().isDark == false)
                                            {
                                                pawnNum++;
                                                if (pawnNum == 4) // If both are white
                                                {
                                                    gameObject.transform.position = placementBeforeMove;
                                                    dubblePlacement = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (pawnTook == false)
                        {
                            if (pawnHasMoved == true) // If the pawn has moved before it moves one spaces
                            {
                                if (movementDifference.y < 0 || movementDifference.y > 1 || movementDifference.x != 0)
                                {
                                    gameObject.transform.position = placementBeforeMove;
                                    legalMove = false;
                                }
                            }
                            else if (pawnHasMoved == false) // If it hasn't then it is allowed to move 2 spaces
                            {
                                if (movementDifference.y < 0 || movementDifference.y > 2 || movementDifference.x != 0) // If the pawn moves more than 2 then it doesn't move and can still move 2
                                {
                                    gameObject.transform.position = placementBeforeMove;
                                    legalMove = false;
                                }
                            }
                        }
                        
                    }
                    else if (kindOfPieceScript.isRook == true)
                    {
                        if (movementDifference.x != 0 && movementDifference.y != 0) //if the rook has moved in both directions, if there is a difference on both
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isBishop == true)
                    {
                        if(movementDifference.x == movementDifference.y || movementDifference.x == -movementDifference.y || -movementDifference.x == movementDifference.y)
                        {
                            //Just Continues
                        }
                        else // In all other cases the move must be illegal
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isKnight == true)
                    {
                        if (movementDifference == new Vector3(2, 1, 0) || movementDifference == new Vector3(-2, 1, 0) || movementDifference == new Vector3(2, -1, 0) || movementDifference == new Vector3(-2, -1, 0) || movementDifference == new Vector3(1, 2, 0) || movementDifference == new Vector3(-1, 2, 0) || movementDifference == new Vector3(1, -2, 0) || movementDifference == new Vector3(-1, -2, 0))
                        {
                            // All possible moves for the knight
                        }
                        else
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isKing == true)
                    {
                        if (movementDifference.x < -1 || movementDifference.x > 1 || movementDifference.y < -1 || movementDifference.y > 1)
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    else if (kindOfPieceScript.isQueen == true) // The queen is basically just a frankensteined rook and bishop
                    {
                        if (movementDifference.x == movementDifference.y || movementDifference.x == -movementDifference.y || -movementDifference.x == movementDifference.y)
                        {
                            // If it moves like a bishop we're happy
                        }
                        else if ((movementDifference.x == 0 && movementDifference.y != 0) || (movementDifference.x != 0 && movementDifference.y == 0))
                        {
                            // If it moves like a rook we're happy
                        }
                        else // But if it doesn't do any of those then that's illegal
                        {
                            gameObject.transform.position = placementBeforeMove;
                            legalMove = false;
                        }
                    }
                    #endregion

                    //2. we check if it takes something
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
                                        ParticleSystem newPSystem = Instantiate(deathParticles, item.transform.position, Quaternion.identity);
                                        newPSystem.Play();
                                    }
                                    else if (item.GetComponent<DarkPiece>().isDark == false) //3. We also check if it tries to go on another piece 
                                    {
                                        num++;
                                        if (num == 4) // If both are white
                                        {
                                            gameObject.transform.position = placementBeforeMove;
                                            dubblePlacement = true; 
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (legalMove == true && dubblePlacement == false)
                    {
                        pieceMovementScript.whiteToMove = false;
                        pawnHasMoved = true;
                        print("it is now blacks turn " + pieceMovementScript.whiteToMove);
                    }
                    else
                    {
                        gameObject.transform.position = placementBeforeMove;
                    }

                }
            }
            #endregion
        }
    }
}
