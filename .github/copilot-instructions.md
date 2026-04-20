# MoneyFlow - Instruções para IA (Copilot/Code Review/Geração de Código)

## 📋 Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura e Estrutura](#arquitetura-e-estrutura)
3. [Padrões e Princípios](#padrões-e-princípios)
4. [Camadas da Aplicação](#camadas-da-aplicação)
5. [Convenções de Código](#convenções-de-código)
6. [Guia de Implementação](#guia-de-implementação)
7. [Checklist para Code Review](#checklist-para-code-review)

---

## 🎯 Visão Geral

### Tecnologias Base
- **.NET Version**: .NET 10.0
- **C# Version**: 14.0
- **Arquitetura**: Clean Architecture
- **Padrão de Design**: CQRS (Command Query Responsibility Segregation)
- **Persistência**: Entity Framework Core
- **Validação**: FluentValidation
- **Mapeamento**: Mapster

### Princípios Fundamentais
1. **DRY (Don't Repeat Yourself)**: Evitar duplicação de código
2. **SOLID**: Aplicar todos os princípios SOLID
3. **Clean Architecture**: Separação clara de responsabilidades
4. **Domain-Driven Design**: Entidades ricas com lógica de negócio
5. **Result Pattern**: Evitar uso de exceções para fluxo de negócio

---

## 🏗️ Arquitetura e Estrutura

### Estrutura de Projetos

```
MoneyFlow/
├── src/
│   ├── core/
│   │   ├── SharedKernel/              # Código compartilhado entre bounded contexts
│   │   ├── MoneyFlow.Domain/          # Entidades, regras de negócio, interfaces de repositório
│   │   └── MoneyFlow.Application/     # Use Cases, DTOs, Handlers, Behaviors
│   ├── infra/
│   │   └── MoneyFlow.Infra/           # Implementação de repositórios, EF Core, serviços externos
│   └── presentation/
│       └── MoneyFlow.API/             # Controllers, Middlewares, Configurações
```

### Dependências entre Camadas

```
API (Presentation) → Application → Domain ← Infrastructure
     ↓                   ↓            ↓
SharedKernel ←──────────┴────────────┴
```

**Regra de Ouro**: 
- Domain **NUNCA** depende de outras camadas
- Application depende apenas de Domain e SharedKernel
- Infrastructure depende de Domain e SharedKernel
- Presentation depende de Application e SharedKernel

---

## 📐 Padrões e Princípios

### 1. Result Pattern

**Uso Obrigatório**: Todas as operações que podem falhar devem retornar `Result<T>` ou `Result`.

#### Estrutura do Result

```csharp
// Sucesso
return Result<Category>.Success(category);
return Result.Success();

// Sucesso com criação a partir de valor
return Result<Category>.Create(category); // null = Failure(Error.NullValue)

// Falha com erro único
return Result.Failure<Category>(Error.RequiredFieldIsEmpty("Name is required"));

// Falha com múltiplos erros
return Result.Failure<Category>(errors);

// Verificação
if (result.IsSuccess)
{
    var value = result.Value;
}

if (result.IsFailure)
{
    var errors = result.Errors;
}
```

#### Tipos de Erros (SharedKernel.Abstractions.Error)

```csharp
Error.None
Error.NullValue
Error.RequiredFieldIsEmpty(string message)
Error.InactiveForeignKey(string message)
Error.NotAuthorized(string message)
Error.ValidationError(string message)
Error.RecordAlreadyExists(string message)
Error.RecordNotFound(string message)
```

### 2. CQRS Pattern

#### Commands (Escrita)
- Modificam o estado da aplicação
- Retornam `Result<T>` ou `Result`
- Implementam `ICommand<T>` ou `ICommand`
- Handlers implementam `ICommandHandler<TCommand, TResponse>`

```csharp
// Command
public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;

// Handler
internal class CreateCategoryCommandHandler(
    ICategoryWriteRepository categoryWriteRepository, 
    IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        CreateCategoryCommand request, 
        CancellationToken cancellationToken = default)
    {
        var category = Category.Create(request.Name);

        if (category.IsFailure)
            return Result.Failure<Guid>(category.Errors!);

        await _categoryWriteRepository.CreateAsync(category.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(category.Value.ExternalId!.Value);
    }
}
```

#### Queries (Leitura)
- Apenas leem dados
- Retornam `Result<T>`
- Implementam `IQuery<T>`
- Handlers implementam `IQueryHandler<TQuery, TResponse>`

**Query para Lista:**
```csharp
// Query
public sealed class GetAllCategoriesQuery : QueryList<CategoryQueryDTO>;

// Handler
internal class GetAllCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
    : IQueryHandler<GetAllCategoriesQuery, BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>
{
    public async Task<Result<BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>> HandleAsync(
        GetAllCategoriesQuery request, 
        CancellationToken cancellationToken = default)
    {
        var categories = await _categoryReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>.Create(
            categories.Adapt<BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>()
        );
    }
}
```

**Query para Item Único:**
```csharp
// Query
public sealed record GetCategoryByExternalIdQuery(Guid ExternalId) : IQuery<CategoryQueryDTO>;

// Handler
internal class GetCategoryByExternalIdQueryHandler(ICategoryReadRepository categoryReadRepository) :
    IQueryHandler<GetCategoryByExternalIdQuery, CategoryQueryDTO>
{
    public async Task<Result<CategoryQueryDTO>> HandleAsync(
        GetCategoryByExternalIdQuery request, 
        CancellationToken cancellationToken = default)
    {
        var category = await _categoryReadRepository.GetByExternalIdAsync(
            request.ExternalId, 
            cancellationToken
        );

        return Result<CategoryQueryDTO>.Create(category.Adapt<CategoryQueryDTO>());
    }
}
```

### 3. Entidades Ricas (Rich Domain Model)

**Características Obrigatórias:**
- Lógica de negócio dentro das entidades
- Métodos `Create` estáticos para criação
- Métodos de negócio públicos
- Setters privados
- Validação interna usando `CheckRequiredFields()`
- Retornam `Result<T>` em todas as operações

```csharp
public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; } = true;

    // Construtor privado apenas para EF Core
    private Category()
    {
    }

    // Construtor para reconstrução (ex: do banco)
    public Category(
        long id, 
        string name, 
        bool active, 
        Guid? externalId = null, 
        DateTimeOffset? createdDate = null, 
        DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        Active = active;
    }

    // Factory Method - SEMPRE retorna Result<T>
    public static Result<Category> Create(string name)
    {
        Category category = new Category(0, name, true, Guid.NewGuid());

        var result = category.CheckRequiredFields();

        if (result.IsFailure)
            return Result.Failure<Category>(result.Errors!);

        return Result.Success(category);
    }

    // Métodos de negócio - SEMPRE retornam Result
    public Result Update(string name, bool? active)
    {
        Name = name;

        if (active is not null)
            Active = (bool)active;

        return CheckRequiredFields();
    }

    // Validação interna
    protected override Result CheckRequiredFields()
    {
        return CheckRequiredField(
            string.IsNullOrWhiteSpace(this.Name), 
            "Category name must be provided"
        );
    }
}
```

### 4. Repository Pattern

#### Separação Read/Write

**Read Repository (Queries):**
```csharp
public interface ICategoryReadRepository
{
    Task<BaseQueryResponse<IEnumerable<Category>>> GetAllAsync(
        QueryParams? queryParams, 
        CancellationToken cancellationToken = default
    );

    Task<Category?> GetByIdAsync(long id, CancellationToken cancellationToken);

    Task<Category?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);
}
```

**Write Repository (Commands):**
```csharp
public interface ICategoryWriteRepository
{
    Task CreateAsync(Category category, CancellationToken cancellationToken = default);

    void Update(Category category, CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default);

    Task<Category?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
}
```

**Implementação Base:**
```csharp
abstract internal class BaseRepository<T>(ApplicationDbContext dbContext) where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);
    }
}
```

### 5. Unit of Work Pattern

```csharp
public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}

// Uso em Handlers
await _categoryWriteRepository.CreateAsync(category.Value, cancellationToken);
await _unitOfWork.CommitAsync(cancellationToken);
```

---

## 📦 Camadas da Aplicação

### 1. SharedKernel

**Propósito**: Código compartilhado entre todos os bounded contexts.

**Contém**:
- `BaseEntity`: Classe base para todas as entidades
- `Result<T>`: Implementação do Result Pattern
- `Error`: Tipos de erros padronizados
- `IMediator`: Interface do mediator
- `ICommand/IQuery`: Interfaces base para CQRS
- `IBusinessRule`: Interface para regras de negócio
- `IDomainEvent`: Interface para eventos de domínio

**BaseEntity:**
```csharp
public abstract class BaseEntity
{
    public long Id { get; set; }
    public Guid? ExternalId { get; init; }
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedOn { get; set; }

    // Validação
    protected Result CheckRule(IBusinessRule rule);
    protected Result CheckRule(bool conditionFailed, Error error);
    protected Result CheckRequiredField(bool fieldIsEmptyOrNull, string errorMessage);
    protected abstract Result CheckRequiredFields();

    // Domain Events
    public IReadOnlyList<IDomainEvent> GetDomainEvents();
    public void ClearDomainEvents();
    protected void RaiseDomainEvent(IDomainEvent domainEvent);
}
```

### 2. Domain Layer

**Propósito**: Lógica de negócio pura, sem dependências externas.

**Estrutura**:
```
Domain/
├── General/
│   ├── Entities/
│   │   ├── Categories/
│   │   │   ├── Category.cs
│   │   │   ├── ICategoryReadRepository.cs
│   │   │   ├── ICategoryWriteRepository.cs
│   │   │   └── CategoryMustBeActiveBusinessRule.cs
│   │   ├── Currencies/
│   │   ├── Markets/
│   │   ├── Sectors/
│   │   └── Users/
│   ├── Enums/
│   ├── Security/
│   └── ValueObjects/
├── Tenant/
└── Abstractions/
    ├── DataAccess/
    │   └── IUnitOfWork.cs
    └── Events/
        └── IDomainEventHandler.cs
```

**Regras**:
- ✅ Entidades ricas com lógica de negócio
- ✅ Interfaces de repositório definidas aqui
- ✅ Business Rules como classes separadas
- ✅ Value Objects imutáveis
- ❌ NÃO pode depender de Infrastructure ou Application
- ❌ NÃO pode ter referências a EF Core, HTTP, etc.

### 3. Application Layer

**Propósito**: Orquestração dos Use Cases da aplicação.

**Estrutura**:
```
Application/
├── UseCases/
│   └── General/
│       ├── Categories/
│       │   ├── Commands/
│       │   │   ├── Create/
│       │   │   │   ├── CreateCategoryCommand.cs
│       │   │   │   └── CreateCategoryCommandHandler.cs
│       │   │   └── Update/
│       │   └── Queries/
│       │       ├── GetAll/
│       │       │   ├── GetAllCategoriesQuery.cs
│       │       │   └── GetAllCategoriesQueryHandler.cs
│       │       └── GetByExternalId/
│       ├── Currencies/
│       ├── Markets/
│       ├── Sectors/
│       └── Users/
├── DTOs/
│   └── General/
│       ├── Categories/
│       │   ├── CategoryQueryDTO.cs
│       │   └── CategoryCommandDTO.cs
│       └── ...
├── Common/
│   ├── Behaviors/
│   │   ├── ValidationFilter.cs
│   │   └── ExceptionFilter.cs
│   ├── Mappings/
│   │   └── MapConfigurations.cs
│   └── Events/
│       └── DomainEventsDispatcher.cs
└── DependencyInjection.cs
```

**Regras**:
- ✅ Handlers usam injeção de dependência via Primary Constructor
- ✅ Validação com FluentValidation (opcional)
- ✅ Mapeamento com Mapster
- ✅ Sempre retornar Result<T>
- ❌ NÃO colocar lógica de negócio aqui (pertence ao Domain)
- ❌ NÃO acessar banco diretamente (usar repositórios)

### 4. Infrastructure Layer

**Propósito**: Implementação de detalhes técnicos.

**Estrutura**:
```
Infra/
├── DataAccess/
│   ├── ApplicationDbContext.cs
│   ├── UnitOfWork.cs
│   ├── Repositories/
│   │   ├── BaseRepository.cs
│   │   ├── CategoryRepository.cs
│   │   └── ...
│   └── Configurations/
│       ├── CategoryConfiguration.cs
│       └── ...
├── Security/
├── Services/
└── DependencyInjection.cs
```

**Regras**:
- ✅ Implementar interfaces definidas no Domain
- ✅ EF Core Configurations
- ✅ Serviços externos (e-mail, storage, etc.)
- ❌ NÃO expor detalhes de implementação para outras camadas

### 5. Presentation Layer (API)

**Propósito**: Expor a aplicação via HTTP.

**Estrutura**:
```
API/
├── Controllers/
│   └── General/
│       ├── CategoriesController.cs
│       ├── CurrenciesController.cs
│       └── ...
├── Security/
│   └── JwtBearerEventsHandler.cs
├── Middlewares/
├── APIs/
│   └── Models/
│       ├── BaseRequest.cs
│       └── BaseResponse.cs
└── Program.cs
```

**Controller Padrão**:
```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Roles.ADMIN_OR_USER)]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<CategoryQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery] BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllCategoriesQuery { Query = queryParams });

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }

    [HttpGet("{externalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<CategoryQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetCategoryByExternalIdQuery(externalId));

        return result.IsSuccess 
            ? Ok(BaseResponse<CategoryQueryDTO>.CreateSuccessResponse(result.Value)) 
            : NoContent();
    }

    [HttpPost]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        var result = await _mediator.SendAsync(new CreateCategoryCommand(request.Data?.Name));

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpPut("{externalId}")]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid externalId, 
        [FromBody] BaseRequest<CategoryCommandDTO> request)
    {
        var result = await _mediator.SendAsync(
            new UpdateCategoryCommand(externalId, request.Data?.Name, request.Data?.Active)
        );

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}
```

---

## 🎨 Convenções de Código

### Nomenclatura

#### Arquivos e Classes
- **Entidades**: `Category`, `User`, `Market` (substantivos no singular)
- **Commands**: `CreateCategoryCommand`, `UpdateUserCommand` (verbo + entidade + Command)
- **Queries**: `GetAllCategoriesQuery`, `GetCategoryByIdQuery` (Get + detalhes + Query)
- **Handlers**: `CreateCategoryCommandHandler`, `GetAllCategoriesQueryHandler` (nome da request + Handler)
- **DTOs**: `CategoryQueryDTO`, `CategoryCommandDTO` (entidade + propósito + DTO)
- **Repositories**: `ICategoryReadRepository`, `ICategoryWriteRepository`
- **Business Rules**: `CategoryMustBeActiveBusinessRule` (descrição + BusinessRule)

#### Variáveis e Parâmetros
- **camelCase**: variáveis locais e parâmetros
- **PascalCase**: propriedades, métodos, classes
- **_underscore**: campos privados (exemplo: `_categoryRepository`)

### Organização de Arquivos

```
Feature/
├── Commands/
│   ├── Create/
│   │   ├── CreateEntityCommand.cs
│   │   ├── CreateEntityCommandHandler.cs
│   │   └── CreateEntityValidator.cs (opcional)
│   ├── Update/
│   └── Delete/
├── Queries/
│   ├── GetAll/
│   │   ├── GetAllEntitiesQuery.cs
│   │   └── GetAllEntitiesQueryHandler.cs
│   └── GetByExternalId/
│       ├── GetEntityByExternalIdQuery.cs
│       └── GetEntityByExternalIdQueryHandler.cs
└── DomainEvents/ (opcional)
```

### Primary Constructors (C# 12+)

**SEMPRE usar Primary Constructors para injeção de dependência:**

```csharp
// ✅ CORRETO
internal class CreateCategoryCommandHandler(
    ICategoryWriteRepository categoryWriteRepository, 
    IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    // ...
}

// ❌ INCORRETO
internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateCategoryCommandHandler(
        ICategoryWriteRepository categoryWriteRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryWriteRepository = categoryWriteRepository;
        _unitOfWork = unitOfWork;
    }
}
```

### Record Types

**Usar `record` para:**
- Commands imutáveis
- Queries imutáveis
- DTOs (opcional, mas preferível)

```csharp
// ✅ CORRETO
public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;
public sealed record GetCategoryByExternalIdQuery(Guid ExternalId) : IQuery<CategoryQueryDTO>;

// ✅ ACEITÁVEL
public sealed class GetAllCategoriesQuery : QueryList<CategoryQueryDTO>;
```

### Async/Await

**SEMPRE usar async/await para operações I/O:**

```csharp
// ✅ CORRETO
public async Task<Result<Guid>> HandleAsync(
    CreateCategoryCommand request, 
    CancellationToken cancellationToken = default)
{
    var category = Category.Create(request.Name);
    
    if (category.IsFailure)
        return Result.Failure<Guid>(category.Errors!);

    await _categoryWriteRepository.CreateAsync(category.Value, cancellationToken);
    await _unitOfWork.CommitAsync(cancellationToken);

    return Result<Guid>.Success(category.Value.ExternalId!.Value);
}
```

### Nullable Reference Types

**Projeto configurado com nullable enabled:**

```csharp
// ✅ CORRETO - indicar quando pode ser null
public string? Name { get; set; }
public Category? GetById(long id);

// ✅ CORRETO - indicar quando NÃO pode ser null
public string Name { get; private set; } = string.Empty;
public Category GetById(long id);
```

---

## 📝 Guia de Implementação

### Criando uma Nova Entidade

**Passo a passo:**

1. **Criar a Entidade no Domain**

```csharp
// Domain/General/Entities/Products/Product.cs
public sealed class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool Active { get; private set; } = true;

    private Product() { }

    public Product(
        long id, 
        string name, 
        decimal price, 
        bool active, 
        Guid? externalId = null, 
        DateTimeOffset? createdDate = null, 
        DateTimeOffset? updatedDate = null)
        : base(id, externalId, createdDate, updatedDate)
    {
        Name = name;
        Price = price;
        Active = active;
    }

    public static Result<Product> Create(string name, decimal price)
    {
        var product = new Product(0, name, price, true, Guid.NewGuid());

        var result = product.CheckRequiredFields();
        if (result.IsFailure)
            return Result.Failure<Product>(result.Errors!);

        return Result.Success(product);
    }

    public Result Update(string name, decimal price, bool? active)
    {
        Name = name;
        Price = price;

        if (active is not null)
            Active = (bool)active;

        return CheckRequiredFields();
    }

    protected override Result CheckRequiredFields()
    {
        var result = CheckRequiredField(
            string.IsNullOrWhiteSpace(Name), 
            "Product name must be provided"
        );
        
        if (result.IsFailure)
            return result;

        return CheckRule(
            Price <= 0, 
            Error.ValidationError("Price must be greater than zero")
        );
    }
}
```

2. **Criar Interfaces de Repositório**

```csharp
// Domain/General/Entities/Products/IProductReadRepository.cs
public interface IProductReadRepository
{
    Task<BaseQueryResponse<IEnumerable<Product>>> GetAllAsync(
        QueryParams? queryParams, 
        CancellationToken cancellationToken = default
    );
    Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<Product?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);
}

// Domain/General/Entities/Products/IProductWriteRepository.cs
public interface IProductWriteRepository
{
    Task CreateAsync(Product product, CancellationToken cancellationToken = default);
    void Update(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(long productId, CancellationToken cancellationToken = default);
    Task<Product?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default);
}
```

3. **Criar DTOs**

```csharp
// Application/DTOs/General/Products/ProductQueryDTO.cs
public class ProductQueryDTO
{
    public Guid ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Active { get; set; }
}

// Application/DTOs/General/Products/ProductCommandDTO.cs
public class ProductCommandDTO
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public bool? Active { get; set; }
}
```

4. **Criar Commands**

```csharp
// Application/UseCases/General/Products/Commands/Create/CreateProductCommand.cs
public sealed record CreateProductCommand(string Name, decimal Price) : ICommand<Guid>;

// Application/UseCases/General/Products/Commands/Create/CreateProductCommandHandler.cs
internal class CreateProductCommandHandler(
    IProductWriteRepository productWriteRepository, 
    IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductWriteRepository _productWriteRepository = productWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(
        CreateProductCommand request, 
        CancellationToken cancellationToken = default)
    {
        var product = Product.Create(request.Name, request.Price);

        if (product.IsFailure)
            return Result.Failure<Guid>(product.Errors!);

        await _productWriteRepository.CreateAsync(product.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(product.Value.ExternalId!.Value);
    }
}
```

5. **Criar Queries**

```csharp
// Application/UseCases/General/Products/Queries/GetAll/GetAllProductsQuery.cs
public sealed class GetAllProductsQuery : QueryList<ProductQueryDTO>;

// Application/UseCases/General/Products/Queries/GetAll/GetAllProductsQueryHandler.cs
internal class GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
    : IQueryHandler<GetAllProductsQuery, BaseQueryResponse<IReadOnlyList<ProductQueryDTO>>>
{
    private readonly IProductReadRepository _productReadRepository = productReadRepository;

    public async Task<Result<BaseQueryResponse<IReadOnlyList<ProductQueryDTO>>>> HandleAsync(
        GetAllProductsQuery request, 
        CancellationToken cancellationToken = default)
    {
        var products = await _productReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<ProductQueryDTO>>>.Create(
            products.Adapt<BaseQueryResponse<IReadOnlyList<ProductQueryDTO>>>()
        );
    }
}

// Application/UseCases/General/Products/Queries/GetByExternalId/GetProductByExternalIdQuery.cs
public sealed record GetProductByExternalIdQuery(Guid ExternalId) : IQuery<ProductQueryDTO>;

// Application/UseCases/General/Products/Queries/GetByExternalId/GetProductByExternalIdQueryHandler.cs
internal class GetProductByExternalIdQueryHandler(IProductReadRepository productReadRepository) :
    IQueryHandler<GetProductByExternalIdQuery, ProductQueryDTO>
{
    private readonly IProductReadRepository _productReadRepository = productReadRepository;

    public async Task<Result<ProductQueryDTO>> HandleAsync(
        GetProductByExternalIdQuery request, 
        CancellationToken cancellationToken = default)
    {
        var product = await _productReadRepository.GetByExternalIdAsync(
            request.ExternalId, 
            cancellationToken
        );

        return Result<ProductQueryDTO>.Create(product.Adapt<ProductQueryDTO>());
    }
}
```

6. **Implementar Repositório**

```csharp
// Infra/DataAccess/Repositories/ProductRepository.cs
internal class ProductRepository(ApplicationDbContext dbContext) 
    : BaseRepository<Product>(dbContext), IProductReadRepository, IProductWriteRepository
{
    public async Task<BaseQueryResponse<IEnumerable<Product>>> GetAllAsync(
        QueryParams? queryParams, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Products.Where(p => !p.IsDeleted);

        // Aplicar filtros, ordenação, paginação...

        return new BaseQueryResponse<IEnumerable<Product>>
        {
            Data = await query.ToListAsync(cancellationToken),
            TotalRows = await query.CountAsync(cancellationToken)
        };
    }

    public async Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
    }

    public async Task<Product?> GetByExternalIdAsync(
        Guid externalId, 
        CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .FirstOrDefaultAsync(p => p.ExternalId == externalId && !p.IsDeleted, cancellationToken);
    }
}
```

7. **Configurar EF Core**

```csharp
// Infra/DataAccess/Configurations/ProductConfiguration.cs
internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ExternalId)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Active)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
```

8. **Criar Controller**

```csharp
// API/Controllers/General/ProductsController.cs
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Roles.ADMIN_OR_USER)]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(BaseQueryResponse<IEnumerable<ProductQueryDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromQuery] BoundQueryParams queryParams)
    {
        var result = await _mediator.SendAsync(new GetAllProductsQuery { Query = queryParams });

        return result.IsSuccess ? Ok(result.Value) : NoContent();
    }

    [HttpGet("{externalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<ProductQueryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid externalId)
    {
        var result = await _mediator.SendAsync(new GetProductByExternalIdQuery(externalId));

        return result.IsSuccess 
            ? Ok(BaseResponse<ProductQueryDTO>.CreateSuccessResponse(result.Value)) 
            : NoContent();
    }

    [HttpPost]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BaseRequest<ProductCommandDTO> request)
    {
        var result = await _mediator.SendAsync(
            new CreateProductCommand(request.Data?.Name!, request.Data!.Price!.Value)
        );

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value));
    }

    [HttpPut("{externalId}")]
    [Authorize(Policy = Roles.ADMIN)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid externalId, 
        [FromBody] BaseRequest<ProductCommandDTO> request)
    {
        var result = await _mediator.SendAsync(
            new UpdateProductCommand(
                externalId, 
                request.Data?.Name, 
                request.Data?.Price, 
                request.Data?.Active
            )
        );

        if (result.IsFailure)
            return BadRequest(BaseResponse<string>.CreateFailureResponse(result.Errors!));

        return NoContent();
    }
}
```

9. **Registrar Dependências**

```csharp
// Infra/DependencyInjection.cs
services.AddScoped<IProductReadRepository, ProductRepository>();
services.AddScoped<IProductWriteRepository, ProductRepository>();
```

---

## ✅ Checklist para Code Review

### 1. Arquitetura e Estrutura

- [ ] As dependências entre camadas estão corretas?
- [ ] Domain não depende de outras camadas?
- [ ] Código está na camada apropriada?
- [ ] Estrutura de pastas segue o padrão estabelecido?

### 2. Result Pattern

- [ ] Todos os métodos que podem falhar retornam `Result<T>` ou `Result`?
- [ ] Não há uso de exceções para controle de fluxo de negócio?
- [ ] Erros são criados usando os métodos estáticos de `Error`?
- [ ] Controllers verificam `result.IsSuccess` antes de retornar?
- [ ] Handlers retornam `Result.Failure<T>()` em caso de erro (não `Result<T>.Failure()`)?

### 3. CQRS

- [ ] Commands e Queries estão separados?
- [ ] Commands implementam `ICommand<T>` ou `ICommand`?
- [ ] Queries implementam `IQuery<T>`?
- [ ] Handlers implementam a interface correta (`ICommandHandler` ou `IQueryHandler`)?
- [ ] Queries para listas usam `QueryList<T>`?
- [ ] Queries para listas retornam `Result<BaseQueryResponse<IReadOnlyList<T>>>`?
- [ ] Queries para item único retornam `Result<T>`?

### 4. Entidades

- [ ] Entidades herdam de `BaseEntity`?
- [ ] Setters são privados?
- [ ] Existe método `Create` estático retornando `Result<T>`?
- [ ] Métodos de negócio retornam `Result` ou `Result<T>`?
- [ ] `CheckRequiredFields()` está implementado?
- [ ] Construtor privado para EF Core existe?
- [ ] Lógica de negócio está na entidade (não no handler)?

### 5. Repositórios

- [ ] Separação Read/Write está implementada?
- [ ] Interfaces estão no Domain?
- [ ] Implementações estão no Infrastructure?
- [ ] Repositórios herdam de `BaseRepository<T>`?
- [ ] Queries de leitura retornam `BaseQueryResponse<IEnumerable<T>>`?

### 6. Handlers

- [ ] Usam Primary Constructor para injeção de dependência?
- [ ] Campos privados readonly são inicializados (`_repository = repository`)?
- [ ] Validam entrada antes de processar?
- [ ] Retornam `Result.Failure<T>()` em caso de falha?
- [ ] Commands usam `IUnitOfWork` para commit?
- [ ] Queries não modificam estado?

### 7. Controllers

- [ ] Usam Primary Constructor?
- [ ] Injetam apenas `IMediator`?
- [ ] Verificam `result.IsSuccess` antes de retornar dados?
- [ ] GET retorna 200 (com dados) ou 204 (sem dados)?
- [ ] POST retorna 201 em sucesso, 400 em erro?
- [ ] PUT retorna 204 em sucesso, 400 em erro?
- [ ] DELETE retorna 204 em sucesso, 400/404 em erro?
- [ ] Usam `BaseResponse<T>.CreateSuccessResponse()` para queries de item único?
- [ ] Usam `BaseResponse<T>.CreateFailureResponse()` para erros?
- [ ] Têm atributos de autorização apropriados?
- [ ] Têm ProducesResponseType configurados?

### 8. Convenções de Código

- [ ] Nomenclatura segue o padrão estabelecido?
- [ ] Primary Constructors são usados?
- [ ] Records são usados para Commands/Queries?
- [ ] Async/await é usado corretamente?
- [ ] CancellationToken é passado para métodos async?
- [ ] Nullable reference types estão corretos?
- [ ] Campos privados usam `_underscore`?
- [ ] Não há código duplicado (DRY)?

### 9. Validação

- [ ] Validações de negócio estão nas entidades?
- [ ] Validações de entrada podem usar FluentValidation?
- [ ] Erros de validação retornam `Error.ValidationError()`?
- [ ] Campos obrigatórios retornam `Error.RequiredFieldIsEmpty()`?

### 10. Performance e Boas Práticas

- [ ] Queries usam projeção (Select) quando apropriado?
- [ ] Não há N+1 queries?
- [ ] Include/ThenInclude são usados corretamente?
- [ ] Query filters (IsDeleted) estão configurados?
- [ ] Índices de banco estão planejados?

### 11. Segurança

- [ ] Endpoints têm autorização configurada?
- [ ] Dados sensíveis não são expostos?
- [ ] Validação de entrada está presente?
- [ ] ExternalId é usado em vez de Id nas APIs?

### 12. Testes (Recomendado)

- [ ] Entidades têm testes unitários?
- [ ] Business rules têm testes?
- [ ] Handlers têm testes?
- [ ] Casos de erro estão testados?

---

## 🚀 Exemplos de Anti-Patterns (O que NÃO fazer)

### ❌ Não usar exceções para controle de fluxo

```csharp
// ❌ INCORRETO
public async Task<CategoryQueryDTO> HandleAsync(...)
{
    var category = await _repository.GetByIdAsync(id);
    if (category is null)
        throw new NotFoundException("Category not found");
    
    return category.Adapt<CategoryQueryDTO>();
}

// ✅ CORRETO
public async Task<Result<CategoryQueryDTO>> HandleAsync(...)
{
    var category = await _repository.GetByIdAsync(id);
    
    return Result<CategoryQueryDTO>.Create(category.Adapt<CategoryQueryDTO>());
}
```

### ❌ Não colocar lógica de negócio em Handlers

```csharp
// ❌ INCORRETO - Lógica de negócio no Handler
public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand request, ...)
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return Result.Failure<Guid>(Error.RequiredFieldIsEmpty("Name is required"));
    
    var category = new Category { Name = request.Name };
    await _repository.CreateAsync(category);
    return Result.Success(category.ExternalId!.Value);
}

// ✅ CORRETO - Lógica de negócio na Entidade
public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand request, ...)
{
    var category = Category.Create(request.Name);
    
    if (category.IsFailure)
        return Result.Failure<Guid>(category.Errors!);
    
    await _repository.CreateAsync(category.Value);
    await _unitOfWork.CommitAsync();
    
    return Result.Success(category.Value.ExternalId!.Value);
}
```

### ❌ Não usar setters públicos em Entidades

```csharp
// ❌ INCORRETO
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Setter público!
}

// ✅ CORRETO
public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    
    public Result Update(string name)
    {
        Name = name;
        return CheckRequiredFields();
    }
}
```

### ❌ Não retornar Result<T>.Failure() diretamente

```csharp
// ❌ INCORRETO
return Result<Guid>.Failure(Error.ValidationError("Invalid"));

// ✅ CORRETO
return Result.Failure<Guid>(Error.ValidationError("Invalid"));
```

### ❌ Não esquecer de verificar IsSuccess nos Controllers

```csharp
// ❌ INCORRETO
public async Task<IActionResult> GetById(Guid id)
{
    var result = await _mediator.SendAsync(new GetCategoryByIdQuery(id));
    return Ok(result); // Retorna Result<T> ao invés do valor!
}

// ✅ CORRETO
public async Task<IActionResult> GetById(Guid id)
{
    var result = await _mediator.SendAsync(new GetCategoryByIdQuery(id));
    
    return result.IsSuccess 
        ? Ok(BaseResponse<CategoryDTO>.CreateSuccessResponse(result.Value)) 
        : NoContent();
}
```

### ❌ Não usar construtores tradicionais quando Primary Constructors estão disponíveis

```csharp
// ❌ INCORRETO
public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly IRepository _repository;
    
    public CreateCategoryHandler(IRepository repository)
    {
        _repository = repository;
    }
}

// ✅ CORRETO
public class CreateCategoryHandler(IRepository repository) 
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly IRepository _repository = repository;
}
```

---

## 📚 Referências Rápidas

### Tipos de Retorno por Operação

| Operação | Retorno do Handler | Retorno do Controller | Status Code |
|----------|-------------------|----------------------|-------------|
| GET (lista) | `Result<BaseQueryResponse<IReadOnlyList<T>>>` | `Ok(result.Value)` ou `NoContent()` | 200 ou 204 |
| GET (item) | `Result<T>` | `Ok(BaseResponse<T>.CreateSuccessResponse(result.Value))` ou `NoContent()` | 200 ou 204 |
| POST | `Result<Guid>` | `Created("", BaseResponse<string>.CreateNewObjectIdResponse(result.Value))` | 201 |
| PUT | `Result` | `NoContent()` ou `BadRequest(...)` | 204 ou 400 |
| DELETE | `Result` | `NoContent()` ou `BadRequest(...)` | 204 ou 400 |

### Interfaces CQRS

```csharp
// Commands
ICommand<TResponse> : IRequest<Result<TResponse>>
ICommand : IRequest<Result>
ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>

// Queries
IQuery<TResponse> : IRequest<Result<TResponse>>
IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>

// Base para listas
QueryList<T> : IQuery<BaseQueryResponse<IReadOnlyList<T>>>
```

### Métodos do Result

```csharp
// Criação
Result.Success()
Result.Success<T>(T value)
Result.Failure(Error error)
Result.Failure(List<Error> errors)
Result.Failure<T>(Error error)
Result.Failure<T>(List<Error> errors)
Result.Create<T>(T? value) // null = Failure(Error.NullValue)

// Verificação
result.IsSuccess
result.IsFailure
result.Value      // Lança exceção se IsFailure
result.Errors     // Lista de erros
```

### Tipos de Error

```csharp
Error.None
Error.NullValue
Error.RequiredFieldIsEmpty(string message)
Error.InactiveForeignKey(string message)
Error.NotAuthorized(string message)
Error.ValidationError(string message)
Error.RecordAlreadyExists(string message)
Error.RecordNotFound(string message)
```

---

## 🎓 Princípios SOLID Aplicados

### Single Responsibility Principle (SRP)
- Cada classe tem uma única responsabilidade
- Handlers fazem apenas orquestração
- Entidades contêm apenas lógica de negócio
- Repositórios fazem apenas acesso a dados

### Open/Closed Principle (OCP)
- Extensível via herança (BaseEntity, BaseRepository)
- Fechado para modificação (interfaces estáveis)

### Liskov Substitution Principle (LSP)
- Implementações podem ser substituídas por suas interfaces
- `IProductRepository` pode ser substituído por qualquer implementação

### Interface Segregation Principle (ISP)
- Repositórios separados em Read/Write
- Interfaces específicas por necessidade

### Dependency Inversion Principle (DIP)
- Depender de abstrações (interfaces), não de implementações
- Domain define interfaces, Infrastructure implementa

---

## 🔄 Fluxo de uma Request

```
HTTP Request
    ↓
Controller (API)
    ↓
IMediator.SendAsync()
    ↓
Handler (Application)
    ↓
Domain Entity (business logic)
    ↓
Repository (Infrastructure)
    ↓
Database
    ↓
Result<T>
    ↓
Controller (verify IsSuccess)
    ↓
HTTP Response (200/201/204/400)
```

---

## 📌 Conclusão

Esta documentação serve como guia definitivo para:
- ✅ Manter consistência no código
- ✅ Facilitar code reviews
- ✅ Onboarding de novos desenvolvedores
- ✅ Gerar código com IA seguindo os padrões do projeto
- ✅ Garantir qualidade e manutenibilidade

**Lembre-se**: Código limpo, testável e manutenível é mais importante que código "inteligente" ou "criativo". Siga os padrões estabelecidos!

---

**Versão**: 1.0  
**Data**: 2025  
**Projeto**: MoneyFlow  
**.NET Version**: 10.0  
**Arquitetura**: Clean Architecture + CQRS + DDD
