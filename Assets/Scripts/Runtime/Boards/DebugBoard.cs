using PPP.Hexagons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPP.Boards {
    public class DebugBoard : MonoBehaviour {
        [SerializeField]
        int radius = 10;
        [SerializeField]
        DebugField debugFieldPrefab = default;
        [SerializeField]
        Transform context = default;
        [SerializeField]
        float spawnInterval = 1;

        Dictionary<Hex3, DebugField> hexagons = default;


        private void Start() {
            StartCoroutine(GenerateBoard());
        }

        IEnumerator GenerateBoard() {
            hexagons = new Dictionary<Hex3, DebugField>();
            foreach (var hex in HexUtility.HexSpiral(radius)) {
                hexagons[hex] = Instantiate(debugFieldPrefab, context);
                hexagons[hex].Init(hex, radius);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
