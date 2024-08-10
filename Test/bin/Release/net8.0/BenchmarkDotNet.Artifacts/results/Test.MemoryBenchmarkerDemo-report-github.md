``` ini

BenchmarkDotNet=v0.10.11, OS=Windows 10.0.22631
Processor=Intel Core i3-10100F CPU 3.60GHz, ProcessorCount=8
.NET Core SDK=8.0.303
  [Host] : .NET Core 8.0.7 (Framework .NET 8.0.7), 64bit RyuJIT


```
|                          Method | Mean | Error | Allocated |
|-------------------------------- |-----:|------:|----------:|
| ConcatStringsUsingStringBuilder |   NA |    NA |       N/A |
|   ConcatStringsUsingGenericList |   NA |    NA |       N/A |

Benchmarks with issues:
  MemoryBenchmarkerDemo.ConcatStringsUsingStringBuilder: DefaultJob
  MemoryBenchmarkerDemo.ConcatStringsUsingGenericList: DefaultJob
