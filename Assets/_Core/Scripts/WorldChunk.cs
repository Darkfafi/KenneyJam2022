using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChunk : MonoBehaviour
{
    public KenneyJamGame Game
    {
        get; private set;
    }

	public void Init(KenneyJamGame game)
    {
        if(Game != null)
        {
            return;
        }

        Game = game;

        foreach(IChunkEntity child in GetComponentsInChildren<IChunkEntity>())
        {
            child.Init(this);
        }
    }

    public void Deinit()
    {
        if(Game == null)
        {
            return;
        }

        foreach(IChunkEntity child in GetComponentsInChildren<IChunkEntity>())
        {
            child.Deinit();
        }

        Game = null;
    }
}
