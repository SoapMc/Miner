using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.FX
{
    [CreateAssetMenu(menuName = "FX/UI Style Sheet")]
    public class UIStandardStyleSheet : ScriptableObject
    {
        [SerializeField] private Color _background = Color.black;
        [SerializeField] private Color _foreground = Color.green;

        public Color BackgroundColor => _background;
        public Color ForegroundColor => _foreground;
    }
}