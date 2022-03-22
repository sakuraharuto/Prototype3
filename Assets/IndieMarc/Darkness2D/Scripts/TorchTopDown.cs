using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieMarc.TopDown;

/// <summary>
/// Allow to toggle on/off the torch, using levers or other
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// </summary>

namespace IndieMarc.Darkness
{

    public class TorchTopDown : MonoBehaviour
    {
        public bool is_active = true;

        [Header("Levers")]
        public LeverState lever_state_required;
        public GameObject[] levers;

        [Header("Animator")]
        public string active_anim = "";
        
        private Animator animator;
        private LightEmit light_emit;

        private List<Lever> lever_list = new List<Lever>();

        void Awake()
        {
            animator = GetComponent<Animator>();
            light_emit = GetComponent<LightEmit>();
        }

        private void Start()
        {
            foreach (GameObject lever in levers)
                InitSwitch(lever);

            if (light_emit)
            {
                light_emit.light_opacity = is_active ? 1f : 0f;
                light_emit.light_enabled = is_active;
            }
        }

        void Update()
        {
            //Check if active
            int nb_switch = GetNbSwitches();
            int nb_total = GetNbSwitchesTotal();
            if (nb_total > 0)
            {
                is_active = (nb_switch >= 1);
            }

            //Animate
            if (animator && active_anim != "")
                animator.SetBool(active_anim, is_active);

            //Update light emit
            if (light_emit)
            {
                float add = 2f * (is_active ? 1f : -1f);
                light_emit.light_opacity += add * Time.deltaTime;
                light_emit.light_opacity = Mathf.Clamp(light_emit.light_opacity, 0f, 1f);
                light_emit.light_enabled = (light_emit.light_opacity > 0.01f);
            }
        }

        private void InitSwitch(GameObject swt)
        {
            if (swt != null)
            {
                if (swt.GetComponent<Lever>())
                    lever_list.Add(swt.GetComponent<Lever>());
            }
        }

        //Check how many levers are activated
        private int GetNbSwitches()
        {
            int nb_switch = 0;

            // Lever ------------
            foreach (Lever lever in lever_list)
            {
                if (lever)
                    nb_switch += (lever.state == lever_state_required) ? lever.door_value : 0;
            }

            return nb_switch;
        }

        //Check how many levers control this torch
        private int GetNbSwitchesTotal()
        {
            return lever_list.Count;
        }

        public void Activate()
        {
            is_active = true;
        }

        public void Deactivate()
        {
            is_active = false;
        }
        
    }

}