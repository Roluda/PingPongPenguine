using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Hex3> Line(Hex3 start, Hex3 end) {
            int n = Hex3.Distance(start, end);
            for(int i = 0; i <= n; i++) {
                float t = (float)i / n;
                yield return Hex3.Round(Mathf.Lerp(start.p, end.p, t), Mathf.Lerp(start.q, end.q, t), Mathf.Lerp(start.s, end.s, t));
            }
        }

        /// <summary>
        /// Performs A* from start to goal on a field defined by available Hexagons. Returns false if no path is available.
        /// The Path contains all hexagons between start (exclusive) and end (inclusive)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        /// <param name="availableHexagons"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool TryFindPath(Hex3 start, Hex3 goal, IEnumerable<Hex3> availableHexagons, out IEnumerable<Hex3> path) {
            Dictionary<Hex3, PathNode> nodes = new Dictionary<Hex3, PathNode>();
            List<PathNode> open = new List<PathNode>();
            List<PathNode> closed = new List<PathNode>();
            foreach (var hex in availableHexagons) {
                nodes[hex] = new PathNode(){
                    hex = hex,
                    pre = null,
                    g = 0,
                    h = Hex3.Distance(hex, goal)
                };
            }
            if (nodes.ContainsKey(start)) {
                open.Add(nodes[start]);
            }
            while(open.Count > 0) {
                var currentCheck = open.OrderBy(node => node.f).First();
                if(currentCheck.hex == goal) {
                    path = BuildPath(currentCheck).Reverse();
                    return true;
                }
                foreach (var hex3 in HexRing(1, currentCheck.hex)) {
                    if (!nodes.ContainsKey(hex3))
                        continue;
                    if (closed.Contains(nodes[hex3]))
                        continue;
                    if (open.Contains(nodes[hex3]) && nodes[hex3].g < currentCheck.g + 1)
                        continue;
                    if (!open.Contains(nodes[hex3])) {
                        open.Add(nodes[hex3]);
                    }
                    nodes[hex3].pre = currentCheck;
                    nodes[hex3].g = currentCheck.g + 1;
                }
                open.Remove(currentCheck);
                closed.Add(currentCheck);
            }
            path = null;
            return false;
        }
        private static IEnumerable<Hex3> BuildPath(PathNode pathNode) {
            while(pathNode.pre != null) {
                yield return pathNode.hex;
                pathNode = pathNode.pre;
            }
        }
        class PathNode {
            public int f => g + h;
            public Hex3 hex;
            public PathNode pre;
            public int g;
            public int h;
        }
    }
}
