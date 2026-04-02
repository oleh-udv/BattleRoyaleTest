namespace Scripts.Infrastructure.Input
{
    using UnityEngine;

    public class InputProvider : IInputProvider
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public Vector3 Axis => ReadInput();
        private Vector3 ReadInput() => new (SimpleInput.GetAxis(Horizontal), 0f, SimpleInput.GetAxis(Vertical));
    }
}