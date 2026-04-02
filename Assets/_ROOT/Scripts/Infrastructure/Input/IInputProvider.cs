namespace Scripts.Infrastructure.Input
{
    using UnityEngine;

    public interface IInputProvider
    {
        Vector3 Axis { get; }
    }
}