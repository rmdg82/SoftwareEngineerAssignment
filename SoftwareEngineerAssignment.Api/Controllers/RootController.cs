using Microsoft.AspNetCore.Mvc;
using SoftwareEngineerAssignment.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SoftwareEngineerAssignment.Api.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    [HttpGet]
    public OkObjectResult GetRoot()
    {
        GiveMeAdviceRequest giveMeAdviceRequest = new()
        {
            Topic = "car",
            Amount = 2,
        };

        return Ok(new
        {
            Message = "Welcome to the Software Engineer Assignment API",
            Endpoints = new
            {
                GiveMeAdvice = new
                {
                    Url = "/api/GiveMeAdvice",
                    Verbs = "POST",
                    Description = "Get a random advice slip setting a custom topic and total advice amount.",
                    ExampleJSON = JsonSerializer.Serialize(
                        giveMeAdviceRequest,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        }),
                },
            }
        });
    }
}