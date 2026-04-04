namespace Scripts.Pickable
{
    using UnityEngine;

    public interface IPickable
    {
        void PickUp(Transform  transform);
        PickableTypes GetPickableType();
        int GetValue();
    }
}