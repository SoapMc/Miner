using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using TMPro;
using System.Text;
using Miner.Management.Events;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Miner.Management.Exceptions;

namespace Miner.UI
{
    public class PartDescriptor : MonoBehaviour
    {
        private const int MAX_EXPECTED_SIZE_OF_DESCRIPTION = 300;

        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private Image _image = null;
        [SerializeField] private TextMeshProUGUI _description = null;

        private StringBuilder _descriptionBuilder = new StringBuilder(MAX_EXPECTED_SIZE_OF_DESCRIPTION);
        
        public void OnShowPartDescription(EventArgs args)
        {
            if (args == null) return;
            

            if (args is ShowPartDescriptionEA partDescription)
            {
                CreateDescription(partDescription.Part, partDescription.State);
            }
            else
            {
                throw new InvalidEventArgsException();
            }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        public void CreateDescription(ReferencePart part, EOfferState state)
        {
            _descriptionBuilder.Clear();
            if(state == EOfferState.Available)
            {
                _name.text = part.Name;
            }
            else if(state == EOfferState.Bought)
            {
                _name.text = "<color=yellow>" + part.Name + "</color>";
            }
            else
            {
                _name.text = "<color=red>" + part.Name + "</color>";
            }
            
            _image.sprite = part.Sprite;
            _descriptionBuilder.Append("<size=14>" + part.ShortDescription + "</size>\n");
            if(state == EOfferState.Available)
                _descriptionBuilder.Append("Cost: " + part.Cost.ToString() + " $\n");
            else if(state == EOfferState.Unavailable)
                _descriptionBuilder.Append("<color=red>Cost: " + part.Cost.ToString() + " $</color>\n");

            string specDesc = part.GetOfferDescription();
            _descriptionBuilder.Append("\n" + specDesc);
            
            if (state == EOfferState.Bought)
            {
                _descriptionBuilder.Append("\n\n<color=yellow>Owned!</color>");
                _descriptionBuilder.Append("\n<size=14><color=yellow>Buy again to get brand new part - fully performant.</color></size>");
            }

            _description.text = _descriptionBuilder.ToString();
        }
    }
}