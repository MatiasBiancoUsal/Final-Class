Shader "Custom/InsideSphere"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Front    
        ZWrite On
        Lighting Off
        Pass
        {
            Color [_Color]
        }
    }
}
