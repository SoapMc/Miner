using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;

namespace Miner.UI
{
    public class InventoryEquipmentElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _status = null;

        public void Initialize(ReferencePart part, string status)
        {
            _name.text = part.Name;
            _name.color = Color.Lerp(Color.red, Color.green, 2 * part.Durability - 1);
            _status.color = _name.color;
            _status.text = status;
        }
    }
}