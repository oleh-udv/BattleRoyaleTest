namespace Scripts.Pickable
{
    using UnityEngine;

    public class PickableCurrency : MonoBehaviour, IPickable
    {
        [SerializeField] private Collider collider;
        [SerializeField] private PickableTypes pickableType;
        [SerializeField] private int value;

        public PickableTypes GetPickableType() => pickableType;
        public int GetValue() => value;
        
        public void PickUp(Transform transform)
        {
            gameObject.SetActive(false);
            collider.enabled = false;
            //animation
        }

        public void PlayFallAnimation()
        {
            
        }
    }
}