using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FarmerScene.EntityUIController
{
    public class MarketUI : MonoBehaviour
    {
        [SerializeField] private Image _marketImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _purchaseText;
        [SerializeField] private Button _purchaseButton;
        //TODO DESCRIPTION

        public Image GetMarketImage => _marketImage;
        public TextMeshProUGUI GetPriceText => _priceText;
        public TextMeshProUGUI GetPurchaseText => _purchaseText;
        public Button GetPurchaseButton => _purchaseButton;
        //TODO DESCRIPTION TEXT GETTER
    }
}
