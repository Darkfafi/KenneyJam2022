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
	private Transform _chunkPrefab;

	#endregion

	#region Variables

	private int _chunkIndex = 1;

	#endregion

	protected void Awake()
	{
		// First Right Chunk
		CreateChunk();

		_speed = Mathf.Max(1f, _speed);

		_chunksContainer
			.TweenMoveX(-_chunkWidth, _chunkWidth / _speed)
			.SetEndIsDelta()
			.SetDynamicSetupStep(RaTweenDynamicSetupStep.Start)
			.SetInfiniteLooping()
			.OnLoop((loopCount) => CreateChunk())
			.OnSetup(() => CreateChunk());
	}

	private void CreateChunk()
	{
		Transform chunk = Instantiate(_chunkPrefab, _chunksContainer);
		chunk.localPosition = new Vector3(_chunkWidth * _chunkIndex, 0f, 0f);
		chunk.name = chunk.name + "[" + _chunkIndex + "]";
		_chunkIndex++;
	}
}
