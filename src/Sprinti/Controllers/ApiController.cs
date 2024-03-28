using Microsoft.AspNetCore.Mvc;

namespace Sprinti.Controllers;

[ApiController]
[Route("api/[controller]", Name = nameof(ApiController))]
public abstract class ApiController : ControllerBase;