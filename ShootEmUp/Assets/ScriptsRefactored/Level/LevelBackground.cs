using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBackground : MonoBehaviour
    {
        private float startPositionY;

        private float endPositionY;

        private float movingSpeedY;

        private float positionX;

        private float positionZ;

        private Transform myTransform;

        [SerializeField]
        private Params parameters;

        private void Awake()
        {
            startPositionY = parameters.StartPositionY;
            endPositionY = parameters.EndPositionY;
            movingSpeedY = parameters.MovementSpeedY;
            myTransform = transform;
            var position = myTransform.position;
            positionX = position.x;
            positionZ = position.z;
        }

        private void FixedUpdate()
        {
            if (myTransform.position.y <= endPositionY)
            {
                myTransform.position = new Vector3(
                    positionX,
                    startPositionY,
                    positionZ
                );
            }

            myTransform.position -= new Vector3(
                positionX,
                movingSpeedY * Time.fixedDeltaTime,
                positionZ
            );
        }

        [Serializable]
        public sealed class Params
        {
            [SerializeField]
            public float StartPositionY;

            [SerializeField]
            public float EndPositionY;

            [SerializeField]
            public float MovementSpeedY;
        }
    }
}