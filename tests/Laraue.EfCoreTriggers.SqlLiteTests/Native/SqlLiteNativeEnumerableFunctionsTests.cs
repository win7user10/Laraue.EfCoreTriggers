﻿using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native;

[Collection(CollectionNames.Sqlite)]
public class SqlLiteNativeEnumerableFunctionsTests : NativeEnumerableFunctionsTests
{
    public SqlLiteNativeEnumerableFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
}