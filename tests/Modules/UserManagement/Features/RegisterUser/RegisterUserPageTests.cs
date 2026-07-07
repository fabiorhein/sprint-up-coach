using Bunit;
using Xunit;
using FluentAssertions;
using SprintUp.Modules.UserManagement.Features.RegisterUser; // For RegisterUserPage

namespace SprintUp.Tests.Modules.UserManagement.Features.RegisterUser;

public class RegisterUserPageTests : TestContext
{
    [Fact]
    public void RegisterPage_ShouldDisplayValidationErrors_WhenSubmittingEmptyForm()
    {
        // Arrange
        var cut = RenderComponent<RegisterUserPage>();

        // Act - Dispara o submit diretamente no form para forçar o pipeline do EditForm
        cut.Find("form").Submit();

        // Assert
        cut.Find("h3").TextContent.Should().Contain("RegisterUserPage");
        
        // Usamos o Markup para verificar se as strings de validação foram injetadas no HTML
        cut.Markup.Should().Contain("O email é obrigatório.");
        cut.Markup.Should().Contain("A senha é obrigatória.");
        cut.Markup.Should().Contain("A confirmação da senha é obrigatória.");
    }
}
