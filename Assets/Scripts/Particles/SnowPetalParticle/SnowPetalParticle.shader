Shader "Custom/SnowPetalParticle"{
	SubShader{
		// アルファを使う
		ZWrite On
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		Blend OneMinusDstColor One
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		CGPROGRAM

		// シェーダーモデルは5.0を指定
	#pragma target 5.0

		// シェーダー関数を設定 
	#pragma vertex vert
	#pragma geometry geom
	#pragma fragment frag

	#include "UnityCG.cginc"

	// テクスチャ
	sampler2D _MainTex;
	sampler2D _PetalTex;
	float4 _Color;

	struct Particle
	{
		float3 rotation;
		float3 defaultRotation;
		float3 startPos;
		float3 pos;
		float3 velocity;
		float size;
		int textureId;
		float rotationSpeed;
	};

	StructuredBuffer<Particle> Particles;

	struct VSOut {
		float4 pos : SV_POSITION;
		float2 tex : TEXCOORD0;
		float size : float;
		int textureId : int;
		float3 rotation : float3;
	};


	// 頂点シェーダ
	VSOut vert(uint id : SV_VertexID)
	{
		// idを元に、弾の情報を取得
		VSOut output;
		output.pos = float4(Particles[id].pos, 1);
		output.tex = float2(0, 0);
		output.textureId = Particles[id].textureId;
		output.rotation = Particles[id].rotation;
		output.size = Particles[id].size;
		return output;
	}

	// ジオメトリシェーダ
	[maxvertexcount(4)]
	void geom(point VSOut input[1], inout TriangleStream<VSOut> outStream)
	{
		VSOut output;

		// 全ての頂点で共通の値を計算しておく
		float4 pos = input[0].pos;
		float size = input[0].size;
		int textureId = input[0].textureId;
		float3 rotation = input[0].rotation;
		// 四角形になるように頂点を生産
		for (int x = 0; x < 2; x++)
		{
			for (int y = 0; y < 2; y++)
			{
				// ビルボード用の行列
				float4x4 billboardMatrix = UNITY_MATRIX_V;
				billboardMatrix._m03 =
					billboardMatrix._m13 =
					billboardMatrix._m23 =
					billboardMatrix._m33 = 0;

				billboardMatrix._m00 = cos(radians(rotation.z));
				billboardMatrix._m01 = -sin(radians(rotation.z));
				billboardMatrix._m10 = sin(radians(rotation.z));
				billboardMatrix._m11 = cos(radians(rotation.z));

				// テクスチャ座標
				float2 tex = float2(x, y);
				output.tex = tex;

				// 頂点位置を計算
				output.pos = pos + mul(float4((tex - float2(0.5,0.5)) * size, 0, 1), billboardMatrix);
				output.pos = mul(UNITY_MATRIX_VP, output.pos);
				output.size = size;
				output.rotation = rotation;
				output.textureId = textureId;
				// ストリームに頂点を追加
				outStream.Append(output);
			}
		}

		// トライアングルストリップを終了
		outStream.RestartStrip();
	}

	float4 _SubColor;
	// ピクセルシェーダー
	fixed4 frag(VSOut i) : COLOR
	{
		float4 col = fixed4(0,0,0,0);
		if (i.textureId == 0) {
			col = tex2D(_MainTex, i.tex) *_Color;
		}
		else {
			col = tex2D(_PetalTex, i.tex) *_SubColor;
		}

		//// アルファが一定値以下なら中断
		if (col.a < 0.25) discard;

		// 色を返す
		return col;
	}

	ENDCG
	}
	}
}