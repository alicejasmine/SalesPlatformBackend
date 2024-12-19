using Api.Service.Project.DTOs;
using ApplicationServices.Project;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Service.Project
{
    public class GetProjectsByOrganizationAliasEndpoint : EndpointBaseAsync.WithRequest<GetProjectsByOrganizationAliasRequestDto>.WithActionResult<List<ProjectResponse>>
    {
        private readonly IProjectService _projectService;

        public GetProjectsByOrganizationAliasEndpoint(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("GetProjectsByOrganizationAlias")]
        [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Get Projects by Organization Alias",
            Description = "Retrieve a list of projects by the organization's alias",
            OperationId = "GetProjectsByOrganizationAlias")
        ]
        public override async Task<ActionResult<List<ProjectResponse>>> HandleAsync([FromQuery] GetProjectsByOrganizationAliasRequestDto dto, CancellationToken cancellationToken = new CancellationToken())
        {
           
            var projects = await _projectService.GetProjectsByOrganizationAlias(dto.OrganizationAlias);

            if (projects.Count==0)
            {
                return Problem(
                    title: "Projects not found",
                    detail: $"No projects found for organization with alias '{dto.OrganizationAlias}'",
                    statusCode: 404
                );
            }
            var projectResponses = projects.Select(ProjectMapper.MapModelToResponse).ToList();

            return Ok(projectResponses);  
        }
    }
}
