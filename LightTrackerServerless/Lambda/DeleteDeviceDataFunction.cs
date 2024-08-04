using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using LightTrackerServerless.Database.Models;
using System.Text.Json;
using LightTrackerServerless.Model;
using LightTrackerServerless.Service;

namespace LightTrackerServerless.Lambda;

public class DeleteDeviceDataFunction: BaseFunction
{
    private readonly IDeviceService _deviceService;

    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public DeleteDeviceDataFunction(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }


    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <remarks>
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Delete, "/{deviceUniqueIdentifier}")]
    public async Task<IHttpResult> DeleteDeviceData(string deviceUniqueIdentifier, APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogInformation("DELETE deviceUniqueIdentifier: " + deviceUniqueIdentifier);

        var userId = getUserIdFromRequest(request);

        if (userId == null)
        {
            context.Logger.LogInformation("Can't define a user");

            return HttpResults.BadRequest();
        }

        try
        {
            await _deviceService.DeleteDevice(userId, deviceUniqueIdentifier);

            return HttpResults.Ok();

        }
        catch (Exception exception)
        {
            context.Logger.LogInformation("Request Error: " + exception.Message + " " + exception.StackTrace);

            return HttpResults.InternalServerError();
        }
    }

    private string? getUserIdFromRequest(APIGatewayProxyRequest request)
    {
        return request.RequestContext?.Authorizer?.Claims["sub"] != null ? request.RequestContext.Authorizer.Claims["sub"] : null;
    }
}
