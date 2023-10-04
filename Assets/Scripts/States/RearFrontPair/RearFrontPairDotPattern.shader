Shader "Unlit/RearFrontPairDotPattern"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ShadowTex("ShadowTexture", 2D) = "white" {}
		_ShodowThreshold("ShodowThreshold",Range(0,1)) = 0
		_UnShodowRate("UnShadowRate",Range(0,1)) = 0
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
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
			sampler2D _ShadowTex;
			float4 _MainTex_ST;
			float _ShodowThreshold;
			float _UnShodowRate;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed2 uv = fixed2(1 - i.uv.x, i.uv.y);
				fixed4 shadowCol = tex2D(_ShadowTex, uv);
				
				fixed4 col = tex2D(_MainTex, i.uv);
				float sb = (shadowCol.r + shadowCol.g + shadowCol.b) / 3.0;
				if (sb < _ShodowThreshold) {
					col.rgb *= _UnShodowRate;
				}
				return col;
			}
			ENDCG
		}
	}
}
