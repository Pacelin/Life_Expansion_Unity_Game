#ifndef CELESTIALS_CUSTOM
#define CELESTIALS_CUSTOM

#ifndef SHADERGRAPH_PREVIEW
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#endif

void raySphereIntersect_float(float3 r0, float3 rd, float3 s0, float sr, out float d)
{
    float a = dot(rd, rd);
    float3 s0_r0 = r0 - s0;
    float b = 2.0 * dot(rd, s0_r0);
    float c = dot(s0_r0, s0_r0) - (sr * sr);
    if (b*b - 4.0*a*c < 0.0)
        d = -1.0;
	else
		d = (-b - sqrt((b*b) - 4.0*a*c)) / (2.0*a);
}

void DepthToWorldPosition_float(float2 uv, float depth, out float3 positionWS)
{
	positionWS = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
}
#endif