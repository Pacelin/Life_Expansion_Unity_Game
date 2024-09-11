using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniOwl.Celestials
{
    public static class PlanetSurfaceMeshBuilder
    {
        private static readonly VertexAttributeDescriptor[] vertexAttributes =
        {
            new(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            new(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
        };

        public static void BuildMeshes(PlanetSurface surface, GameObject surfaceGO, PlanetHeightData heightData, int resolution)
        {
            var planet = (PlanetObject)surface.List;
            
            int vertexCount = (resolution + 1) * (resolution + 1);
            int indexCount = 6 * resolution * resolution;

            float diameter = 2f * (planet.Physical.radius + planet.Physical.amplitude);
            var bounds = new Bounds(Vector3.zero, Vector3.one * diameter);

            Mesh.MeshDataArray quadMeshes = Mesh.AllocateWritableMeshData(6);

            var meshFilters = surfaceGO.GetComponentsInChildren<MeshFilter>();
            var sharedMeshes = meshFilters.Select(mf => mf.sharedMesh).ToArray();

            for (int face = 0; face < 6; face++)
            {
                PlanetProgressReporter.ReportDescription($"Face {face + 1} of 6");

                Mesh.MeshData mesh = quadMeshes[face];
                mesh.SetVertexBufferParams(vertexCount, vertexAttributes);
                mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
                
                QuadBuildData quad = new()
                {
                    vertices = mesh.GetVertexData<Vertex>(),
                    indices = mesh.GetIndexData<ushort>(),
                };
                
                BuildFace(surface, face, quad, heightData.heights[face], heightData.normals[face], resolution);

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
                mesh.SetSubMesh(0, descriptor, surface.Model.updateFlags);
            }
            
            PlanetProgressReporter.ReportStage("Finalize Meshes");
            
            Mesh.ApplyAndDisposeWritableMeshData(quadMeshes, sharedMeshes, surface.Model.updateFlags);

            for (int face = 0; face < 6; face++)
            {
                PlanetProgressReporter.ReportDescription($"Face {face + 1} of 6");

                var mesh = sharedMeshes[face];
                
                if (surface.Model.recalculateNormals)
                    mesh.RecalculateNormals();
                if (surface.Model.recalculateTangents)
                    mesh.RecalculateTangents();
                if (surface.Model.optimizeMesh)
                    mesh.Optimize();
                
                mesh.bounds = bounds;
                mesh.name = $"SM_{planet.name}_{face}";
                mesh.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                mesh.UploadMeshData(false);
            }
        }

        private static void BuildFace(PlanetSurface surface, int face, in QuadBuildData quad, in NativeArray<float> heights, in NativeArray<float3> normals, int resolution)
        {
            var planet = (PlanetObject)surface.List;
            
            int dir = face % 3;
            int ax1 = (face + 1) % 3;
            int ax2 = (face + 2) % 3;
            bool backFace = face > 2;
            
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
                radius = planet.Physical.radius,
                amplitude = planet.Physical.amplitude,
            };

            var indicesJob = new BuildPlanetQuadIndicesJobUInt16()
            {
                indexOffset = GetIndexOffset(backFace, resolution),
                indices = quad.indices,
                resolution = resolution,
                resolutionPlus1 = resolution + 1,
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
