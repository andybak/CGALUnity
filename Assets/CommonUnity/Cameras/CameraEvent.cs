using UnityEngine;
using System;

namespace Common.Unity.Cameras
{

    public delegate void CameraEventHandler(Camera camera);

    [RequireComponent(typeof(Camera))]
    public abstract class CameraEvent : MonoBehaviour
    {

        protected Camera Camera { get; private set; }

        void Start()
        {
            Camera = GetComponent<Camera>();
        }

    }

}
