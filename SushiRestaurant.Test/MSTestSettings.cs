using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: DoNotParallelize]
[assembly: Parallelize(Workers = 1, Scope = ExecutionScope.MethodLevel)]