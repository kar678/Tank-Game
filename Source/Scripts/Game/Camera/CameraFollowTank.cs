using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogwheel.Player
{
    public class CameraFollowTank : MonoBehaviour
    {
        private GameObject playerTank;
        Vector3 velocity = Vector3.zero;
        public float smoothTime = .15f;

        void Awake()
        {
            playerTank = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveCamera();
        }

        void MoveCamera()
        {
            Vector3 targetPos = playerTank.transform.position;

            targetPos.y = playerTank.transform.position.y + 10;
            targetPos.z = playerTank.transform.position.z + -10;
            targetPos.x = playerTank.transform.position.x + -5;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}