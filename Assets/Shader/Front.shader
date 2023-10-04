 Shader "Custom/Front"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_CurtainTex("CurtainTexture", 2D) = "white" {}
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
			sampler2D _CurtainTex;

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
				fixed4 curtainColor = tex2D(_CurtainTex, i.uv);
				fixed4 resultColor = fixed4(0,0,0,0);
				//カーテンのかかった場所
				if (curtainColor.a > 0) {
						resultColor = curtainColor;
				}
				else {
						resultColor = mainColor;
				}
				return resultColor;
			}
			ENDCG
		}
	}
}
