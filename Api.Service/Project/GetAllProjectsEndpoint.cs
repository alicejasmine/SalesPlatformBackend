using Api.Service.Project.DTOs;
using ApplicationServices.Project;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Project;

public class GetAllProjectsEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult<List<ProjectResponse>>
{
    private readonly IProjectService _projectService;

    public GetAllProjectsEndpoint(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("GetAllProjects")]
    [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get All Projects",
        Description = "Retrieve all projects",
        OperationId = "GetAllProjects")
    ]
    public override async Task<ActionResult<List<ProjectResponse>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var projects = await _projectService.GetAllProjects();

            if (projects == null || !projects.Any())
            {
                return Problem(
                    title: "No Projects Found",
                    detail: "There are no projects available in the system.",
                    statusCode: 404
                );
            }

            var response = projects.Select(ProjectMapper.MapModelToResponse).ToList();
            return new ActionResult<List<ProjectResponse>>(response);
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Error Retrieving Projects",
                detail: ex.Message,
                statusCode: 500
            );
        }
    }
}