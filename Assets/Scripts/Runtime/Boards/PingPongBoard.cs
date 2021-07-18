using PPP.Hexagons;
using PPP.Penguine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PPP.Boards {
    public class PingPongBoard : MonoBehaviour {
        [SerializeField]
        int radius = 7;
        [SerializeField]
        float spawnInterval = 0.05f;
        [SerializeField]
        PingPongField fieldPrefab = default;
        [SerializeField]
        Transform context = default;
        [SerializeField]
        float obstacleChance = 0.15f;
        [SerializeField]
        Penguin[] penguinPrefabs;
        [SerializeField]
        int penguinCount = 4;

        [SerializeField]
        Target targetObject = default;
        [SerializeField]
        float targetHeight = 1;

        public Hex3 target = default;
        public TargetDemand demand = default;

        Dictionary<Hex3, PingPongField> hexagons = new Dictionary<Hex3, PingPongField>();

        List<Penguin> penguins = new List<Penguin>();

        List<Hex3> backupPositions = new List<Hex3>();

        private void Start() {
            NewBoard();
        }

        IEnumerator BuildBoard() {
            foreach (var hex in HexUtility.HexSpiral(radius)) {
                hexagons[hex] = Instantiate(fieldPrefab, context);
                hexagons[hex].Init(hex);
                hexagons[hex].SetObstacle(Random.value<obstacleChance);
                yield return new WaitForSeconds(spawnInterval);
            }
            for(int i=0; i < penguinCount; i++) {
                var newPengu = Instantiate(penguinPrefabs[i % penguinPrefabs.Length], context);
                newPengu.Init(RandomFreeSpace(), this);
                penguins.Add(newPengu);
            }
            SetTargetRandom();
        }

        public Hex3 RandomFreeSpace() {
            var availableHexes = hexagons.Where(hex => !hex.Value.isObstacle && !penguins.Any(pengu => pengu.hexPosition == hex.Key)).ToList();
            return availableHexes[Random.Range(0, availableHexes.Count)].Key;
        }

        public void SetTargetRandom() {
            penguins.ForEach(pengu => pengu.backupPosition = pengu.hexPosition);
            var possibleDemands = (TargetDemand[])System.Enum.GetValues(typeof(TargetDemand));
            demand = possibleDemands[Random.Range(0, possibleDemands.Length)];
            target = RandomFreeSpace();
            var position = Hex3.HexToCartesian(target);
            targetObject.transform.localPosition = new Vector3(position.x, targetHeight, position.y);
            targetObject.gameObject.SetActive(true);
            targetObject.SetTargetType((int)demand);
        }

        public bool TryReachTarget(Penguin pengu, Direction direction) {
            return pengu.hexPosition.Neighbor(direction) == target && pengu.demand == demand;
        }


        public Hex3 Slide(Hex3 start, Direction direction) {
            while(CanMove(start, direction)) {
                start = start.Neighbor(direction);
            }
            return start;
        }

        public bool CanMove(Hex3 start, Direction direction) {
            var neighbor = start.Neighbor(direction);
            bool penguBlock = penguins.Any(pengu => pengu.hexPosition == neighbor);
            bool targetBlock = target == neighbor;
            return hexagons.ContainsKey(neighbor) && !hexagons[neighbor].isObstacle && !penguBlock && !targetBlock;
        }

        public void Rollback() {
            penguins.ForEach(pengu => pengu.Rollback());
        }

        public void NewBoard() {
            StopAllCoroutines();
            for(int i = context.childCount-1; i>0; i--) {
                Destroy(context.GetChild(i).gameObject);
            }
            hexagons.Clear();
            penguins.Clear();
            targetObject.gameObject.SetActive(false);
            StartCoroutine(BuildBoard());
        }
    }
}
