var container = new DependencyContainer();
container.AddTransient<HelloService>();
container.AddTransient<ConsumerService>();
container.AddTransient<MessageService>();

var resolver = new DependencyResolver(container);

var service1 = resolver.GetService<ConsumerService>();
service1.Print();

var service2 = resolver.GetService<ConsumerService>();
service2.Print();

var service3 = resolver.GetService<ConsumerService>();
service3.Print();


public class DependencyResolver
{
    private DependencyContainer _container;

    public DependencyResolver(DependencyContainer container)
    {
        _container = container;
    }

    public T GetService<T>()
    {
        return (T) GetService(typeof(T));
    }

    public object GetService(Type type)
    {
        var dependency = _container.GetDependency(type);
        var constructor = dependency.Type.GetConstructors().Single();
        var parameter = constructor.GetParameters();

        if (parameter.Length > 0)
        {
            var parameterImplementations = new object[parameter.Length];

            for (int i = 0; i < parameter.Length; i++)
            {
                parameterImplementations[i] = GetService(parameter[i].ParameterType);
            }
            
            return CreateImplementation(dependency, t => Activator.CreateInstance(t, parameterImplementations));
        }

        return CreateImplementation(dependency, t => Activator.CreateInstance(t));
    }

    public object CreateImplementation(Dependency dependency, Func<Type, object> factory)
    {
        if (dependency.IsImplemented)
        {
            return dependency.Implementation;
        }
        
        var implementation = factory(dependency.Type);

        if (dependency.LifeTime == DependencyLifeTime.Singleton)
        {
            dependency.AddImplementation(implementation);
        }
        
        return implementation;
    }
}

public class DependencyContainer
{
    private List<Dependency> _dependencies;
    public DependencyContainer()
    {
        _dependencies = new List<Dependency>();
    }


    public void AddSingleton<T>()
    {
        _dependencies.Add(new Dependency(typeof(T), DependencyLifeTime.Singleton));
    }
    
    public void AddTransient<T>()
    {
        _dependencies.Add(new Dependency(typeof(T), DependencyLifeTime.Transient));
    }

    public Dependency GetDependency(Type type)
    {
        return _dependencies.First(x => x.Type.Name == type.Name);
    }
}

public class Dependency
{
    public Dependency(Type type, DependencyLifeTime lifeTime)
    {
        Type = type;
        LifeTime = lifeTime;
    }

    public Type Type { get; set; }
    public DependencyLifeTime LifeTime { get; set; }
    public object Implementation { get; set; }
    public bool IsImplemented { get; set; }

    public void AddImplementation(object i)
    {
        Implementation = i;
        IsImplemented = true;
    }
}

public enum DependencyLifeTime
{
    Singleton = 0,
    Transient = 1
}

public class HelloService
{
    private MessageService _message;
    public HelloService(MessageService message)
    {
        _message = message;
    }
    
    public void Print()
    {
        Console.WriteLine($"Hello service {_message.Message()}");
    }
}

public class ConsumerService
{
    private HelloService _hello;

    public ConsumerService(HelloService hello)
    {
        _hello = hello;
    }

    public void Print()
    {
        _hello.Print();
    }
}

public class MessageService
{
    private int _random;
    public MessageService()
    {
        _random = new Random().Next();
    }
    public string Message()
    {
        return $"Yo #{_random}";
    }
}