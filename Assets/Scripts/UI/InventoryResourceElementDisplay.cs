using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;

namespace Miner.UI
{
    public class InventoryResourceElementDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _amount = null;

        public void Initialize(CargoTable.Element element)
        {
            _name.text = element.Type.Name;
            _amount.text = element.Amount.ToString();
        }
    }
}