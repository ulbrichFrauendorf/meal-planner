namespace invensys.iserve.Domain.Exceptions;

public class UnsupportedPatternException(string regex) : Exception($"Regex pattern \"{regex}\" is unsupported.")
{
}
