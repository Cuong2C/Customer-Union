namespace Customer_Union.Authentication;

public interface ISqlServerValidatorHandler
{
    public bool Validate(string clientSource, string token);
}
