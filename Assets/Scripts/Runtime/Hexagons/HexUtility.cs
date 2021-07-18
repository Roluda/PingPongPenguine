using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PPP.Hexagons {
    public class HexUtility {
        public static IEnumerable<Hex3> HexRing(int radius) {
            return HexRing(radius, new Hex3(0, 0, 0));
        }

        public static IEnumerable<Hex3> HexRing(int radius, Hex3 center) {
            var hex = center + Hex3.Direction(Direction.downLeft) * radius;
            for (int i = 0; i<6; i++) {
                for(int j=0; j<radius; j++) {
                    yield return hex;
                    hex = hex.Neighbor((Direction)i);
                }
            }
        }

        public static IEnumerable<Hex3> HexSpiral(int radius) {
            return HexSpiral(radius, new Hex3(0, 0, 0));
        }

        public static IEnumerable<Hex3> HexSpiral(int radius, Hex3 center) {
            yield return center;
            for (int i = 1; i<=radius; i++) {
                foreach(var hex in HexRing(i, center)) {
                    yield return hex;
                }
            }
        }
    }
}
