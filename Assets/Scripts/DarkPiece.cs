using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DarkPiece : MonoBehaviour
{
    Component[] mySpriteRendList; // Creates a array of components
    public bool isDark = false; // Needs to be public for other scripts to accses, or at least can't be private
    void Start()
    {


        if (gameObject.transform.position.y > 0) // If the starting y postion of the piece is more than 0 it is dubbed as a dark piece
        {
            isDark = true;
        }
        else // Otherwise the piece is not dark
        {
            isDark = false;
        }

        if (isDark == true) // Changes the rotation of the dark pieces as well as coloring them black
        {
            gameObject.transform.eulerAngles = new Vector3(180, 0, 0); // Turns the piece upside down


            mySpriteRendList = GetComponentsInChildren<SpriteRenderer>(); // fills the array of components with the sprite renderers of the children 

            foreach (SpriteRenderer renderer in mySpriteRendList) // Puts every component from the array, one after the other, into the temporary renderer variable and then proceds to change its color to black
            {
                renderer.color = Color.black;
            }
        }

    }
}
