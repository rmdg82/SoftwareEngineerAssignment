using Microsoft.AspNetCore.Mvc;
using SoftwareEngineerAssignment.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareEngineerAssignment.Tests.Unit.Controllers;

public class RootControllerTests
{
    private readonly RootController _controller = new();

    [Fact]
    public void Get_ReturnsOk()
    {
        // Arrange
        var expectedMessage = "Welcome to the Software Engineer Assignment API";

        // Act
        var result = _controller.GetRoot();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);

        var actualMessage = (string)result.Value!.GetType().GetProperty("Message")!.GetValue(result.Value)!;
        Assert.Equal(expectedMessage, actualMessage);
    }
}