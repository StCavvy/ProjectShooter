using UnityEngine;
using UnityEngine.Events;

namespace UIScripting
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private EAnimationTypes animationType;
        [SerializeReference] private AnimationParams animationParams;

        private UISwitcher switcher;
        private Vector2 startAnchoredPosition;


        public PanelAnimator panelAnimator;
        public UnityEvent OnStartedOpening;
        public UnityEvent OnStartedClosing;
        public UnityEvent OnPanelOpened;
        public UnityEvent OnPanelClosed;

        public EAnimationTypes AnimationType => animationType;
        public AnimationParams AnimationParams => animationParams;
        public Vector2 StartAnchoredPosition => startAnchoredPosition;

        private void Awake()
        {
            startAnchoredPosition = ((RectTransform)transform).anchoredPosition;
            EnsureValidState();
        }

        private void OnValidate()
        {
            EnsureValidState();
        }

        private void EnsureValidState()
        {
            if (animationType == EAnimationTypes.None)
            {
                animationParams = null;
                return;
            }

            if (animationParams == null || !IsParamsMatchingType())
            {
                animationParams = AnimationParamsFactory.Create(animationType);
            }
        }

        private bool IsParamsMatchingType()
        {
            return animationType switch
            {
                EAnimationTypes.Fade => animationParams is FadeParams,
                EAnimationTypes.PopUp => animationParams is PopUpParams,
                EAnimationTypes.FadeAndPopUp => animationParams is PopUpParams,
                EAnimationTypes.Slide => animationParams is SlideParams,
                EAnimationTypes.Rotate => animationParams is RotateParams,
                EAnimationTypes.Bounce => animationParams is BounceParams,
                EAnimationTypes.Curtain => animationParams is CurtainParams,
                _ => false
            };
        }

        public void AnimateIn()
        {
            if (gameObject.activeSelf) return;
            Init();
            OnStartedOpening?.Invoke();
            switcher.AnimateIn(this);
        }

        public void AnimateOut()
        {
            if (!gameObject.activeSelf) return;
            Init();
            OnStartedClosing?.Invoke();
            switcher.AnimateOut(this);
        }

        public void OpenPanel()
        {
            Init();
            switcher.OpenPanel(this);
        }

        private void Init()
        {
            if (switcher == null) switcher = FindAnyObjectByType<UISwitcher>();
            if (panelAnimator == null) panelAnimator = new PanelAnimator();
        }
    }
}
