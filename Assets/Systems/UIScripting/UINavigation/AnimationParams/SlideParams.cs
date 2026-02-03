namespace UIScripting
{
    [System.Serializable]
    public class SlideParams : AnimationParams
    {
        public ESides SlideFrom = ESides.Up;
        public bool ReverseDirections = false;
        public float Distance = 200f;
    }
}