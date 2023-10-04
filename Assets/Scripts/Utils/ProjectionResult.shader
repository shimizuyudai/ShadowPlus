Shader "Custom/ProjectionResult"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Threshold("_Threshold", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 100
		//Blend OneMinusDstColor One
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _ShadowTex;
			sampler2D _RearTex;

			float4 _MainTex_ST;
			float _Threshold;

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
				fixed4 result = fixed4(0,0,0,1);
				fixed4 frontCol = tex2D(_MainTex, i.uv);
				fixed4 rearCol = tex2D(_RearTex, float2(1-i.uv.x,i.uv.y));
				result.rgb = rearCol.rgb*rearCol.a + frontCol.rgb*(1 - rearCol.a);
				return result;
			}
			ENDCG
		}
	}
}
