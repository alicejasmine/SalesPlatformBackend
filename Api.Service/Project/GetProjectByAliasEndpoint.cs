using Api.Service.Project.DTOs;
using ApplicationServices.Project;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Project;

public class GetProjectByAliasEndpoint : EndpointBaseAsync.WithRequest<GetProjectByProjectAliasRequestDto>.WithActionResult<ProjectResponse>
{
    private readonly IProjectService _projectService;

    public GetProjectByAliasEndpoint(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("GetProjectByAlias")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Project by Alias",
        Description = "Retrieve project details by alias",
        OperationId = "GetProjectByAlias")
    ]
    public override async Task<ActionResult<ProjectResponse>> HandleAsync([FromQuery] GetProjectByProjectAliasRequestDto dto, CancellationToken cancellationToken = new CancellationToken())
    {
        var project = await _projectService.GetProjectByAlias(dto.Alias);

        if (project == null)
        {
            return Problem(
                title: "Project not found",
                detail: $"No project found with alias '{dto.Alias}'",
                statusCode: 404
            );
        }

        return new ActionResult<ProjectResponse>(ProjectMapper.MapModelToResponse(project));
    }
}