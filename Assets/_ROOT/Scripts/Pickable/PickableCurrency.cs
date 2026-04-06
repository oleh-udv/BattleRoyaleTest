namespace Scripts.Pickable
{
    using DG.Tweening;
    using UnityEngine;

    public class PickableCurrency : MonoBehaviour, IPickable
    {
        [SerializeField] private Collider collider;
        [SerializeField] private PickableTypes pickableType;
        [SerializeField] private int value;
        [SerializeField] private Transform visual;
        
        [Header("Animation")]
        [SerializeField] private float jumpDuration;
        [SerializeField] private float scaleTime;
        [SerializeField] private float jumpPower;
        
        private Sequence animationSequence;
        
        public PickableTypes GetPickableType() => pickableType;
        public int GetValue() => value;

        private void OnDestroy()
        {
            animationSequence?.Kill();
        }
        
        public void PickUp(Transform unitTransform)
        {
            collider.enabled = false;
            animationSequence?.Kill();

            animationSequence = DOTween.Sequence();
            transform.SetParent(unitTransform);
            
            animationSequence.Append(transform.DOLocalJump(Vector3.zero, jumpPower, 1, jumpDuration));
            animationSequence.Join(transform.DOScale(Vector3.zero, scaleTime));
            
            animationSequence.OnComplete(EndPickUpEndPickUp);
        }

        public void PlayFallAnimation()
        {
            visual.transform.eulerAngles = Vector3.up * Random.Range(0, 360);
        }

        private void EndPickUpEndPickUp()
        {
            Destroy(gameObject);
        }
    }
}