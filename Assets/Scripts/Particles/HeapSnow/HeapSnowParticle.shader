Shader "Custom/HeapSnowParticle"{
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

	struct Particle
	{
		float3 startPos;
		float3 pos;
		float3 velocity;
		float3 startVelocity;
		float4 color;
		float size;
	};

	StructuredBuffer<Particle> Particles;
	float _Size;
	struct VSOut {
		float4 pos : SV_POSITION;
		float2 tex : TEXCOORD0;
		float4 col : Color;
		int textureId : int;
		float size : float;
	};
	

	// 頂点シェーダ
	VSOut vert(uint id : SV_VertexID)
	{
		// idを元に、弾の情報を取得
		VSOut output;
		output.col = Particles[id].color;
		output.pos = float4(Particles[id].pos, 1);
		output.tex = float2(0, 0);
		output.size = Particles[id].size;
		return output;
	}
	float _Angle;

	// ジオメトリシェーダ
	[maxvertexcount(4)]
	void geom(point VSOut input[1], inout TriangleStream<VSOut> outStream)
	{
		VSOut output;

		// 全ての頂点で共通の値を計算しておく
		float4 pos = input[0].pos;
		float size = input[0].size;
		int textureId = input[0].textureId;
		float4 col = input[0].col;
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

				/*billboardMatrix._m00 = cos(radians(_Angle));
				billboardMatrix._m01 = -sin(radians(_Angle));
				billboardMatrix._m10 = sin(radians(_Angle));
				billboardMatrix._m11 = cos(radians(_Angle));*/

				// テクスチャ座標
				float2 tex = float2(x, y);
				output.tex = tex;

				// 頂点位置を計算
				output.pos = pos + mul(float4((tex - float2(0.5,0.5)) * size*_Size, 0, 1), billboardMatrix);
				output.pos = mul(UNITY_MATRIX_VP, output.pos);
				output.col = col;
				output.textureId = textureId;
				output.size = size;
				// ストリームに頂点を追加
				outStream.Append(output);
			}
		}

		// トライアングルストリップを終了
		outStream.RestartStrip();
	}
	// ピクセルシェーダー
	fixed4 frag(VSOut i) : COLOR
	{
		fixed4 col = tex2D(_MainTex, i.tex) *i.col;
		//// アルファが一定値以下なら中断
		if (col.a < 0.1) discard;
		// 色を返す
		return col;
	}

	ENDCG
	}
	}
}