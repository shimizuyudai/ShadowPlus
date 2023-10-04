// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/HeapSnowImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _ShadowTex;
			float _Threshold;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 shadowCol = tex2D(_ShadowTex,i.uv);
				float shadowBrightness = (shadowCol.r + shadowCol.g + shadowCol.b)/3;
				fixed4 result = fixed4(0,0,0,0);
				if (col.g > 0.5) {
					if (col.r < 0.5 && col.g > 0.5) {
						result = fixed4(1, 1, 1, 1);
					}
				}

				if (shadowBrightness > 0.25) {
					result = fixed4(1, 1, 1, 1);
				}
				return result;
			}
			ENDCG
		}
	}
}
