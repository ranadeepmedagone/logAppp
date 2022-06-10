namespace logapp.Utilities;

public static class UserConstants
{
    public const string Id = nameof(Id);

    public const string Email = nameof(Email);

    public const string Name = nameof(Name);

    public const string IsSuperUser = nameof(IsSuperUser);
    public const string Status = nameof(Status);

}

public enum TableNames
{
    user,
    log,
    tag,
    log_tag,

    tag_type,

    log_seen,

    user_tag
}