using PPP.Hexagons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPP.Boards {
    public class PingPongField : MonoBehaviour {

        [SerializeField]
        GameObject icePrefab = default;
        [SerializeField]
        GameObject mountainPrefab = default;
        [SerializeField]
        float mountainMinHeight = 2;
        [SerializeField]
        float mountainMaxHeight = 3;

        public bool isObstacle;

        GameObject currentObject;

        public void Init(Hex3 hex) {
            var cartesian = Hex3.HexToCartesian(hex);
            transform.localPosition = new Vector3(cartesian.x, 0, cartesian.y);
        }

        public void SetObstacle(bool obstacle) {
            isObstacle = obstacle;
            if (currentObject) {
                Destroy(currentObject);
            }
            currentObject = Instantiate(obstacle ? mountainPrefab : icePrefab, transform);
            if (obstacle) {
                var localScale = currentObject.transform.localScale;
                localScale.y *= Random.Range(mountainMinHeight, mountainMaxHeight);
                currentObject.transform.localScale = localScale;
            }
        }
    }
}
