namespace UIScripting
{
    public static class AnimationParamsFactory
    {
        public static AnimationParams Create(EAnimationTypes type)
        {
            switch (type)
            {
                case EAnimationTypes.Fade:
                    return new FadeParams();

                case EAnimationTypes.PopUp:
                    return new PopUpParams();

                case EAnimationTypes.FadeAndPopUp:
                    return new PopUpParams();

                case EAnimationTypes.Slide:
                    return new SlideParams();

                case EAnimationTypes.Rotate:
                    return new RotateParams();

                case EAnimationTypes.Bounce:
                    return new BounceParams();

                case EAnimationTypes.Curtain:
                    return new CurtainParams();

                case EAnimationTypes.None:
                default:
                    return null;
            }
        }
    }
}