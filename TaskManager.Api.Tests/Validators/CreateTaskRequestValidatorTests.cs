using FluentValidation.TestHelper;
using TaskManager.Api.Contracts;
using TaskManager.Api.Validators;

namespace TaskManager.Api.Tests.Validators;

public class CreateTaskRequestValidatorTests
{
    [Fact]
    public async Task Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        CreateTaskRequest request = new CreateTaskRequest
        {
            Title = "",
            Priority = Models.TaskPriority.Low
        };

        CreateTaskRequestValidator validator = new CreateTaskRequestValidator();

        // Act
        var result = await validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}