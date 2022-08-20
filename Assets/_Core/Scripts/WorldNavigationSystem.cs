using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaTweening;

public class WorldNavigationSystem : MonoBehaviour
{
	#region Editor Variables

	[Header("Movement Settings")]
	[SerializeField]
	private float _speed = 5f;

	[Header("Chunk Settings")]
	[SerializeField]
	private Transform _chunksContainer = null;

	[SerializeField]
	private int _chunkWidth = 14;

	[SerializeField]
	private Transform _startChunkPrefab = null;

	[SerializeField]
	private Transform _chunkPrefab = null;

	#endregion

	#region Variables

	private bool _initialized = false;
	private int _chunkIndex = 0;
	private RaTween _loopTween = null;
	private List<Transform> _chunks = new List<Transform>();
	private float _duration;

	#endregion

	#region Properties

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

	public void Initialize()
	{
		if(_initialized)
		{
			return;
		}

		// Mark Start Base Chunk As Created
		ResetLoop();

		// Stabalize Variables
		_speed = Mathf.Max(1f, _speed);
		_duration = (_chunkWidth / _speed);

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
				.TweenMoveX(-_chunkWidth, _duration)
				.SetLocalPosition()
				.SetEndIsDelta()
				.SetDynamicSetupStep(RaTweenDynamicSetupStep.Start)
				.SetInfiniteLooping()
				.OnSetup(()=> CreateChunk())
				.OnUpdate(() => { DistanceTravelled += Time.deltaTime * _duration;})
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
			Destroy(_chunks[i].gameObject);
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
		Transform prefab = _chunkIndex == 0 ? _startChunkPrefab : _chunkPrefab;

		Transform chunk = Instantiate(prefab, _chunksContainer);
		chunk.localPosition = new Vector3(_chunkWidth * _chunkIndex, 0f, 0f);
		chunk.name = chunk.name + "[" + _chunkIndex + "]";
		_chunkIndex++;

		_chunks.Add(chunk);

		if(_chunks.Count > 4)
		{
			Transform tail = _chunks[0];
			_chunks.RemoveAt(0);
			Destroy(tail.gameObject);
		}
	}

	#endregion
}
