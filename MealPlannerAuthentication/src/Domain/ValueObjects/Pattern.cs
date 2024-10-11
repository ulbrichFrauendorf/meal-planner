
namespace invensys.iserve.Domain.ValueObjects;

public class Pattern : ValueObject
{
   static Pattern()
   {
   }

   private Pattern()
   {
   }

   private Pattern(string regex)
   {
      Regex = regex;
   }

   public static Pattern From(string regex)
   {
      var pattern = new Pattern { Regex = regex };

      return !SupportedPatterns.Contains(pattern) ? throw new UnsupportedPatternException(regex) : pattern;
   }

   public static Pattern OnlyAlphabeticCharactersWithApostrophe => new("^[a-zA-Z'_ ]*$");

   public static Pattern Email => new(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

   public static Pattern Password => new(@"^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=.*\W).{8,30}$");

   public string Regex { get; private set; } = "";

   public static implicit operator string(Pattern pattern)
   {
      return pattern.ToString();
   }

   public static explicit operator Pattern(string regex)
   {
      return From(regex);
   }

   public override string ToString()
   {
      return Regex;
   }

   protected static IEnumerable<Pattern> SupportedPatterns
   {
      get
      {
         yield return OnlyAlphabeticCharactersWithApostrophe;
         yield return Email;
         yield return Password;
      }
   }

   protected override IEnumerable<object> GetEqualityComponents()
   {
      yield return Regex;
   }
}
