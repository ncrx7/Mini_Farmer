using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PrefabUIs
{
    public class ProductButtonPanelController : MonoBehaviour
    {
        [SerializeField] private Button _increaseProductionButton;
        [SerializeField] private Button _reduceProductionButton;
        [SerializeField] private TextMeshProUGUI _resourceAmountText;

        public Button GetIncreaseButton => _increaseProductionButton;
        public Button GetReduceButton => _reduceProductionButton;
        public TextMeshProUGUI GetResourceAmountText => _resourceAmountText;
    }
}
