using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Unity.Cameras
{

    [AddComponentMenu("Common/Cameras/Pre Cull Event")]
    [RequireComponent(typeof(Camera))]
    public class PreCullEvent : CameraEvent
    {

        public CameraEventHandler OnEvent = delegate { };

        void OnPreCull()
        {
            OnEvent(Camera);
        }

        public static void AddEvent(Camera camera, CameraEventHandler onPostRenderEvent)
        {
            if (camera == null) return;

            PreCullEvent e = camera.GetComponent<PreCullEvent>();
            if (e != null)
                e.OnEvent += onPostRenderEvent;
        }

        public static void RemoveEvent(Camera camera, CameraEventHandler onPostRenderEvent)
        {
            if (camera == null) return;

            PreCullEvent e = camera.GetComponent<PreCullEvent>();
            if (e != null)
                e.OnEvent -= onPostRenderEvent;
        }

    }

}