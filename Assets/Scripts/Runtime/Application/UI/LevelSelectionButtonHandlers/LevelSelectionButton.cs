using Application.UI;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Application.UI.LevelSelectionButtonHandlers
{
    public class LevelSelectionButton : SimpleButton
    {
        private readonly Color _selectedColor = new(0.65f, 0.71f, 1f);

        public readonly Subject<(LevelSelectionButton, int)> LevelSelectedSubject = new();

        private int _level;
        private TextMeshProUGUI _levelIndexText;
        private Image _image;
        private bool _locked;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _levelIndexText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start() => Button.onClick.AddListener(OnLevelSelected);

        private void OnDestroy() => Button.onClick.RemoveAllListeners();

        public void UpdateSelection(bool selected) => _image.color = selected ? _selectedColor : Color.white;

        public int GetLevel() => _level;

        public void SetLevel(int level)
        {
            _level = level;
            _levelIndexText.text = level.ToString();
        }

        public void SetLocked(Sprite sprite)
        {
            _locked = true;
            _image.sprite = sprite;
        }

        private void OnLevelSelected()
        {
            if(!_locked)
                LevelSelectedSubject?.OnNext((this, _level));
        }
    }
}