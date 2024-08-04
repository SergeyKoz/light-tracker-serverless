using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using LightTrackerServerless.Database;
using LightTrackerServerless.Lambda;
using LightTrackerServerless.Repository;
using LightTrackerServerless.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LightTrackerServerless.Tests;

public class FunctionTest
{
    public FunctionTest()
    {
    }

    [Fact]
    public async void TestDbUpdateDevice()
    {
        var dbContext = new LightTrackerContext();
        await removeUserDevices(dbContext, "userId");

        var deviceRepository = new DeviceRepository(new LightTrackerContext());
        var deviceService = new DeviceService(new MapService(), deviceRepository);
        var putFunction = new PutDeviceDataFunction(deviceService);
        var postFunction = new PostDeviceDataFunction(deviceService);
        var deleteFunction = new DeleteDeviceDataFunction(deviceService);

        // add first device
        var body = "{\r\n    \"DeviceName\": \"DESKTOP-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"uuid1\",\r\n    \"BatteryLevel\": 1.0,\r\n    \"BatteryStatus\": 4,\r\n    \"NetworkReachability\": 3\r\n}";
        var request = buildRequest("userId", body);        
        var response = await postFunction.PutDeviceData(request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        // add second device
        body = "{\r\n    \"DeviceName\": \"MOBILE-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"uuid2\",\r\n    \"BatteryLevel\": 0.5,\r\n    \"BatteryStatus\": 1,\r\n    \"NetworkReachability\": 1\r\n}";
        request = buildRequest("userId", body);
        response = await postFunction.PutDeviceData(request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        // update firts device
        body = "{\r\n    \"DeviceName\": \"DESKTOP-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"uuid1\",\r\n    \"BatteryLevel\": 0.99,\r\n    \"BatteryStatus\": 4,\r\n    \"NetworkReachability\": 3\r\n}";
        request = buildRequest("userId", body);
        response = await putFunction.PutDeviceData(request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();

        assertDeviceContains(apiGatewayResponse);
        Assert.Contains("DESKTOP-A70LNK8", apiGatewayResponse);
        Assert.Contains("MOBILE-A70LNK8", apiGatewayResponse);
        Assert.Contains("0.99", apiGatewayResponse);


        Assert.Equal(2, await countUserDevices(dbContext, "userId"));

        response = await deleteFunction.DeleteDeviceData("uuid1", request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        response = await deleteFunction.DeleteDeviceData("uuid2", request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        Assert.Equal(0, await countUserDevices(dbContext, "userId"));
    }

    [Fact]
    public async void TestInMemoryRepository()
    {
        Environment.SetEnvironmentVariable("REPOSITORY_TYPE", "memory");
        var deviceService = new DeviceService(new MapService(), new InMemoryRepository());
        var function = new PutDeviceDataFunction(deviceService);

        var body = "{\r\n    \"DeviceName\": \"DESKTOP-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"61f847aab444ee67483118c395e231a2decc47b0\",\r\n    \"BatteryLevel\": 1.0,\r\n    \"BatteryStatus\": 4,\r\n    \"NetworkReachability\": 3\r\n}";
        var request = buildRequest("userId", body);
        var response = await function.PutDeviceData(request, new TestLambdaContext());

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();

        assertDeviceContains(apiGatewayResponse);
    }

    [Fact]
    public async void TestInMemoryRepositoryUpdateDevice()
    {
        Environment.SetEnvironmentVariable("REPOSITORY_TYPE", "memory");

        var deviceService = new DeviceService(new MapService(), new InMemoryRepository());
        var function = new PutDeviceDataFunction(deviceService);

        // add first device
        var body = "{\r\n    \"DeviceName\": \"DESKTOP-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"uuid1\",\r\n    \"BatteryLevel\": 1.0,\r\n    \"BatteryStatus\": 4,\r\n    \"NetworkReachability\": 3\r\n}";
        var request = buildRequest("userId", body);
        var response = await function.PutDeviceData(request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        // add second device
        body = "{\r\n    \"DeviceName\": \"MOBILE-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"uuid2\",\r\n    \"BatteryLevel\": 0.5,\r\n    \"BatteryStatus\": 1,\r\n    \"NetworkReachability\": 1\r\n}";
        request = buildRequest("userId", body);
        response = await function.PutDeviceData(request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        // update firts device
        body = "{\r\n    \"DeviceName\": \"DESKTOP-A70LNK8\",\r\n    \"DeviceUniqueIdentifier\": \"uuid1\",\r\n    \"BatteryLevel\": 0.99,\r\n    \"BatteryStatus\": 4,\r\n    \"NetworkReachability\": 3\r\n}";
        request = buildRequest("userId", body);
        response = await function.PutDeviceData(request, new TestLambdaContext());
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var serializationOptions = new HttpResultSerializationOptions { Format = HttpResultSerializationOptions.ProtocolFormat.RestApi };
        var apiGatewayResponse = new StreamReader(response.Serialize(serializationOptions)).ReadToEnd();

        assertDeviceContains(apiGatewayResponse);
        Assert.Contains("DESKTOP-A70LNK8", apiGatewayResponse);
        Assert.Contains("MOBILE-A70LNK8", apiGatewayResponse);
        Assert.Contains("0.99", apiGatewayResponse);
    }

    private APIGatewayProxyRequest buildRequest(string userId, string body)
    {
        var authorizer = new APIGatewayCustomAuthorizerContext
        {
            Claims = new Dictionary<string, string>()
            {
                ["sub"] = userId
            },
        };

        var requestContext = new APIGatewayProxyRequest.ProxyRequestContext()
        {
            Authorizer = authorizer,
        };

        var request = new APIGatewayProxyRequest();
        request.Body = body;
        request.RequestContext = requestContext;

        return request;
    }

    private void assertDeviceContains(string apiGatewayResponse)
    {
        Assert.Contains("UserId", apiGatewayResponse);
        Assert.Contains("DeviceName", apiGatewayResponse);
        Assert.Contains("DeviceUniqueIdentifier", apiGatewayResponse);
        Assert.Contains("BatteryLevel", apiGatewayResponse);
        Assert.Contains("BatteryStatus", apiGatewayResponse);
        Assert.Contains("NetworkReachability", apiGatewayResponse);
        Assert.Contains("CreatedAt", apiGatewayResponse);
        Assert.Contains("UpdatedAt", apiGatewayResponse);
    }

    private async Task removeUserDevices(LightTrackerContext dbContext, string userId)
    {
        dbContext.Devices.RemoveRange(dbContext.Devices.Where(device => device.UserId == userId));
        
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task<int> countUserDevices(LightTrackerContext dbContext, string userId)
    {
        return await dbContext.Devices.Where(device => device.UserId == userId).CountAsync();
    }
}
