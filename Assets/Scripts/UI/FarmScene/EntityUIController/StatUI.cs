using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FarmerScene.EntityUIController
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statAmountText;
        [SerializeField] private Image _statIconImage;

        public TextMeshProUGUI GetStatAmountText => _statAmountText;
        public Image GetStatIconImage => _statIconImage;
    }
}
