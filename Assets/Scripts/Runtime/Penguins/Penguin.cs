using PPP.Boards;
using PPP.Hexagons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPP.Penguine {
    public class Penguin : MonoBehaviour {
        [SerializeField]
        float floorHeight = 1f;

        [SerializeField]
        float moveSpeed = 10f;
        [SerializeField]
        public TargetDemand demand = default;

        public Hex3 hexPosition = default;
        public Hex3 backupPosition = default;


        PingPongBoard parentBoard = default;


        public void Init(Hex3 position, PingPongBoard board) {
            parentBoard = board;
            hexPosition = position;
            var cartesian = Hex3.HexToCartesian(position);
            transform.localPosition = new Vector3(cartesian.x, floorHeight, cartesian.y);
        }

        public void Slide(Direction direction) {
            if (parentBoard.TryReachTarget(this, direction)) {
                parentBoard.SetTargetRandom();
            } else {
                hexPosition = parentBoard.Slide(hexPosition, direction);
                if (parentBoard.TryReachTarget(this, direction)) {
                    parentBoard.SetTargetRandom();
                }
            }
        }

        public IEnumerable<Direction> AvailableDirections() {
            foreach(var direction in (Direction[]) Enum.GetValues(typeof(Direction))){
                if(parentBoard.CanMove(hexPosition, direction) || parentBoard.target == hexPosition.Neighbor(direction)) {
                    yield return direction;
                }
            }
        }

        public void Rollback() {
            hexPosition = backupPosition;
        }

        // Update is called once per frame
        void Update() {
            var cartesian = Hex3.HexToCartesian(hexPosition);
            var translation = new Vector3(cartesian.x, floorHeight, cartesian.y) - transform.localPosition;
            if(translation.sqrMagnitude > Mathf.Pow(Time.deltaTime*moveSpeed,2)) {
                transform.Translate(translation.normalized * Time.deltaTime * moveSpeed, Space.World);
                transform.rotation = Quaternion.LookRotation(translation, Vector3.up);
            }
        }
    }
}
