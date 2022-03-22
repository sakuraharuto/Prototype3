using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Light source for the darkness system
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.Darkness
{

    public class LightEmit : MonoBehaviour
    {

        public bool light_enabled = true;
        public float radius = 2f;
        public float attenuation_radius = 0.5f;
        [Range(0f, 1f)]
        public float light_opacity = 1f;

        private static Vector3 reference_pos;
        private static List<LightEmit> light_list = new List<LightEmit>();

        void Awake()
        {
            light_list.Add(this);
        }

        void OnDestroy()
        {
            light_list.Remove(this);
        }

        public static LightEmit[] GetAll()
        {
            return light_list.ToArray();
        }

        //Get nearest LightEmits from position
        public static LightEmit[] GetNearestFrom(Vector3 position)
        {
            reference_pos = position;
            List<LightEmit> activated_lights = new List<LightEmit>();
            foreach (LightEmit light in light_list)
            {
                if (light.light_enabled && light.radius > 0.001f)
                {
                    activated_lights.Add(light);
                }
            }
            activated_lights.Sort(ByDistance);
            return activated_lights.ToArray();
        }

        //Function used to sort light by distance
        private static int ByDistance(LightEmit a, LightEmit b)
        {
            float dstToA = Vector3.Distance(reference_pos, a.transform.position);
            float dstToB = Vector3.Distance(reference_pos, b.transform.position);
            return dstToA.CompareTo(dstToB);
        }
    }

}