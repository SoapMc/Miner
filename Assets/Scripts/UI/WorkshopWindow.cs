using UnityEngine;

namespace Miner.UI
{
    public class WorkshopWindow : Window
    {
        [SerializeField] private PartListGrid _partListGrid = null;

        protected override void OnAppearingFinished()
        {
            
        }

        private void Start()
        {
            _partListGrid.Load();
            _firstSelectedObject = _partListGrid.GetFirstSelectedObject();
            SelectFirstObject();
        }
    }
}