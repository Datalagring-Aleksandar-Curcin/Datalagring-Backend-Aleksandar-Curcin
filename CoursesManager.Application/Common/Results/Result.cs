namespace CoursesManager.Application.Common.Results;

public readonly record struct Deleted;

public static class Result
{
    public static Deleted Deleted => default;
}
