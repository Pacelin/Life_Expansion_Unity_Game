using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniOwl.Celestials
{
    public static class PlanetMeshBuilder
    {
        private static readonly VertexAttributeDescriptor[] vertexAttributes =
        {
            new(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new(VertexAttribute.Normal, VertexAttributeFormat.Float16, 4),
            new(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2),
        };

        public static void BuildMeshes(PlanetSettings settings, PlanetHeightData heightData)
        {
            PlanetCreator.ReportStage("Build Meshes");

            Planet planet = settings.Planet;
            
            int resolution = settings.Model.resolution;
            
            int vertexCount = (resolution + 1) * (resolution + 1);
            int indexCount = 6 * resolution * resolution;

            float diameter = 2f * (settings.Physical.radius + settings.Physical.amplitude);
            var bounds = new Bounds(Vector3.zero, Vector3.one * diameter);

            Mesh.MeshDataArray quadMeshes = Mesh.AllocateWritableMeshData(6);
            
            for (int face = 0; face < 6; face++)
            {
                PlanetCreator.ReportDescription($"Face {face + 1} of 6");

                Mesh.MeshData mesh = quadMeshes[face];
                mesh.SetVertexBufferParams(vertexCount, vertexAttributes);
                mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
                
                QuadBuildData quad = new()
                {
                    vertices = mesh.GetVertexData<Vertex>(),
                    indices = mesh.GetIndexData<ushort>(),
                };

                BuildFace(settings, face, quad, heightData.heights[face], heightData.normals[face]);

                var descriptor = new SubMeshDescriptor()
                {
                    bounds = bounds,
                    baseVertex = 0,
                    firstVertex = 0,
                    indexCount = indexCount,
                    indexStart = 0,
                    topology = MeshTopology.Triangles,
                    vertexCount = vertexCount,
                };
                mesh.subMeshCount = 1;
                mesh.SetSubMesh(0, descriptor, settings.Model.updateFlags);

                planet.SurfaceSharedMeshes[face].bounds = bounds;
                planet.SurfaceSharedMeshes[face].UploadMeshData(true);
                planet.SurfaceSharedMeshes[face].name = $"SM_{planet.name}_{face}";
            }
            
            PlanetCreator.ReportStage("Finalize Meshes");
            
            Mesh.ApplyAndDisposeWritableMeshData(quadMeshes, planet.SurfaceSharedMeshes, settings.Model.updateFlags);

            for (int face = 0; face < 6; face++)
            {
                PlanetCreator.ReportDescription($"Face {face + 1} of 6");

                var mesh = planet.SurfaceSharedMeshes[face];
                
                if (settings.Model.recalculateNormals)
                    mesh.RecalculateNormals();
                if (settings.Model.recalculateTangents)
                    mesh.RecalculateTangents();
                if (settings.Model.optimizeMesh)
                    mesh.Optimize();
            }
        }

        private static void BuildFace(PlanetSettings settings, int face, in QuadBuildData quad, in NativeArray<float> heights, in NativeArray<float3> normals)
        {
            int dir = face % 3;
            int ax1 = (face + 1) % 3;
            int ax2 = (face + 2) % 3;
            bool backFace = face > 2;
            
            int resolution = settings.Model.resolution;

            float3 baseVertex = float3.zero;
            baseVertex[dir] = math.select(0, resolution, backFace);
            
            var verticesJob = new BuildPlanetQuadVerticesJob()
            {
                vertices = quad.vertices,
                heights = heights,
                normals = normals,
                ax1 = ax1, ax2 = ax2,
                resolution = resolution,
                resolutionPlus1 = resolution + 1,
                baseVertex = baseVertex,
                radius = settings.Physical.radius,
                amplitude = settings.Physical.amplitude,
            };

            var indicesJob = new BuildPlanetQuadIndicesJob()
            {
                indexOffset = GetIndexOffset(backFace, resolution),
                indices = quad.indices,
                resolution = resolution,
            };

            var handle = verticesJob.ScheduleParallel(quad.vertices.Length, 0, default);
            handle = indicesJob.Schedule(handle);
            handle.Complete();
        }

        private static NativeArray<int> GetIndexOffset(bool backFace, int resolution)
        {
            if (backFace)
                return new NativeArray<int>(new int[] { resolution + 2, 1, 0, resolution + 1, resolution + 2, 0 }, Allocator.TempJob);
            return new NativeArray<int>(new int[] { 0, 1, resolution + 2, 0, resolution + 2, resolution + 1 }, Allocator.TempJob);
        }
    }
}
