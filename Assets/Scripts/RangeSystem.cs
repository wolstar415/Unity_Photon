using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSystem : MonoBehaviour
{
    public MainPlayerInfo playerInfo;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerInfo.PV.IsMine)
        {
            return;
        }
        if (MainNetManager.instance.Intro)
            return;
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (playerInfo.IsKiller)
        {

                playerInfo.Enemy.Add(other.gameObject);
                playerInfo.Killer_Colli();
            
        }
        else
        {
                playerInfo.friendly.Add(other.gameObject);
                playerInfo.Normal_Colli();

        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!playerInfo.PV.IsMine)
        {
            return;
        }
        if (MainNetManager.instance.Intro)
            return;
        if (!other.CompareTag("Player"))
        {
            return;
        }

   
        if (playerInfo.IsKiller)
        {

            playerInfo.Enemy.Remove(other.gameObject);
            playerInfo.Killer_Colli();

        }
        else
        {
            playerInfo.friendly.Remove(other.gameObject);
            playerInfo.Normal_Colli();

        }
    }
}
