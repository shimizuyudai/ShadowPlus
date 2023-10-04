// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/SnowImageEffect"
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
			float4 _Color;
			float _Rate;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 result = fixed4(0,0,0,0);
				fixed4 col = tex2D(_MainTex, i.uv);
				float brightness = (col.r + col.g + col.b + col.a)/4;
				if (brightness > 0.1) {
					brightness = saturate(brightness*_Rate);
					col = _Color*brightness;
				}
				
				//col.a = _Color.a;
				return col;
			}
			ENDCG
		}
	}
}
