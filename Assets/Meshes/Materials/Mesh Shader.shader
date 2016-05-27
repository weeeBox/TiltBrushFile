Shader "Custom/Vertex Color" {
	Properties {
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Off
		Lighting Off
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = IN.color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
