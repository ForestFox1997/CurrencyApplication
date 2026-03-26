using Yarp.ReverseProxy.Configuration;

namespace Gateway.Configuration
{
    public static class ReverseProxyConfig
    {
        public static IReadOnlyList<RouteConfig> GetRoutes()
        {
            return
            [
                new RouteConfig
                {
                    RouteId = "finance-route",
                    ClusterId = "finance-cluster",
                    Match = new RouteMatch
                    {
                        Path = "/finance/{**catch-all}"
                    },
                    Transforms =
                    [
                        new Dictionary<string, string>
                        {
                            { "PathRemovePrefix", "/finance" }
                        }
                    ]
                },
                new RouteConfig
                {
                    RouteId = "user-route",
                    ClusterId = "user-cluster",
                    Match = new RouteMatch
                    {
                        Path = "/user/{**catch-all}"
                    },
                    Transforms =
                    [
                        new Dictionary<string, string>
                        {
                            { "PathRemovePrefix", "/user" }
                        }
                    ]
                }
            ];
        }

        public static IReadOnlyList<ClusterConfig> GetClusters()
        {
            return
            [
                new ClusterConfig
                {
                    ClusterId = "finance-cluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "d1",
                            new DestinationConfig
                            {
                                Address = "http://finance-service:8080/"
                            }
                        }
                    }
                },
                new ClusterConfig
                {
                    ClusterId = "user-cluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "d1",
                            new DestinationConfig
                            {
                                Address = "http://user-service:8080/"
                            }
                        }
                    }
                }
            ];
        }
    }
}