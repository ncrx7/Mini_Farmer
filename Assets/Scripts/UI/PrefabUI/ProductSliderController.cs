using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PrefabUIs
{
    public class ProductSliderController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _productInStorageText;
        [SerializeField] private TextMeshProUGUI _productTimeText;
        [SerializeField] private TextMeshProUGUI _storageCapacityRateText;
        [SerializeField] private Image _productionProduceImage;

        public Slider GetSlider => _slider;
        public TextMeshProUGUI GetCurrentStorageText => _productInStorageText;
        public TextMeshProUGUI GetProductTimeText => _productTimeText;
        public TextMeshProUGUI GetStorageCapacityRateText => _storageCapacityRateText;
        public Image GetProductionProduceImage => _productionProduceImage;
    }
}