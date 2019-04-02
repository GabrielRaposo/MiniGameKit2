using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GataclismaNaPista
{
    public class CatPlayer : PlayerInfo
    {
        //Gambiarra esse scoreBar. É pra trocar a cor do scoreBar de acordo com o player.
        public Image scoreBar;
        public SpriteRenderer catSprite;

        public override void Start()
        {
            base.Start();
            
            scoreBar.color = base.color;

            float H, S, V;
            Color.RGBToHSV(base.color, out H, out S, out V);
            catSprite.color = Color.HSVToRGB(H, 0.4f, V);
        }
    }

}
