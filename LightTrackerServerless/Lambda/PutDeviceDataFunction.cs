using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using LightTrackerServerless.Database.Models;
using System.Text.Json;
using LightTrackerServerless.Model;
using LightTrackerServerless.Service;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
// [assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LightTrackerServerless.Lambda;

public class PutDeviceDataFunction: BaseFunction
{
    private readonly IDeviceService _deviceService;

    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public PutDeviceDataFunction(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }


    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <remarks>
    /// This uses the <see href="https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.Annotations/README.md">Lambda Annotations</see> 
    /// programming model to bridge the gap between the Lambda programming model and a more idiomatic .NET model.
    /// 
    /// This automatically handles reading parameters from an APIGatewayProxyRequest
    /// as well as syncing the function definitions to serverless.template each time you build.
    /// 
    /// If you do not wish to use this model and need to manipulate the API Gateway 
    /// objects directly, see the accompanying Readme.md for instructions.
    /// </remarks>
    /// <param name="context">Information about the invocation, function, and execution environment</param>
    /// <returns>The response as an implicit <see cref="APIGatewayProxyResponse"/></returns>
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Put, "/")]
    public async Task<IHttpResult> PutDeviceData(APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogInformation("PUT RequestBody" + request.Body);

        var userId = getUserIdFromRequest(request);

        if (userId == null)
        {
            context.Logger.LogInformation("Can't define a user");

            return HttpResults.BadRequest();
        }

        var device = getDeviceFromRequest(request, userId);

        if (device == null)
        {
            context.Logger.LogInformation("Can't parse a device parameters");

            return HttpResults.BadRequest();
        }

        try
        {
            /* var isMemoryRepository = Environment.GetEnvironmentVariable("REPOSITORY_TYPE") == "memory";
            var devices = isMemoryRepository
                ? await _deviceService.AddDevice(device)
                : await _deviceService.UpdateDevice(device); */

            var devices = await _deviceService.UpdateDevice(device);
            var responce = JsonSerializer.Serialize(devices);
            context.Logger.LogInformation("ResponceBody" + responce);

            return HttpResults.Ok(responce);

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

    private DeviceDto? getDeviceFromRequest(APIGatewayProxyRequest request, string userId)
    {
        var device = JsonSerializer.Deserialize<DeviceDto>(request.Body);

        if (device == null)
        {
            return null;
        }

        device.UserId = userId;
        device.ReceivedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return device;
    }
}
