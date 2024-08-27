#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

/*#if defined(SHADERGRAPH_PREVIEW)
#else
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma multi_compile _ SHADOWS_SHADOWMASK
#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
#pragma multi_compile _ LIGHTMAP_ON
#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
#endif*/


void MainLight_float(float3 posWS, out float3 direction, out float3 color, out float distanceAtten, out float shadowAtten)
{
    #if SHADERGRAPH_PREVIEW
    direction = normalize(float3(0.5, 0.5, 0));
    color = 1;
    distanceAtten = 1;
    shadowAtten = 1;
    #else
    #if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(WorldPos);
    float4 shadowCoord = ComputeScreenPos(clipPos);
    #else
    const float4 shadowCoord = TransformWorldToShadowCoord(posWS);
    #endif
    const Light mainLight = GetMainLight(shadowCoord);
    direction = mainLight.direction;
    color = mainLight.color;
    distanceAtten = mainLight.distanceAttenuation;

    #if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
    shadowAtten = 1.0h;
    #else
    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
    float4 shadowStrength = GetMainLightShadowParams();
    shadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, shadowStrength, false); 
    #endif
    #endif
}

void DirectSpecular_float(float smoothness, float3 direction, float3 normalWS, float3 viewWS, out float3 specular)
{
    const float4 white = 1;

#if defined(SHADERGRAPH_PREVIEW)
    specular = 0;
#else
    smoothness = exp2(10 * smoothness + 1);
    normalWS = normalize(normalWS);
    viewWS = SafeNormalize(viewWS);
    specular = LightingSpecular(white, direction, normalWS, viewWS, white, smoothness);
#endif
}

void AdditionalLights_float(float smoothness, float3 positionWS, float3 normalWS, float3 viewWS, out float3 diffuse, out float3 specular, out float shadowAtten)
{
    const float4 white = 1;
    
    float3 diffuseColor = 0;
    float3 specularColor = 0;
    float shadowAttenuation = 0;

#if !defined(SHADERGRAPH_PREVIEW)
    smoothness = exp2(10 * smoothness + 1);
    normalWS = normalize(normalWS);
    viewWS = SafeNormalize(viewWS);
    const int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        const Light light = GetAdditionalLight(i, positionWS, 1);
        
        const half3 attenuatedLightColor = light.color;
        const float additionalShadowAtten = light.distanceAttenuation * light.shadowAttenuation;
        
        diffuseColor += LightingLambert(attenuatedLightColor * additionalShadowAtten, light.direction, normalWS);
        specularColor += LightingSpecular(attenuatedLightColor, light.direction, normalWS, viewWS, white, smoothness);
        shadowAttenuation += additionalShadowAtten;
    }
#endif

    diffuse = diffuseColor;
    specular = specularColor;
    shadowAtten = shadowAttenuation;
}
#endif