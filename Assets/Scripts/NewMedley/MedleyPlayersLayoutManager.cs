using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedleyPlayersLayoutManager : MonoBehaviour
{
    public List<MedleyPlayerLayout> playerLayouts;

    public void PlaceIcons(List<MeddleyPlayerIcon> icons)
    {
        var layout = playerLayouts[icons.Count];

        layout.SetIcons(icons);
        layout.AnimateEntrance(icons);
    }
    
}
