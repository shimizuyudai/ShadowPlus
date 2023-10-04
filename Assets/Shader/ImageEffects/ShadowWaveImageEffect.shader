// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ShadowWaveImageEffect"
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
			sampler2D _BlurredShadowTex;
			float _Amplitude;
			float _Frequency;
			float _ShadowBrightnessThreshold;
			float _ElapsedTime;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 shadowColor = tex2D(_BlurredShadowTex, i.uv);
				float shadowBrightness = (shadowColor.r + shadowColor.g + shadowColor.b) / 3;
				float v = i.uv.y;
				//fixed4 temp = tex2D(_MainTex, float2(i.uv.x, v));
				float darkness = 0;
				if (shadowBrightness > _ShadowBrightnessThreshold) {
					v = v + (sin(i.uv.x*_Frequency + _ElapsedTime)*_Amplitude*pow(shadowBrightness, 2));
					//float brightness = (temp.r + temp.g + temp.b) / 3;
					/*if (brightness < 0.75) {
						darkness = pow(shadowBrightness, 5);
					}*/
					
					if (v > 1) {
						v -= 1;
					}
					else if (v < 0) {
						v += 1;
					}
				}
				float4 col = tex2D(_MainTex, float2(i.uv.x, v));

				/*if (shadowBrightness > _ShadowBrightnessThreshold) {
					float brightness = (col.r + col.g + col.b) / 3;
					if (brightness < 0.75) {
					darkness = pow(shadowBrightness, 5);
						col.rgb -= float3(darkness, darkness, darkness);
						col = saturate(col);
					}
				}*/
				
				return col;
			}
			ENDCG
		}
	}
}
