using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace PPP.Hexagons {
    public struct Hex3 {
        public readonly int p;
        public readonly int q;
        public readonly int s;

        public Hex3(int p, int q, int s) {
            Assert.IsTrue(IsValid(p, q, s), "Cube coordinates assume p+q+s=0");
            this.p = p;
            this.q = q;
            this.s = s;
        }

        public int length => Mathf.Max(Mathf.Abs(p), Mathf.Abs(q), Mathf.Max(s));

        public Hex3 Neighbor(Direction direction) {
            return Neighbor(this, direction);
        }

        static readonly Hex3[] neighbors = {
            new Hex3(0, 1, -1), new Hex3(1, 0, -1), new Hex3(1, -1, 0),
            new Hex3(0, -1, 1), new Hex3(-1, 0, 1), new Hex3(-1, 1, 0),

        };

        public static Hex3 CartesianToHex(Vector2 position, Orientation orientation = Orientation.PointyTop, float hexSize = 1) {
            float p;
            float q;
            float s;
            switch (orientation) {
                case Orientation.PointyTop:
                    p = (Mathf.Sqrt(3) / 3 * position.x - 1f / 3 * position.y) / hexSize;
                    q = (2f / 3 * position.y) / hexSize;
                    s = -p - q;
                    return Round(p, q, s);
                case Orientation.FlatTop:
                    p = (2f / 3 * position.x) / hexSize;
                    q = (-1f / 3 * position.x + Mathf.Sqrt(3) / 3 * position.y) / hexSize;
                    s = -p - q;
                    return Round(p, q, s);
                default:
                    throw new NotImplementedException("This orientation does not exist");
            }
        }

        public static Vector2 HexToCartesian(Hex3 hex, Orientation orientation = Orientation.PointyTop, float hexSize = 1) {
            float x;
            float y;
            switch (orientation) {
                case Orientation.PointyTop:
                    x = hexSize * (Mathf.Sqrt(3) * hex.p + Mathf.Sqrt(3) * 0.5f * hex.q);
                    y = hexSize * (3 * 0.5f * hex.q);
                    return new Vector2(x, y);
                case Orientation.FlatTop:
                    x = hexSize * (3 * 0.5f * hex.p);
                    y = hexSize * (Mathf.Sqrt(3) * 0.5f * hex.p + Mathf.Sqrt(3) * hex.q);
                    return new Vector2(x, y);
                default:
                    throw new NotImplementedException("This orientation does not exist");
            }
        }

        public static Hex3 Neighbor(Hex3 origin, Direction direction) {
            return origin + neighbors[(int)direction];
        }

        public static Hex3 Direction(Direction direction) {
            return neighbors[(int)direction];
        }

        public static int Distance(Hex3 a, Hex3 b) {
            return (a - b).length;
        }

        public static Hex3 Round(float p, float q, float s) {
            float roundP = Mathf.Round(p);
            float roundQ = Mathf.Round(q);
            float roundS = Mathf.Round(s);
            float deltaP = Mathf.Abs(roundP - p);
            float deltaQ = Mathf.Abs(roundQ - q);
            float deltaS = Mathf.Abs(roundS - s);
            if (deltaP > deltaQ && deltaP > deltaS) {
                roundP = -roundQ - roundS;
            } else if (deltaQ > deltaS) {
                roundQ = -roundP - roundS;
            } else {
                roundS = -roundQ - roundP;
            }
            return new Hex3((int)roundP, (int)roundQ, (int)roundS);
        }


        public static Hex3 operator +(Hex3 a, Hex3 b) {
            return new Hex3(a.p + b.p, a.q + b.q, a.s + b.s);
        }
        public static Hex3 operator +(Hex3 a) {
            return a;
        }
        public static Hex3 operator -(Hex3 a, Hex3 b) {
            return new Hex3(a.p - b.p, a.q - b.q, a.s - b.s);
        }
        public static Hex3 operator -(Hex3 a) {
            return new Hex3(-a.p, -a.q, -a.s);
        }
        public static Hex3 operator *(Hex3 a, int b) {
            return new Hex3(a.p * b, a.q * b, a.s * b);
        }
        public static Hex3 operator *(int a, Hex3 b) {
            return new Hex3(a * b.p, a * b.q, a * b.s);
        }
        public static Hex3 operator /(Hex3 a, int b) {
            return new Hex3(a.p / b, a.q / b, a.s / b);
        }

        public static bool operator ==(Hex3 a, Hex3 b) {
            return a.Equals(b);
        }

        public static bool operator !=(Hex3 a, Hex3 b) {
            return !a.Equals(b);
        }

        public override bool Equals(object obj) {
            if (!(obj is Hex3 hex))
                return false;
            return p == hex.p && q == hex.q && s == hex.s;
        }

        public override int GetHashCode() {
            int hash = 13;
            hash = (hash * 7) + p.GetHashCode();
            hash = (hash * 7) + q.GetHashCode();
            hash = (hash * 7) + s.GetHashCode();
            return hash;
        }

        public override string ToString() {
            return $"({p}, {q}, {s})";
        }

        static bool IsValid(int p, int q, int s) {
            return p + q + s == 0;
        }
    }
}
