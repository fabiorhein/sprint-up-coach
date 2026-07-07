using SprintUp.Modules.UserManagement.Domain.Enums;
using SprintUp.Shared.Domain;

namespace SprintUp.Modules.UserManagement.Domain.Entities;

public class User : AggregateRoot
{
    public Guid Id { get; private set; }
    public string? Email { get; private set; }
    public string? PasswordHash { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public decimal WeightKg { get; private set; }
    public int HeightCm { get; private set; }
    public bool LgpdConsent { get; private set; }
    public DateTimeOffset? ConsentDateUtc { get; private set; }
    public UserStatus Status { get; private set; }

    // Construtor privado para EF Core e métodos de fábrica/recuperação
    private User() { }

    public User(
        string email,
        string passwordHash,
        DateOnly birthDate,
        Gender gender,
        decimal weightKg,
        int heightCm,
        bool lgpdConsent)
    {
        if (birthDate.AddYears(18) > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException("O atleta deve ter 18 anos ou mais para se registrar.", nameof(birthDate));
        }

        if (weightKg <= 0)
        {
            throw new ArgumentException("O peso (WeightKg) deve ser maior que zero.", nameof(weightKg));
        }

        if (heightCm <= 0)
        {
            throw new ArgumentException("A altura (HeightCm) deve ser maior que zero.", nameof(heightCm));
        }

        Id = Guid.NewGuid(); 
        Email = email;
        PasswordHash = passwordHash;
        BirthDate = birthDate;
        Gender = gender;
        WeightKg = weightKg;
        HeightCm = heightCm;
        LgpdConsent = lgpdConsent;
        ConsentDateUtc = lgpdConsent ? DateTimeOffset.UtcNow : null;
        Status = UserStatus.EmailValidationPending;
    }
}
