﻿using System;
using Demon.Fody;
using Fody;
using Xunit;

#pragma warning disable 618

namespace Tests
{
    public class AssemblyToProcessTests
    {
        private readonly TestResult _result;

        public AssemblyToProcessTests()
        {
            var weaver = new ModuleWeaver();

            _result = weaver.ExecuteTestRun("AssemblyToProcess.dll");
        }

        [Fact]
        public void StaticAdvice()
        {
            var type = _result.Assembly.GetType("AssemblyToProcess.StaticBeforeTarget");

            var instance = (dynamic)Activator.CreateInstance(type);

            Assert.Throws<ApplicationException>(() => instance.Target(5));
        }
    }
}