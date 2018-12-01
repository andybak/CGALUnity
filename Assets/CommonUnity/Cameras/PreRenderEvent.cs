using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Unity.Cameras
{

    [AddComponentMenu("Common/Cameras/Pre Render Event")]
    [RequireComponent(typeof(Camera))]
    public class PreRenderEvent : CameraEvent
    {

        public CameraEventHandler OnEvent = delegate { };

        void OnPreRender()
        {
            OnEvent(Camera);
        }

        public static void AddEvent(Camera camera, CameraEventHandler onPostRenderEvent)
        {
            if (camera == null) return;

            PreRenderEvent e = camera.GetComponent<PreRenderEvent>();
            if (e != null)
                e.OnEvent += onPostRenderEvent;
        }

        public static void RemoveEvent(Camera camera, CameraEventHandler onPostRenderEvent)
        {
            if (camera == null) return;

            PreRenderEvent e = camera.GetComponent<PreRenderEvent>();
            if (e != null)
                e.OnEvent -= onPostRenderEvent;
        }

    }

}
