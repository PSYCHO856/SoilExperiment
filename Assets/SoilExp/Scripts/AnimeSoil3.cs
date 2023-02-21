using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeSoil3 : MonoBehaviour
{
    public Animation anime1;
    public Animation anime2;
    public Animation anime3;

    public void Anime1Play()
    {
        anime1.gameObject.SetActive(true);
        anime1.Play();
    }
    
    public void Anime2Play()
    {
        anime2.gameObject.SetActive(true);

        anime2.Play();
    }
    
}
