using UnityEngine;

namespace UI
{
    public class PlayerModel
    {
        public Vector3 InitialPosition { get; private set; }
        public bool IsFirstLanded { get; private set; }

        public void SetInitialPosition(Vector3 position)
        {
            InitialPosition = position;
            IsFirstLanded = true;
        }

        public float GetPositionDifference(Vector3 currentPosition)
        {
            return Mathf.Abs(currentPosition.y - InitialPosition.y);
        }

    }
}