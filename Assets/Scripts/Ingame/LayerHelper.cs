using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHelper {

	public enum Layer
    {
        GROUND, PLAYER1, PLAYER2, PLAYER3, PLAYER4, PLAYER1BLOCK, PLAYER2BLOCK, PLAYER3BLOCK, PLAYER4BLOCK, GOUND_NO_COLLISION
    }

    public static int getLayer(Layer layer)
    {
        switch(layer)
        {
            case Layer.GROUND: return 8;
            case Layer.PLAYER1: return 9;
            case Layer.PLAYER2: return 10;
            case Layer.PLAYER3: return 11;
            case Layer.PLAYER4: return 12;
            case Layer.PLAYER1BLOCK: return 13;
            case Layer.PLAYER2BLOCK: return 14;
            case Layer.PLAYER3BLOCK: return 15;
            case Layer.PLAYER4BLOCK: return 16;
            case Layer.GOUND_NO_COLLISION: return 17;
        }
        return 0;
    }

    public static int getLayers(Layer[] layers)
    {
        int mask = 0;
        foreach (Layer layer in layers)
        {
            mask = mask | 1 << getLayer(layer);
        }
        return mask;
    }
}
