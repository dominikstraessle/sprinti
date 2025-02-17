using Microsoft.AspNetCore.Mvc;

namespace Sprinti.Dashboard;

[ApiController]
[Route("api/[controller]", Name = nameof(ApiController))]
public abstract class ApiController : ControllerBase;