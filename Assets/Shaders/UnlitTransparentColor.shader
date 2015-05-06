Shader "Unlit/Transparent Color"
{
	Properties 
	{
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	}

	Category 
	{
		Tags 
		{ 
			Queue = Transparent 
		}
		Lighting Off
		Cull Back
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}	
				
		SubShader 
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					Combine texture * primary
				}
			}
		}
	}
}