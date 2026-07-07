using FluentAssertions;
using SprintUp.Modules.UserManagement.Domain.Entities;
using SprintUp.Modules.UserManagement.Domain.Enums;
using Xunit;
using System;

namespace SprintUp.Tests.Modules.UserManagement.Features.RegisterUser;

public class UserTests
{
    [Fact]
    public void User_ShouldThrowArgumentException_WhenBirthDateResultsInUnder18YearsOld()
    {
        // Arrange: Definir uma data de nascimento que resultaria em menos de 18 anos
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var birthDateUnder18 = today.AddYears(-17).AddDays(1); // Ex: 17 anos e 1 dia

        // Act
        Action act = () => new User(
            email: "test@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDateUnder18,
            gender: Gender.Male,
            weightKg: 70.0M,
            heightCm: 175,
            lgpdConsent: true
        );

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O atleta deve ter 18 anos ou mais para se registrar. (Parameter 'birthDate')")
            .Because("a entidade User deve impor a restrição de idade mínima de 18 anos.");
    }

    [Fact]
    public void User_ShouldNotThrowException_WhenBirthDateResultsIn18YearsOldOrMore()
    {
        // Arrange: Definir uma data de nascimento que resultaria em 18 anos ou mais
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var birthDate18OrMore = today.AddYears(-18); // Ex: Exatamente 18 anos

        // Act
        Action act = () => new User(
            email: "test@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDate18OrMore,
            gender: Gender.Female,
            weightKg: 60.0M,
            heightCm: 160,
            lgpdConsent: true
        );

        // Assert
        act.Should().NotThrow<ArgumentException>()
            .Because("a entidade User não deve lançar exceção para idade igual ou superior a 18 anos.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void User_ShouldThrowArgumentException_WhenWeightKgIsZeroOrNegative(decimal invalidWeight)
    {
        // Arrange
        var birthDateValid = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-20);

        // Act
        Action act = () => new User(
            email: "test@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDateValid,
            gender: Gender.NonBinary,
            weightKg: invalidWeight,
            heightCm: 170,
            lgpdConsent: true
        );

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O peso (WeightKg) deve ser maior que zero. (Parameter 'weightKg')")
            .Because("WeightKg deve ser um valor positivo.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void User_ShouldThrowArgumentException_WhenHeightCmIsZeroOrNegative(int invalidHeight)
    {
        // Arrange
        var birthDateValid = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-20);

        // Act
        Action act = () => new User(
            email: "test@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDateValid,
            gender: Gender.PreferNotToSay,
            weightKg: 75.0M,
            heightCm: invalidHeight,
            lgpdConsent: true
        );

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("A altura (HeightCm) deve ser maior que zero. (Parameter 'heightCm')")
            .Because("HeightCm deve ser um valor positivo.");
    }

    [Fact]
    public void User_ShouldSetConsentDateUtc_WhenLgpdConsentIsTrue()
    {
        // Arrange
        var birthDateValid = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-20);
        var expectedBefore = DateTimeOffset.UtcNow;

        // Act
        var user = new User(
            email: "consent@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDateValid,
            gender: Gender.Male,
            weightKg: 80.0M,
            heightCm: 180,
            lgpdConsent: true
        );
        var expectedAfter = DateTimeOffset.UtcNow;

        // Assert
        user.LgpdConsent.Should().BeTrue();
        user.ConsentDateUtc.Should().NotBeNull();
        user.ConsentDateUtc.Should().BeOnOrAfter(expectedBefore.Subtract(TimeSpan.FromSeconds(1)))
            .And.BeOnOrBefore(expectedAfter.Add(TimeSpan.FromSeconds(1)));
    }

    [Fact]
    public void User_ShouldNotSetConsentDateUtc_WhenLgpdConsentIsFalse()
    {
        // Arrange
        var birthDateValid = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-20);

        // Act
        var user = new User(
            email: "noconsent@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDateValid,
            gender: Gender.Female,
            weightKg: 55.0M,
            heightCm: 165,
            lgpdConsent: false
        );

        // Assert
        user.LgpdConsent.Should().BeFalse();
        user.ConsentDateUtc.Should().BeNull();
    }

    [Fact]
    public void User_ShouldHaveEmailValidationPendingStatus_UponCreation()
    {
        // Arrange
        var birthDateValid = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-20);

        // Act
        var user = new User(
            email: "status@example.com",
            passwordHash: "hashedPassword123",
            birthDate: birthDateValid,
            gender: Gender.NonBinary,
            weightKg: 70.0M,
            heightCm: 170,
            lgpdConsent: true
        );

        // Assert
        user.Status.Should().Be(UserStatus.EmailValidationPending);
    }
}
