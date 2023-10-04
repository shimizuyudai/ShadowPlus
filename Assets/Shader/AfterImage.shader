Shader "Custom/AfterImage"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Saturation("Saturation", Range(0,1)) = 0.5
		_Brightness("Brightness", Range(0,1)) = 0.5
		_MinBrightnessThreshold("MinBrightnessThreshold", Range(0,1)) = 0.5
		_MaxBrightnessThreshold("_MaxBrightnessThreshold", Range(0,1)) = 0.5
		_ShadowBrightnessThreshold("_ShadowBrightnessThreshold", Range(0,1)) = 0.5
			_Threshold("_Threshold", Range(0,1)) = 0.05
	}
	SubShader
	{
			Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
			LOD 100
			Blend OneMinusDstColor One
			//Blend SrcAlpha OneMinusSrcAlpha

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
			float _Saturation;
			float _Brightness;
			float _MinBrightnessThreshold;
			float _MaxBrightnessThreshold;
			sampler2D _ShadowTex;
			float _ShadowBrightnessThreshold;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float map(float currentVal, float tempAStart, float tempAGoal, float tempBStart, float tempBGoal)
			{

				return ((currentVal - tempAStart) / (tempAGoal - tempAStart)) * (tempBGoal - tempBStart) + tempBStart;

			}

			float3 HUEtoRGB(float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}

			float3 HSVtoRGB(float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);
				return ((RGB - 1) * HSV.y + 1) * HSV.z;
			}
			float _Threshold;
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				
				fixed4 shadowCol = tex2D(_ShadowTex, i.uv);
				float shadowBrightness = (shadowCol.r + shadowCol.g + shadowCol.b) / 3;
				fixed4 result = float4(0, 0, 0, 0);//tex2D(_NoiseTex, i.uv);
				if (shadowBrightness < _ShadowBrightnessThreshold) {
					fixed4 col = tex2D(_MainTex, i.uv);
					float brightness = (col.r + col.g + col.b) / 3;
					if (brightness > _MinBrightnessThreshold && brightness < _MaxBrightnessThreshold) {
						float hue = map(brightness, _MinBrightnessThreshold, _MaxBrightnessThreshold, 0, 1);
						float3 rgb = HSVtoRGB(float3(hue, _Saturation, _Brightness));
						rgb *= brightness;
						if ((rgb.r + rgb.g + rgb.b)/3 > _Threshold) {
							result = float4(rgb.r,rgb.g,rgb.b,brightness);//. = float4((result.rgb*(1 - brightness) + rgb*brightness),1);
						}
					}
				}
				
				
				return result;
			}
			ENDCG
		}
	}
}
