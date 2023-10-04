Shader "Custom/WaveGenerator"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

//		Pass
//	{
//		CGPROGRAM
//#pragma vertex vert
//#pragma fragment frag
//		// make fog work
//#pragma multi_compile_fog
//
//#include "UnityCG.cginc"
//
//		struct appdata
//	{
//		float4 vertex : POSITION;
//		float2 uv : TEXCOORD0;
//	};
//
//	sampler2D _MainTex;
//	float4 _MainTex_ST;
//
//	struct v2f
//	{
//		float2 uv : TEXCOORD0;
//		UNITY_FOG_COORDS(1)
//			float4 vertex : SV_POSITION;
//	};
//
//	v2f vert(appdata v)
//	{
//		v2f o;
//		o.vertex = UnityObjectToClipPos(v.vertex);
//		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//		UNITY_TRANSFER_FOG(o,o.vertex);
//		return o;
//	}
//	
//	fixed4 frag(v2f i) : SV_Target
//	{
//		fixed4 col = fixed4(0, 0, 0, 1);
//		return col;
//	}
//		ENDCG
//	}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _OverlapTex;
			sampler2D _ShadowTex;
			sampler2D _ShadowOverlapTex;
			sampler2D _ShadowOverrideTex;
			float4 _LineColor;
			float _Amplitude0;
			float _Frequency0;
			float _Amplitude1;
			float _Frequency1;
			float _Phase0;
			float _Phase1;
			float _ElapsedTime;
			float _LineWidth;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 0);
				float v0 = 0.5 + sin(i.uv.x*_Frequency0 + _Phase0 + _ElapsedTime)*_Amplitude0*0.5;
				float v1 = 0.5 + sin(i.uv.x*_Frequency1 + _Phase1 + _ElapsedTime)*_Amplitude1*0.5;
				fixed4 shadowColor = tex2D(_ShadowTex, i.uv);
				float shadowBrightness = (shadowColor.r + shadowColor.g + shadowColor.b) / 3;

				if (length(float2(i.uv.x, v0) - i.uv) < _LineWidth) {
					col = _LineColor;
				}
				else if (length(float2(i.uv.x, v1) - i.uv) < _LineWidth) {
					col = _LineColor;
				}
				else if (i.uv.y < v0 && i.uv.y > v1) {
					col = tex2D(_ShadowOverlapTex, i.uv)*shadowBrightness + tex2D(_OverlapTex, i.uv)*(1-shadowBrightness);
				}
				else if (i.uv.y > v0 && i.uv.y < v1) {
					col = tex2D(_ShadowOverlapTex, i.uv)*shadowBrightness + tex2D(_OverlapTex, i.uv)*(1 - shadowBrightness);
				}
				else {
					col = tex2D(_ShadowOverrideTex, i.uv)*shadowBrightness + tex2D(_MainTex, i.uv)*(1 - shadowBrightness);
				}

			return col;
			}
			ENDCG
		}
	}
}
