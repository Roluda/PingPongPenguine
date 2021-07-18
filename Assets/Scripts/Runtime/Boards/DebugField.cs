using PPP.Hexagons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PPP.Boards {
    public class DebugField : MonoBehaviour {

        [SerializeField]
        TMP_Text pText = default;
        [SerializeField]
        TMP_Text qText = default;
        [SerializeField]
        TMP_Text sText = default;
        [SerializeField]
        SpriteRenderer hexagonRenderer = default;

        public void Init(Hex3 hex, int boardRadius) {
            var cartesian = Hex3.HexToCartesian(hex);
            transform.localPosition = new Vector3(cartesian.x, 0, cartesian.y);
            pText.SetText($"{hex.p}P");
            qText.SetText($"{hex.q}Q");
            sText.SetText($"{hex.s}S");
            float r = Mathf.InverseLerp(-1, 1, (float)hex.p / boardRadius);
            float g = Mathf.InverseLerp(-1, 1, (float)hex.q / boardRadius);
            float b = Mathf.InverseLerp(-1, 1, (float)hex.s / boardRadius);
            hexagonRenderer.color = new Color(r,g,b);
        }
    }
}
