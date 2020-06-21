﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;

namespace Miner.UI
{
    public class UsableItemElementDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _amount = null;

        public void Initialize(UsableItemTable.Element element)
        {
            _name.text = element.Item.Name;
            _amount.text = element.Amount.ToString();
        }
    }
}