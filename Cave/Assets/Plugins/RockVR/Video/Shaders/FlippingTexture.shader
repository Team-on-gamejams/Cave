﻿Shader "RockVR/FlippingTexture" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess("Shininess", Range(0.01, 1)) = 0.078125
        _ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
        _MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
    _Cube("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
    }
        SubShader{
        LOD 300
        Tags{ "RenderType" = "Opaque" }

        CGPROGRAM
#pragma surface surf BlinnPhong

        sampler2D _MainTex;
    samplerCUBE _Cube;

    fixed4 _Color;
    fixed4 _ReflectColor;
    half _Shininess;

    uniform float4x4 _Rotation;

    struct Input {
        float2 uv_MainTex;
        float3 worldRefl;
    };

    void surf(Input IN, inout SurfaceOutput o) {
        fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
        fixed4 c = tex * _Color;
        o.Albedo = c.rgb;
        o.Gloss = tex.a;
        o.Specular = _Shininess;

        fixed4 reflcol = texCUBE(_Cube, mul(_Rotation, float4(IN.worldRefl,0)));
        reflcol *= tex.a;
        o.Emission = reflcol.rgb * _ReflectColor.rgb;
        o.Alpha = reflcol.a * _ReflectColor.a;
    }
    ENDCG
    }

        FallBack "Reflective/VertexLit"
}
