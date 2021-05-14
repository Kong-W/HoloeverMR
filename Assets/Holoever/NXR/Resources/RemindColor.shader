

Shader "NAR/RemindColor" {
	Properties{
		_Color("Main Color", COLOR) = (1,1,1,1)
	}
		SubShader{
		Tags
	{
		"Queue" = "Transparent"
		"RenderType" = "Transparent"
	}

		Pass{
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Color[_Color]
	}
	}
}