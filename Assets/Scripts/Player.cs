using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hays = 0;
    public int bestHays = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("HayBale"))
        {
            hays++;
            // add effect
            FindObjectOfType<AudioManager>().Play("hitWheat");
        }
        else if (collision.name.Contains("Rock"))
        {
            hays -= 5;
            FindObjectOfType<AudioManager>().Play("hitRock");
        }
        collision.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (hays > bestHays)
            bestHays = hays;
    }

}
