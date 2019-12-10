using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSystem : MonoBehaviour
{

    void Start()
    {

        PieceMovement.deathParticles = GetComponent<ParticleSystem>(); // Sets the static value of deathParticles to be the particlesystem 

    }

}
