Shader "Custom/ProteanSnow"{
	SubShader{
		// アルファを使う
		ZWrite On
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 100
		Blend OneMinusDstColor One
		Cull Off

		Pass{
		CGPROGRAM

		// シェーダーモデルは5.0を指定
		#pragma target 5.0

		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _SubTex;
	float4 _Color;
	float3  _Angle;

	struct Particle
	{
		float3 startPos;
		float3 pos;
		float3 velocity;
		float3 angle;
		float size;
		int textureId;
	};

	StructuredBuffer<Particle> _Particles;
	float _IdOffset;
	float _Rotation;

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
	};

	struct v2f
	{
		float4 position : SV_POSITION;
		float3 normal : NORMAL;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		int textureId : int;
	};

	inline int getId(float2 uv1)
	{
		return (int)(uv1.x + 0.5) + (int)_IdOffset;
	}

	float3 rotate(float3 p, float3 rotation)
	{
		float3 a = normalize(rotation);
		float angle = length(rotation);
		if (abs(angle) < 0.001) return p;
		float s = sin(angle);
		float c = cos(angle);
		float r = 1.0 - c;
		float3x3 m = float3x3(
			a.x * a.x * r + c,
			a.y * a.x * r + a.z * s,
			a.z * a.x * r - a.y * s,
			a.x * a.y * r - a.z * s,
			a.y * a.y * r + c,
			a.z * a.y * r + a.x * s,
			a.x * a.z * r + a.y * s,
			a.y * a.z * r - a.x * s,
			a.z * a.z * r + c
			);
		return mul(m, p);
	}

	v2f vert(appdata v)
	{
		Particle p = _Particles[getId(v.uv1)];
		v.vertex.xyz = rotate(v.vertex.xyz, p.angle);
		v.vertex.xyz *= p.size;
		v.vertex.xyz += p.pos;
		v2f o;
		o.uv1 = v.uv1;
		o.uv0 = v.uv0;
		o.position = mul(UNITY_MATRIX_VP, v.vertex);
		o.normal = v.normal;
		o.textureId = p.textureId;
		return o;
	}


	// ピクセルシェーダー
	fixed4 frag(v2f i) : COLOR
	{
		float4 col = fixed4(1,0,0,1);// tex2D(_MainTex, i.uv0)*_Color;
		if (i.textureId == 0) {
			col = tex2D(_MainTex, i.uv0)*_Color;
		}
		else {
			col = tex2D(_SubTex, i.uv0)*_Color;
		}

		if (col.a < 0.3) discard;

		return col;
	}

		ENDCG
	}
	}
}

