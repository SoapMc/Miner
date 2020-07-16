using UnityEngine.EventSystems;
using UnityEngine;

namespace Miner.Management
{
    public class CustomInput : BaseInput
    {
        protected override void Start()
        {
            StandaloneInputModule standaloneInputModule = GetComponent<StandaloneInputModule>();
            standaloneInputModule.inputOverride = this;
        }

        //public override bool mousePresent => false;
        /*
        public override string compositionString
        {
            get { return Input.compositionString; }
        }

        
        public override IMECompositionMode imeCompositionMode
        {
            get { return Input.imeCompositionMode; }
            set { Input.imeCompositionMode = value; }
        }

        
        public override Vector2 compositionCursorPos
        {
            get { return Vector2.zero; }
            set { Input.compositionCursorPos = value; }
        }

        

        public override bool GetMouseButtonDown(int button)
        {
            return false;
        }

        public override bool GetMouseButtonUp(int button)
        {
            return false;
        }

        public override bool GetMouseButton(int button)
        {
            return false;
        }

        public override Vector2 mousePosition
        {
            get { return Vector2.zero; }
        }

        public override Vector2 mouseScrollDelta
        {
            get => Vector2.zero;
        }

        public override bool touchSupported
        {
            get => false;
        }

        public override int touchCount
        {
            get => 0;
        }

        public override Touch GetTouch(int index)
        {
            return new Touch();
        }*/
    }
}
