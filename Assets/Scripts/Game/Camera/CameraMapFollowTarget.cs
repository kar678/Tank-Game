using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogwheel.Player
{
    public class CameraMapFollowTarget : MonoBehaviour
    {
        private GameObject playerTank;
        Vector3 velocity = Vector3.zero;
        public float smoothTime = .15f;

        // Start is called before the first frame update
        void Start()
        {
            playerTank = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 targetPos = playerTank.transform.position;

            targetPos.y = 100;
            targetPos.z = playerTank.transform.position.z;
            targetPos.x = playerTank.transform.position.x;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
