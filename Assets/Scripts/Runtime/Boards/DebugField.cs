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

        public void Init(Hex3 hex) {
            var cartesian = Hex3.HexToCartesian(hex);
            transform.localPosition = new Vector3(cartesian.x, 0, cartesian.y);
            pText.SetText($"{hex.p}P");
            qText.SetText($"{hex.q}Q");
            sText.SetText($"{hex.s}S");
        }

        public void SetColor(Color color) {
            hexagonRenderer.color = color;
        }
    }
}
