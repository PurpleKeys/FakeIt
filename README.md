# FakeIt
Takes some of the pain of working with dependencies and parameters, helped by Moq.

## Dependencies
Given we have a class with a large amount of dependencies, and we know we only need an implementation or Moq for a subset of parameters the test code gets ugly quickly. Using FakeIt `Make` we can reduce the amount of fluff.

Given the class under test has a lot of dependencies:
```C#
public class Service
{
    public Service(ILog log, IRepo repo, IAudit audit, IValidator validator){...}
}
```

We can now do the following:

To use `Mock.Of<>` for all dependencies:
```C#
 Make.WithFakes<Service>();
```

Specify a specific value for one, or more depednencies and use `Mock.Of<>` for all other dependencies:
```C#
var deps = new Dictionary<string, object?>{
    {"validator" : Mock.Of<IValidator>(v => v.IsValid(It.IsAny<object>()) == false)
};

 Make.WithFakes<Service>(deps);
```
