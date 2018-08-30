using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHelper {

	public enum Layer
    {
        GROUND, PLAYER
    }

    public static int getLayer(Layer layer)
    {
        switch(layer)
        {
            case Layer.GROUND: return 8;
            case Layer.PLAYER: return 9;
        }
        return 0;
    }
}
