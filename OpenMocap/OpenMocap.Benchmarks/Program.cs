// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using OpenMocap.Benchmarks;

var config = ManualConfig
    .Create(DefaultConfig.Instance)
    .WithOptions(ConfigOptions.DisableOptimizationsValidator);

var cpuSummary = BenchmarkRunner.Run<MLMocapBenchmarkCpu>(config);
var gpu0Summary = BenchmarkRunner.Run<MLMocapBenchmarkCudaGpu0>(config);
