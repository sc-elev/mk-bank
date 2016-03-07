#!/bin/bash
packages/OpenCover.4.6.519/tools/OpenCover.Console.exe \
    -register:user  \
    -target:packages/NUnit.ConsoleRunner.3.2.0/tools/nunit3-console.exe \
    -targetargs:Bank.Tests/Bank.Tests.csproj
packages/ReportGenerator.2.4.4.0/tools/ReportGenerator.exe \
    -reports:results.xml  \
    -targetdir:test-reports
