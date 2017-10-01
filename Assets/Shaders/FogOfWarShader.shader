// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/FogOfWar" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_SectorAmplification("Sector amplification", Float) = 0.5
		_RangeAmplification("Range amplification", Float) = 0.5
		_Range("Range", Float) = 3.0
		_Fov("Cosine of FOV", Range(-1.0, 1.0)) = 0.5
		_FovRange("FOV Range", Float) = 10.0
		_PlayerPos("Player position", Vector) = (0,0,0,1)
		_PlayerDir("Player direction", Vector) = (0,1,0,1)
	}

	SubShader{
		Tags{ "Queue" = "Overlay+100" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
#pragma surface surf Lambert alpha:fade vertex:vert

		sampler2D _MainTex;
		fixed4 _Color;
		float _SectorAmplification;
		float _RangeAmplification;
		float _Range;
		float _Fov;
		float _FovRange;
		float4 _PlayerPos;
		float4 _PlayerDir;

		struct Input {
			float2 uv_MainTex;
			float2 location;
		};

		float visibleSectorPower(float2 nearVertex) {
			float2 dir = nearVertex.xy - _PlayerPos.xy;

			float dist = length(dir);
			float rangeVal = clamp(1.0 - dist / _FovRange, 0.0, 1.0);

			float fov = dot(normalize(_PlayerDir.xy), normalize(dir));
			float fovVal = clamp((fov - _Fov) / (1 - _Fov), 0.0, 1.0);

			float val = rangeVal * fovVal;

			return 1.0f / _SectorAmplification * val;
		}

		float visibleRangePower(float2 nearVertex) {
			float dist = length(nearVertex.xy - _PlayerPos.xy);
			float val = clamp(1.0 - dist / _Range, 0.0, 1.0);

			return 1.0f / _RangeAmplification * val;
		}

		void vert(inout appdata_full vertexData, out Input outData) {
			float4 pos = UnityObjectToClipPos(vertexData.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xy;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float alpha = (1.0 - (baseColor.a + visibleSectorPower(IN.location) + visibleRangePower(IN.location)));

			o.Albedo = baseColor.rgb;
			o.Alpha = alpha;
		}

		ENDCG
	}

	Fallback "Legacy Shaders/Transparent/VertexLit"
}
