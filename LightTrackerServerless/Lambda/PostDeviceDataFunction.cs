using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LightTrackerServerless.Model;
using LightTrackerServerless.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LightTrackerServerless.Lambda
{
    public class PostDeviceDataFunction: BaseFunction
    {
        private readonly IDeviceService _deviceService;

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public PostDeviceDataFunction(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <remarks>       
        [LambdaFunction]
        [RestApi(LambdaHttpMethod.Post, "/")]
        public async Task<IHttpResult> PutDeviceData(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogInformation("POST RequestBody" + request.Body);

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
                var devices = await _deviceService.AddDevice(device);
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
}
