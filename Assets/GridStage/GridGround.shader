// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/GridGround" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_LineColor ("Line Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _LineColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

		half getLattice(float3 worldPos, float scale, float lineWidth)
		{
			float3 wp = worldPos * scale;
			float halfW = lineWidth * 0.5 * scale;
			half x = cos(clamp(frac(wp.x + 0.5) - 0.5, -halfW, halfW) * 1.57079633 / halfW);
			half y = cos(clamp(frac(wp.y + 0.5) - 0.5, -halfW, halfW) * 1.57079633 / halfW);
			half z = cos(clamp(frac(wp.z + 0.5) - 0.5, -halfW, halfW) * 1.57079633 / halfW);
			return max(x * max(y, z), max(y * max(x, z), z * max(x, y)));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			half thin = getLattice(IN.worldPos.xyz, 1.0, 0.03);
			half thick = getLattice(IN.worldPos.xyz, 0.1, 0.03);
			o.Albedo = lerp(tex.rgb * _Color.rgb, _LineColor.rgb, max(thin, thick));
			o.Emission = fixed3(0, 0, 0);
			//o.Emission = lerp(fixed3(0, 0, 0), _Color.rgb, max(thin, thick));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = tex.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
