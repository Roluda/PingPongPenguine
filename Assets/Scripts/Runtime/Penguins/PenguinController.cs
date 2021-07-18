using PPP.Hexagons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace PPP.Penguine {
    public class PenguinController : MonoBehaviour {

        [SerializeField]
        LayerMask penguinLayer = default;
        [SerializeField]
        float rayCastDistance = 50f;

        [SerializeField]
        Vector3 directionsContextOffset = Vector3.up;
        [SerializeField]
        Transform directionsContext = default;
        [SerializeField]
        List<Button> buttons = default;

        Penguin selectedPenguin = default;

        private void Start() {
            foreach(var button in buttons) {
                button.onClick.AddListener(() => MoveSelectedPenguin(buttons.IndexOf(button)));
            }
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, rayCastDistance, penguinLayer)) {
                    if(hit.collider.TryGetComponent(out selectedPenguin)){
                        directionsContext.gameObject.SetActive(true);
                    }
                }
            }

            if (selectedPenguin) {
                List<int> directions = selectedPenguin.AvailableDirections().Select(direction => (int)direction).ToList();
                for (int i= 0; i<buttons.Count; i++) {
                    if (directions.Contains(i)) {
                        buttons[i].gameObject.SetActive(true);
                    } else {
                        buttons[i].gameObject.SetActive(false);
                    }
                }
                directionsContext.transform.position = selectedPenguin.transform.position + directionsContextOffset;
            }
            directionsContext.gameObject.SetActive(selectedPenguin);
        }

        public void MoveSelectedPenguin(int direction) {
            Assert.IsTrue(selectedPenguin);
            selectedPenguin.Slide((Direction)direction);
        }
    }
}
