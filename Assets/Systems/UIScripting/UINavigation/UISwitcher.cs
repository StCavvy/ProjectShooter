using System.Collections;
using UnityEngine;

namespace UIScripting
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] private UIPanel defaultPanel;
        private UIPanel currentPanel;

        private void Awake()
        {
            currentPanel = defaultPanel;
        }
        public void OpenPanel(UIPanel target)
        {
            if (currentPanel != null) target.panelAnimator.AnimateOut(currentPanel);
            target.panelAnimator.AnimateIn(target);
            currentPanel = target;
        }

        public void AnimateIn(UIPanel target)
        {
            target.panelAnimator.AnimateIn(target);
        }

        public void AnimateOut(UIPanel target)
        {
            target.panelAnimator.AnimateOut(target);
        }

    }
}
