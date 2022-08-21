using RaTweening;
using System.Collections.Generic;
using UnityEngine;

public class WorldNavigationSystem : MonoBehaviour
{
	#region Consts

	public const int ChunkSize = 14;

	#endregion

	#region Editor Variables

	[Header("Movement Settings")]
	[SerializeField]
	private float _speed = 5f;

	[Header("Chunk Settings")]
	[SerializeField]
	private Transform _chunksContainer = null;

	[SerializeField]
	private WorldChunk _startChunkPrefab = null;

	[SerializeField]
	private ChunkData[] _chunkData = null;

	#endregion

	#region Variables

	private bool _initialized = false;
	private int _chunkIndex = 0;
	private RaTween _loopTween = null;
	private List<WorldChunk> _chunks = new List<WorldChunk>();
	private float _duration;

	#endregion

	#region Properties

	public KenneyJamGame Game
	{
		get; private set;
	}

	public float DistanceTravelled
	{
		get; private set;
	}

	public bool IsRunning => _loopTween != null && _loopTween.IsPlaying;

	#endregion

	#region Lifecycle

	protected void OnDestroy()
	{
		_loopTween?.Kill();
		_loopTween = null;
	}

	#endregion

	#region Public Methods

	public void Initialize(KenneyJamGame game)
	{
		if(_initialized)
		{
			return;
		}

		Game = game;

		// Mark Start Base Chunk As Created
		ResetLoop();

		// Stabalize Variables
		_speed = Mathf.Max(1f, _speed);
		_duration = (ChunkSize / _speed);

		// Completed
		_initialized = true;
	}

	public void ResumeLoop()
	{
		if(_loopTween != null)
		{
			_loopTween.Resume();
		}
		else
		{
			_loopTween = _chunksContainer
				.TweenMoveX(-ChunkSize, _duration)
				.SetLocalPosition()
				.SetEndIsDelta()
				.SetDynamicSetupStep(RaTweenDynamicSetupStep.Start)
				.SetInfiniteLooping()
				.OnSetup(()=> CreateChunk())
				.OnUpdate(() => 
				{ 
					DistanceTravelled += Time.deltaTime * _speed;
				})
				.OnLoop((loopCount) => CreateChunk());
		}
	}

	public void PauseLoop()
	{
		_loopTween?.Pause();
	}

	public void ResetLoop()
	{
		for(int i = 0; i < _chunks.Count; i++)
		{
			WorldChunk chunk = _chunks[i];
			chunk.Deinit();
			Destroy(chunk.gameObject);
		}

		_chunks.Clear();

		_loopTween?.Kill();
		_loopTween = null;

		_chunksContainer.transform.localPosition = Vector3.zero;
		_chunkIndex = 0;
		DistanceTravelled = 0f;

		// Create Start Chunk
		CreateChunk();

		// Create Right Chunk
		CreateChunk();
	}

	#endregion

	#region Private Methods

	private void CreateChunk()
	{
		WorldChunk prefab = _chunkIndex == 0 ? _startChunkPrefab : GetChunk();

		WorldChunk chunk = Instantiate(prefab, _chunksContainer);
		chunk.transform.localPosition = new Vector3(ChunkSize * _chunkIndex, 0f, 0f);
		chunk.name = chunk.name + "[" + _chunkIndex + "]";
		_chunkIndex++;

		_chunks.Add(chunk);

		chunk.Init(Game);

		if(_chunks.Count > 4)
		{
			WorldChunk tail = _chunks[0];
			tail.Deinit();
			_chunks.RemoveAt(0);
			Destroy(tail.gameObject);
		}
	}

	private WorldChunk GetChunk()
	{
		List<ChunkData> availableChunks = new List<ChunkData>();
		for(int i = 0; i < _chunkData.Length; i++)
		{
			ChunkData currentData = _chunkData[i];
			if(_chunkIndex >= currentData.ChunkRange.x && _chunkIndex <= currentData.ChunkRange.y)
			{
				availableChunks.Add(currentData);
			}
		}

		ChunkData data = availableChunks[Random.Range(0, availableChunks.Count)];
		return data.Chunks[Random.Range(0, data.Chunks.Length)];
	}

	#endregion

	#region Nested

	[System.Serializable]
	private class ChunkData
	{
		public Vector2Int ChunkRange;
		public WorldChunk[] Chunks;
	}

	#endregion
}
