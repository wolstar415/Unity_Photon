using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Animator ani;
    public SpriteRenderer sr;
    public float speed = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {

        Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
            if (dir.x < 0f)
            {
                PV.RPC("FlipSet", RpcTarget.AllBuffered, true);
            }
            else if(dir.x > 0f)
            {

                PV.RPC("FlipSet", RpcTarget.AllBuffered, false);
            }
        transform.position += dir * speed * Time.deltaTime;
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") ==0)
            {
                ani.SetBool("isMove", false);
            }
            else
            {
                ani.SetBool("isMove", true);

            }
        }
    }

    [PunRPC]
    void FlipSet(bool b)
    {
        sr.flipX = b;
    }
}
