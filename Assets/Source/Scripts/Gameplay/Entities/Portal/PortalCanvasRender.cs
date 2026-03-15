using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class PortalCanvasRender : MonoBehaviour
    {
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private RawImage _image;

        public void SetRender(RenderTexture cameratexture, RenderTexture imageTexture)
        {
            _renderCamera.targetTexture = cameratexture;
            _image.texture = imageTexture;
        }
    }
}
