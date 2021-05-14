

Shader "NAR/SolidColor" {
    Properties {
        _Color ("Main Color", COLOR) = (1,1,1,1)
    }
    SubShader {
        Pass {
            ZWrite Off
            ZTest Always
            Color [_Color]
        }
    }
}