using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.users.domain.Subscriptions.Enums;

public sealed record Currency
{
    public static readonly Currency Pln = new Currency("PLN", "Złoty");
    
    public string Shorcut { get; }
    public string Name { get; }

    public Currency(
        string shorcut,
        string name)
    {
        Shorcut = shorcut;
        Name = name;
    }

    public static Currency FromValue(string shortcut) => shortcut switch
    {
        "PLN" => Pln,
        _ => throw new InvalidArgumentException("Currency.InvalidFormat")
    };
};