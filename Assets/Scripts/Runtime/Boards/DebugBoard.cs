using PPP.Hexagons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField]
        float spawnChance = 0.8f;

        Dictionary<Hex3, DebugField> hexagons = new Dictionary<Hex3, DebugField>();


        private void Start() {
            NewBoard();
        }

        public void NewBoard() {
            StopAllCoroutines();
            for (int i = context.childCount - 1; i > 0; i--) {
                Destroy(context.GetChild(i).gameObject);
            }
            hexagons.Clear();
            StartCoroutine(GenerateBoard());
        }

        IEnumerator GenerateBoard() {
            hexagons = new Dictionary<Hex3, DebugField>();
            foreach (var hex in HexUtility.HexSpiral(radius)) {
                if (Random.value < spawnChance) {
                    hexagons[hex] = Instantiate(debugFieldPrefab, context);
                    hexagons[hex].Init(hex);
                    hexagons[hex].SetColor(ColorByCoordinates(hex));
                }
                yield return new WaitForSeconds(spawnInterval);
            }

            Hex3 startHex = hexagons.ElementAt(Random.Range(0, hexagons.Count)).Key;
            Hex3 endHex = hexagons.ElementAt(Random.Range(0, hexagons.Count)).Key;
            if (HexUtility.TryFindPath(startHex, endHex, hexagons.Keys, out var path)){
                hexagons[startHex].SetColor(Color.blue);
                foreach (var hex in path) {
                    hexagons[hex].SetColor(new Color(0.3f, 0.3f, 0.3f));
                    yield return new WaitForSeconds(spawnInterval);
                }
                hexagons[endHex].SetColor(Color.red);
            }

        }

        Color ColorByCoordinates(Hex3 hex) {
            float r = Mathf.InverseLerp(-1, 1, (float)hex.p / radius);
            float g = Mathf.InverseLerp(-1, 1, (float)hex.q / radius);
            float b = Mathf.InverseLerp(-1, 1, (float)hex.s / radius);
            return new Color(r, g, b);
        }
    }
}
