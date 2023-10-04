Shader "Hidden/PerlinNoise"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ElapsedTimeRate("ElapsedTimeRate",Range(0,1)) = 0
		_Rate("Rate",Vector) = (0,0,0,0)

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

			fixed2 random2(fixed2 st){
            	st = fixed2( dot(st,fixed2(127.1,311.7)),
            	               dot(st,fixed2(269.5,183.3)) );
            	return -1.0 + 2.0*frac(sin(st)*43758.5453123);
      		  }

			float rand(float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
			}

			


        float perlinNoise(fixed2 st) 
        {
            fixed2 p = floor(st);
            fixed2 f = frac(st);
            fixed2 u = f*f*(3.0-2.0*f);

            float v00 = random2(p+fixed2(0,0));
            float v10 = random2(p+fixed2(1,0));
            float v01 = random2(p+fixed2(0,1));
            float v11 = random2(p+fixed2(1,1));

            return lerp( lerp( dot( v00, f - fixed2(0,0) ), dot( v10, f - fixed2(1,0) ), u.x ),
                         lerp( dot( v01, f - fixed2(0,1) ), dot( v11, f - fixed2(1,1) ), u.x ), 
                         u.y)+0.5f;
        }

		float fBm(fixed2 st)
		{
			float f = 0;
			fixed2 q = st;

			f += 0.5000*perlinNoise(q); q = q*2.01;
			f += 0.2500*perlinNoise(q); q = q*2.02;
			f += 0.1250*perlinNoise(q); q = q*2.03;
			f += 0.0625*perlinNoise(q); q = q*2.01;

			return f;
		}


		

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
			float _ElapsedTime;
			float2 _Rate;
			fixed4 frag (v2f i) : SV_Target
			{
				float brightness = fBm(float2(i.uv.x*_Rate.x + _ElapsedTime, i.uv.y*_Rate.y + _ElapsedTime));
				float4 col = float4(brightness, brightness, brightness, 1);
				return col;
			}
			ENDCG
		}
	}
}
