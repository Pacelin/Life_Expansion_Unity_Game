using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    public readonly partial struct noise3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float3 GetProjectedGradient(in float3 sphereNormal)
        {
            float3 grad = -gradient;
            float3 projectedGradient = grad - math.dot(grad, sphereNormal) * sphereNormal;
            return projectedGradient;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float3 GetNormal(in float3 sphereNormal, in float k)
        {
            float3 projectedGradient = GetProjectedGradient(sphereNormal);
            return math.normalize(sphereNormal + k * projectedGradient);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator noise3(in float4 a)
        {
            return new noise3(a.x, a.yzw);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator +(in noise3 a)
        {
            return new noise3
            (
                a.value,
                a.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator +(in noise3 a, in float b)
        {
            return new noise3
            (
                a.value + b,
                a.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator +(in float a, in noise3 b)
        {
            return new noise3
            (
                a + b.value,
                b.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator +(in noise3 a, in noise3 b)
        {
            return new noise3
            (
                a.value + b.value,
                a.gradient + b.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator -(in noise3 a)
        {
            return new noise3
            (
                -a.value,
                -a.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator -(in noise3 a, in float b)
        {
            return new noise3
            (
                a.value - b,
                a.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator -(in float a, in noise3 b)
        {
            return new noise3
            (
                a - b.value,
                -b.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator -(in noise3 a, in noise3 b)
        {
            return new noise3
            (
                a.value - b.value,
                a.gradient - b.gradient
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator *(in noise3 a, in float b)
        {
            return new noise3
            (
                a.value * b,
                a.gradient * b
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator *(in float a, in noise3 b)
        {
            return new noise3
            (
                a * b.value,
                a * b.gradient
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator *(in noise3 a, in noise3 b)
        {
            return new noise3
            (
                a.value * b.value,
                a.value * b.gradient + a.gradient * b.value
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator /(in noise3 a, in float b)
        {
            return new noise3
            (
                a.value / b,
                a.gradient / b
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator /(in float a, in noise3 b)
        {
            return new noise3
            (
                a / b.value,
                -a * b.gradient / (b.value * b.value)
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 operator /(in noise3 a, in noise3 b)
        {
            return new noise3
            (
                a.value / b.value,
                (a.gradient * b.value - b.gradient * a.value) / (b.value * b.value)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static noise3 pow(in noise3 a, in float b)
        {
            return new noise3
            (
                math.pow(a.value, b),
                b * a.gradient * math.pow(a.value, b - 1f)
            );
        }
    }
}