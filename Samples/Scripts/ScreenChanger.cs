using UnityEngine;

namespace Dialogues.Samples.Scripts
{
    [CreateAssetMenu(fileName = "ScreenChanger", menuName = "Facticus/Dialogues/Samples/ScreenChanger", order = 0)]
    public class ScreenChanger : ScriptableObject
    {
        public enum ScreenColor
        {
            Red,
            Green
        }

        [SerializeField] private ScreenColor _screenColor;
        [SerializeField] private Color _green;
        [SerializeField] private Color _red;

        private Color GetColor(ScreenColor color)
        {
            switch (color)
            {
                case ScreenColor.Green:
                    return _green;
                case ScreenColor.Red:
                    return _red;
                default: return Color.clear;
            }
        }

        public void ToggleColor()
        {
            var color = _screenColor == ScreenColor.Green ? ScreenColor.Red : ScreenColor.Green;
            SetColor(color);
        }
        
        public void SetColor(ScreenColor color)
        {
            _screenColor = color;
            var newColor = GetColor(color);

            Camera.main.backgroundColor = newColor;
        }

        public bool IsGreen()
        {
            return _screenColor == ScreenColor.Green;
        }
        
        public bool IsRed()
        {
            return _screenColor == ScreenColor.Red;
        }
    }
}