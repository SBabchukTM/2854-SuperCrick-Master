using System;
using Application.Services.Audio;
using Core.Services.Audio;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.UI
{
    [RequireComponent(typeof(Animation), typeof(Button))]
    public class SimpleButton : MonoBehaviour
    {
        private Button _button;
        private Animation _pressAnimation;
        private IAudioService _audioService;

        public Button Button => _button;
        public bool Clicked { get; private set; }

#if UNITY_EDITOR
        private void Reset()
        {
            _button = GetComponent<Button>();
            _pressAnimation = GetComponent<Animation>();

            UnityEventTools.AddPersistentListener(_button.onClick, PlayPressAnimation); 
            _pressAnimation.playAutomatically = false;

            _pressAnimation.clip = Resources.Load<AnimationClip>("ButtonClickAnim");
            _pressAnimation.AddClip(Resources.Load<AnimationClip>("ButtonClickAnim"), "ButtonClickAnim");
        }
#endif

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
            
            _button = GetComponent<Button>();
            _pressAnimation = GetComponent<Animation>();
        }

        public virtual void PlayPressAnimation()
        {
            Clicked = true;
            _pressAnimation.Play();
            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }
    }
}