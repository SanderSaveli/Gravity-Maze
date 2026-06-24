using SanderSaveli.UDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class GameVersionView : TextByTableKey
    {
        protected override void SetText(string text)
        {
            string txt = string.Format(text, Application.version);
            base.SetText(txt);
        }
    }
}
