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
