using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class PortalController : MonoBehaviour
    {
        public PortalNormal PortalNormal => _portalNormal;
        public PortalTeleportator PortalTeleportator => _portalTeleportator;
        public PortalClipController ClipController => _clipController;
        public PortalCanvasRender CanvasRender => _canvasRender;

        public Action<bool> OnPrepareForTeleport;

        [SerializeField] private PortalNormal _portalNormal;
        [SerializeField] private PortalTeleportator _portalTeleportator;
        [SerializeField] private PortalClipController _clipController;
        [SerializeField] private PortalCanvasRender _canvasRender;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out _))
            {
                OnPrepareForTeleport?.Invoke(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out _))
            {
                OnPrepareForTeleport?.Invoke(false);
            }
        }
    }
}
