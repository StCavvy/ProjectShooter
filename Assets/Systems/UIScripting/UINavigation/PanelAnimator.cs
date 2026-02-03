using UnityEngine;
using DG.Tweening;

namespace UIScripting
{
    public sealed class PanelAnimator
    {
        private UIPanel panel;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private AnimationParams animParams;

        public void AnimateIn(UIPanel panel)
        {
            if (panel.gameObject.activeSelf)
                return;

            Animate(panel, true);
        }

        public void AnimateOut(UIPanel panel)
        {
            if (!panel.gameObject.activeSelf)
                return;

            Animate(panel, false);
        }

        private void Animate(UIPanel panel, bool inwards)
        {
            this.panel = panel;

            canvasGroup = panel.GetComponent<CanvasGroup>();
            rectTransform = panel.GetComponent<RectTransform>();
            animParams = panel.AnimationParams;

            if (canvasGroup == null || rectTransform == null)
            {
                Debug.LogError($"UIPanel {panel.name} missing required components");
                return;
            }

            canvasGroup.DOKill();
            rectTransform.DOKill();

            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            switch (panel.AnimationType)
            {
                case EAnimationTypes.Fade:
                    Fade(inwards);
                    break;

                case EAnimationTypes.PopUp:
                    PopUp(inwards);
                    break;

                case EAnimationTypes.FadeAndPopUp:
                    FadeAndPopUp(inwards);
                    break;

                case EAnimationTypes.Curtain:
                    Curtain(inwards);
                    break;

                case EAnimationTypes.Slide:
                    Slide(inwards);
                    break;

                case EAnimationTypes.Rotate:
                    Rotate(inwards);
                    break;

                case EAnimationTypes.Bounce:
                    Bounce(inwards);
                    break;

                case EAnimationTypes.None:
                default:
                    Finish(inwards);
                    break;
            }
        }

        private void Fade(bool inwards)
        {
            if (animParams is not FadeParams p)
            {
                Finish(inwards);
                return;
            }

            canvasGroup.alpha = inwards ? 0f : 1f;

            canvasGroup
                .DOFade(inwards ? 1f : 0f, p.Duration)
                .OnComplete(() => Finish(inwards));
        }

        private void PopUp(bool inwards)
        {
            if (animParams is not PopUpParams p)
            {
                Finish(inwards);
                return;
            }

            canvasGroup.alpha = 1f;
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            rectTransform.localScale = inwards ? Vector3.one * p.PopScale : Vector3.one;

            rectTransform
                .DOScale(inwards ? Vector3.one : Vector3.one * p.PopScale, p.Duration)
                .OnComplete(() => Finish(inwards));
        }

        private void FadeAndPopUp(bool inwards)
        {
            if (animParams is not PopUpParams p)
            {
                Finish(inwards);
                return;
            }

            canvasGroup.gameObject.SetActive(true);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            canvasGroup.alpha = inwards ? 0f : 1f;
            rectTransform.localScale = inwards ? Vector3.one * p.PopScale : Vector3.one;

            Sequence sequence = DOTween.Sequence();
            sequence.Join(canvasGroup.DOFade(inwards ? 1f : 0f, p.Duration));
            sequence.Join(rectTransform.DOScale(inwards ? Vector3.one : Vector3.one * p.PopScale, p.Duration));

            sequence.OnComplete(() => Finish(inwards));
        }

        private void Curtain(bool inwards)
        {
            if (animParams is not CurtainParams p)
            {
                Finish(inwards);
                return;
            }

            if (inwards)
            {
                canvasGroup.gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            Vector2 basePos = panel.StartAnchoredPosition;
            float height = ((RectTransform)rectTransform.parent).rect.height;
            Vector2 off = basePos + Vector2.up * (p.FromTop ? height : -height);

            if (inwards)
            {
                rectTransform.anchoredPosition = off;
                rectTransform.localScale = new Vector3(1f, p.StartScaleY, 1f);
            }
            else
            {
                rectTransform.anchoredPosition = basePos;
                rectTransform.localScale = Vector3.one;
            }

            rectTransform.DOAnchorPos(inwards ? basePos : off, p.Duration);
            rectTransform
                .DOScaleY(inwards ? 1f : p.StartScaleY, p.Duration)
                .OnComplete(() => Finish(inwards));
        }

        private void Slide(bool inwards)
        {
            if (animParams is not SlideParams p)
            {
                Finish(inwards);
                return;
            }

            if (inwards)
            {
                canvasGroup.gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            Vector2 basePos = panel.StartAnchoredPosition;

            Vector2 direction = p.SlideFrom switch
            {
                ESides.Up => Vector2.up,
                ESides.Down => Vector2.down,
                ESides.Left => Vector2.left,
                ESides.Right => Vector2.right,
                _ => Vector2.zero
            };

            Vector2 offset = direction * p.Distance;

            if (!inwards && p.ReverseDirections)
                offset = -offset;

            rectTransform.anchoredPosition = inwards ? basePos + offset : basePos;

            rectTransform
                .DOAnchorPos(inwards ? basePos : basePos + offset, p.Duration)
                .OnComplete(() => Finish(inwards));
        }

        private void Rotate(bool inwards)
        {
            if (animParams is not RotateParams p)
            {
                Finish(inwards);
                return;
            }

            if (inwards)
            {
                canvasGroup.gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            Quaternion from = Quaternion.Euler(0f, 0f, p.RotateAngle);
            Quaternion to = Quaternion.identity;

            rectTransform.localRotation = inwards ? from : to;

            rectTransform
                .DOLocalRotateQuaternion(inwards ? to : from, p.Duration)
                .OnComplete(() => Finish(inwards));
        }

        private void Bounce(bool inwards)
        {
            if (animParams is not BounceParams p)
            {
                Finish(inwards);
                return;
            }

            if (inwards)
            {
                canvasGroup.gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            Vector2 basePos = panel.StartAnchoredPosition;
            Vector2 offset = Vector2.up * p.Amplitude;

            rectTransform.anchoredPosition = inwards ? basePos + offset : basePos;

            Sequence sequence = DOTween.Sequence();

            if (inwards)
            {
                sequence.Append(
                    rectTransform.DOAnchorPos(basePos - offset, p.Duration * 0.25f)
                );
                sequence.Append(
                    rectTransform.DOAnchorPos(basePos, p.Duration * 0.75f)
                );
            }
            else
            {
                sequence.Append(
                    rectTransform.DOAnchorPos(basePos - offset, p.Duration * 0.25f)
                );
                sequence.Append(
                    rectTransform.DOAnchorPos(basePos + offset, p.Duration * 0.75f)
                );
            }

            sequence.OnComplete(() => Finish(inwards));
        }

        private void Finish(bool inwards)
        {
            canvasGroup.alpha = inwards ? 1f : 0f;
            canvasGroup.interactable = inwards;
            canvasGroup.blocksRaycasts = inwards;

            if (!inwards)
            {
                canvasGroup.gameObject.SetActive(false);
                panel.OnPanelClosed?.Invoke();
            }
            else
            {
                panel.OnPanelOpened?.Invoke();
            }
        }
    }

}
