﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/TenkokuDepth" {

Properties {
   _MainTex ("", 2D) = "white" {}
}


SubShader
{
ZTest Always Cull Off ZWrite Off Fog { Mode Off }
Pass
{
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
	uniform float4 _MainTex_TexelSize;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv_depth : TEXCOORD1;
	};
	
	v2f vert (appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.uv_depth = v.texcoord.xy;
			
		return o;
	}


	half4 frag (v2f i) : SV_Target {

		float4 origColor;
		half4 retValue;

		//get textures
		origColor = tex2D(_MainTex, i.uv);
		retValue.rgb = origColor.rgb;

		//DX11
		#if SHADER_API_D3D11
			float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth);
			float dpth = LinearEyeDepth(rawDepth);
			retValue.a = dpth/500;
		#else
			//DX9
			float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth);
			float dpth = Linear01Depth(rawDepth);
			retValue.a = dpth;
		#endif
		
//retValue.rgb = retValue.a;
retValue.a = 0.0 + max(retValue.r,max(retValue.g,retValue.b));
//if (retValue.a >= 0.01){
//	retValue.a = 1.0;
//}

		return retValue;
	}

	ENDCG
}
}

Fallback off

}



