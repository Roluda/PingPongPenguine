using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPP.Boards {
    public class Target : MonoBehaviour {
        [SerializeField]
        Renderer targetRenderer = default;


        [SerializeField]
        Material[] materials = default;
        public void SetTargetType(int type) {
            targetRenderer.sharedMaterial = materials[type % materials.Length];
        }
    }
}
