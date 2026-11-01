using Moq;

namespace LearnToDev.Tests.Examples;


// 1. The interface we want to fake
public interface IExampleRepository
{
    Task<string?> GetSchoolNameAsync(int id);
}

// 2. The class we want to test. It gets the repository through the constructor
public class ExampleService
{
    private readonly IExampleRepository _repository;

    public ExampleService(IExampleRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> GetSchoolTitleAsync(int id)
    {
        var name = await _repository.GetSchoolNameAsync(id);

        if (name == null)
        {
            return "Ukjent skole";
        }
        return "Skole: " + name;
    }
}

// 3. The tests. Name pattern: Method_Situation_ExpectedResult
public class ExampleServiceTests
{
    [Fact]
    public async Task GetSchoolTitleAsync_SchoolExists_ReturnsTitleWithName()
    {
        // Arrange: a fake repository that returns "Frontend" for id 1
        var repository = new Mock<IExampleRepository>();
        repository.Setup(r => r.GetSchoolNameAsync(1)).ReturnsAsync("Frontend");
        var service = new ExampleService(repository.Object);

        // Act
        var result = await service.GetSchoolTitleAsync(1);

        // Assert
        Assert.Equal("Skole: Frontend", result);
        repository.Verify(r => r.GetSchoolNameAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetSchoolTitleAsync_SchoolDoesNotExist_ReturnsUnknownSchool()
    {
        var repository = new Mock<IExampleRepository>();
        repository.Setup(r => r.GetSchoolNameAsync(99)).ReturnsAsync((string?)null);
        var service = new ExampleService(repository.Object);

        var result = await service.GetSchoolTitleAsync(99);

        Assert.Equal("Ukjent skole", result);
        repository.Verify(r => r.GetSchoolNameAsync(99), Times.Once);
    }
}

