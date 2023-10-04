 Shader "Custom/Rear"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ShadowStencilTex("ShadowStencilTexture", 2D) = "white" {}
		_ShadowMaskTex("ShadowMaskTexture", 2D) = "white" {}
		_CurtainTex("CurtainTexture", 2D) = "white" {}
		_ShadowColor("ShadowColor", Color) = (0,0,0,0)
		_Visible("Visible",Int) = 0
		_ShadowBrightnessThreshold("ShadowBrightnessThreshold", Range(0,1)) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile _ ISUSEMASK
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _ShadowStencilTex;
			sampler2D _ShadowMaskTex;
			sampler2D _CurtainTex;
			float4 _ShadowColor;
			float _ShadowBrightnessThreshold;

			float _a;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainColor = tex2D(_MainTex, i.uv);
				fixed4 curtainColor = tex2D(_CurtainTex, float2(1-i.uv.x,i.uv.y));
				fixed4 resultColor = fixed4(0,0,0,0);

				//カーテンのかかった場所
				if (curtainColor.a > 0) {
					fixed4 shadowStencilColor = tex2D(_ShadowStencilTex, i.uv);
					float shadowStencilBrightness = (shadowStencilColor.r + shadowStencilColor.g + shadowStencilColor.b) / 3;
					if (shadowStencilBrightness < _ShadowBrightnessThreshold) {
						shadowStencilBrightness = 0;
					}
					if (shadowStencilBrightness > 0) {
						resultColor = _ShadowColor;
						resultColor.a = shadowStencilBrightness;
					}
				}
				else {
				#ifdef ISUSEMASK
					fixed4 shadowMaskColor = tex2D(_ShadowMaskTex, i.uv);
					float shadowMaskBrightness = (shadowMaskColor.r + shadowMaskColor.g + shadowMaskColor.b) / 3;
					if (shadowMaskBrightness < _ShadowBrightnessThreshold) {
						shadowMaskBrightness = 0;
					}
					if (shadowMaskBrightness > 0) {
						resultColor = mainColor;
						resultColor.a = shadowMaskBrightness;
					}
				#else
					resultColor = mainColor;
				#endif
				}
				

				return resultColor;
			}
			ENDCG
		}
	}
}
