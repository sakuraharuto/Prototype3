using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Darkness System script to put on the top darkness layer object
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.Darkness
{

    public class Darkness : MonoBehaviour
    {
        [Header("Camera")]
        public bool follow_camera = false;
        public Camera main_camera = null;
        public Vector3 follow_offset;

        [Header("Opacity")]
        public float edges_radius = 1f;

        [Range(0f, 1f)]
        public float opacity = 1f;

        private Camera target_camera;
        private SpriteRenderer sprite_renderer;
        private Collider2D collide;

        private const int light_max_size = 12;//Max is 12 LightEmit
        private LightEmit[] lights;

        void Start()
        {
            target_camera = main_camera;
            if (target_camera == null)
                target_camera = Camera.main;

            sprite_renderer = GetComponent<SpriteRenderer>();
            collide = GetComponent<Collider2D>();
            sprite_renderer.enabled = true;
            sprite_renderer.material.SetFloat("_Editor", 0f);
            lights = LightEmit.GetNearestFrom(target_camera.transform.position);

            Bounds bounds = new Bounds(collide.bounds.center, collide.bounds.size);
            sprite_renderer.material.SetVector("_Bounds", new Vector4(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y));
            sprite_renderer.material.SetFloat("_EdgesRadius", edges_radius);
            sprite_renderer.material.SetFloat("_Opacity", Mathf.Clamp01(opacity));
        }

        void Update()
        {
            //Set the bounds
            Bounds offset_bounds = new Bounds(collide.bounds.center, collide.bounds.size);
            sprite_renderer.material.SetVector("_Bounds", new Vector4(offset_bounds.min.x, offset_bounds.min.y, offset_bounds.max.x, offset_bounds.max.y));
            sprite_renderer.material.SetFloat("_EdgesRadius", edges_radius);
            sprite_renderer.material.SetFloat("_Opacity", Mathf.Clamp01(opacity));

            //Will get all nearests (shader supports 12 max)
            lights = LightEmit.GetNearestFrom(target_camera.transform.position);

            //Loop on all light emits
            for (int i = 0; i < light_max_size; i++)
            {
                //Check if light enabled
                LightEmit alight = null;
                if (i < lights.Length)
                {
                    if (lights[i].enabled && lights[i].light_enabled)
                    {
                        alight = lights[i];
                    }
                }

                //Set shader properties
                if (alight)
                {
                    //X,Y=Position    Z=Radius     W= Attenuation & Opacity
                    float attenuation = Mathf.Clamp(alight.attenuation_radius, 0.001f, 99f);
                    float lopacity = Mathf.Clamp(Mathf.Round(alight.light_opacity * 100f), 0f, 100f) * 100f;
                    sprite_renderer.material.SetVector("_Light" + (i + 1) + "_Pos", new Vector4(alight.transform.position.x, alight.transform.position.y, alight.radius, attenuation + lopacity));
                }
                else
                {
                    sprite_renderer.material.SetVector("_Light" + (i + 1) + "_Pos", new Vector4(9999f, 9999f, 0.001f, 1f));
                }
            }
        }

        private void LateUpdate()
        {
            //Follow the camera
            if (follow_camera)
                transform.position = target_camera.transform.position + follow_offset;
        }
        
        //Determine if point is hidden by darkness or not
        public bool IsHiddenByDarkness(Vector3 point)
        {
            return GetPointAlpha(point) > 0.5f;
        }

        public bool IsInsideBounds(Vector3 point)
        {
            Bounds bounds = new Bounds(collide.bounds.center, collide.bounds.size);
            return (point.x > bounds.min.x && point.x < bounds.max.x && point.y > bounds.min.y && point.y < bounds.max.y);
        }

        //Get the darkness alpha value at position 1=darkness 0=visible
        public float GetPointAlpha(Vector3 point)
        {
            float alpha = 1f;

            //Makes sure its inside the bounds
            if (!follow_camera && !IsInsideBounds(point))
                return 0f; //If outside bounds, return 0;

            //Remove each light alpha
            for (int i = 0; i < light_max_size; i++)
            {
                //Check if light enabled
                if (i < lights.Length)
                {
                    LightEmit alight = lights[i];
                    if (alight.enabled && alight.light_enabled)
                    {
                        //Remove light alpha
                        alpha -= GetLightAlpha(alight, point);
                    }
                }
            }
            return Mathf.Clamp01(alpha) * Mathf.Clamp01(opacity);
        }

        //Simulate the shader calculations, but for just 1 point
        private float GetLightAlpha(LightEmit alight, Vector3 pos)
        {
            float radius = alight.radius + 0.001f; //Avoid divide by 0
            float attenuation = Mathf.Clamp(alight.attenuation_radius, 0.001f, 99f);
            float lopacity = Mathf.Clamp01(alight.light_opacity);

            //Attenuation
            float leng = (alight.transform.position - pos).magnitude;
            float atten = Mathf.Clamp(radius - leng, 0.0f, radius) / radius;
            return Mathf.Clamp01((radius / attenuation) * atten * atten) * lopacity;
        }
    }

}