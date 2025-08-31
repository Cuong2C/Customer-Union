namespace Longbeach.Authentication
{
    public interface ISqlServerValidatorHandler
    {
        public bool Validate(string clientSource, string token);
    }
}
