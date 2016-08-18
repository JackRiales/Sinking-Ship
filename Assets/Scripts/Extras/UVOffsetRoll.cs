﻿using UnityEngine;
using System.Collections;

public class UVOffsetRoll : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
	public string textureName = "_MainTex";
	private Vector2 uvOffset = Vector2.zero;

	protected void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}
}