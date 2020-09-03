@model AbpDtoGenerator.GeneratorModels.ViewModel
@{
    var data = Model.Server;
}
# 小神探的代码生成器(ABP Code Power Tools )使用说明文档

 
  
[仓库地址--内网](http://10.1.2.202:10080/MoonsControls/xstCodeGenerator)

[仓库地址--外网](http://gitlab.cyanstream.com/MoonsControls/xstCodeGenerator)

>欢迎您使用 ABP Code Power Tools ，.NET Core 版本。
开发代码生成器的初衷是为了让大家专注于业务开发，
而基础设施的地方，由代码生成器实现，节约大家的实现。
实现提高效率、共赢的局面。

欢迎到：[Github地址](https://github.com/52ABP/52ABP.CodeGenerator) 提供您的脑洞，
如果合理的功能我会实现哦~



### 配置AbpAutoMapper
 

请打开@{@data.ReplaceInfo.SolutionNamespace}.Application类库中@{@data.ReplaceInfo.SolutionNamespace}ApplicationModule.cs中的 PreInitialize 方法中:

```csharp
// 自定义AutoMapper类型映射
// 如果没有这一段就把本所有代码复制上去
Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
{
    // ....其他代码

    // 只需要复制这一段
    @{@data.Entity.Name}DtoAutoMapper.CreateMappings(configuration);

    // ....其他代码
});

```
### EntityFrameworkCore功能配置

打开EntityFrameworkCore类库在 **@{@data.ReplaceInfo.SolutionNamespace}DbContext**类文件中添加以下代码段：

```csharp
public DbSet<@data.Entity.Name>  @{@data.Entity.Name}s { get; set; }

 ```
以实现将实体配置到数据库上下文中。

#### 添加迁移记录

如果该实体的属性值未发生改变可以跳过当前小节

打开**程序包管理器控制台**工具，指定默认项目类库以**EntityFrameworkCore**结尾，然后执行以下命令:

添加一条迁移记录

```
Add-Migration AddNew@{@data.Entity.Name}Entity_Migration
```

同步实体文件到数据库中
```
Update-Database
```

接下来配置好多语言功能，然后运行项目后，即可前往前端项目的配置


## 列表显示

实体的列表显示为由xml文件配置，路径为`wwwroot\ConfigFiles\ListViews\`中的文件。


## 查询条件配置
 
前端页面上的查询条件可通过路径  `wwwroot\ConfigFiles\PageFilters\`中的文件配置。


## 多语言的配置
 
多语言是由其他工具提供，这里不作说明。





## 实体渲染

实体所在文件夹名称

@{@data.Entity.ParentDirLowerName}

@{@data.Entity.ParentDirName}
 