﻿using Domain.Sample;

namespace Test.Fixtures.Sample;

public static class SampleModelFixture
{
    public static SampleModel DefaultSample { get; } = new SampleModel(
        Guid.NewGuid(),
        "DefualtSample",
        "Description",
        1234,
        DateTime.Now,
        DateTime.Now
    );
}