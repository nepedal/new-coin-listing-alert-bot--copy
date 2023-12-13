namespace Shared.Delegates;

public delegate T ServiceResolver<out T>(string key);
