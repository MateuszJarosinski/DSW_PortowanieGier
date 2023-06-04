using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Jobs
{
    public class CubesManager : MonoBehaviour
    {
        [SerializeField] private CubeController cubePrefab;
        [SerializeField] private int cubesToSpawn = 1000;
        
        private List<CubeController> _spawnedCubes = new List<CubeController>();
        private NativeArray<float3> _cubesPosition;
        private FindClosestAndFarthestJob _job;
        private JobHandle _jobHandle;

        private void Awake()
        {
            Spawn();   
        }
        
        private void Update()
        {
            UpdatePositionDots();
        }

        private void LateUpdate()
        {
            _jobHandle.Complete();
            
            var result = _job.result;

            for (int i = 0; i < _spawnedCubes.Count; i++)
            {
                _spawnedCubes[i].FirstClosest = _spawnedCubes[result[i].firstClosest].transform.position;
                _spawnedCubes[i].SecondClosest = _spawnedCubes[result[i].secondClosest].transform.position;
                _spawnedCubes[i].ThirdClosest = _spawnedCubes[result[i].thirdClosest].transform.position;
                _spawnedCubes[i].Farthest = _spawnedCubes[result[i].farthest].transform.position;
            }
        }

        private void Spawn()
        {
            for (int i = 0; i < cubesToSpawn; i++)
            {
                _spawnedCubes.Add(Instantiate(cubePrefab, UnityEngine.Random.insideUnitSphere * 100f, Quaternion.identity, transform));   
            }
        }

        private void UpdatePositionDots()
        {
            var spawnedDotsCubesCount = _spawnedCubes.Count;
            _cubesPosition = new NativeArray<float3>(spawnedDotsCubesCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var result = new NativeArray<Result>(spawnedDotsCubesCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            for (int i = 0; i < spawnedDotsCubesCount; i++)
            {
                _cubesPosition[i] = _spawnedCubes[i].transform.position;   
            }

            _job = new FindClosestAndFarthestJob(spawnedDotsCubesCount, _cubesPosition, result);

            _jobHandle = _job.Schedule();
        }

        [BurstCompile]
        private struct FindClosestAndFarthestJob : IJob
        {
            private int length;
            private NativeArray<float3> positions;
            public NativeArray<Result> result;

            public FindClosestAndFarthestJob(int length, NativeArray<float3> positions, NativeArray<Result> result)
            {
                this.length = length;
                this.positions = positions;
                this.result = result;
            }

            public void Execute()
            {
                for (int i = 0; i < length; i++)
                {
                    var closestDistances = new NativeArray<float>(3, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
                    var closestIndexes = new NativeArray<int>(3, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
                    
                    float farthestDistance = -math.INFINITY;
                    int farthestIndex = -1;
    
                    for (int j = 0; j < 3; j++)
                    {
                        closestDistances[j] = math.INFINITY;
                        closestIndexes[j] = -1;
                    }
                    
                    for (int j = 0; j < length; j++)
                    {
                        if (j == i) continue;
                        
                        var distance = math.distance(positions[i], positions[j]);
                        
                        for (int k = 0; k < 3; k++)
                        {
                            if (distance < closestDistances[k])
                            {
                                for (int n = 2; n > k; n--)
                                {
                                    closestDistances[n] = closestDistances[n - 1];
                                    closestIndexes[n] = closestIndexes[n - 1];
                                }
                                
                                closestDistances[k] = distance;
                                closestIndexes[k] = j;
                                break;
                            }
                        }

                        if (distance > farthestDistance)
                        {
                            farthestDistance = distance;
                            farthestIndex = j;
                        }
                    }
                    
                    result[i] = new Result()
                    {
                        firstClosest = closestIndexes[0], 
                        secondClosest = closestIndexes[1],
                        thirdClosest = closestIndexes[2], 
                        farthest = farthestIndex
                    };   
                }
            }
        }
        
        private struct Result
        {
            public int firstClosest;
            public int secondClosest;
            public int thirdClosest;
            public int farthest;
        }
    }
}