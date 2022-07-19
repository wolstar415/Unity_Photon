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
    private static readonly int IsMove = Animator.StringToHash("isMove");

    void Update()
    {
        if (PV.IsMine)
            //본인만
        {
            Vector3 dir =
                Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
            //Input.GetAxis 를 이용해서 가로세로 움직임을 받습니다.
            if (dir.x < 0f)
            {
                PV.RPC(nameof(RPC_FlipSet), RpcTarget.AllBuffered, true);
            }
            else if (dir.x > 0f)
            {
                PV.RPC(nameof(RPC_FlipSet), RpcTarget.AllBuffered, false);
            }
            //2d이기때문에 왼쪽 오른쪽 바라보게 해야합니다.
            
            
            transform.position += dir * (speed * Time.deltaTime);
            
            
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                ani.SetBool(IsMove, false);
            }
            else
            {
                ani.SetBool(IsMove, true);
            }
            //애니메이션 설정부분 
        }
    }

    [PunRPC]
    void RPC_FlipSet(bool b)
    {
        sr.flipX = b;
    }
}