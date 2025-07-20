using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class AllaysAppear : MonoBehaviour
    {
        public GameObject[] allays;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (GameObject allay in allays)
                {
                    allay.SetActive(true);
                }
            }
        }
    }
}
