using UnityEngine;

namespace Application.Services.CameraProvider
{
    public interface ICameraProvider
    {
        Camera MainCamera { get; }
        float WorldScreenHeight { get; }
        float WorldScreenWidth { get; }

        void SetMainCamera(Camera camera);
    }
}