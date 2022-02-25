using Xunit;

namespace moosik.api.integration.test.ContainerManager;

[CollectionDefinition("Docker Container")]
public class ContainerManagerCollection : ICollectionFixture<ContainerManager>
{
    
}