Shader "Custom/Terrain" {
	// set the colour of our surface at that point
	// blend different textures based on the height of the surface
	Properties{
		testTexture("Texture",2D) = " white"{}
		testScale("Scale",Float)=1
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types/; 
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		const static int maxColorCount = 8; // the maximum color that we can have 
		const static float epsilon = 1E-4; // greek letter


		int baseColorCount; // the numbe of  color that we have set up
		float3 baseColors[maxColorCount];
		float baseStartHeights[maxColorCount];
		float baseBlends[maxColorCount];

		float minHeight;
		float maxHeight;

		sampler2D testTexture;
		float testScale;

		struct Input {
			float3 worldPos;
			float3 worldNormal; //  the surface that light up
		};

		float inverseLerp(float a, float b , float value) { // a = min, b= max, value = current
			return saturate((value - a) / (b - a)); // saturate means to clamp it in the range 0-1
		}
		// set the color of the surface at that point
		void surf(Input IN, inout SurfaceOutputStandard o) {
			// the percent of terrain's world position height between min and max
			float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);

			for (int i = 0; i < baseColorCount; i++) {
				//drawstrength is for distributing the color of terrain based on the height 
				//blends color( 0 to 1) 結合 by adding interpolating point 插入點
			 float drawStrength = inverseLerp(-baseBlends[i]/2-epsilon, baseBlends[i] / 2, heightPercent - baseStartHeights[i]);
				//float drawStrength = saturate(sign(heightPercent - baseStartHeights[i]));
				o.Albedo = o.Albedo *(1 - drawStrength) + baseColors[i] * drawStrength;
			}
			float3 scaledWorldPos = IN.worldPos / testScale;
			float3 blendAxes = abs(IN.worldNormal); // no matter it is negative or positive 
			float3 xProjection = tex2D(testTexture, IN.worldPos.yz)*blendAxes.x;
			float3 yProjection = tex2D(testTexture, IN.worldPos.xz)*blendAxes.y;
			float3 zProjection = tex2D(testTexture, IN.worldPos.xy)*blendAxes.z;
			o.Albedo = xProjection + yProjection + zProjection;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
